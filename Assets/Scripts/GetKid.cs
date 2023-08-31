using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GetKid : MonoBehaviour
{
    [SerializeField] ParticleSystem gotKidEffect;
    Tilemap[] tilemaps;
    Timer timer;
    Player player;
    int maxKids;
    int goneKids = 0;
    bool shouldSpawn;

    void Awake()
    {
        player = FindObjectOfType<Player>();
        tilemaps = GetComponentsInChildren<Tilemap>();
        timer = FindObjectOfType<Timer>();
        maxKids = tilemaps.Length;
    }

    public void SpawnKids()
    {
        for (int i = 0; i < tilemaps.Length; i++)
        {
            float randomSpawn = UnityEngine.Random.Range(0, maxKids);
            if (randomSpawn > (maxKids / 2f))
            {
                shouldSpawn = true;
            }
            else if (randomSpawn <= (maxKids / 2f))
            {
                shouldSpawn = false;
                goneKids++;
            }
            tilemaps[i].gameObject.SetActive(shouldSpawn);

            if (goneKids == 5 && !timer.timeStopped)
            {
                int extraSpawn = (int)UnityEngine.Random.Range(0, maxKids);
                tilemaps[extraSpawn].gameObject.SetActive(true);
                goneKids = 0;
            }
        }

    }

    public void PlayEffect()
    {
        ParticleSystem instance = Instantiate(gotKidEffect, player.transform.position, Quaternion.identity);
        Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
    }
}
