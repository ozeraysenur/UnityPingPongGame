using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour {
    private Rigidbody rb;
    public Transform tableR;

    public AudioClip player_racket;
    private AudioSource playerAudio;

    private float horizontalBoundary = 8f;
    private float zTopBoundary = -1f;
    private float zBottomBoundary = -21f;
    private float yBottomBoundary = 8.5f;
    private float yTopBoundary = 14f;
    private Score scoreManagerScript;
    private RestartGame restartGameScript;

    public float zSensitivity;
    public float xSensitivity;
    public float yPositionChangePerSecond = 5f;

    private Vector3 inputVelocity;

    private void Start() {
        scoreManagerScript = GameObject.Find("Score").GetComponent<Score>();
        restartGameScript = GameObject.Find("RestartGame").GetComponent<RestartGame>();
        playerAudio = GetComponent<AudioSource>();
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    private void Update() {
        // Get the raw input values
        if (!scoreManagerScript.hasGameStarted) {
            rb.isKinematic = true;
        }
        if (scoreManagerScript.hasGameStarted) {
            rb.isKinematic = false;
            float xMovement = (Input.GetAxis("Mouse X") * xSensitivity);
            float zMovement = (Input.GetAxis("Mouse Y") * zSensitivity);

            // Calculate the vertical movement
            float yChange = 0f;

            if (Input.GetMouseButton(0)) {
                yChange = yPositionChangePerSecond;
            }
            else if (Input.GetMouseButton(1)) {
                yChange = -yPositionChangePerSecond;
            }

            // Store the input velocity for later use in FixedUpdate()
            inputVelocity = new Vector3(xMovement, yChange, zMovement);
        }
    }

    private void FixedUpdate() {
        if (scoreManagerScript.hasGameStarted) {
            // Apply input velocity directly to the rigidbody
            rb.velocity = inputVelocity;

            // Clamp the position
            Vector3 clampedPosition = rb.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, -horizontalBoundary, horizontalBoundary);
            clampedPosition.z = Mathf.Clamp(clampedPosition.z, zBottomBoundary, zTopBoundary);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, yBottomBoundary, yTopBoundary);
            
            // Update the position of the rigidbody
            rb.position = Vector3.Lerp(rb.position,clampedPosition, 0.5f);
        }
        RotateTowardsTable();
    }




    private void OnCollisionEnter(Collision collision) {

        playerAudio.PlayOneShot(player_racket, 2f);

        Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();

        if (collision.gameObject.CompareTag("Ball") && scoreManagerScript.isServed) {
            rb.isKinematic = true;
            ballRb.velocity += new Vector3(0, 10, 10);
            ballRb.velocity = new Vector3(ballRb.velocity.x, Mathf.Clamp(ballRb.velocity.y, -15, 25), ballRb.velocity.z);
        }
        else if (!scoreManagerScript.isServed) {
            rb.isKinematic = true;
        }
    }
    private void OnCollisionExit(Collision collision) {
        rb.isKinematic = false;
    }

    void RotateTowardsTable() {
        Vector3 racketPos = transform.position;
        Vector3 tablePos = tableR.position + new Vector3(0, 2, 10);

        float xRot;
        float yRot;
        float zRot;
        float xDiff = tablePos.x - racketPos.x;
        float zDiff = tablePos.z - racketPos.z;


        // if not moving horizontally, use the regular yRot and zRot values
        yRot = Mathf.Atan(xDiff / zDiff) * Mathf.Rad2Deg * 1;
        zRot = Mathf.Atan2(xDiff, zDiff) * Mathf.Rad2Deg * 2;
        

        if (!scoreManagerScript.isServed && restartGameScript.getTurn() == "Player") {
            // Calculate the x rotation angle to face the table
            xRot = Mathf.Atan2(0.8f, zDiff) * Mathf.Rad2Deg * 5f + 15;
            // Rotate the racket towards the table
            transform.rotation = Quaternion.Euler(xRot, yRot, Mathf.Clamp(zRot, -30, 30));
            return;
        }
        else if (transform.rotation.eulerAngles.x > 0) {
            // Gradually reduce x rotation towards 0
            float decayRate = 75f; // Adjust this value to control the rate of decay
            xRot = Mathf.Lerp(transform.rotation.eulerAngles.x, 0f, Time.deltaTime * decayRate);
            transform.rotation = Quaternion.Euler(xRot, yRot, Mathf.Clamp(zRot, -30, 30));
        }
        else {
            // If x rotation is already 0, maintain the regular rotation
            transform.rotation = Quaternion.Euler(0, yRot, Mathf.Clamp(zRot, -30, 30));
        }

    }

}

