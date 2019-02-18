using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameOver : MonoBehaviour {

    //public int totalTimes;
    //public float waitTime;

    public float blinkTime;
    public float typeSpeed;
    public enum GameOverOption
    {
        Restart,
        Quit
    };

    private delegate void AfterEffect();
    private Text mText;
    private const string message = "!DELETED!";
    private const char BOX = '▁';
    private const string binary = "01011001011011110111010110000101110011011010110110011101110111011100000110100101110101011100010110110101100011";
    private Scene currentScene;
    private int initialTotal;
    private GameOverOption option = GameOverOption.Restart;
    private EventSystem eventSystem;
    private int lastIdx = 0;
    private bool isOn = true;
    private bool isReady = false;

    // Use this for initialization
    void Start () {
        mText = transform.GetChild(1).GetComponent<Text>();
        currentScene = SceneManager.GetActiveScene();
        //initialTotal = totalTimes;
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry moveEntry = new EventTrigger.Entry();
        moveEntry.eventID = EventTriggerType.Move;
        moveEntry.callback.AddListener((data) => { MoveTrigger((AxisEventData)data); });
        trigger.triggers.Add(moveEntry);

        EventTrigger.Entry submitEntry = new EventTrigger.Entry();
        submitEntry.eventID = EventTriggerType.Submit;
        submitEntry.callback.AddListener((data) => { SubmitTrigger(data); });
        trigger.triggers.Add(submitEntry);
	}
	
	// Update is called once per frame
	void Update () {
        if (isReady && eventSystem.currentSelectedGameObject != gameObject)
            eventSystem.SetSelectedGameObject(gameObject);
	}

    public void MoveTrigger(AxisEventData data)
    {
        string result = mText.text;

        switch (data.moveDir)
        {
            case MoveDirection.Down:
            case MoveDirection.Up:
                result = result.Replace("< ", "");
                result = result.Replace(" >", "");
                option = NextOption();
                if (option == GameOverOption.Restart)
                    result = result.Replace("Restart", "< Restart >");
                else if (option == GameOverOption.Quit)
                    result = result.Replace("Quit", "< Quit >");
                mText.text = result;
                break;
            default:
                break;
        }
    }

    private GameOverOption NextOption()
    {
        if (option == GameOverOption.Restart)
            return GameOverOption.Quit;
        else
            return GameOverOption.Restart;
    }

    public void SubmitTrigger(BaseEventData data)
    {
        switch (option)
        {
            case GameOverOption.Restart:
                StopAllCoroutines();
                Restart();
                break;
            case GameOverOption.Quit:
                GiveUp();
                break;
            default:
                break;
        }
    }

    private void InitUI()
    {
        GameManager.instance.SetUI(gameObject);
        isReady = true;
    }

    private int FindBrick()
    {
        for(int i = 0; i < mText.text.Length; i++)
        {
            if (mText.text[i].Equals(BOX))
            {
                lastIdx = i;
                return i;
            }
        }

        return lastIdx;
    }

    IEnumerator Blink()
    {
        char[] temp;
        int boxIdx = -1;

        while (true)
        {
            temp = mText.text.ToCharArray();
            boxIdx = FindBrick();

            if (isOn)
                temp[boxIdx] = ' ';
            else
                temp[boxIdx] = BOX;

            isOn = !isOn;
            mText.text = new string(temp);
            yield return new WaitForSeconds(blinkTime);
        }
    }

    IEnumerator Type(float startDelay, string message, float endDelay, AfterEffect action)
    {
        char[] temp = mText.text.ToCharArray();
        int msgIdx = 0;

        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        StopCoroutine("Blink");
        temp[lastIdx] = BOX;
        mText.text = new string(temp);
        isOn = true;

        while (msgIdx < message.Length)
        {
            mText.text = mText.text.Insert(lastIdx++, message.Substring(msgIdx++, 1));
            yield return new WaitForSeconds(typeSpeed);
        }

        StartCoroutine("Blink");
        if (endDelay > 0f)
            yield return new WaitForSeconds(endDelay);

        if (action != null)
            action();
    }

    /*
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
                result += binary[Random.Range(0, binary.Length)];
            }

            result = result.Insert(0, constant);

            mText.text = result;
            yield return new WaitForSeconds(waitTime);
        }

        totalTimes = initialTotal;
        totalText.text = "You took: " + totalTime;
    }
    */

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
        transform.localScale = Vector3.one;
        StartCoroutine(Blink());
        StartCoroutine(Type(1f, "Error: Deleted (Total Time: " + totalStr + ")\n\n< Restart >\nQuit", 0f, () => InitUI()));
    }
}
