using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExtraCanvasFunctions : MonoBehaviour
{
    [SerializeField] float sizeMultiplier = 5f;
    [SerializeField] float scaleModify = 2f;
    Canvas referencedCanvas;
    Button[] canvasButtons = new Button[6];
    MenuButton referencedButton;
    Navigation referencedNavigation;
    Navigation originalControlNavigation;
    Navigation newControlNavigation;
    int canvasButtonsIndex;
    Material originalMaterial;
    Color originalColor;
    public bool optionsScreenDisplayed;

    void Update()
    {
        if (PlayerPrefsManager.GetVolumePrefs() % 0.05f != 0)
        {
            PlayerPrefsManager.SetVolumePrefs(Mathf.Round(PlayerPrefsManager.GetVolumePrefs() * 100) / 100);
        }

        if (PlayerPrefsManager.GetSFXPrefs() % 0.05f != 0)
        {
            PlayerPrefsManager.SetSFXPrefs(Mathf.Round(PlayerPrefsManager.GetSFXPrefs() * 100) / 100);
        }
    }

    public Button[] GetCanvasButtons()
    {
        return canvasButtons;
    }

    public void LoadCanvas(Canvas selectedCanvas, MenuButton selectedButton, Material selectedMaterial)
    {
        Canvas newSelectedCanvas = Instantiate(selectedCanvas, transform.position, transform.rotation);
        MenuButton[] newSelectedButtons = newSelectedCanvas.GetComponentsInChildren<MenuButton>();
        newSelectedButtons[0].GetComponent<Button>().Select();
        newSelectedButtons[0].GetComponentInChildren<TextMeshProUGUI>().fontMaterial = selectedMaterial;
        if (newSelectedButtons[0].GetComponent<Animator>())
        {
            newSelectedButtons[0].GetComponent<Animator>().SetBool("optionSelected", true);
        }

        if (originalMaterial == null)
        {
            originalMaterial = newSelectedButtons[0].GetComponentInChildren<TextMeshProUGUI>().fontMaterial;
            originalColor = newSelectedButtons[0].GetComponentInChildren<TextMeshProUGUI>().color;
        }

        if (GameObject.Find("SFXAmount") && GameObject.Find("VolumeAmount") &&
                    GameObject.Find(PlayerPrefsManager.GetGameModePrefs()))
        {
            GameObject tempSFX = GameObject.Find("SFXAmount");
            tempSFX.GetComponentInChildren<TextMeshProUGUI>().text = (PlayerPrefsManager.GetSFXPrefs() * 100) + "%";
            GameObject tempVolume = GameObject.Find("VolumeAmount");
            tempVolume.GetComponentInChildren<TextMeshProUGUI>().text = (PlayerPrefsManager.GetVolumePrefs() * 100) + "%";
            GameObject tempGameMode = GameObject.Find(PlayerPrefsManager.GetGameModePrefs());
            tempGameMode.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(118, 14, 14, 255);
        }

        if (newSelectedCanvas.GetComponentsInChildren<Button>() != null)
        {
            canvasButtonsIndex = 0;
            foreach (Button child in newSelectedCanvas.GetComponentsInChildren<Button>())
            {
                if (!child.GetComponent<MenuButton>())
                {
                    canvasButtons[canvasButtonsIndex] = child;
                    canvasButtonsIndex++;
                }
            }
        }

        foreach (TextMeshProUGUI child in newSelectedCanvas.GetComponentsInChildren<TextMeshProUGUI>())
        {
            foreach (Image childImage in newSelectedCanvas.GetComponentsInChildren<Image>())
            {
                child.color = new Color32(0, 0, 0, 0);
                childImage.color = new Color32(0, 0, 0, 0);
            }
        }

        originalControlNavigation.mode = selectedButton.GetComponent<Button>().navigation.mode;
        originalControlNavigation.selectOnUp = selectedButton.GetComponent<Button>().navigation.selectOnUp;
        originalControlNavigation.selectOnDown = selectedButton.GetComponent<Button>().navigation.selectOnDown;
        newControlNavigation.mode = Navigation.Mode.None;
        selectedButton.GetComponent<Button>().navigation = newControlNavigation;

        gameObject.GetComponent<Animator>().SetTrigger("ExtrasEnter");
        StartCoroutine(LoadingCanvas(newSelectedCanvas));

        FindObjectOfType<StartMenu>().SetupMenuButtonBorders(FindObjectOfType<StartMenu>().GetMenuButtonBorders(),
            newSelectedCanvas, newSelectedButtons, sizeMultiplier, scaleModify);

        referencedCanvas = newSelectedCanvas;
        referencedButton = selectedButton;
        referencedNavigation = originalControlNavigation;
    }

    public void CloseCanvas()
    {
        referencedButton.GetComponent<Button>().navigation = referencedNavigation;
        Destroy(referencedCanvas.gameObject);
        GetComponent<Animator>().SetTrigger("ExtrasExit");
    }


    IEnumerator LoadingCanvas(Canvas otherCanvas)
    {
        yield return new WaitForSeconds(0.5f);

        if (otherCanvas.name != FindObjectOfType<StartMenu>().GetOptionsCanvas().name + "(Clone)")
        {
            ControlsCanvasLerp(otherCanvas);
        }
        else
        {
            OptionsCanvasLerp(otherCanvas);
        }
    }

    void ControlsCanvasLerp(Canvas otherCanvas)
    {
        TextMeshProUGUI[] controlsTexts = otherCanvas.GetComponentsInChildren<TextMeshProUGUI>();
        Image[] controlsImages = otherCanvas.GetComponentsInChildren<Image>();

        Color32 textFullColor = new Color32(202, 97, 0, 255);
        Color32 imageFullColor = new Color32(255, 255, 255, 255);
        Color32 buttonFullColor = new Color32(118, 14, 14, 255);

        foreach (TextMeshProUGUI child in controlsTexts)
        {
            foreach (Image childImage in controlsImages)
            {
                if (child.text != "Exit")
                {
                    child.color = textFullColor;
                }
                else
                {
                    child.color = imageFullColor;
                }

                childImage.color = imageFullColor;

            }
        }
    }

    void OptionsCanvasLerp(Canvas otherCanvas)
    {
        TextMeshProUGUI[] controlsTexts = otherCanvas.GetComponentsInChildren<TextMeshProUGUI>();
        Image[] controlsImages = otherCanvas.GetComponentsInChildren<Image>();

        Color32 textFullColor = new Color32(202, 97, 0, 255);
        Color32 imageFullColor = new Color32(255, 255, 255, 255);
        Color32 buttonFullColor = new Color32(118, 14, 14, 255);
        Color32 volumeBarFullColor = new Color32(55, 47, 76, 255);

        foreach (TextMeshProUGUI child in controlsTexts)
        {
            foreach (Image childImage in controlsImages)
            {
                child.color = textFullColor;

                if (childImage.name.Contains("Amount"))
                {
                    childImage.color = volumeBarFullColor;
                }
                else if (childImage.name.Contains("volumeButton"))
                {
                    childImage.color = new Color(0, 0, 0, 0);
                }
                else if (childImage.name.Contains("buttonUp") ||
                        childImage.name.Contains("buttonDown"))
                {
                    childImage.color = new Color(0, 0, 0, 0);
                }
                else
                {
                    childImage.color = imageFullColor;
                }

                if (child.GetComponent<MenuButton>() || child.name == "Exit Text")
                {
                    child.color = buttonFullColor;
                }

                if (child.name == "GameModeText")
                {
                    child.color = imageFullColor;
                }
            }
        }
        optionsScreenDisplayed = true;
    }

    public void DirectionalButtonPressed()
    {
        if (FindObjectOfType<StartMenu>().switchingOptions)
        {
            FindObjectOfType<StartMenu>().switchingOptions = false;
            return;
        }

        GameObject pressedButton = EventSystem.current.currentSelectedGameObject;
        pressedButton.GetComponent<Animator>().SetTrigger("PressedVolumeButton");
        GameObject buttonRow = pressedButton.transform.parent.gameObject;
        if (pressedButton.name.Contains("SFX"))
        {
            if (pressedButton.name.Contains("Decrease"))
            {
                PlayerPrefsManager.SetSFXPrefs(PlayerPrefsManager.GetSFXPrefs() - 0.05f);
            }
            else if (pressedButton.name.Contains("Increase"))
            {
                PlayerPrefsManager.SetSFXPrefs(PlayerPrefsManager.GetSFXPrefs() + 0.05f);
            }
            buttonRow.GetComponentsInChildren<TextMeshProUGUI>()[1].text =
                    Mathf.Round((PlayerPrefsManager.GetSFXPrefs() * 100)) + "%";
        }
        else if (pressedButton.name.Contains("Volume"))
        {
            if (pressedButton.name.Contains("Decrease"))
            {
                PlayerPrefsManager.SetVolumePrefs(PlayerPrefsManager.GetVolumePrefs() - 0.05f);
            }
            else if (pressedButton.name.Contains("Increase"))
            {
                PlayerPrefsManager.SetVolumePrefs(PlayerPrefsManager.GetVolumePrefs() + 0.05f);
            }
            buttonRow.GetComponentsInChildren<TextMeshProUGUI>()[1].text =
                    Mathf.Round((PlayerPrefsManager.GetVolumePrefs() * 100)) + "%";
        }
    }
}
