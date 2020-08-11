using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlayAudioClipButton : PlayAudioClip
{
    protected override void Awake()
    {
        base.Awake();
        GetComponent<Button>().onClick.AddListener(PlayClip);
    }
}
