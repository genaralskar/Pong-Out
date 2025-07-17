using UnityEngine;

public class PlayerWinAudioPlayer : PlayAudioClip
{
    public AudioClip clip2;

    public override void PlayClip()
    {
        if(!GameManager.winningPlayer)
        {
            ac.PlayVoiceClip(clip);
        }
        else
        {
            ac.PlayVoiceClip(clip2);
        }
        
    }
}
