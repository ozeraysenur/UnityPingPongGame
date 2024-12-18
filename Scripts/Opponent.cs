using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    public Transform tableR;
    public GameObject target;
    public Transform[] targets;
    private Score scoreManagerScript;
    private BallMovement ballMovementScript;
    private RestartGame restartGameScript;

    public AudioClip opponent_racket;
    private AudioSource opponentAudio;

    private bool isCollided = false;

    public float speed = 50;
    public float forceMultiplier;
    Vector3 startPosition;
    Vector3 targetPosition;
    Vector3 ballPosition;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
        startPosition = transform.position;
        scoreManagerScript = GameObject.Find("Score").GetComponent<Score>();
        restartGameScript = GameObject.Find("RestartGame").GetComponent<RestartGame>();
        opponentAudio = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Ball").GetComponent<BallMovement>() != null) {
            ballMovementScript = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallMovement>();
            ballPosition = GameObject.FindGameObjectWithTag("Ball").transform.position;
        }
        else {
        }
        if (!scoreManagerScript.gameOver && scoreManagerScript.hasGameStarted && ballPosition.z > -2 && !isCollided) {
            Move();
        }
        RotateTowardsTable();

    }

    void Move() 
    {
        targetPosition = startPosition;

        if (ballMovementScript.numberOfTimesHitTableO > 0) {
            targetPosition = ballPosition;
            if (ballMovementScript.hitter != "Opponent") {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
        }
        if (ballMovementScript.hitter == "Opponent") {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
        }
        else if(ballMovementScript.numberOfTimesHitTableO == 0 && ballMovementScript.hitter != "Opponent") {
            targetPosition = new Vector3(ballPosition.x, startPosition.y, startPosition.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }

        //transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
        //targetPosition.x = ballPosition.x;
        /* if (ballPosition.y > 9 && targetPosition.y < yTopBoundary) {
             targetPosition.y += 0.05f;
         }
         else if (ballPosition.y < 9 && targetPosition.y > yBottomBoundary) {
             targetPosition.y -= 0.05f;
         }
         if (ballPosition.z > 4 && transform.position.z > zFrontBoundary) {
             targetPosition.z -= 0.05f;
         }
         else if (ballPosition.z < 4 && transform.position.z < zBackBoundary) {
             targetPosition.z += 0.05f;
         }

         if (scoreManagerScript.gameOver) {
             targetPosition = new Vector3(0, 9.6f, 14);
         }
       */
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

    }


    Vector3 PickTarget() {

        int randomValue = Random.Range(0, targets.Length);
        return targets[randomValue].position;
    }

    void RotateTowardsTable() {
        Vector3 racketPos = transform.position;
        Vector3 tablePos = tableR.position;
        float zRot;
        float xRot;
        float xDiff = tablePos.x - racketPos.x;
        float zDiff = tablePos.z - racketPos.z;

        zRot = Mathf.Atan2(xDiff, zDiff) * Mathf.Rad2Deg * 2;

        if (!scoreManagerScript.isServed && restartGameScript.getTurn() == "Opponent") {
            // Calculate the x rotation angle to face the table
            xRot = Mathf.Atan2(0.8f, zDiff) * Mathf.Rad2Deg * 2f;
            // Rotate the racket towards the table
            transform.rotation = Quaternion.Euler(xRot, 0, Mathf.Clamp(zRot, 30, -30));
            return;
        }
        else if (transform.rotation.eulerAngles.x < 0) {
            // Gradually reduce x rotation towards 0
            float decayRate = 75f; // Adjust this value to control the rate of decay
            xRot = Mathf.Lerp(transform.rotation.eulerAngles.x, 0f, Time.deltaTime * decayRate);
            transform.rotation = Quaternion.Euler(xRot, 0, Mathf.Clamp(zRot, 30, -30));
        }
        else {
            // If x rotation is already 0, maintain the regular rotation
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(zRot,30,-30));
        }

    }

    private void OnCollisionEnter(Collision collision) {
        opponentAudio.PlayOneShot(opponent_racket, 2f);
        ballMovementScript.opponentCanHit = true;
        if (collision.gameObject.CompareTag("Ball") && ballMovementScript.opponentCanHit) {
            isCollided = true;

            Vector3 dir = PickTarget() - transform.position;
            if (restartGameScript.getTurn() == "Opponent" && !scoreManagerScript.isServed) {

                Vector3 force = (dir.normalized + new Vector3(0, -5, -10)) * forceMultiplier;
                collision.gameObject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
            }
            else {
                collision.gameObject.GetComponent<Rigidbody>().velocity = dir.normalized * forceMultiplier + new Vector3(0, 30, 0);
            }
            //ballMovementScript.opponentCanHit = false;
        }

    }
    private void OnCollisionExit(Collision collision) {
        isCollided = false;
    }



}
