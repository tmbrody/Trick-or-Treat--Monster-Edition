using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedSprites : MonoBehaviour
{
    [SerializeField] Sprite secondSprite;
    [SerializeField] float animationDuration = 0.5f;
    Sprite originalSprite;
    float animationTiming;
    bool isSecondFrame;

    void Start()
    {
        originalSprite = GetComponent<Image>().sprite;
        animationTiming = animationDuration;
    }

    void Update()
    {
        if (animationTiming - Time.timeSinceLevelLoad < 0.005f)
        {
            animationTiming += 0.5f;
            if (!isSecondFrame)
            {
                GetComponent<Image>().sprite = secondSprite;
                isSecondFrame = true;
            }
            else
            {
                GetComponent<Image>().sprite = originalSprite;
                isSecondFrame = false;
            }
        }
    }
}
