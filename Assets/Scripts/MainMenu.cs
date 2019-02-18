using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {

    public enum MainMenuOption
    {
        Play,
        Credits,
        Exit
    };
    private delegate void AfterEffect();

    public float waitTime;
    public int totalTimes;
    public float blinkTime;
    public float typeSpeed;

    private const string title = "Corrupted";
    private const char BOX = '▁';
    private const string binary = "01011001011011110111010110000101110011011010110110011101110111011100000110100101110101011100010110110101100011";
    private Text mText;
    private bool isOpen = false;
    private int lastIdx = 0;
    private bool isOn = true;
    private bool isReady = false;
    private MainMenuOption option = MainMenuOption.Play;
    private EventSystem eventSystem;

    // Use this for initialization
    void Start () {
		mText = transform.GetChild(0).GetComponent<Text>();
        mText.text = "▁";
        StartCoroutine("Blink");
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Move;
        entry.callback.AddListener((data) => { MoveTrigger((AxisEventData)data); });
        trigger.triggers.Add(entry);

        EventTrigger.Entry submitEntry = new EventTrigger.Entry();
        submitEntry.eventID = EventTriggerType.Submit;
        submitEntry.callback.AddListener((data) => { SubmitTrigger(data); });
        trigger.triggers.Add(submitEntry);

        StartCoroutine(Type(2.23f, "Corrupted.exe\n    < Play >\n    Credits\n    Exit\nSelect an Option . . .", 0f, () => InitUI()));
	}
	
	// Update is called once per frame
	void Update () {
        if (isReady && eventSystem.currentSelectedGameObject != gameObject)
            eventSystem.SetSelectedGameObject(gameObject);
	}

    private void InitUI()
    {
        Debug.Log("UI Initialized");
        eventSystem.SetSelectedGameObject(gameObject);
        isReady = true;
    }

    public void Play()
    {
        StopAllCoroutines();
        SceneManager.LoadScene("Quirkless");
    }

    private void Credits()
    {
        if (!isOpen)
        {
            isOpen = true;
            StartCoroutine(Type(0f, "\n\nMade by Quirkless Games\nTomas Marquez - Level Creation\nRyan Morales - Art Assets\nDakarai Simmons - Programming", 0f, null));
        }
    }

    public void Exit()
    {
        StopAllCoroutines();
        Application.Quit();
    }

    public void MoveTrigger(AxisEventData data)
    {
        string result = mText.text;

        switch (data.moveDir)
        {
            case MoveDirection.Up:
                result = result.Replace("< ", "");
                result = result.Replace(" >", "");
                option = PrevOption();
                if (option == MainMenuOption.Play)
                    result = result.Replace("Play", "< Play >");
                else if (option == MainMenuOption.Credits)
                    result = result.Replace("Credits", "< Credits >");
                else
                    result = result.Replace("Exit", "< Exit >");
                mText.text = result;
                break;
            case MoveDirection.Down:
                result = result.Replace("< ", "");
                result = result.Replace(" >", "");
                option = NextOption();
                if (option == MainMenuOption.Play)
                    result = result.Replace("Play", "< Play >");
                else if (option == MainMenuOption.Credits)
                    result = result.Replace("Credits", "< Credits >");
                else
                    result = result.Replace("Exit", "< Exit >");
                mText.text = result;
                break;
            default:
                break;
        }
    }

    public void SubmitTrigger(BaseEventData data)
    {
        Debug.Log("Submit triggered.");
        switch (option)
        {
            case MainMenuOption.Play:
                StopAllCoroutines();
                SceneManager.LoadScene("Quirkless");
                break;
            case MainMenuOption.Credits:
                Credits();
                break;
            case MainMenuOption.Exit:
                StartCoroutine(Type(0f, "\n\nGoodbye.", 1.3f, () => Application.Quit()));
                break;
            default:
                break;
        }
    }

    private MainMenuOption NextOption()
    {
        if(option == MainMenuOption.Play)
            return MainMenuOption.Credits;
        else if(option == MainMenuOption.Credits)
            return MainMenuOption.Exit;
        else
            return MainMenuOption.Play;
    }

    private MainMenuOption PrevOption()
    {
        if (option == MainMenuOption.Play)
            return MainMenuOption.Exit;
        else if (option == MainMenuOption.Credits)
            return MainMenuOption.Play;
        else
            return MainMenuOption.Credits;
    }

    IEnumerator Type(float startDelay, string message, float endDelay, AfterEffect action)
    {
        char[] temp = mText.text.ToCharArray();
        int msgIdx = 0;

        if(startDelay > 0f)
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

        if(action != null)
            action();
    }

    /*
    IEnumerator BlinkOption()
    {
        int left = 18, right = 25;
        MainMenuOption lastOption = option;
        bool isShown = true;

        // Yes, this infinite loop is intentional
        while (true)
        {
            if(lastOption != option)
            {
                switch (option)
                {
                    case MainMenuOption.Play:
                        lastOption = option;
                        left = 18;
                        right = 25;
                        break;
                    case MainMenuOption.Credits:
                        lastOption = option;
                        break;
                    case MainMenuOption.Exit:
                        lastOption = option;
                        break;
                    default:
                        break;
                }
            }

            if (isShown)
            {

            }
        }
    }
    */

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

            if (isOn)
                temp[boxIdx] = ' ';
            else
                temp[boxIdx] = BOX;

            isOn = !isOn;
            mText.text = new string(temp);
            yield return new WaitForSeconds(blinkTime);
        }
    }
}
