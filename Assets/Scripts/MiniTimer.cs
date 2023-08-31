using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI miniTimer;
    [SerializeField] float timeDuration = 2f;
    void Update()
    {
        timeDuration -= Time.deltaTime;
        miniTimer.text = timeDuration.ToString("0.00");
    }
}
