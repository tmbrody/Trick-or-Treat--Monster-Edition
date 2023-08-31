using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StateCanvases : MonoBehaviour
{
    [SerializeField] Canvas winCanvas;
    [SerializeField] Canvas loseCanvas;
    [SerializeField] Canvas pauseCanvas;
    [SerializeField] float loadTime = 5f;
    [SerializeField] float randomTime = 0.3f;
    [SerializeField] GameObject kidsFinalScoreholder;
    [SerializeField] GameObject timeFinalScoreholder;
    KidScore kidScore;
    Timer timer;
    LoadScene loadScene;
    Player player;
    GameObject pauseObject;
    GameObject loseObject;
    GameObject winObject2;
    GameObject winObject3;

    void Awake()
    {
        kidScore = FindObjectOfType<KidScore>();
        timer = FindObjectOfType<Timer>();
        loadScene = new LoadScene();
        player = FindObjectOfType<Player>();
    }

    public void LoadPauseCanvas()
    {
        if (!player.isPaused)
        {
            pauseObject = Instantiate(pauseCanvas.gameObject, Camera.main.transform.position, Quaternion.identity);
            player.isPaused = true;
        }
        else
        {
            Destroy(pauseObject);
            player.isPaused = false;
        }
    }

    public void LoadLoseCanvas()
    {
        loseObject = Instantiate(loseCanvas.gameObject, Camera.main.transform.position,
                                Quaternion.identity);
        player.isPaused = true;
        loseObject.GetComponentsInChildren<Button>()[0].Select();
    }

    public IEnumerator WinScreen()
    {
        GameObject newWinCanvas = Instantiate(winCanvas.gameObject, Camera.main.transform.position,
                                Quaternion.identity);
        GameObject newKidsScore = null;
        GameObject newTimeScore = null;

        TextMeshProUGUI kidsFinalScore = newWinCanvas.GetComponentsInChildren<TextMeshProUGUI>()[3];
        TextMeshProUGUI timeFinalScore = newWinCanvas.GetComponentsInChildren<TextMeshProUGUI>()[4];

        if (PlayerPrefsManager.GetGameModePrefs().Contains("Endless"))
        {
            newWinCanvas.GetComponentsInChildren<TextMeshProUGUI>()[2].gameObject.SetActive(false);
            timeFinalScore.gameObject.SetActive(false);
        }

        for (float i = 0f; i < randomTime; i += Time.deltaTime)
        {
            int randomScore = UnityEngine.Random.Range(0, 100);
            int randomMinutes = UnityEngine.Random.Range(0, 100);
            int randomSeconds = UnityEngine.Random.Range(0, 100);
            kidsFinalScore.text = randomScore.ToString("00");
            timeFinalScore.text = randomMinutes.ToString("00") + ":" +
                                 randomSeconds.ToString("00");
            newTimeScore = Instantiate(timeFinalScore.gameObject, Camera.main.transform.position,
                                    Quaternion.identity);
            newKidsScore = Instantiate(kidsFinalScore.gameObject, Camera.main.transform.position,
                                    Quaternion.identity);
            yield return new WaitForSeconds(0.05f);
            Destroy(winObject2); Destroy(winObject3);
        }

        kidsFinalScore.text = kidScore.currentKids.ToString("00");
        timer.timeStopped = true;
        timeFinalScore.text = timer.TimeScore().ToString();
        newTimeScore = Instantiate(timeFinalScore.gameObject, Camera.main.transform.position,
                        Quaternion.identity);
        newKidsScore = Instantiate(kidsFinalScore.gameObject, Camera.main.transform.position,
                                Quaternion.identity);
        yield return new WaitForSeconds(loadTime);
        loadScene.LoadNextScene();
    }
}
