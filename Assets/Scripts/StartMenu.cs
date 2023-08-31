using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StartMenu : MonoBehaviour
{
    [SerializeField] MenuButtonBorder[] menuButtonBorders;
    [SerializeField] Canvas screenCanvas;
    [SerializeField] float scaleModify = 2f;
    [SerializeField] Image animatedButton;
    [SerializeField] Canvas controlsCanvas;
    [SerializeField] Canvas optionsCanvas;
    [SerializeField] Canvas coverScreenCanvas;
    [SerializeField] float loadTime = 2f;
    [SerializeField] Material menuButtonSelectedMaterial;
    [SerializeField] Vector3 buttonsActualSize = new Vector3(0.7f, 0.7f, 0.7f);
    [SerializeField] Sprite focusedButtonSprite;
    [SerializeField] Material newMaterial;
    [SerializeField] Material oldMaterial;
    [SerializeField] Material oldButtonMaterial;
    MenuButtonBorder menuButtonBorder;
    MenuButton[] screenCanvasMenuButtons;
    MenuButton[] otherCanvasMenuButtons;
    Canvas otherMenuCanvas;
    Color32 originalColor;
    Color32 originalButtonColor;
    Material originalMaterial;
    MenuButton selectedMenuButton;
    MenuButton previousMenuButton;
    Vector3[] previousButtonPositions = new Vector3[4];
    static int transferValue;
    int alternatingInt;
    String currentMenuButtonName;
    MenuButton currentlySelectedButton;
    public bool switchingOptions;
    const int GLOWINTERVAL = 3;
    int glowIntervalTracker = 3;

    void Start()
    {
        if (PlayerPrefsManager.GetGameModePrefs().Length == 0)
        {
            PlayerPrefsManager.SetDefaultGameModePrefs();
            PlayerPrefsManager.SetDefaultSFXPrefs();
            PlayerPrefsManager.SetDefaultVolumePrefs();
        }

        screenCanvasMenuButtons = FindObjectsOfType<MenuButton>();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            screenCanvas.gameObject.SetActive(true);
            screenCanvasMenuButtons[0].GetComponent<Button>().Select();
            previousMenuButton = screenCanvasMenuButtons[0];
            ModifyMenuButtonColors();
            SetupMenuButtonBorders(menuButtonBorders, screenCanvas,
                                    screenCanvasMenuButtons, 1f, scaleModify);
        }
    }

    void Update()
    {
        if (otherMenuCanvas != null && otherMenuCanvas.name.Contains("Options") && FindObjectOfType<ExtraCanvasFunctions>().optionsScreenDisplayed)
        {
            GameObject.Find(PlayerPrefsManager.GetGameModePrefs()).GetComponent<TextMeshProUGUI>().color = new Color32(118, 14, 14, 255);
        }

        if (glowIntervalTracker - Time.timeSinceLevelLoad < 0.005f)
        {
            glowIntervalTracker += GLOWINTERVAL;
            StartCoroutine(FindObjectOfType<LevelManager>().ShinyText(oldMaterial, newMaterial));
        }
    }

    void OnSubmit()
    {
        if (EventSystem.current.currentSelectedGameObject == screenCanvasMenuButtons[0].gameObject)
        {
            StartCoroutine(SpiralTransition());
        }
        else if (EventSystem.current.currentSelectedGameObject == screenCanvasMenuButtons[1].gameObject)
        {
            FindObjectOfType<ExtraCanvasFunctions>().LoadCanvas(controlsCanvas, screenCanvasMenuButtons[1], menuButtonSelectedMaterial);
        }
        else if (EventSystem.current.currentSelectedGameObject == screenCanvasMenuButtons[2].gameObject)
        {
            FindObjectOfType<ExtraCanvasFunctions>().LoadCanvas(optionsCanvas, screenCanvasMenuButtons[2], menuButtonSelectedMaterial);
        }
        else if (EventSystem.current.currentSelectedGameObject == GameObject.Find("ControlsExitButton"))
        {
            otherCanvasMenuButtons = null;
            otherMenuCanvas = null;
            FindObjectOfType<ExtraCanvasFunctions>().CloseCanvas();
            screenCanvasMenuButtons[1].GetComponent<Button>().Select();
            previousMenuButton = screenCanvasMenuButtons[1];
            StartCoroutine(MovingButtonBorders());
        }
        else if (EventSystem.current.currentSelectedGameObject == GameObject.Find("OptionsExitButton"))
        {
            if (scaleModify == 0.8f)
            {
                scaleModify /= 4;
            }
            otherCanvasMenuButtons = null;
            otherMenuCanvas = null;
            FindObjectOfType<ExtraCanvasFunctions>().CloseCanvas();
            screenCanvasMenuButtons[2].GetComponent<Button>().Select();
            previousMenuButton = screenCanvasMenuButtons[2];
            StartCoroutine(MovingButtonBorders());
        }
        else if (EventSystem.current.currentSelectedGameObject == GameObject.Find("GameModeText"))
        {
            Debug.Log(GameObject.Find(PlayerPrefsManager.GetGameModePrefs()).name);
            EventSystem.current.currentSelectedGameObject.GetComponent<Animator>().SetBool("optionSelected", false);
            switchingOptions = true;
            GameObject.Find(PlayerPrefsManager.GetGameModePrefs()).GetComponent<Button>().Select();
            EventSystem.current.currentSelectedGameObject.GetComponent<Animator>().SetBool("optionSelected", true);
        }
        else if (EventSystem.current.currentSelectedGameObject == GameObject.Find("VolumeText"))
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<Animator>().SetBool("optionSelected", false);
            switchingOptions = true;
            FindObjectOfType<ExtraCanvasFunctions>().GetCanvasButtons()[2].Select();
            FindObjectOfType<ExtraCanvasFunctions>().GetCanvasButtons()[2].GetComponentsInChildren<Image>()[1].color =
                                                                           new Color(1, 1, 1, 1);
        }
        else if (EventSystem.current.currentSelectedGameObject == GameObject.Find("SFXText"))
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<Animator>().SetBool("optionSelected", false);
            switchingOptions = true;
            FindObjectOfType<ExtraCanvasFunctions>().GetCanvasButtons()[4].Select();
            FindObjectOfType<ExtraCanvasFunctions>().GetCanvasButtons()[4].GetComponentsInChildren<Image>()[1].color =
                                                                            new Color(1, 1, 1, 1);
        }
        else if (EventSystem.current.currentSelectedGameObject == GameObject.Find("GameModeNormal") ||
                    EventSystem.current.currentSelectedGameObject == GameObject.Find("GameModeEndless"))
        {
            PlayerPrefsManager.SetGameModePrefs(EventSystem.current.currentSelectedGameObject.name);
            EventSystem.current.currentSelectedGameObject.GetComponent<Animator>().SetBool("optionSelected", false);
            GameObject.Find("GameModeText").GetComponent<Button>().Select();
            EventSystem.current.currentSelectedGameObject.GetComponent<Animator>().SetBool("optionSelected", true);
        }
    }

    IEnumerator SpiralTransition()
    {
        coverScreenCanvas.GetComponent<Animator>().SetTrigger("StartGame");
        yield return new WaitForSeconds(loadTime);
        FindObjectOfType<LoadScene>().LoadNextScene();
    }

    void OnCancel()
    {
        foreach (Image child in EventSystem.current.currentSelectedGameObject.transform.parent.GetComponentsInChildren<Image>())
        {
            if (child.sprite == focusedButtonSprite)
            {
                child.color = new Color(1, 1, 1, 0);
            }
        }

        if (EventSystem.current.currentSelectedGameObject.name.Contains("SFX"))
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<Animator>().SetBool("optionSelected", false);
            GameObject.Find("SFXText").GetComponent<Button>().Select();
            EventSystem.current.currentSelectedGameObject.GetComponent<Animator>().SetBool("optionSelected", true);
            ModifyMenuButtonColors();
        }
        else if (EventSystem.current.currentSelectedGameObject.name.Contains("Volume"))
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<Animator>().SetBool("optionSelected", false);
            GameObject.Find("VolumeText").GetComponent<Button>().Select();
            EventSystem.current.currentSelectedGameObject.GetComponent<Animator>().SetBool("optionSelected", true);
            ModifyMenuButtonColors();
        }
        else if (EventSystem.current.currentSelectedGameObject.name.Contains("GameMode"))
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<Animator>().SetBool("optionSelected", false);
            GameObject.Find("GameModeText").GetComponent<Button>().Select();
            EventSystem.current.currentSelectedGameObject.GetComponent<Animator>().SetBool("optionSelected", true);
            ModifyMenuButtonColors();
        }
    }

    void OnNavigate()
    {
        alternatingInt++;

        if (alternatingInt == 2)
        {
            alternatingInt = 0;

            if (scaleModify == 0.8f)
            {
                scaleModify /= 4;
            }

            if (EventSystem.current.currentSelectedGameObject.name.Contains("Decrease"))
            {
                foreach (Image child in EventSystem.current.currentSelectedGameObject.transform.parent.GetComponentsInChildren<Image>())
                {
                    if (child.sprite == focusedButtonSprite)
                    {
                        child.color = new Color(1, 1, 1, 0);
                    }
                }
                EventSystem.current.currentSelectedGameObject.GetComponentsInChildren<Image>()[1].color = new Color(1, 1, 1, 1);
            }
            else if (EventSystem.current.currentSelectedGameObject.name.Contains("Increase"))
            {
                foreach (Image child in EventSystem.current.currentSelectedGameObject.transform.parent.GetComponentsInChildren<Image>())
                {
                    if (child.sprite == focusedButtonSprite)
                    {
                        child.color = new Color(1, 1, 1, 0);
                    }
                }
                EventSystem.current.currentSelectedGameObject.GetComponentsInChildren<Image>()[1].color = new Color(1, 1, 1, 1);
            }
            else if (EventSystem.current.currentSelectedGameObject.GetComponent<MenuButton>())
            {
                if (otherMenuCanvas != null)
                {
                    foreach (MenuButton child in FindObjectsOfType<MenuButton>())
                    {
                        if (child.GetComponent<TextMeshProUGUI>() &&
                            child.GetComponent<TextMeshProUGUI>().color == new Color(1, 1, 1, 1))
                        {
                            child.GetComponent<TextMeshProUGUI>().color = new Color32(118, 14, 14, 255);
                            child.GetComponent<TextMeshProUGUI>().fontMaterial = oldButtonMaterial;
                            child.GetComponent<Animator>().SetBool("optionSelected", false);
                        }
                    }
                }

                if (EventSystem.current.currentSelectedGameObject.name != currentMenuButtonName)
                {
                    if (currentMenuButtonName == null)
                    {
                        currentMenuButtonName = EventSystem.current.currentSelectedGameObject.name;
                        ModifyMenuButtonColors();
                        StartCoroutine(MovingButtonBorders());
                    }
                    else
                    {
                        currentMenuButtonName = EventSystem.current.currentSelectedGameObject.name;
                        ModifyMenuButtonColors();
                        if (EventSystem.current.currentSelectedGameObject.GetComponent<MenuButton>() && !otherMenuCanvas)
                        {
                            StartCoroutine(MovingButtonBorders());
                        }
                        else if (optionsCanvas)
                        {
                            if (EventSystem.current.currentSelectedGameObject.name.Contains("ExitButton"))
                            {
                                EventSystem.current.currentSelectedGameObject.GetComponent<Button>().Select();

                                if (scaleModify == 0.2f)
                                {
                                    scaleModify *= 4;
                                }

                                StartCoroutine(MovingButtonBorders());
                            }
                            else
                            {
                                foreach (Animator child in EventSystem.current.currentSelectedGameObject.transform.parent.parent.GetComponentsInChildren<Animator>())
                                {
                                    if (child.GetComponent<Animator>().runtimeAnimatorController.name == "OptionsBounce")
                                    {
                                        if (child.GetBool("optionSelected"))
                                        {
                                            child.GetComponent<Animator>().SetBool("optionSelected", false);
                                        }
                                        EventSystem.current.currentSelectedGameObject.GetComponent<Animator>().SetBool("optionSelected", true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                ModifyMenuButtonColors();
            }
        }

    }

    public MenuButtonBorder[] GetMenuButtonBorders()
    {
        return menuButtonBorders;
    }

    public Canvas GetControlsCanvas()
    {
        return controlsCanvas;
    }

    public Canvas GetOptionsCanvas()
    {
        return optionsCanvas;
    }

    void ModifyMenuButtonColors()
    {
        MenuButton[] coloredMenuButtons = null;
        Button[] coloredButtons = null;
        if (otherCanvasMenuButtons != null)
        {
            coloredMenuButtons = otherCanvasMenuButtons;
            if (FindObjectOfType<ExtraCanvasFunctions>().GetCanvasButtons().Length == 6)
            {
                coloredButtons = FindObjectOfType<ExtraCanvasFunctions>().GetCanvasButtons();
            }
        }
        else
        {
            coloredMenuButtons = screenCanvasMenuButtons;
        }

        if (coloredButtons != null)
        {
            if (originalButtonColor.a == 0)
            {
                originalButtonColor = coloredButtons[0].GetComponent<TextMeshProUGUI>().color;
            }

            if (coloredButtons[0].gameObject == EventSystem.current.currentSelectedGameObject)
            {
                PlayerPrefsManager.SetGameModePrefs(EventSystem.current.currentSelectedGameObject.name);
                coloredButtons[0].GetComponent<Animator>().SetBool("optionSelected", true);
                coloredButtons[1].GetComponent<TextMeshProUGUI>().color = new Color32(202, 97, 0, 255);
                coloredButtons[1].GetComponent<Animator>().SetBool("optionSelected", false);
            }
            else if (coloredButtons[1].gameObject == EventSystem.current.currentSelectedGameObject)
            {
                PlayerPrefsManager.SetGameModePrefs(EventSystem.current.currentSelectedGameObject.name);
                coloredButtons[1].GetComponent<Animator>().SetBool("optionSelected", true);
                coloredButtons[0].GetComponent<TextMeshProUGUI>().color = new Color32(202, 97, 0, 255);
                coloredButtons[0].GetComponent<Animator>().SetBool("optionSelected", false);
            }
        }

        for (int i = 0; i < coloredMenuButtons.Length; i++)
        {
            if (coloredMenuButtons[i].gameObject == EventSystem.current.currentSelectedGameObject)
            {
                if (selectedMenuButton && otherCanvasMenuButtons == null)
                {
                    previousMenuButton = selectedMenuButton;
                }

                selectedMenuButton = coloredMenuButtons[i];

                originalColor = coloredMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().color;
                originalMaterial = coloredMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().fontMaterial;
                coloredMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
                coloredMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().fontMaterial = menuButtonSelectedMaterial;
            }
            else
            {
                if (coloredMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().color == new Color(1, 1, 1, 1)
                && !coloredMenuButtons[i].name.Contains("GameMode"))
                {
                    coloredMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().color = originalColor;
                    coloredMenuButtons[i].GetComponentInChildren<TextMeshProUGUI>().fontMaterial = originalMaterial;
                }
            }
        }
    }


    public IEnumerator MovingButtonBorders()
    {
        SetupMovingMenuButtonBorders();

        yield return new WaitForEndOfFrame();

        foreach (Button child in screenCanvas.GetComponentsInChildren<Button>())
        {
            child.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        if (otherCanvasMenuButtons != null)
        {
            SetupMenuButtonBorders(menuButtonBorders, otherMenuCanvas,
                                otherCanvasMenuButtons, 200f, scaleModify);
        }
        else
        {
            SetupMenuButtonBorders(menuButtonBorders, screenCanvas,
                                    screenCanvasMenuButtons, 1f, scaleModify);
        }
    }

    public void SetupMenuButtonBorders(MenuButtonBorder[] newMenuButtonBorders,
                                        Canvas selectedCanvas, MenuButton[] selectedButtons,
                                        float sizeMultiplier, float scaleModifier)
    {
        if (selectedCanvas != screenCanvas)
        {
            otherCanvasMenuButtons = selectedButtons;
            otherMenuCanvas = selectedCanvas;
            for (int h = 0; h < selectedButtons.Length; h++)
            {
                if (selectedButtons[h].gameObject == EventSystem.current.currentSelectedGameObject)
                {
                    currentlySelectedButton = selectedButtons[h];
                }
            }
        }
        else
        {
            for (int h = 0; h < screenCanvasMenuButtons.Length; h++)
            {
                if (screenCanvasMenuButtons[h].gameObject == EventSystem.current.currentSelectedGameObject)
                {
                    currentlySelectedButton = screenCanvasMenuButtons[h];
                }
            }
        }

        for (int i = 0; i < newMenuButtonBorders.Length; i++)
        {
            Vector3[] selectedButtonCorners = currentlySelectedButton.GetMenuButtonCorners();

            menuButtonBorder = Instantiate(newMenuButtonBorders[i],
                                            selectedButtonCorners[i],
                                            transform.rotation);
            menuButtonBorder.transform.SetParent(selectedCanvas.transform);
            menuButtonBorder.transform.localScale = buttonsActualSize;

            if (selectedCanvas.name == controlsCanvas.name + "(Clone)" ||
                (selectedCanvas.name == optionsCanvas.name + "(Clone)" &&
                !EventSystem.current.currentSelectedGameObject.name.Contains("OptionsExit")))
            {
                menuButtonBorder.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }

            StartCoroutine(menuButtonBorder.ButtonBorderIdle(currentlySelectedButton.gameObject,
                                selectedCanvas, scaleModifier, sizeMultiplier));
        }
    }

    void SetupMovingMenuButtonBorders()
    {
        MenuButton selectedCanvasButton = null;
        Canvas selectedMovingCanvas = null;
        if (otherMenuCanvas != null)
        {
            selectedMovingCanvas = otherMenuCanvas;
            for (int h = 0; h < otherCanvasMenuButtons.Length; h++)
            {
                if (otherCanvasMenuButtons[h].gameObject == EventSystem.current.currentSelectedGameObject)
                {
                    selectedCanvasButton = otherCanvasMenuButtons[h];
                }
            }
        }
        else
        {
            selectedMovingCanvas = screenCanvas;
            for (int h = 0; h < screenCanvasMenuButtons.Length; h++)
            {
                if (screenCanvasMenuButtons[h].gameObject == EventSystem.current.currentSelectedGameObject)
                {
                    selectedCanvasButton = screenCanvasMenuButtons[h];
                }
            }
        }

        for (int i = 0; i < menuButtonBorders.Length; i++)
        {
            Vector3[] selectedButtonCorners = selectedCanvasButton.GetMenuButtonCorners();

            menuButtonBorder = Instantiate(menuButtonBorders[i],
                                                 selectedButtonCorners[i],
                                                 transform.rotation);
            menuButtonBorder.transform.SetParent(selectedMovingCanvas.transform);
            menuButtonBorder.transform.localScale = buttonsActualSize;

            Image movingButton = Instantiate(animatedButton, previousMenuButton.transform.position, transform.rotation);
            movingButton.transform.SetParent(selectedMovingCanvas.transform);
            movingButton.transform.localScale = selectedMovingCanvas.GetComponentInChildren<MenuButton>().transform.localScale;

            if (selectedMovingCanvas != screenCanvas)
            {
                foreach (Image child in FindObjectsOfType<Image>())
                {
                    if (child.name.Contains("buttonUp") || child.name.Contains("buttonDown"))
                    {
                        child.color = new Color(0, 0, 0, 0);
                    }
                }
            }

            foreach (MenuButton child in selectedMovingCanvas.GetComponentsInChildren<MenuButton>())
            {
                if (child.GetComponent<Image>() != null)
                {
                    child.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                }
            }

            StartCoroutine(menuButtonBorder.ButtonBorderMove(previousButtonPositions[i],
                               selectedMenuButton, selectedButtonCorners[i], movingButton));
        }
    }

    public void GetPreviousButtonPosition(Vector3 deletedButtonPosition)
    {
        if (transferValue == 3)
        {
            previousButtonPositions[transferValue] = deletedButtonPosition;
            transferValue = 0;
        }
        else if (transferValue == 2)
        {
            previousButtonPositions[transferValue] = deletedButtonPosition;
            transferValue++;
        }
        else if (transferValue == 1)
        {
            previousButtonPositions[transferValue] = deletedButtonPosition;
            transferValue++;
        }
        else if (transferValue == 0)
        {
            previousButtonPositions[transferValue] = deletedButtonPosition;
            transferValue++;
        }
    }
}
