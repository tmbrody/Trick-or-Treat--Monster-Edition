using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Warnings : MonoBehaviour
{
    [SerializeField] Tilemap kidTilemap;
    [SerializeField] Vector2 screeningSize;
    Vector2 maxDistance;
    Tilemap selectedKid;
    SpriteRenderer warningSprite;
    float screenHalfWidth;
    float screenHalfHeight;
    float screenMaxWidth;
    float screenMaxHeight;
    float XDistanceFromKid;
    float YDistanceFromKid;
    float warningPositionWidth;
    float warningPositionHeight;
    Color32 visibleSpriteColor;
    Color32 invisibleSpriteColor;
    Player playerPosition;
    BoxCollider2D kidOffsetPosition;

    void Awake()
    {
        warningSprite = gameObject.GetComponent<SpriteRenderer>();
        playerPosition = FindObjectOfType<Player>();
        kidOffsetPosition = kidTilemap.GetComponent<BoxCollider2D>();
        visibleSpriteColor = new Color32(255, 255, 255, 255);
        invisibleSpriteColor = new Color32(255, 255, 255, 0);
        warningSprite.color = invisibleSpriteColor;

        screenHalfHeight = (FindObjectOfType<CinemachineVirtualCamera>().m_Lens.OrthographicSize);

        screenHalfWidth = FindObjectOfType<CanvasScaler>().referenceResolution.x / FindObjectOfType<CanvasScaler>().referenceResolution.y;
        screenHalfWidth *= screenHalfHeight;
        screenHalfHeight -= 1f;
        screenHalfWidth -= 1f;
    }


    void Update()
    {
        if (FindObjectOfType<Timer>() && FindObjectOfType<Timer>().GetTimeStopped()) { return; }
        screenMaxHeight = screenHalfHeight + playerPosition.transform.position.y;

        screenMaxWidth = screenHalfWidth + playerPosition.transform.position.x;

        maxDistance = (Vector2)transform.position + screeningSize;

        XDistanceFromKid = Mathf.Abs(kidOffsetPosition.offset.x - playerPosition.transform.position.x);
        YDistanceFromKid = Mathf.Abs(kidOffsetPosition.offset.y - playerPosition.transform.position.y);

        if (XDistanceFromKid > screenHalfWidth)
        {
            warningPositionWidth = screenMaxWidth;
        }
        if (YDistanceFromKid > screenHalfHeight)
        {
            warningPositionHeight = screenMaxHeight;
        }

        if (playerPosition.transform.position.x > kidOffsetPosition.offset.x)
        {
            warningPositionWidth -= (2 * screenHalfWidth);
        }
        if (playerPosition.transform.position.y > kidOffsetPosition.offset.y)
        {
            warningPositionHeight -= (2 * screenHalfHeight);
        }

        transform.position = new Vector3(warningPositionWidth, warningPositionHeight, 90);

        if (kidTilemap.isActiveAndEnabled)
        {
            if (XDistanceFromKid < screenMaxWidth && YDistanceFromKid < screenMaxHeight)
            {
                warningSprite.color = invisibleSpriteColor;
            }
            else
            {
                warningSprite.color = visibleSpriteColor;
            }
        }
        else
        {
            warningSprite.color = invisibleSpriteColor;
        }
    }
}
