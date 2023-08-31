using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] AudioSource musicAudio;
    [SerializeField] AudioSource walkingAudio;
    [SerializeField] AudioSource getKidAudio;
    [SerializeField] Canvas loadingCanvas;
    [SerializeField] Canvas rightBeforeStartCanvas;
    [SerializeField] float loadTime = 2f;
    static bool introFinished;
    public bool startAudio;

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && !introFinished)
        {
            StartCoroutine(StartLevel());
        }
        else if (SceneManager.GetActiveScene().buildIndex != 0 && introFinished)
        {
            startAudio = true;
        }
    }

    public AudioSource GetMusicAudio()
    {
        return musicAudio;
    }

    public AudioSource GetWalkingAudio()
    {
        return walkingAudio;
    }

    public AudioSource GetGetKidAudio()
    {
        return getKidAudio;
    }

    public Canvas GetLoadingCanvas()
    {
        return loadingCanvas;
    }

    public Canvas GetRightBeforeStartCanvas()
    {
        return rightBeforeStartCanvas;
    }

    IEnumerator StartLevel()
    {
        FindObjectOfType<Timer>().timeStopped = true;
        loadingCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(loadTime);
        loadingCanvas.GetComponent<Animator>().SetTrigger("StartLevel");
        yield return new WaitForSeconds(2f);
        loadingCanvas.gameObject.SetActive(false);
        startAudio = true;
        rightBeforeStartCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.67f);
        rightBeforeStartCanvas.gameObject.SetActive(false);
        FindObjectOfType<Timer>().timeStopped = false;
        introFinished = true;
    }

    public IEnumerator ShinyText(Material oldMaterial, Material newMaterial)
    {
        float time = 0;
        float duration = 0.5f;
        Vector2 beginningGoalShine = new Vector2(0, 0);
        Vector2 endGoalShine = new Vector2(6.28f, 0);

        foreach (TextMeshProUGUI child in GameObject.FindObjectsOfType<TextMeshProUGUI>())
        {
            if (child.name.Contains("Goal") || child.name.Contains("Information"))
            {
                child.fontMaterial = newMaterial;
            }
        }

        while (time < duration)
        {
            Vector2 goalShine = Vector2.Lerp(beginningGoalShine, endGoalShine,
                                                                (time / duration));
            foreach (TextMeshProUGUI child in GameObject.FindObjectsOfType<TextMeshProUGUI>())
            {
                if (child.name.Contains("Goal") || child.name.Contains("Information"))
                {
                    child.fontMaterial.SetFloat(ShaderUtilities.ID_LightAngle, goalShine.x);
                }
            }
            time += Time.deltaTime;
            yield return null;
        }

        foreach (TextMeshProUGUI child in GameObject.FindObjectsOfType<TextMeshProUGUI>())
        {
            if (child.name.Contains("Goal") || child.name.Contains("Information"))
            {
                child.fontMaterial.SetFloat(ShaderUtilities.ID_LightAngle, endGoalShine.x);
                child.fontMaterial = oldMaterial;
            }
        }
    }

    public void End()
    {
        FindObjectOfType<Timer>().timeStopped = true;
        FindObjectOfType<Canvas>().gameObject.SetActive(false);
        FindObjectOfType<Player>().GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 3f);
        StartCoroutine(FindObjectOfType<StateCanvases>().WinScreen());
    }
}