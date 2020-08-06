using System.Collections;
using UnityEngine;

public class PaddleBlock : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Transform st;
    private Collider2D col;

    [SerializeField]
    private float breakTime = 1f;
    [SerializeField]
    private float gravityMod = 1f;
    [SerializeField]
    private float forceMod = 1f;

    private WaitForEndOfFrame wait = new WaitForEndOfFrame();

    public bool activePaddle = true;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        st = sprite.transform;
        col = GetComponent<Collider2D>();
    }

    public void DisablePaddle()
    {
        col.enabled = false;
        MoveArt();
    }

    public void ResetPaddle()
    {
        activePaddle = true;
        col.enabled = true;
        sprite.color = Color.white;
        st.localPosition = Vector2.zero;
        st.localRotation = Quaternion.identity;
    }

    private void MoveArt()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        //get inital velocity
        Vector2 move = Vector2.zero;
        move.x = Random.Range(0f, 1f);
        move.y = Random.Range(0f, 1f);
        move *= forceMod;

        //get rotation speed
        float rSpeed = Random.Range(10f, 180f);

        //get fade amount
        float fadeAmount = breakTime / 1f;

        float timer = 0;
        while(timer < breakTime)
        {
            timer += Time.deltaTime;

            //move
            st.localPosition = (Vector2)st.localPosition + (move * Time.deltaTime);
            move.y -= gravityMod * Time.deltaTime;

            //rotate
            Vector3 rot = st.localRotation.eulerAngles;
            rot.z += rSpeed * Time.deltaTime;
            st.localRotation = Quaternion.Euler(rot);

            //fade color
            Color c = sprite.color;
            c.a -= fadeAmount * Time.deltaTime;
            sprite.color = c;

            yield return wait;
        }

        activePaddle = false;
    }
}
