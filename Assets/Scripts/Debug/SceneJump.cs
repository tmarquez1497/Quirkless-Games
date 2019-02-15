using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneJump : MonoBehaviour {

    private List<Scene> scenes;

	// Use this for initialization
	void Start () {
        scenes = new List<Scene>();
        for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            scenes.Add(SceneManager.GetSceneByBuildIndex(i));
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            SceneManager.LoadScene(1);
        }
	}
}
