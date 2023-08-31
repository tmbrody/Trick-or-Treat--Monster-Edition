using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMovement : MonoBehaviour
{
    Animator animator;
    Rigidbody2D myRigidbody;
    Tilemap[] tilemaps = new Tilemap[5];
    int tilemapIndex = 0;
    int offsetXIndex = 0;
    int offsetYIndex = 0;
    float[] OffsetsX = new float[5];
    float[] OffsetsY = new float[5];
    Timer timer;
    void Awake()
    {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        timer = FindObjectOfType<Timer>();

        foreach (Tilemap child in FindObjectsOfType<Tilemap>())
        {
            if (child.GetComponent<BoxCollider2D>() != null)
            {
                tilemaps[tilemapIndex] = child;
                tilemapIndex++;
                OffsetsX[offsetXIndex] = child.GetComponent<BoxCollider2D>().offset.x;
                offsetXIndex++;
                OffsetsY[offsetYIndex] = child.GetComponent<BoxCollider2D>().offset.y;
                offsetYIndex++;
            }
        }
    }

    void Update()
    {
        if (gameObject.tag == "Enemy1")
        {
            //  StartCoroutine(Enemy1Movement());
        }
        if (gameObject.tag == "Enemy2")
        {
            Enemy2Movement();
        }
        if (gameObject.tag == "Enemy4")
        {
            Enemy4Movement();
        }
        if (gameObject.tag == "Enemy5")
        {
            Enemy5Movement();
        }
    }
    /*   IEnumerator Enemy1Movement()
       {
           myRigidbody.velocity = new Vector2(-moveSpeed, 0f);
           myRigidbody.velocity = new Vector2(-moveSpeed, 0f);
           myRigidbody.velocity = new Vector2(-moveSpeed, 0f);
           myRigidbody.velocity = new Vector2(-moveSpeed, 0f);
       } */

    void Enemy2Movement()
    {
        throw new NotImplementedException();
    }

    void Enemy4Movement()
    {
        throw new NotImplementedException();
    }
    void Enemy5Movement()
    {
        throw new NotImplementedException();
    }
}
