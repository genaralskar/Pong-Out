using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 300f;

    private Rigidbody2D rb;

    private Coroutine c;
    private WaitForFixedUpdate wait = new WaitForFixedUpdate();

    private List<GameObject> blocks = new List<GameObject>();
    private List<PaddleBlock> pBlocks = new List<PaddleBlock>();

    private GameManager gm;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gm = FindObjectOfType<GameManager>();
        gm.RoundStart += ResetBlocks;

        GetBlocks();
    }

    public void BreakBlock(Vector2 position)
    {
        FindNearestBlock(position).SetActive(false);
    }

    private void OnMove(InputValue value)
    {
        float dir = value.Get<float>();

        rb.velocity = (Vector2.up * dir * moveSpeed * Time.fixedDeltaTime);
    }

    private void GetBlocks()
    {
        foreach(var p in GetComponentsInChildren<PaddleBlock>())
        {
            blocks.Add(p.gameObject);
            pBlocks.Add(p);
        }
    }

    private void ResetBlocks()
    {
        rb.MovePosition(new Vector2(transform.position.x, 0));

        foreach (var b in blocks)
        {
            b.SetActive(true);
        }
    }

    private GameObject FindNearestBlock(Vector2 point)
    {
        GameObject g = GetFirstActiveBlock();
        float closeDist = Vector2.Distance(point, g.transform.position);
        if (g == null) Debug.LogError("Not active blocks on the paddle, but you're trying to get one!");

        foreach(var b in blocks)
        {
            if (!b.activeSelf) continue;

            if(Vector2.Distance(point, b.transform.position) < closeDist)
            {
                g = b;
                closeDist = Vector2.Distance(point, b.transform.position);
            }
        }

        return g;
    }

    private GameObject GetFirstActiveBlock()
    {
        foreach(var b in blocks)
        {
            if(b.activeSelf)
            {
                return b;
            }
        }
        return null;
    }
}
