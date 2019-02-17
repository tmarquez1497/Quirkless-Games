using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    public int totalTimes;
    public float waitTime;

    private Text mText;
    private const string message = "!DELETED!";
    private const string binary = "01011001011011110111010110000101110011011010110110011101110111011100000110100101110101011100010110110101100011";
    private string totalTime = "";
    private Text totalText;
    private Scene currentScene;
    private int initialTotal;
    private GameObject firstBtn;

    // Use this for initialization
    void Start () {
        mText = transform.GetChild(0).GetComponent<Text>();
        totalText = transform.GetChild(1).GetComponent<Text>();
        totalText.text = totalTime;
        currentScene = SceneManager.GetActiveScene();
        initialTotal = totalTimes;
        firstBtn = transform.GetChild(2).gameObject;
	}
	
	// Update is called once per frame
	void Update () {

	}

    IEnumerator Animate()
    {
        string result = "";
        string constant = "";
        int j = 0;

        while (totalTimes-- >= 0)
        {
            result = "";
            if (totalTimes % 9 == 0)
                constant += message[j++];

            for (int i = 0; i < (message.Length - constant.Length); i++)
            {
                /*if (totalTimes > 0)
                    result += binary[Random.Range(0, binary.Length)];
                else
                    result = message;
                    */
                result += binary[Random.Range(0, binary.Length)];
            }

            result = result.Insert(0, constant);

            mText.text = result;
            yield return new WaitForSeconds(waitTime);
        }

        totalTimes = initialTotal;
        totalText.text = "You took: " + totalTime;
    }

    public void Restart()
    {
        SceneManager.LoadScene(currentScene.name);
    }

    public void GiveUp()
    {
        Application.Quit();
    }

    public void StartAnimate(string totalStr)
    {
        totalTime = totalStr;
        transform.localScale = Vector3.one;
        GameManager.instance.SetUI(firstBtn);
        StartCoroutine(Animate());
    }
}
