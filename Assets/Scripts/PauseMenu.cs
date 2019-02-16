using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    private bool isOn = false;

	// Use this for initialization
	void Start () {
        
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
            GameManager.instance.isPaused = false;
            transform.localScale = Vector3.zero;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetCursor(true);
        }
        
    }

    public void GetOut()
    {
        Application.Quit();
    }
}
