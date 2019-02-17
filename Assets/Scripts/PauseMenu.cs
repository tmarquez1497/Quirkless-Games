using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    private bool isOn = false;
    private GameObject first;

	// Use this for initialization
	void Start () {
        first = transform.GetChild(1).gameObject;
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
            GameManager.instance.SetUI(first);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetCursor(false);
        }
        else
        {
            GameManager.instance.isPaused = false;
            GameManager.instance.SetUI(null);
            transform.localScale = Vector3.zero;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetCursor(true);
        }
        
    }

    public void GetOut()
    {
        Application.Quit();
    }
}
