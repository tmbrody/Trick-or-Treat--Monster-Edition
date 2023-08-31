using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy3 : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    Rigidbody2D myRigidbody;
    Timer timer;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        timer = FindObjectOfType<Timer>();
    }

    void Update()
    {
        if (timer.timeStopped)
        {
            myRigidbody.velocity = new Vector2(0f, 0f);
        }

        if (Mathf.Sign(myRigidbody.velocity.x) == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "NoEnemy" || other.tag == "BarrierWest" ||
            other.tag == "BarrierEast" || other.tag == "BarrierSouth" ||
            other.tag == "BarrierNorth")
        {
            StartCoroutine(CorrectPath());
        }
    }

    IEnumerator CorrectPath()
    {
        if (Mathf.Sign(myRigidbody.velocity.x) == -1)
        {
            myRigidbody.velocity = new Vector2(10f, 0);
            yield return new WaitForSeconds(1f);
        }
        else if (Mathf.Sign(myRigidbody.velocity.x) == 1)
        {
            myRigidbody.velocity = new Vector2(-10f, 0);
            yield return new WaitForSeconds(1f);
        }
        else if (Mathf.Sign(myRigidbody.velocity.y) == -1)
        {
            myRigidbody.velocity = new Vector2(0, 10f);
            yield return new WaitForSeconds(1f);
        }
        else if (Mathf.Sign(myRigidbody.velocity.y) == 1)
        {
            myRigidbody.velocity = new Vector2(0, -10f);
            yield return new WaitForSeconds(1f);
        }
    }

    public void Enemy3PathX()
    {
        float signValue = UnityEngine.Random.Range(0, 2);
        if (signValue == 0)
        {
            myRigidbody.velocity = new Vector2(-moveSpeed, 0f);
        }
        else if (signValue == 1)
        {
            myRigidbody.velocity = new Vector2(moveSpeed, 0f);
        }
    }

    public void Enemy3PathY()
    {
        float signValue = UnityEngine.Random.Range(0, 2);
        if (signValue == 0)
        {
            myRigidbody.velocity = new Vector2(0f, -moveSpeed);
        }
        else if (signValue == 1)
        {
            myRigidbody.velocity = new Vector2(0f, moveSpeed);
        }
    }
}
