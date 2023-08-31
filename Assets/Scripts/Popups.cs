using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Popups : MonoBehaviour
{
    [SerializeField] GameObject positivePopupText;
    [SerializeField] GameObject negativePopupText;

    public void PositivePopupLoop()
    {
        GameObject popup = Instantiate(positivePopupText, transform.position, Quaternion.identity);
        Destroy(popup, 1f);
    }

    public void NegativePopupLoop()
    {
        GameObject popup = Instantiate(negativePopupText, transform.position, Quaternion.identity);
        Destroy(popup, 1f);
    }
}
