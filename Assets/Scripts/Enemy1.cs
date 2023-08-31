using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    Rigidbody2D myRigidbody;
    Timer timer;
    GameObject barrier;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        timer = FindObjectOfType<Timer>();
        myRigidbody.velocity = new Vector2(moveSpeed, 0f);
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

        if (transform.position.y > 15f)
        {
            Vector2 originalVelocity = myRigidbody.velocity;
            myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y - 5);
            //yield return new WaitForSeconds(1f);
            myRigidbody.velocity = new Vector2(-originalVelocity.x, -2f);
        }
        else if (transform.position.y < -3f)
        {
            Vector2 originalVelocity = myRigidbody.velocity;
            myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y + 5);
            //yield return new WaitForSeconds(1f);
            myRigidbody.velocity = new Vector2(-originalVelocity.x, 2f);
        }
        else
        {
            myRigidbody.velocity = new Vector2(1f, 5f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "NoEnemy" || other.tag == "BarrierWest" ||
            other.tag == "BarrierEast" || other.tag == "BarrierSouth" ||
            other.tag == "BarrierNorth")
        {
            barrier = other.gameObject;
            //StartCoroutine(CorrectPath(barrier));
        }
    }

    // IEnumerator CorrectPath(GameObject specificBarrier)
    // {
    /*    if (transform.position.y > 15f)
        {
            Vector2 originalVelocity = myRigidbody.velocity;
            myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y - 5);
            yield return new WaitForSeconds(1f);
            myRigidbody.velocity = new Vector2(-originalVelocity.x, -2f);
        }
        else if (transform.position.y < -3f)
        {
            Vector2 originalVelocity = myRigidbody.velocity;
            myRigidbody.velocity = new Vector2(0f, myRigidbody.velocity.y + 5);
            yield return new WaitForSeconds(1f);
            myRigidbody.velocity = new Vector2(-originalVelocity.x, 2f);
        }
        else
        {
            myRigidbody.velocity = new Vector2(1f, 5f);
        } */

    /*if (Mathf.Sign(myRigidbody.velocity.x) == -1)
    {
        myRigidbody.velocity = new Vector2(10f, -1f);
        yield return new WaitForSeconds(1f);
    }
    else if (Mathf.Sign(myRigidbody.velocity.x) == 1)
    {
        myRigidbody.velocity = new Vector2(-10f, 1f);
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
    } */
    // }
}
