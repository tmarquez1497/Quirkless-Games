using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour {

    public enum PauseOption
    {
        Resume,
        Quit
    };

    public float blinkTime;

    private bool isOn = false;
    private const char BOX = '▁';
    private Text mText;
    private bool blinkOn = true;
    private int lastIdx = -1;
    private bool isReady = false;
    private PauseOption option = PauseOption.Resume;
    private EventSystem eventSystem;

	// Use this for initialization
	void Start () {
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
	
	// Update is called once per frame
	void Update () {
        if (isReady && eventSystem.currentSelectedGameObject != gameObject)
            eventSystem.SetSelectedGameObject(gameObject);
	}

    public void PlayPause()
    {
        isOn = !isOn;
        if (isOn)
        {
            transform.localScale = Vector3.one;
            mText.text = "Game Paused\n\n< Resume >\nQuit▁";
            GameManager.instance.SetUI(gameObject);
            isReady = true;
            StartCoroutine("Blink");
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetCursor(false);
        }
        else
        {
            isReady = false;
            StopCoroutine("Blink");
            GameManager.instance.isPaused = false;
            GameManager.instance.SetUI(null);
            transform.localScale = Vector3.zero;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetCursor(true);
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
                if (option == PauseOption.Resume)
                    result = result.Replace("Resume", "< Resume >");
                else if (option == PauseOption.Quit)
                    result = result.Replace("Quit", "< Quit >");
                mText.text = result;
                break;
            default:
                break;
        }
    }

    public void SubmitTrigger(BaseEventData data)
    {
        switch (option)
        {
            case PauseOption.Resume:
                PlayPause();
                break;
            case PauseOption.Quit:
                StopAllCoroutines();
                GetOut();
                break;
            default:
                break;
        }
    }

    private PauseOption NextOption()
    {
        if (option == PauseOption.Resume)
            return PauseOption.Quit;
        else
            return PauseOption.Resume;
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

    IEnumerator Blink()
    {
        char[] temp;
        int boxIdx = -1;

        // Yes, this infinite loop is intentional
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

    public void GetOut()
    {
        Application.Quit();
    }
}
