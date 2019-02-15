using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    private bool isOn = false;
    private GameManager gameManager = null;

	// Use this for initialization
	void Start () {
        if (GameObject.Find("GameManager") != null)
            gameManager = GameManager.instance;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void PlayPause()
    {
        isOn = !isOn;
        if (isOn)
        {
            transform.localScale = Vector3.one;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetCursor(false);
        }
        else
        {
            gameManager.isPaused = false;
            transform.localScale = Vector3.zero;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetCursor(true);
        }
        
    }

    public void GetOut()
    {
        Application.Quit();
    }
}
