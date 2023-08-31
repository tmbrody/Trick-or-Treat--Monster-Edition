using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectedMaterial : MonoBehaviour
{
    [SerializeField] Vector2 animatedOffset;
    Vector2 offset;

    void Update()
    {
        offset -= Time.deltaTime * animatedOffset;
        GetComponent<TextMeshProUGUI>().fontMaterial.SetTextureOffset(ShaderUtilities.ID_FaceTex, offset);
    }

    void OnDestroy()
    {
        GetComponent<TextMeshProUGUI>().fontMaterial.SetTextureOffset(ShaderUtilities.ID_FaceTex, new Vector2(0, 0));
    }
}
