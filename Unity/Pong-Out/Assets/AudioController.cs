using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private GameManager gm;
    private AudioSource source;

    public AudioClip playerLeftScores;
    public AudioClip playerRightScores;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        source = GetComponent<AudioSource>();

        gm.PlayerLeftScores += PlayerLeftScoresHandler;
        gm.PlayerRightScores += PlayerRightScoresHandler;
    }

    public void PlayClip(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    private void PlayerLeftScoresHandler()
    {
        PlayClip(playerLeftScores);
    }

    private void PlayerRightScoresHandler()
    {
        PlayClip(playerRightScores);
    }
}
