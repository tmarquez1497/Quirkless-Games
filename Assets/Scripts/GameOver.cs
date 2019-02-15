using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    public float totalTime;
    public float waitTime;

    private Text mText;
    private string message = "You've Been Spotted!!!";
    private const string alphabet = "ABCČĆDĐEFGHIJKLMNOPQRSŠTUVWXYZŽabcčćdđefghijklmnopqrsštuvwxyzžĂÂÊÔƠƯăâêôơư1234567890‘?’“!”(%)[#]{@}/&\\<-+÷×=>®©$€£¥¢:;,.*";
    private bool isAnimating = false;
    private float timeLeft;

    // Use this for initialization
    void Start () {
        mText = transform.GetChild(0).GetComponent<Text>();
        timeLeft = totalTime;
	}
	
	// Update is called once per frame
	void Update () {
		if(isAnimating && mText.text.Equals(message))
        {
            StopCoroutine("Animate");
            isAnimating = false;
            timeLeft = totalTime;
        }
	}

    IEnumerator Animate()
    {
        string result = "";
        isAnimating = true;

        while (timeLeft > waitTime)
        {
            result = "";
            timeLeft -= waitTime;
            for (int i = 0; i < mText.text.Length; i++)
            {
                if (timeLeft > waitTime)
                    result += alphabet[Random.Range(0, alphabet.Length)];
                else
                    result = message;
            }
            mText.text = result;
            //Debug.Log("Time Left: " + timeLeft);
            yield return new WaitForSeconds(waitTime);
        }

        

        
    }

    public void StartAnimate()
    {
        StartCoroutine(Animate());
    }
}
