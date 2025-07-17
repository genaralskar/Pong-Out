using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioClip : MonoBehaviour
{
    protected AudioController ac;
    public AudioClip clip;

    protected virtual void Awake()
    {
        ac = FindObjectOfType<AudioController>();
    }

    public virtual void PlayClip()
    {
        ac.PlayClip(clip);
    }
    
}
