using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartGame : MonoBehaviour {


     public GameObject ballPrefab;
     private Score scoreManagerScript;
     private Rigidbody playerRb;
     private Rigidbody opponentRb;
     string turn = "Opponent";
     private int playerTurnCount = 0;
     private int opponentTurnCount = 0;
     Vector3 spawnPos;
    // Start is called before the first frame update
    void Start()
     {
        scoreManagerScript = GameObject.Find("Score").GetComponent<Score>();
        playerRb = GameObject.Find("Player").GetComponent <Rigidbody>();
        opponentRb = GameObject.Find("Opponent").GetComponent<Rigidbody>();
        SpawnBalls();
    }

    private void Update() {
        if (scoreManagerScript.gameOver && !scoreManagerScript.isSetOver()) {
            StartCoroutine(RestartCountdownRoutine());
            scoreManagerScript.gameOver = false;
            scoreManagerScript.isServed = false;

        }
    }
    public string getTurn() {
        return turn;
    }
    public void setTurn(string t) {
        turn = t;
    }

    IEnumerator RestartCountdownRoutine() {
        yield return new WaitForSeconds(1);
        playerRb.transform.position = new Vector3(0, 8.5f, -17);
        opponentRb.transform.position = new Vector3(0, 8.5f, 14);
        scoreManagerScript.hasGameStarted = false;
        ChangeTurn();
        SpawnBalls();
    }

    void SpawnBalls() {
        if (turn == "Opponent") {
            spawnPos = new Vector3(0, 9, 12);
        }
        else if (turn == "Player") {
            spawnPos = new Vector3(playerRb.position.x, 9.5f, -12);
        }
            Instantiate(ballPrefab, spawnPos, ballPrefab.transform.rotation);
    }

    void ChangeTurn() {

        if (!scoreManagerScript.isLet()) {

            if (scoreManagerScript.isDeuce()) {

                if (turn == "Player") {
                    turn = "Opponent";
                }
                else if (turn == "Opponent") {
                    turn = "Player";
                }
            }
            else {
                if (turn == "Player") {
                    playerTurnCount++;
                    if (playerTurnCount > 1) {
                        turn = "Opponent";
                        opponentTurnCount = 0;
                        playerTurnCount = 0;
                    }

                }
                else if (turn == "Opponent") {
                    opponentTurnCount++;
                    if (opponentTurnCount > 1) {
                        turn = "Player";
                        playerTurnCount = 0;
                        opponentTurnCount = 0;
                    }
                }
            }
        }
    }
}
