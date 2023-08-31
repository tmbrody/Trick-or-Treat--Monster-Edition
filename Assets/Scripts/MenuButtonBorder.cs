using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonBorder : MonoBehaviour
{
    [SerializeField] float verticalMovement = 0.1f;
    [SerializeField] float horizontalMovement = 0.1f;
    static int onlyOneAnimatedButton;

    public IEnumerator ButtonBorderIdle(GameObject menuButton, Canvas verifyCanvas,
                                            float scaleModify, float sizeMultiplier)
    {
        float time = 0;
        float duration = 0.11f;
        int numberOfPhases = 4;

        if (verifyCanvas.name != FindObjectOfType<StartMenu>().GetControlsCanvas().name + "(Clone)" &&
            menuButton.GetComponent<Image>() != null)
        {
            menuButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }

        while (menuButton == EventSystem.current.currentSelectedGameObject)
        {
            for (int i = 1; i <= numberOfPhases; i++)
            {
                Vector2 topButtonBorderPosition = new Vector2(0, 0);
                Vector2 bottomButtonBorderPosition = new Vector2(0, 0);
                Vector2 buttonMaxScale = new Vector2(0, 0);
                Vector2 buttonMinScale = new Vector2(0, 0);

                if (menuButton != null)
                {
                    topButtonBorderPosition = new Vector2(transform.position.x + (horizontalMovement * sizeMultiplier),
                        transform.position.y + (verticalMovement * sizeMultiplier));
                    bottomButtonBorderPosition = new Vector2(transform.position.x - (horizontalMovement * sizeMultiplier),
                       transform.position.y - (verticalMovement * sizeMultiplier));

                    buttonMaxScale = new Vector2(menuButton.transform.localScale.x + scaleModify,
                       menuButton.transform.localScale.y + scaleModify);
                    buttonMinScale = new Vector2(menuButton.transform.localScale.x - scaleModify,
                       menuButton.transform.localScale.y - scaleModify);
                }

                time = 0;


                if (i == 1 || i == 4)
                {
                    while (time < duration && menuButton != null)
                    {
                        menuButton.transform.localScale =
                        Vector2.Lerp(menuButton.transform.localScale, buttonMaxScale, time / duration);
                        transform.position = Vector2.Lerp(transform.position,
                        topButtonBorderPosition, time / duration);
                        time += Time.deltaTime;

                        if (EventSystem.current.currentSelectedGameObject != menuButton)
                        {
                            time += 10;
                        }
                        else
                        {
                            yield return null;
                        }
                    }

                    if (menuButton != null)
                    {
                        menuButton.transform.localScale = buttonMaxScale;
                        transform.position = topButtonBorderPosition;
                    }
                }

                if (i == 2 || i == 3)
                {
                    while (time < duration && menuButton != null)
                    {
                        menuButton.transform.localScale =
                        Vector2.Lerp(menuButton.transform.localScale, buttonMinScale, time / duration);
                        transform.position = Vector2.Lerp(transform.position,
                        bottomButtonBorderPosition, time / duration);
                        time += Time.deltaTime;

                        if (EventSystem.current.currentSelectedGameObject != menuButton)
                        {
                            time += 10;
                        }
                        else
                        {
                            yield return null;
                        }
                    }

                    if (menuButton != null)
                    {
                        menuButton.transform.localScale = buttonMinScale;
                        transform.position = bottomButtonBorderPosition;
                    }
                }
            }
        }

        if (menuButton != null)
        {
            FindObjectOfType<StartMenu>().GetPreviousButtonPosition(transform.position);
            Destroy(gameObject);
        }
    }

    public IEnumerator ButtonBorderMove(Vector2 previousButtonPosition,
        MenuButton newMenuButton, Vector2 newMenuButtonPosition, Image animatedButton)
    {
        if (newMenuButton != null && newMenuButton.GetComponent<Image>())
        {
            newMenuButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }

        if (onlyOneAnimatedButton >= 1)
        {
            Destroy(animatedButton.gameObject);
        }

        onlyOneAnimatedButton++;

        transform.position = previousButtonPosition;

        float time = 0;
        float duration = 0.28f;

        Vector3 newButtonBorderPosition = new Vector3(transform.position.x,
                                                        newMenuButtonPosition.y, 0);

        if (onlyOneAnimatedButton == 1)
        {
            while (time < duration && newMenuButton != null)
            {
                animatedButton.transform.position = Vector3.Lerp(animatedButton.transform.position,
                newMenuButton.transform.position, time / duration);

                transform.position = Vector3.Lerp(transform.position,
                newButtonBorderPosition, time / duration);

                time += Time.deltaTime;

                yield return null;
            }
        }
        else
        {
            while (time < duration)
            {
                transform.position = Vector3.Lerp(transform.position,
                newButtonBorderPosition, time / duration);

                time += Time.deltaTime;

                yield return null;
            }
        }

        if (animatedButton && newMenuButton != null)
        {
            animatedButton.transform.position = newMenuButton.transform.position;
            Destroy(animatedButton.gameObject);
        }

        transform.position = newButtonBorderPosition;

        if (onlyOneAnimatedButton == 4)
        {
            onlyOneAnimatedButton = 0;
        }

        Destroy(gameObject);
    }
}