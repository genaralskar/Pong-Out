using System.Collections;
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
    public Vector2 moveDirection = new Vector2(3, 2);
    private Vector2 nmd;
    private bool flipY = false;
    private bool flipX = false;
    private bool ballMoving;

    [SerializeField]
    private Vector2 bounds = new Vector2(10, 10);

    [SerializeField]
    private GameObject art;

    private Rigidbody2D rb;

    private WaitForFixedUpdate fixedWait = new WaitForFixedUpdate();
    private WaitForSeconds sWait = new WaitForSeconds(.25f);
    private Coroutine mc;

    private Animator anims;
    private GameManager gm;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        anims = GetComponent<Animator>();
        nmd = moveDirection.normalized;
    }

    private void OnEnable()
    {
        ResetBall();
    }

    private void OnDisable()
    {
        ballMoving = false;
        StopAllCoroutines();
    }

    private void Start()
    {
        //gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //get direction for flip stuff
        Vector2 colPoint = collision.ClosestPoint(transform.position);
        Vector2 dir = (Vector2)transform.position - colPoint;

        PaddleController pc = collision.attachedRigidbody.GetComponent<PaddleController>();
        if (pc)
        {
            pc.BreakBlock(colPoint);
        }

        if (Mathf.Abs(dir.x) < Mathf.Abs(dir.y))
        {
            //flip y
            
            if (!flipY && colPoint.y < 0) //if travling up and the collider point is below it!
            {
                FlipY();
                return;
            }
            else if(flipY && colPoint.y > 0) //if traveling down and collider point is above it!
            {
                FlipY();
                return;
            }
        }
       
        
        //flip x
        if (!flipX && transform.position.x > 0)
        {
            //traveling right, so only make it flip if its moving right
            FlipX();
        }
        if (flipX && transform.position.x < 0)
        {
            //travling left, so only make it flip if its moving left
            FlipX();
        }
    }

    public void ResetBall()
    {
        transform.position = Vector2.zero;
        StopAllCoroutines();
        moveSpeed = startSpeed;
        anims.Play("ballActivate");
        moveDirection = GetRandomDirection(!flipX);
        StartCoroutine(StartBall());
    }

    private void EndBall()
    {
        //send event that ball is blown up, for audio and particle
        gm.BallExploded?.Invoke(transform.position);
        Debug.Log("Ball Ended");
        gameObject.SetActive(false);
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
        moveDirection.y *= -1;
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

    private float GetXPos(float t, float speed)
    {
        float xPos = transform.position.x;

        //reached the walls, shouldn't flip, just end it!
        if (xPos >= 5 || xPos <= -5)
        {
            EndBall();
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

    private Vector2 GetRandomDirection(bool startLeft)
    {
        float y = Random.Range(-.75f, .75f);
        float x = 1;
        if (startLeft) x = -1;
        Debug.Log("Random Direction:" + new Vector2(x, y));
        if (y < 0) flipY = true;
        return new Vector2(x, y);
    }
}
