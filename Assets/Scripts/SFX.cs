using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    [SerializeField] AudioClip monsterGrabAudio;
    [SerializeField] AudioClip monsterStep1;
    [SerializeField] [Range(0, 1f)] float monsterStepAudio = 1f;
    [SerializeField] AudioClip monsterStep2;
    AudioSource audioSource;
    static SFX instance;

    void Awake()
    {
        ManageSingleton();
    }

    void ManageSingleton()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }
}