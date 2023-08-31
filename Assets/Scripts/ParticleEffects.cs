using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffects : MonoBehaviour
{
    [SerializeField] ParticleSystem gotKidEffect;

    public void PlayEffect()
    {
        ParticleSystem instance = Instantiate(gotKidEffect, transform.position, Quaternion.identity);
        Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
    }

}
