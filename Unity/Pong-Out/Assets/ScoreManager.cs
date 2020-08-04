using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public SOInt player1Score;
    public SOInt player2Score;
    public TextMeshProUGUI p1ScoreText;
    public TextMeshProUGUI p2ScoreText;

    private GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        gm.UpdateScore += UpdateScoreActionHandler;
        gm.ResetScores += ResetScoresHandler;
    }

    private void UpdateScoreActionHandler(bool player)
    {
        //false = left player, true = right player
        if(!player)
        {
            player1Score.value++;
            gm.PlayerLeftScores?.Invoke();
            if(player1Score >= 10)
            {
                //end game
            }
            else
            {
                p1ScoreText.text = player1Score.value.ToString();
                
            }
        }
        else
        {
            player2Score.value++;
            gm.PlayerRightScores?.Invoke();
            if(player2Score >= 10)
            {
                //end game
            }
            else
            {
                p2ScoreText.text = player2Score.value.ToString();
                
            }
        }
    }

    public GameManager.ScoreInfo CheckScores(int maxScore)
    {
        GameManager.ScoreInfo si = new GameManager.ScoreInfo();
        if(player1Score >= maxScore)
        {
            si.winner = true;
            si.player = false;
        }

        if(player2Score >= maxScore)
        {
            si.winner = true;
            si.player = true;
        }

        return si;
    }

    private void ResetScoresHandler()
    {
        player1Score.value = 0;
        p1ScoreText.text = "0";
        player2Score.value = 0;
        p2ScoreText.text = "0";

    }
}
