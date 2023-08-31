using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Tilemaps;

public class KidScore : MonoBehaviour
{
    [SerializeField] float goalKids = 10;
    [SerializeField] public TextMeshProUGUI goalText;
    [SerializeField] TextMeshProUGUI topKidText;
    [SerializeField] TextMeshProUGUI onesScore;
    [SerializeField] TextMeshProUGUI aboveOnesScoreText;
    [SerializeField] TextMeshProUGUI removeOnesScoreText;
    [SerializeField] TextMeshProUGUI tensScore;
    [SerializeField] TextMeshProUGUI aboveTensScoreText;
    [SerializeField] TextMeshProUGUI removeTensScoreText;
    [SerializeField] Animator realScoreAnim;
    Tilemap[] tilemaps;
    Tilemap gateTilemap;
    public int currentKids = 0;
    public bool gateOpen;
    Popups popups;

    void Awake()
    {
        goalText.text = goalKids + " Kids";

        onesScore.text = currentKids.ToString();
        aboveOnesScoreText.text = (currentKids % 10).ToString();
        removeOnesScoreText.text = (currentKids % 10).ToString();

        tensScore.text = (currentKids / 10).ToString();
        aboveTensScoreText.text = (currentKids / 10).ToString();
        removeTensScoreText.text = (currentKids / 10).ToString();

        popups = FindObjectOfType<Popups>();

        tilemaps = FindObjectsOfType<Tilemap>();
        for (int i = 0; i < tilemaps.Length; i++)
        {
            if (tilemaps[i].gameObject.layer == 8)
            {
                gateTilemap = tilemaps[i];
            }
        }

        if (PlayerPrefsManager.GetGameModePrefs().Contains("Endless"))
        {
            goalKids = float.MaxValue;
            goalText.text = "Infinite";
            gateTilemap.gameObject.SetActive(false);
        }
    }

    public void IncreaseKidScore()
    {
        if ((currentKids + 1) % 10 != 0)
        {
            KidScoreIncrease();
        }
        else
        {
            TensScoreIncrease();
        }

        popups.PositivePopupLoop();

        if (currentKids >= goalKids)
        {
            foreach (TextMeshProUGUI child in GetComponentsInChildren<TextMeshProUGUI>())
            {
                child.color = new Color(0.7924528f, 0.6980392f, 0, 1);
            }
            gateTilemap.gameObject.SetActive(false);
            gateOpen = true;
        }
    }

    void KidScoreIncrease()
    {
        removeOnesScoreText.text = (currentKids % 10).ToString();
        realScoreAnim.SetTrigger("GotKid");
        currentKids++;
        onesScore.text = (currentKids % 10).ToString();
        aboveOnesScoreText.text = (currentKids % 10).ToString();
    }

    void TensScoreIncrease()
    {
        removeTensScoreText.text = (currentKids / 10).ToString();
        removeOnesScoreText.text = (currentKids % 10).ToString();
        realScoreAnim.SetTrigger("GotTenKids");
        currentKids++;
        tensScore.text = (currentKids / 10).ToString();
        aboveTensScoreText.text = (currentKids / 10).ToString();
        onesScore.text = (currentKids % 10).ToString();
        aboveOnesScoreText.text = (currentKids % 10).ToString();
    }

    public void DecreaseKidScore()
    {
        if (currentKids <= 0) { return; }

        if ((currentKids - 1) % 10 != 9)
        {
            KidScoreDecrease();
        }
        else
        {
            TensScoreDecrease();
        }

        popups.NegativePopupLoop();

        if (currentKids < goalKids)
        {
            foreach (TextMeshProUGUI child in GetComponentsInChildren<TextMeshProUGUI>())
            {
                child.color = new Color(0.7924528f, 0.3799767f, 0, 1);
            }
            gateTilemap.gameObject.SetActive(true);
            gateOpen = false;
        }
    }

    void KidScoreDecrease()
    {
        removeOnesScoreText.text = (currentKids % 10).ToString();
        realScoreAnim.SetTrigger("LostKid");
        currentKids--;
        onesScore.text = (currentKids % 10).ToString();
        aboveOnesScoreText.text = (currentKids % 10).ToString();
    }

    void TensScoreDecrease()
    {
        removeTensScoreText.text = (currentKids / 10).ToString();
        removeOnesScoreText.text = (currentKids % 10).ToString();
        realScoreAnim.SetTrigger("LostTenKids");
        currentKids--;
        tensScore.text = (currentKids / 10).ToString();
        aboveTensScoreText.text = (currentKids / 10).ToString();
        onesScore.text = (currentKids % 10).ToString();
        aboveOnesScoreText.text = (currentKids % 10).ToString();
    }
}
