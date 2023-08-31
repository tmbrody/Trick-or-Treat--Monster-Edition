using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScroller : MonoBehaviour
{
    [SerializeField] Vector2 moveSpeed;
    Material material;
    Vector2 offset;

    void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        offset = Time.deltaTime * moveSpeed;
        material.mainTextureOffset += offset;
    }
}
