using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] GameObject[] menuButtonCornerObjects;
    Vector3[] menuRealButtonCorners = new Vector3[4];

    public Vector3[] GetMenuButtonCorners()
    {
        for (int i = 0; i < menuRealButtonCorners.Length; i++)
        {
            menuRealButtonCorners[i] = menuButtonCornerObjects[i].transform.position;
        }
        return menuRealButtonCorners;
    }
}
