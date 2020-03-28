using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager MyInstance//Singelton Design Pattern
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    private Button[] actionButtons;//A reference to all action buttons

    private KeyCode action1, action2, action3;//Key code used for executing the action buttons

    [SerializeField]
    private GameObject targetFrame;

    private Stat healthStat;

    [SerializeField]
    private Image portraitFrame;
   
    void Start()//Use it to initialization
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();//Look for the Stat script in the TargetFrane Object children to initialize it.

        //keybinds
        action1 = KeyCode.Alpha1;
        action2 = KeyCode.Alpha2;
        action3 = KeyCode.Alpha3;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(action1))
        {
            ActionButtonOnClick(0);
        }
        else if (Input.GetKeyDown(action2))
        {
            ActionButtonOnClick(1);
        }
        else if (Input.GetKeyDown(action3))
        {
            ActionButtonOnClick(2);
        }
    }
    private void ActionButtonOnClick(int btnIndex)//Exectues an action based on the button clicked
    {
        actionButtons[btnIndex].onClick.Invoke();
    }

    public void ShowTargetFrame(NPC target)
    {
        targetFrame.SetActive(true);

        healthStat.Initialize(target.Myhealth.MyCurrentValue, target.Myhealth.MyMaxValue);

        portraitFrame.sprite = target.MyPortrait;

        target.healthChanged += new HealthChanged(UpdateTargetFrame);

        target.characterRemoved += new CharacterRemoved(HideTargetFrame);
    }

    public void HideTargetFrame()
    {
        targetFrame.SetActive(false);
    }

    public void UpdateTargetFrame(float health)
    {
        healthStat.MyCurrentValue = health;
    }
}
