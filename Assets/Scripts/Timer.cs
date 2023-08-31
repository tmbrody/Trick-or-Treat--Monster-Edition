using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] GameObject goalTextContainer;
    [SerializeField] float secondsRemaining = 185;
    [SerializeField] int spawnInterval = 5;
    [SerializeField] int glowInterval = 3;
    [SerializeField] int enemyMoveInterval = 1;
    [SerializeField] Material newMaterial;
    [SerializeField] Material oldMaterial;
    int tempSpawnInterval;
    int tempGlowInterval;
    int tempEnemyMoveInterval;
    int initialSeconds;
    float initialSpawnSeconds;
    public bool timeStopped;
    GetKid getKid;
    StateCanvases stateCanvases;
    Player player;
    EnemyMovement enemyMovement;
    int timeScore;
    string timeScoreText;
    bool changeEnemy3Path;

    void Awake()
    {
        tempSpawnInterval = spawnInterval;
        tempGlowInterval = glowInterval;
        tempEnemyMoveInterval = enemyMoveInterval;
        stateCanvases = FindObjectOfType<StateCanvases>();
        getKid = FindObjectOfType<GetKid>();
        player = FindObjectOfType<Player>();
        enemyMovement = FindObjectOfType<EnemyMovement>();
        initialSeconds = (int)secondsRemaining;
        initialSpawnSeconds = secondsRemaining;

        if (PlayerPrefsManager.GetGameModePrefs().Contains("Endless"))
        {
            timeText.color = new Color(1, 1, 1, 0);
            secondsRemaining = float.MaxValue;
        }
        else
        {
            DisplayTime(secondsRemaining);
        }
    }

    void Update()
    {
        if (!timeStopped)
        {
            ModifySeconds();
        }
    }

    void ModifySeconds()
    {
        if (player.isPaused) { return; }

        if (secondsRemaining > 1)
        {
            secondsRemaining -= Time.deltaTime;

            if (enemyMoveInterval - Time.timeSinceLevelLoad <= 0.005f)
            {
                enemyMoveInterval += tempEnemyMoveInterval;
                if (!changeEnemy3Path)
                {
                    FindObjectOfType<Enemy3>().Enemy3PathX();
                    changeEnemy3Path = true;
                }
                else
                {
                    FindObjectOfType<Enemy3>().Enemy3PathY();
                    changeEnemy3Path = false;
                }
            }

            if (glowInterval - Time.timeSinceLevelLoad <= 0.005f)
            {
                glowInterval += tempGlowInterval;
                StartCoroutine(FindObjectOfType<LevelManager>().ShinyText(oldMaterial, newMaterial));
            }

            if (spawnInterval - Time.timeSinceLevelLoad <= 0.005f)
            {
                spawnInterval += tempSpawnInterval;
                getKid.SpawnKids();
            }
        }
        else
        {
            secondsRemaining = 0;
            timeStopped = true;
            stateCanvases.LoadLoseCanvas();
        }
        DisplayTime(secondsRemaining);
    }

    void DisplayTime(float secondsDisplayed)
    {
        if (secondsDisplayed < 0)
        {
            secondsDisplayed = 0;
        }
        float minutes = Mathf.FloorToInt(secondsRemaining / 60);
        float seconds = Mathf.FloorToInt(secondsRemaining % 60);

        timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public string TimeScore()
    {
        timeScore = initialSeconds - (int)secondsRemaining;
        float scoreMinutes = Mathf.RoundToInt(timeScore / 60);
        float scoreSeconds = Mathf.RoundToInt(timeScore % 60);
        timeScoreText = scoreMinutes.ToString("00") + ":" + scoreSeconds.ToString("00");

        return timeScoreText;
    }

    public bool GetTimeStopped()
    {
        return timeStopped;
    }
}
