using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class YouWin : MonoBehaviour {

    private Scene currentScene;
    private Text timeText;
    private GameObject firstBtn;

	// Use this for initialization
	void Start () {
        currentScene = SceneManager.GetActiveScene();
        timeText = transform.GetChild(3).GetComponent<Text>();
        firstBtn = transform.GetChild(2).gameObject;
	}

    public void PlayAgain()
    {
        SceneManager.LoadScene(currentScene.name);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void OnWin(string time)
    {
        timeText.text = "You Took: " + time;
        transform.localScale = Vector3.one;
        GameManager.instance.SetUI(firstBtn);
    }
}
