using UnityEngine;

public class AudioController : MonoBehaviour
{
    private GameManager gm;
    private BallController ball;
    private AudioSource source;
    private AudioSource voiceSource;

    public AudioClip gameStart;

    public AudioClip playerLeftScores;
    public AudioClip playerRightScores;

    public AudioClip BallExplode;
    public AudioClip BallBounce;

    public AudioClip voicePing;
    public AudioClip voicePong;
    private bool pingPoingFlip = false;


    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        ball = gm.ball.GetComponent<BallController>();
        source = GetComponents<AudioSource>()[0];
        voiceSource = GetComponents<AudioSource>()[1];

        gm.GameStart += GameStartHandler;
        gm.InputCheckEnds += InputCheckEndsHandler;

        gm.PlayerLeftScores += PlayerLeftScoresHandler;
        gm.PlayerRightScores += PlayerRightScoresHandler;

        gm.BallExploded += BallExplodedHandler;
        gm.BallBounce += BallBounceHandler;
    }

    public void PlayClip(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    public void PlayVoiceClip(AudioClip clip)
    {
        voiceSource.clip = clip;
        voiceSource.Play();
    }

    private void GameStartHandler()
    {
        
    }

    private void InputCheckEndsHandler()
    {
        PlayVoiceClip(gameStart);
    }

    private void PlayerLeftScoresHandler()
    {
        //Debug.Log("Left Player Score");
        PlayVoiceClip(playerLeftScores);
    }

    private void PlayerRightScoresHandler()
    {
        PlayVoiceClip(playerRightScores);
    }

    private void BallExplodedHandler(Vector2 point)
    {
        PlayClip(BallExplode);
    }

    private void BallBounceHandler(bool x)
    {
        if(gm.funnyVoice)
        {
            if(!x)
            {
                PlayClip(BallBounce);
            }
            else if(ball.FlipXBool)
            {
                PlayVoiceClip(voicePing);
            }
            else
            {
                PlayVoiceClip(voicePong);
            }
            pingPoingFlip = !pingPoingFlip;
        }
        else
        {
            PlayClip(BallBounce);
        }
    }
}
