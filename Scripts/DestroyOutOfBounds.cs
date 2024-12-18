using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private float lowerBound = -7;
    private Score scoreManagerScript;


    // Start is called before the first frame update
    void Start()
    {
        scoreManagerScript = GameObject.Find("Score").GetComponent<Score>();
    }

    // Update is called once per frame
    void Update() {
        if (transform.position.y < lowerBound) {
            Destroy(gameObject);
            scoreManagerScript.gameOver = true;
        }
        if (scoreManagerScript.gameOver) {
            Destroy(gameObject);

        }
    }
}
