using System.Collections;
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMove(InputValue value)
    {
        float dir = value.Get<float>();

        rb.velocity = (Vector2.up * dir * moveSpeed * Time.fixedDeltaTime);
    }

    private IEnumerator Move(float dir)
    {
        while (true)
        {
            
            yield return wait;
        }
    }
}
