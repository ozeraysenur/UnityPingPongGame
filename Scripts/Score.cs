using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    [SerializeField] TMP_Text playerScoreText;
    [SerializeField] TMP_Text OpponentScoreText;
    [SerializeField] TMP_Text playerSetScoreText;
    [SerializeField] TMP_Text OpponentSetScoreText;

    public AudioClip ball_ground;
    public AudioClip lose_audio;
    public AudioClip win_audio;
    private AudioSource scoreAudio;

    private RestartGame restartGameScript;

    public int playerScore = 0;
    public int opponentScore = 0;
    public int setPlayerScore = 0;
    public int setOpponentScore = 0;
    public bool isServed = false;
    public bool hasGameStarted = false;
    public bool gameOver = false;


    private BallMovement ballMovementScript;

    // Start is called before the first frame update
    void Start()
    {
        restartGameScript = GameObject.Find("RestartGame").GetComponent<RestartGame>();
        scoreAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

        int initialPlayerScore = playerScore;
        int initialOpponentScore = opponentScore;

        if (!gameOver) {
            if (GameObject.FindGameObjectWithTag("Ball").GetComponent<BallMovement>() != null) {
                ballMovementScript = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallMovement>();
            }
            else {
                
            }


            // if serve isn't done and ball hits one side of the table more than once
            if (!isServed) {

                if (isLet()) {
                    gameOver = true;
                }

                if (ballMovementScript.numberOfTimesHitTable > 1) {

                    if (ballMovementScript.hitter == "Player") {

                        if (ballMovementScript.numberOfTimesHitTableO > 1 && ballMovementScript.numberOfTimesHitTableP == 0) {
                            gameOver = true;
                            Debug.Log("Serve hitter : Player  opponent-side-hit > 1  player-side-hit = 0");
                            opponentScore++;
                            scoreAudio.PlayOneShot(lose_audio, 2f);
                        }
                        else if (ballMovementScript.numberOfTimesHitTableO == 0 && ballMovementScript.numberOfTimesHitTableP > 1) {
                            gameOver = true;
                            Debug.Log("Serve hitter : Player  opponent-side-hit = 0  player-side-hit > 1");
                            opponentScore++;
                            scoreAudio.PlayOneShot(lose_audio, 2f);
                        }
                        else if (ballMovementScript.numberOfTimesHitTableO > 1 && ballMovementScript.numberOfTimesHitTableP == 1) {
                            gameOver = true;
                            Debug.Log("Serve hitter : Player  opponent-side-hit > 1  player-side-hit = 1");
                            playerScore++;
                            scoreAudio.PlayOneShot(win_audio, 2f);
                        }
                        else if (ballMovementScript.numberOfTimesHitTableO == 1 && ballMovementScript.numberOfTimesHitTableP > 1) {
                            gameOver = true;
                            Debug.Log("Serve hitter : Player  opponent-side-hit = 1  player-side-hit > 1");
                            opponentScore++;
                            scoreAudio.PlayOneShot(lose_audio, 2f);
                        }
                    }

                    else if (ballMovementScript.hitter == "Opponent") {

                        if (ballMovementScript.numberOfTimesHitTableP > 1 && ballMovementScript.numberOfTimesHitTableO == 0) {
                            gameOver = true;
                            Debug.Log("Serve hitter : Opponent  opponent-side-hit = 0  player-side-hit > 1");
                            playerScore++;
                            scoreAudio.PlayOneShot(win_audio, 2f);
                        }
                        else if (ballMovementScript.numberOfTimesHitTableP == 0 && ballMovementScript.numberOfTimesHitTableO > 1) {
                            gameOver = true;
                            Debug.Log("Serve hitter : Opponent  opponent-side-hit > 1  player-side-hit = 0");
                            playerScore++;
                            scoreAudio.PlayOneShot(win_audio, 2f);
                        }
                        else if (ballMovementScript.numberOfTimesHitTableO > 1 && ballMovementScript.numberOfTimesHitTableP == 1) {
                            gameOver = true;
                            Debug.Log("Serve hitter : Opponent  opponent-side-hit > 1  player-side-hit = 1");
                            opponentScore++;
                            scoreAudio.PlayOneShot(lose_audio, 2f);
                        }
                        else if (ballMovementScript.numberOfTimesHitTableO == 1 && ballMovementScript.numberOfTimesHitTableP > 1) {
                            gameOver = true;
                            Debug.Log("Serve hitter : Opponent  opponent-side-hit = 1  player-side-hit > 1");
                            playerScore++;
                            scoreAudio.PlayOneShot(win_audio, 2f);
                        }
                    }
                }

            }
            // if the serve was done and the ball hits the table more than one times
            else if(isServed) {
                if (ballMovementScript.numberOfTimesHitTable > 1) {

                    if (ballMovementScript.hitter == "Player") {

                        if (ballMovementScript.numberOfTimesHitTableO > 1 && ballMovementScript.numberOfTimesHitTableP == 0) {
                            gameOver = true;
                            Debug.Log("hitter : Player  opponent-side-hit > 1  player-side-hit = 0");
                            playerScore++;
                            scoreAudio.PlayOneShot(win_audio, 2f);
                        }
                        else if (ballMovementScript.numberOfTimesHitTableO == 1 && ballMovementScript.numberOfTimesHitTableP == 1) {
                                gameOver = true;
                                Debug.Log("hitter : Player  opponent-side-hit = 1  player-side-hit = 1");
                                opponentScore++;
                            scoreAudio.PlayOneShot(lose_audio, 2f);
                        }
                        else if (ballMovementScript.numberOfTimesHitTableO == 0 && ballMovementScript.numberOfTimesHitTableP > 1) {
                                gameOver = true;
                                Debug.Log("hitter : Player  opponent-side-hit = 0  player-side-hit > 1");
                                opponentScore++;
                            scoreAudio.PlayOneShot(lose_audio, 2f);
                        }
                    }

                    else if (ballMovementScript.hitter == "Opponent") {

                        if (ballMovementScript.numberOfTimesHitTableP > 1 && ballMovementScript.numberOfTimesHitTableO == 0) {
                            gameOver = true;
                            Debug.Log("hitter : Opponent  opponent-side-hit > 1  player-side-hit = 0");
                            opponentScore++;
                            scoreAudio.PlayOneShot(lose_audio, 2f);
                        }
                        else if (ballMovementScript.numberOfTimesHitTableP == 1 && ballMovementScript.numberOfTimesHitTableO == 1) {
                            gameOver = true;
                            Debug.Log("hitter : Opponent  opponent-side-hit = 1  player-side-hit = 1");
                            playerScore++;
                            scoreAudio.PlayOneShot(win_audio, 2f);
                        }
                        else if (ballMovementScript.numberOfTimesHitTableP == 0 && ballMovementScript.numberOfTimesHitTableO > 1) {
                            gameOver = true;
                            Debug.Log("hitter : Opponent  opponent-side-hit = 0  player-side-hit > 1");
                            playerScore++;
                            scoreAudio.PlayOneShot(win_audio, 2f);
                        }
                    }
                }
            }

            // if the ball is on ground part
            if (ballMovementScript.isOnGround) {

                scoreAudio.PlayOneShot(ball_ground, 2f);

                //Debug.Log("if the ball is on ground part");
                if (ballMovementScript.hitter == "Player") {

                    if (ballMovementScript.whichSide == "Player") {
                        Debug.Log("Hitter: " + ballMovementScript.hitter + "  WhichSide: " + ballMovementScript.whichSide);
                        gameOver = true;
                        opponentScore++;
                        scoreAudio.PlayOneShot(lose_audio, 2f);
                    }
                    else if (ballMovementScript.whichSide == "Opponent") {
                        Debug.Log("Hitter: " + ballMovementScript.hitter + "  WhichSide: " + ballMovementScript.whichSide);
                        gameOver = true;
                        playerScore++;
                        scoreAudio.PlayOneShot(win_audio, 2f);
                    }
                    else {
                        Debug.Log("Hitter: " + ballMovementScript.hitter + "  WhichSide: " + ballMovementScript.whichSide);
                        gameOver = true;
                        opponentScore++;
                        scoreAudio.PlayOneShot(lose_audio, 2f);
                    }
                }

                else if (ballMovementScript.hitter == "Opponent") {

                    if (ballMovementScript.whichSide == "Player") {
                        Debug.Log("Hitter: " + ballMovementScript.hitter + "  WhichSide: " + ballMovementScript.whichSide);
                        gameOver = true;
                        opponentScore++;
                        scoreAudio.PlayOneShot(lose_audio, 2f);
                    }
                    else if (ballMovementScript.whichSide == "Opponent") {
                        Debug.Log("Hitter: " + ballMovementScript.hitter + "  WhichSide: " + ballMovementScript.whichSide);
                        gameOver = true;
                        playerScore++;
                        scoreAudio.PlayOneShot(win_audio, 2f);
                    }
                    else {
                        Debug.Log("Hitter: " + ballMovementScript.hitter + "  WhichSide: " + ballMovementScript.whichSide);
                        gameOver = true;
                        playerScore++;
                        scoreAudio.PlayOneShot(win_audio, 2f);
                    }
                }
            }
            UpdateScore();
            UpdateFinalScore();

        }
    }


    void UpdateFinalScore() {


        playerSetScoreText.text = "Set Score : " + setPlayerScore;
        OpponentSetScoreText.text = "Set Score : " + setOpponentScore;

        if (setPlayerScore == 3 || setOpponentScore == 3) {
            SceneManager.LoadSceneAsync(2);
        }
    }

    public bool isSetOver() {
        if (playerScore < 10 && opponentScore < 10) {
            return false;
        }
        else if (isDeuce()) {
            return false;
        }

        else if ((playerScore > 10 || opponentScore > 10) && ((playerScore - opponentScore >= 2) || (opponentScore - playerScore >= 2))) {
            if (playerScore > opponentScore) {
                setPlayerScore++;
                scoreAudio.PlayOneShot(win_audio, 2f);
                playerScore = 0;
                opponentScore = 0;
            }
            else {
                setOpponentScore++;
                playerScore = 0;
                opponentScore = 0;
            }
            string[] strings = { "Player", "Opponent" };
            restartGameScript.setTurn(strings[Random.Range(0, strings.Length)]);
            return true;
        }
        else {
            return false;
        }
        
    }

    public bool isDeuce() {
        if ((playerScore >= 10 && opponentScore >= 10)) {
            if (playerScore == opponentScore) {
                return true;
            }
            else if (playerScore - opponentScore == 1) {
                return true;
            }
            else if (opponentScore - playerScore == 1) {
                return true;
            }
        }
        return false;
    }

    public bool isLet() {
        if (ballMovementScript.isHitNet) {

            if ((ballMovementScript.hitter == "Player" || ballMovementScript.hitter == "Opponent") &&
                ballMovementScript.numberOfTimesHitTableP == 1 && ballMovementScript.numberOfTimesHitTableO == 1) {

                return true;

            }
        }
        return false;
    }

    void UpdateScore() {

        playerScoreText.text = "Player : " + playerScore;
        OpponentScoreText.text = "Opponent : " + opponentScore;
    
    }

}
