using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlickeryMaterial : MonoBehaviour
{
    [SerializeField] Material newMaterial;
    [SerializeField] float loadingTimeInterval = 0.4f;
    Material originalMaterial;
    float loadingTime;
    void Start()
    {
        originalMaterial = GetComponent<TextMeshProUGUI>().fontMaterial;
        loadingTime = loadingTimeInterval;
    }

    void Update()
    {
        if (loadingTime - Time.timeSinceLevelLoad < 0.005f)
        {
            loadingTime += loadingTimeInterval;
            int randomInt = Random.Range(0, 2);
            if (randomInt == 1)
            {
                GetComponent<TextMeshProUGUI>().fontMaterial = originalMaterial;
            }
            else
            {
                GetComponent<TextMeshProUGUI>().fontMaterial = newMaterial;
            }
        }
    }
}
