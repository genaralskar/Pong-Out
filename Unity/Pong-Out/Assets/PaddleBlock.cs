using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleBlock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Ball"))
            gameObject.SetActive(false);
    }
}
