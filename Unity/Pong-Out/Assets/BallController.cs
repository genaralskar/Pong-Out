using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private float startSpeed = 5f;
    [SerializeField]
    private float maxSpeed = 100f;
    [SerializeField]
    private float moveSpeed = 5;
    [SerializeField]
    private float speedAccleration = .25f;
    public Vector2 moveDirection = new Vector2(1, 1);
    private Vector2 nmd;
    [SerializeField]
    private Vector2 bounds = new Vector2(10, 10);
    [SerializeField]
    private float width = .25f;

    public Vector2 velocity = new Vector2(1f, .5f);

    [SerializeField]
    private GameObject art;

    private bool ballMoving;

    private Rigidbody2D rb;

    private WaitForFixedUpdate fixedWait = new WaitForFixedUpdate();
    private WaitForSeconds sWait = new WaitForSeconds(.25f);
    private Coroutine mc;

    private Animator anims;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anims = GetComponent<Animator>();
        nmd = moveDirection.normalized;
    }

    private void OnEnable()
    {
        ResetBall();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        //get direction for flip stuff
        Vector2 colPoint = collision.ClosestPoint(transform.position);
        Vector2 dir = (Vector2)transform.position - colPoint;
        if(Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            //flip x
            FlipX();
        }
        else
        {
            //flip y
            FlipY();
        }

        PaddleController pc = collision.attachedRigidbody.GetComponent<PaddleController>();
        if(pc)
        {
            pc.BreakBlock(colPoint);
        }
    }

    public void StopBall()
    {
        ballMoving = false;
    }

    private void ResetBall()
    {
        transform.position = Vector2.zero;
        StopAllCoroutines();
        moveSpeed = startSpeed;
        anims.Play("ballActivate");
        StartCoroutine(StartBall());
    }

    public void AddSpeed()
    {
        if (moveSpeed >= maxSpeed) return;
        moveSpeed += speedAccleration;

        if (moveSpeed > maxSpeed)
            moveSpeed = maxSpeed;
    }

    public void FlipX()
    {
        flipX = !flipX;
        AddSpeed();
    }

    public void FlipY()
    {
        flipY = !flipY;
        AddSpeed();
    }

    private IEnumerator StartBall()
    {
        yield return sWait;
        ballMoving = false;

        int timer = 0;
        bool flash = true;
        while (timer < 5)
        {
            //flash
            art.SetActive(flash);
            flash = !flash;
            timer++;
            yield return sWait;
        }

        if(mc != null)
        {
            StopCoroutine(mc);
        }
        mc = StartCoroutine(MoveBall());
    }

    private IEnumerator MoveBall()
    {
        ballMoving = true;
        float startTime = Time.time;
        float pongTimer = Time.time;
        float pongOffset = 0;
        while (ballMoving)
        {
            
            Vector2 newPos = Vector2.zero;
            //x movement
            newPos.x = GetXPos(Time.time, moveSpeed);

            //y movement
            newPos.y = GetYPos(Time.time - startTime, moveSpeed);

            if ((Time.time - pongTimer) * moveSpeed >= bounds.y + pongOffset)
            {
                //Debug.Log($"Pong! timer:{(Time.time - pongTimer) * moveSpeed}, valueCheck:{bounds.y + pongOffset}");
                pongOffset += bounds.y;
                //pongTimer = Time.time;
            }


            rb.MovePosition(newPos);

            yield return fixedWait;
        }
    }

    private bool flipY = false;

    private float GetYPos(float t, float speed)
    {
        //float max = bounds.y - width;
        float max = bounds.y;
        t += max / 2;

        //if(t % max)

        //return Mathf.PingPong(t * speed, max) - (max/2);

        float yPos = transform.position.y;

        if (yPos >= 5)
        {
            flipY = true;
            AddSpeed();
        }
        if (yPos <= -5)
        {
            flipY = false;
            AddSpeed();
        }


        if (!flipY)
        {
            yPos += Time.fixedDeltaTime * moveSpeed * nmd.y;
        }
        else
        {
            yPos -= Time.fixedDeltaTime * moveSpeed * nmd.y;
        }

        //between -5 and 5
        return yPos;
    }

    private bool flipX = false;

    private float GetXPos(float t, float speed)
    {
        float xPos = transform.position.x;

        if (xPos >= 5)
        {
            flipX = true;
            AddSpeed();
        }
        if(xPos  <= -5)
        {
            flipX = false;
            AddSpeed();
        }


        if(!flipX)
        {
            xPos += Time.fixedDeltaTime * moveSpeed * nmd.x;
        }
        else
        {
            xPos -= Time.fixedDeltaTime * moveSpeed * nmd.x;
        }

        //between -5 and 5
        return xPos;
    }
}
