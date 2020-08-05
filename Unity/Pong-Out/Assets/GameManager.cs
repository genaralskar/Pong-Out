using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static bool winningPlayer;
    public static bool tutorialDone = false;

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
    public UnityAction<bool> BallBounce;


    private ScoreManager sm;

    [SerializeField]
    private int scoreToWin = 10;
    public static int winScore = 10;
    public GameObject ball;

    [Header("UI Stuff")]
    public GameObject gameUI;
    public GameObject mainMenuUI;
    public GameObject pauseMenuUI;
    public GameObject endGameUI;

    public Animator mainMenuAnimator;
    public Animator scoreScreenAnimator;
    public Animator winnerScreenAnimator;

    public TextMeshProUGUI winnerText;

    [Header("Audio Stuff")]
    public bool funnyVoice = false;

    private void Awake()
    {
        sm = FindObjectOfType<ScoreManager>();
        winScore = scoreToWin;

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
        if(!tutorialDone)
        {
            InputCheckStart?.Invoke();
        }
        else
        {
            InputCheckEndsHandler();
        }
    }

    private void InputCheckEndsHandler()
    {
        //ball spawns
        RoundStart?.Invoke();
    }

    private void RoundStartHandler()
    {
        scoreScreenAnimator.SetBool("active", true);
        ball.SetActive(true);
    }

    private void BallExplodedHandler(Vector2 pos)
    {
        //Debug.Log("Ball Exploded!");
        UpdateScore?.Invoke(pos.x < 0);
        RoundEnd?.Invoke();
    }

    private void RoundEndHandler()
    {
        //Debug.Log("Round End");
        ball.SetActive(false);
        //check score
        ScoreInfo si = sm.CheckScores(scoreToWin);
        //Debug.Log($"winner {si.winner}, player {si.player}");
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

    private IEnumerator RoundPadding()
    {
        yield return new WaitForSeconds(2);
        RoundStart?.Invoke();
    }

    private void GameEndHandler(bool player)
    {
        winningPlayer = player;
        StartCoroutine(EndGameCo(player));
    }

    private IEnumerator EndGameCo(bool player)
    {
        //disable scores
        scoreScreenAnimator.SetBool("active", false);

        yield return new WaitForSeconds(2f);
        //play in animation
        if (!player)
        {
            //left player
            //show player left wins!
            winnerText.text = "Player Left Wins!";
            winnerScreenAnimator.SetBool("active", true);
        }
        else
        {
            //right player
            //show player right wins!
            winnerText.text = "Player Right Wins!";
            winnerScreenAnimator.SetBool("active", true);
        }
    }

    public void MainMenuHandler()
    {

        StartCoroutine(MainMenuIn());

    }

    private IEnumerator MainMenuIn()
    {
        if (winnerScreenAnimator.GetBool("active"))
        {
            winnerScreenAnimator.SetBool("active", false);
            yield return new WaitForSeconds(1.2f);
        }
        mainMenuAnimator.SetBool("mainMenu", true);

    }

    public void UseVoiceSounds(bool value)
    {
        funnyVoice = value;
    }

    public struct ScoreInfo
    {
        public bool winner;
        public bool player;
    }
}
