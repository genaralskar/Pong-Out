using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputChecker : MonoBehaviour
{
    private GameManager gm;

    public bool p1Up;
    public bool p1Down;
    public bool p2Up;
    public bool p2Down;

    public Animator p1UpAnims;
    public Animator p1DownAnims;
    public Animator p2UpAnims;
    public Animator p2DownAnims;

    private Coroutine c;
    private bool checkingInputs;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();

        gm.InputCheckStart += InputCheckStartHandler;
    }

    private void OnP1Move(InputValue value)
    {
        if (!checkingInputs) return;

        float dir = value.Get<float>();

        if(dir > 0)
        {
            p1Up = true;
            p1UpAnims.SetBool("active", false);
        }

        if(dir < 0)
        {
            p1Down = true;
            p1DownAnims.SetBool("active", false);
        }
    }

    private void OnP2Move(InputValue value)
    {
        if (!checkingInputs) return;

        float dir = value.Get<float>();

        if (dir > 0)
        {
            p2Up = true;
            p2UpAnims.SetBool("active", false);
        }

        if (dir < 0)
        {
            p2Down = true;
            p2DownAnims.SetBool("active", false);
        }
    }

    private void InputCheckStartHandler()
    {
        p1Up = false;
        p1Down = false;
        p2Up = false;
        p2Down = false;

        p1UpAnims.SetBool("active", true);
        p1DownAnims.SetBool("active", true);
        p2UpAnims.SetBool("active", true);
        p2DownAnims.SetBool("active", true);

        if (c == null)
            c = StartCoroutine(Checker());
    }

    private IEnumerator Checker()
    {
        checkingInputs = true;
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        while(!p1Up || !p1Down || !p2Up || !p2Down)
        {
            yield return wait;
        }

        GameManager.tutorialDone = true;
        gm.InputCheckEnds?.Invoke();
        c = null;
    }
}
