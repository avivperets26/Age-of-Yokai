using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }


    // A reference to all the action buttons
    [SerializeField]
    private ActionButton[] actionButtons;

    [SerializeField]
    private CanvasGroup[] menus;

    [SerializeField]
    private GameObject targetFrame;

    private Stat healthStat;

    [SerializeField]
    private Text levelTexet;

    [SerializeField]
    private Image portraitFrame;


    // A reference to the keybind menu
    [SerializeField]
    private CanvasGroup keybindMenu;

    [SerializeField]
    private CanvasGroup spellBook;

    [SerializeField]
    private GameObject tooltip;

    [SerializeField]
    private CharacterPanel charPanel;

    private Text tooltipText;


    [SerializeField]
    private RectTransform tooltipRect;

    // A reference to all the kibind buttons on the menu
    private GameObject[] keybindButtons;

    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");

        tooltipText = tooltip.GetComponentInChildren<Text>();
    }

    // Use this for initialization
    void Start()
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))//Manu
        {
            OpenClose(menus[0]);
        }
        else if (Input.GetKeyDown(KeyCode.M))//Spell Book
        {
            OpenClose(menus[1]);
        }
        else if (Input.GetKeyDown(KeyCode.C))//Character Panel
        {         
            OpenClose(menus[2]);
        }
        else if (Input.GetKeyDown(KeyCode.L))//Quest Log
        {
            OpenClose(menus[3]);
        }
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    OpenClose(keybindMenu);
        //}
        //else if (Input.GetKeyDown(KeyCode.M))
        //{
        //    OpenClose(spellBook);
        //}



    }

    public void ShowTargetFrame(Enemy target)
    {
        targetFrame.SetActive(true);

        healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);

        portraitFrame.sprite = target.MyPortrait;

        levelTexet.text = target.MyLevel.ToString();

        target.healthChanged += new HealthChanged(UpdateTargetFrame);

        target.characterRemoved += new CharacterRemoved(HideTargetFrame);

        if (target.MyLevel >= Hero.MyInstance.MyLevel +5)
        {
            levelTexet.color = Color.red;
        }
        else if (target.MyLevel == Hero.MyInstance.MyLevel + 3 || target.MyLevel == Hero.MyInstance.MyLevel + 4)
        {
            levelTexet.color = new Color32(255,124,0,255);
        }
        else if (target.MyLevel >= Hero.MyInstance.MyLevel - 2 && target.MyLevel <= Hero.MyInstance.MyLevel + 2)
        {
            levelTexet.color = Color.yellow;
        }
        else if (target.MyLevel <= Hero.MyInstance.MyLevel-3 && target.MyLevel > XPManager.CalculateGrayLevel())
        {
            levelTexet.color = Color.green;
        }
        else
        {
            levelTexet.color = Color.grey;
        }
    }

    public void HideTargetFrame()
    {
        targetFrame.SetActive(false);
    }


    // Updates the targetframe
    public void UpdateTargetFrame(float health)
    {
        healthStat.MyCurrentValue = health;
    }


    // Updates the text on a keybindbutton after the key has been changed
    public void UpdateKeyText(string key, KeyCode code)
    {
        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();
    }

    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    public void OpenClose(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void OpenSingle(CanvasGroup canvasGroup)
    {
        foreach (CanvasGroup canvas in menus)
        {
            CloseSingle(canvas);
        }

        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void CloseSingle(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;

        canvasGroup.blocksRaycasts = false;

    }

    public void UpdateStackSize(IClickable clickable)//Updates the stacksize on a clickable slot
    {
        if (clickable.MyCount > 1) //If our slot has more than one item on it
        {
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.enabled = true;
            clickable.MyIcon.enabled = true;
        }
        else //If it only has 1 item on it
        {
            clickable.MyStackText.enabled = false;
            clickable.MyIcon.enabled = true;
        }
        if (clickable.MyCount == 0) //If the slot is empty, then we need to hide the icon
        {
            clickable.MyIcon.enabled = false;
            clickable.MyStackText.enabled = false;
        }
    }

    public void ClearStackCount(IClickable clickable)
    {
        clickable.MyStackText.enabled = false;
        clickable.MyIcon.enabled = true;
    }

    public void ShowTooltip(Vector2 pivot, Vector3 position, IDescribable description)//Shows the tooltip
    {
        tooltipRect.pivot = pivot;

        tooltip.SetActive(true);

        tooltip.transform.position = position;

        tooltipText.text = description.GetDescription();
    }

    public void HideTooltip()//Hides the tooltip
    {
        tooltip.SetActive(false);
    }

    public void RefreshToolTip(IDescribable description)
    {
        tooltipText.text = description.GetDescription();
    }
}
