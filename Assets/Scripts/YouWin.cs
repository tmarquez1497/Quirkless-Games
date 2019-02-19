using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class YouWin : MonoBehaviour {

    public enum WinOption
    {
        PlayAgain,
        Exit
    };
    private delegate void AfterEffect();

    public float blinkTime;
    public float typeSpeed;

    private Scene currentScene;
    private WinOption option = WinOption.PlayAgain;
    private bool blinkOn = true;
    private int lastIdx = 0;
    private bool isReady = false;
    private Text mText;
    private const char BOX = '▁';
    private EventSystem eventSystem;

    // Use this for initialization
    void Start () {
        currentScene = SceneManager.GetActiveScene();
        mText = transform.GetChild(1).GetComponent<Text>();
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

    private void Update()
    {
        if (isReady && eventSystem.currentSelectedGameObject != gameObject)
            eventSystem.SetSelectedGameObject(gameObject);
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
        transform.localScale = Vector3.one;
        GameManager.instance.SetUI(gameObject);
        StartCoroutine(Type(0f, "You Win!!! (Total Time: " + time + ")\n\n< Play Again >\nExit", 0f, () => { isReady = true; }));
    }

    private int FindBrick()
    {
        for (int i = 0; i < mText.text.Length; i++)
        {
            if (mText.text[i].Equals(BOX))
            {
                lastIdx = i;
                return i;
            }
        }

        return lastIdx;
    }

    private WinOption NextOption()
    {
        if (option == WinOption.PlayAgain)
            return WinOption.Exit;
        else
            return WinOption.PlayAgain;
    }

    public void SubmitTrigger(BaseEventData data)
    {
        switch (option)
        {
            case WinOption.PlayAgain:
                StopAllCoroutines();
                PlayAgain();
                break;
            case WinOption.Exit:
                StartCoroutine(Type(0f, "\n\nGoodbye.", 1.3f, () => Application.Quit()));
                break;
            default:
                break;
        }
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
                if (option == WinOption.PlayAgain)
                    result = result.Replace("Play Again", "< Play Again >");
                else if (option == WinOption.Exit)
                    result = result.Replace("Exit", "< Exit >");
                mText.text = result;
                break;
            default:
                break;
        }
    }

    IEnumerator Blink()
    {
        char[] temp;
        int boxIdx = -1;

        while (true)
        {
            temp = mText.text.ToCharArray();
            boxIdx = FindBrick();

            if (blinkOn)
                temp[boxIdx] = ' ';
            else
                temp[boxIdx] = BOX;

            blinkOn = !blinkOn;
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
        blinkOn = true;

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
}
