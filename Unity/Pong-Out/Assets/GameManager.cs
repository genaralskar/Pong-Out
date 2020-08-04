using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityAction GameStart;
    public UnityAction RoundStart;
    public UnityAction RoundEnd;
    public UnityAction<bool> GameEnd;

    //Input check stuff
    public UnityAction InputCheckStart;
    public UnityAction InputCheckEnds;

    //score manager stuff
    public UnityAction<bool> UpdateScore;
    public UnityAction ResetScores;
    public UnityAction PlayerLeftScores;
    public UnityAction PlayerRightScores;
    public SOInt leftPlayerScore;
    public SOInt rightPlayerScore;

    //ball stuff
    public UnityAction<Vector2> BallExploded;

    private ScoreManager sm;

    [SerializeField]
    private int scoreToWin = 10;
    public GameObject ball;

    [Header("UI Stuff")]
    public GameObject gameUI;
    public GameObject mainMenuUI;
    public GameObject pauseMenuUI;
    public GameObject endGameUI;
    public Animator mainMenuAnimator;
    public Animator gameMenuAnimator;

    private void Awake()
    {
        sm = FindObjectOfType<ScoreManager>();

        InputCheckEnds += InputCheckEndsHandler;
        GameEnd += GameEndHandler;
        RoundStart += RoundStartHandler;
        RoundEnd += RoundEndHandler;
        BallExploded += BallExplodedHandler;
    }

    public void StartNewGame()
    {
        ResetScores?.Invoke();

        gameUI.SetActive(true);
        mainMenuAnimator.SetBool("mainMenu", false);

        //game start
        GameStart?.Invoke();
        //input check
        InputCheckStart?.Invoke();
        InputCheckEndsHandler();
    }

    private void InputCheckEndsHandler()
    {
        //ball spawns
        RoundStart?.Invoke();
    }

    private void RoundStartHandler()
    {
        ball.SetActive(true);
    }

    private void BallExplodedHandler(Vector2 pos)
    {
        Debug.Log("Ball Exploded!");
        RoundEnd?.Invoke();

        UpdateScore?.Invoke(pos.x < 0);
    }

    private void RoundEndHandler()
    {
        Debug.Log("Round End");
        ball.SetActive(false);
        //check score
        ScoreInfo si = sm.CheckScores(scoreToWin);
        if(si.winner == true)
        {
            //if a winner, end the game
            GameEnd?.Invoke(si.player);
        }
        else
        {
            //else start next round
            StartCoroutine(RoundPadding());
        }
        
    }

    private void GameEndHandler(bool player)
    {
        if (!player)
        {
            //left player
            //show player left wins!
        }
        else
        {
            //right player
            //show player right wins!
        }
    }

    private IEnumerator RoundPadding()
    {
        yield return new WaitForSeconds(2);
        RoundStart?.Invoke();
    }

    public struct ScoreInfo
    {
        public bool winner;
        public bool player;
    }
}
