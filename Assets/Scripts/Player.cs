using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Animator animator;
    PlayerMovement myPlayerMovement;
    GetKid getKid;
    KidScore kidScore;
    StateCanvases stateCanvases;
    Timer timer;
    Tilemap[] tilemaps = new Tilemap[3];
    int tilemapCounter = 0;
    Vector2 myVector2;
    Vector2 trackedVector2;
    public bool isSpecial;
    int playClip;
    public bool isPaused;
    GameObject kid;
    LevelManager levelManager;

    void Awake()
    {
        animator = GetComponent<Animator>();
        myPlayerMovement = new PlayerMovement();
        getKid = FindObjectOfType<GetKid>();
        kidScore = FindObjectOfType<KidScore>();
        timer = FindObjectOfType<Timer>();
        stateCanvases = FindObjectOfType<StateCanvases>();
        levelManager = FindObjectOfType<LevelManager>();

        foreach (Tilemap child in FindObjectsOfType<Tilemap>())
        {
            if (child.tag == "Spooky")
            {
                tilemaps[tilemapCounter] = child;
                tilemapCounter++;
            }
        }

        levelManager.GetMusicAudio().gameObject.SetActive(false);
        levelManager.GetWalkingAudio().gameObject.SetActive(false);
        levelManager.GetGetKidAudio().gameObject.SetActive(false);
    }
    void Update()
    {
        if (!timer.timeStopped)
        {
            OnEnable();
            FixedUpdate();
            MoveAnimations();
        }
        else
        {
            OnDisable();
            levelManager.GetWalkingAudio().gameObject.SetActive(false);
            levelManager.GetGetKidAudio().gameObject.SetActive(false);
        }

        if (FindObjectOfType<LevelManager>().startAudio)
        {
            levelManager.GetMusicAudio().gameObject.SetActive(true);

            if (PlayerPrefsManager.GetSFXPrefs() == 0)
            {
                levelManager.GetWalkingAudio().gameObject.SetActive(false);
                levelManager.GetGetKidAudio().gameObject.SetActive(false);
            }
            else
            {
                levelManager.GetWalkingAudio().volume = PlayerPrefsManager.GetSFXPrefs();
                levelManager.GetGetKidAudio().volume = PlayerPrefsManager.GetSFXPrefs();
            }

            levelManager.GetMusicAudio().volume = PlayerPrefsManager.GetVolumePrefs();
        }
    }

    void OnNavigate()
    {
        if (!isPaused)
        {
            return;
        }
    }

    void OnEnable()
    {
        myPlayerMovement.Player.Enable();
    }

    void OnDisable()
    {
        myPlayerMovement.Player.Disable();
    }

    void FixedUpdate()
    {
        myVector2 = myPlayerMovement.Player.Move.ReadValue<Vector2>();

        if (((Mathf.Abs(myVector2.x) > Mathf.Epsilon && Mathf.Abs(myVector2.x) != 1f) ||
             (Mathf.Abs(myVector2.y) > Mathf.Epsilon && Mathf.Abs(myVector2.y) != 1f)))
        {
            myVector2 = new Vector2(0f, 0f);
        }

        if (Mathf.Abs(myVector2.x) == 1f || Mathf.Abs(myVector2.y) == 1f)
        {
            trackedVector2 = myVector2;
        }
    }

    void MoveAnimations()
    {
        bool isMovingUp = myVector2.y > 0;
        bool isMovingDown = myVector2.y < 0;
        bool isMovingLeft = myVector2.x < 0;
        bool isMovingRight = myVector2.x > 0;
        animator.SetBool("moveUp", isMovingUp);
        animator.SetBool("moveDown", isMovingDown);
        animator.SetBool("moveRight", isMovingRight);
        animator.SetBool("moveLeft", isMovingLeft);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        kid = other.gameObject;
        if (other.tag == "Special")
        {
            isSpecial = true;
        }
        else if (other.tag == "Gate")
        {
            if (kidScore.gateOpen) { return; }
            transform.position = new Vector2(transform.position.x - trackedVector2.x,
                                           transform.position.y - trackedVector2.y);
        }
        else if (other.tag == "Win")
        {
            levelManager.End();
        }
        else if (other.tag == "Enemy")
        {
            kidScore.DecreaseKidScore();
            transform.position = new Vector2(transform.position.x - (trackedVector2.x),
                                            transform.position.y - (trackedVector2.y));
        }
        else
        {
            isSpecial = false;
        }
    }

    void OnMove()
    {
        float newX = transform.position.x + myVector2.x;
        float newY = transform.position.y + myVector2.y;
        Vector3 newVector = new Vector3(newX, newY, 0);
        for (int i = 0; i < tilemaps.Length; i++)
        {
            if (tilemaps[i].GetTile(Vector3Int.FloorToInt(newVector)) != null) { return; }
        }
        transform.position = newVector;

        levelManager.GetWalkingAudio().gameObject.SetActive(false);

        if (!levelManager.GetGetKidAudio().isPlaying)
        {
            levelManager.GetGetKidAudio().gameObject.SetActive(false);
        }

        playClip++;

        if (playClip == 2)
        {
            playClip = 0;
            levelManager.GetWalkingAudio().gameObject.SetActive(true);
        }
    }

    void OnInspect()
    {
        if (isSpecial && kid.tag != "Floor")
        {
            isSpecial = false;
            kid.SetActive(false);
            getKid.PlayEffect();

            if (levelManager.GetWalkingAudio().isActiveAndEnabled || levelManager.GetWalkingAudio().volume != 0)
            {
                levelManager.GetGetKidAudio().gameObject.SetActive(true);
            }
            else
            {
                levelManager.GetGetKidAudio().gameObject.SetActive(false);
            }

            kidScore.IncreaseKidScore();
        }
    }

    void OnPause()
    {
        if (!levelManager.GetRightBeforeStartCanvas().isActiveAndEnabled &&
            !levelManager.GetLoadingCanvas().isActiveAndEnabled)
        {
            stateCanvases.LoadPauseCanvas();
            if (isPaused)
            {
                FindObjectsOfType<Button>()[0].Select();
                timer.timeStopped = true;
            }
            else
            {
                timer.timeStopped = false;
            }
        }
    }
}
