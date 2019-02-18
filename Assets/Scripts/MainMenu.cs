using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public float waitTime;
    public int totalTimes;

    private const string title = "Corrupted";
    private const string binary = "01011001011011110111010110000101110011011010110110011101110111011100000110100101110101011100010110110101100011";
    private Text mText;
    private Animator credits;
    private bool isOpen = false;

    // Use this for initialization
    void Start () {
		mText = transform.GetChild(0).GetComponent<Text>();
        StartCoroutine("Animate");
        credits = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play()
    {
        StopAllCoroutines();
        SceneManager.LoadScene("Quirkless");
    }

    public void Credits()
    {
        isOpen = !isOpen;
        if (isOpen)
            credits.SetTrigger("Open");
        else
            credits.SetTrigger("Close");
    }

    public void Exit()
    {
        StopAllCoroutines();
        Application.Quit();
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
                constant += title[j++];

            for(int i = 0; i < (title.Length - constant.Length); i++)
            {
                //if(totalTimes > 0)
                //{
                    result += binary[Random.Range(0, binary.Length)];
               // }
                //else
                //{
                    //result = title;
                //}
            }

            result = result.Insert(0, constant);
            
            mText.text = result;
            yield return new WaitForSeconds(waitTime);
        }
    }
}
