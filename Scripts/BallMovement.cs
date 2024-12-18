using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BallMovement : MonoBehaviour {
    private Rigidbody rb;
    private Player playerControllerScript;
    private Score scoreManagerScript;
    private RestartGame restartGameScript;

    public AudioClip ball_table;
    private AudioSource ballAudio;

    public float maxSpeed;
    public float decelerationForce;
    public string hitter;
    public string whichSide;
    public int numberOfTimesHitTable = 0;
    public int numberOfTimesHitTableP = 0;
    public int numberOfTimesHitTableO = 0;
    public bool isOnGround;
    public bool firstHit = false;
    public bool opponentCanHit = false;
    public string firstHitter;
    public bool isHitNet = false;

    private Vector3 newPosition;


    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        playerControllerScript = GameObject.Find("Player").GetComponent<Player>();
        scoreManagerScript = GameObject.Find("Score").GetComponent<Score>();
        restartGameScript = GameObject.Find("RestartGame").GetComponent<RestartGame>();
        ballAudio = GetComponent<AudioSource>();
        numberOfTimesHitTableO++;
    }

    void FixedUpdate() {
        if (!scoreManagerScript.hasGameStarted && Input.GetMouseButton(0)) {
            if (restartGameScript.getTurn() == "Player") {
                transform.position = newPosition;
            }
            scoreManagerScript.hasGameStarted = true;
        }

        if (!firstHit) {
            ServeShot();
        }
        if (rb.velocity.magnitude > maxSpeed) {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
        if (rb.velocity.magnitude > 0f) {
            // Calculate the deceleration force in the opposite direction of velocity
            Vector3 deceleration = -rb.velocity.normalized * decelerationForce;

            // Apply the deceleration force to slow down the ball
            rb.AddForce(deceleration, ForceMode.Acceleration);
        }


    }

    private void OnCollisionEnter(Collision collision) {
        if (!scoreManagerScript.isServed) {
            rb.useGravity = true;
            if (!firstHit) {
                firstHitter = collision.gameObject.name;
            }
            firstHit = true;
            if (collision.gameObject.CompareTag("Racket") && !collision.gameObject.name.Equals(firstHitter)) {
                scoreManagerScript.isServed = true;
            }
        }
        else if (collision.gameObject.CompareTag("Racket")) {
            rb.velocity += new Vector3(0, 6, 0);

        }

        if (collision.gameObject.name == "Player") {
            numberOfTimesHitTableP = 0;
            numberOfTimesHitTableO = 0;
            numberOfTimesHitTable = 0;
            hitter = "Player";
        }
        else if (collision.gameObject.name == "Opponent") {
            if (numberOfTimesHitTableO >= 1 && scoreManagerScript.isServed) {
                opponentCanHit = true;
            }
            numberOfTimesHitTableP = 0;
            numberOfTimesHitTableO = 0;
            numberOfTimesHitTable = 0;
            hitter = "Opponent";
        }
        else if (collision.gameObject.name == "net") {
            isHitNet = true;
        }

    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Ground")) {
            isOnGround = true;

        }
        else if (other.CompareTag("PlayerSide")) {
            ballAudio.PlayOneShot(ball_table, 2f);
            numberOfTimesHitTableP++;
            numberOfTimesHitTable++;
            whichSide = "Player";
        }
        else if (other.CompareTag("OpponentSide")) {
            ballAudio.PlayOneShot(ball_table, 2f);
            numberOfTimesHitTableO++;
            numberOfTimesHitTable++;
            whichSide = "Opponent";
        }

    }

    private void ServeShot() {
        if (restartGameScript.getTurn() == "Player") {
            float xPosition = playerControllerScript.transform.position.x;
            float yPosition = playerControllerScript.transform.position.y + 1.3f;
            newPosition = new Vector3(xPosition, yPosition, -14.5f);
            transform.position = newPosition;
        }

    }


}