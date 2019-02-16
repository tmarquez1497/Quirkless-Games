using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;          // There can only be one copy of this gameobject. This stores the copy.

    public Color spotColor;                             // Color of all patrol spots.
    [RangeAttribute(0f, 10f)] public float spotSize;    // Size of the patrol spots, ranges from 0 to 10 units. (In editor only).
    [HideInInspector] public bool isPaused = false;     // A boolean to check if the game is paused or not.
    public GameObject gameOverMenu;
    public GameObject pauseMenuPanel;
    public Text timerText;

    private GameOver gameOver;
    private PauseMenu pauseMenu;
    private bool isStopped = false;
    private float mTime = 0f;

    private void Awake()
    {
        /*
         If there is no other copy of the gameobject, this becomes that copy.
         If there already is one, destroy this extra copy.
         */
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        // Tell Unity to keep this gameobject when loading and unloading scenes.
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        gameOver = gameOverMenu.GetComponent<GameOver>();
        pauseMenu = pauseMenuPanel.GetComponent<PauseMenu>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!instance.isStopped)
            {
                isPaused = !isPaused;
                pauseMenu.PlayPause();
            }
        }

        if(!instance.isPaused && !instance.isStopped)
        {
            if (mTime <= 1000f)
            {
                timerText.text = Format(mTime);
                mTime += Time.deltaTime;
            }
            else
            {
                timerText.color = Color.red;
                timerText.text = "1:00.000";
                mTime += Time.deltaTime;
            }
        }
    }

    private string Format(float ms)
    {
        string result = "";
        float hours = Mathf.Floor(ms / 3600f);
        float minutes = Mathf.Floor((ms - (hours * 3600f)) / 60f);
        float seconds = Mathf.Floor(ms - (hours * 3600f) - (minutes * 60f));

        if (minutes < 10f) result += "0";
        result += minutes + ":";
        if (seconds < 10f) result += "0";
        result += seconds + "." + (ms % 1000f);

        return result;
    }

    public void GameOver()
    {
        instance.isPaused = true;
        instance.isStopped = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetCursor(false);
        gameOver.StartAnimate();
    }

    private void OnDrawGizmos()
    {
        /**
         * 1. Find all the patrol spots (taged with "PatrolSpot")
         * 2. Create a sphere in the editor with the size and color you give it in the inspector.
         */
        GameObject[] points = GameObject.FindGameObjectsWithTag("PatrolSpot");

        Gizmos.color = spotColor;
        for (int i = 0; i < points.Length; i++)
            Gizmos.DrawSphere(points[i].transform.position, spotSize);
    }
}
