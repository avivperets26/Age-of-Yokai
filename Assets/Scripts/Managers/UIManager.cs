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
    private GameObject targetFrame;

    private Stat healthStat;

    [SerializeField]
    private Image portraitFrame;


    // A reference to the keybind menu
    [SerializeField]
    private CanvasGroup keybindMenu;

    [SerializeField]
    private CanvasGroup spellBook;


    // A reference to all the kibind buttons on the menu
    private GameObject[] keybindButtons;

    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
    }

    // Use this for initialization
    void Start()
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenClose(keybindMenu);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            OpenClose(spellBook);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            InventoryScript.MyInstance.OpenClose();
        }
    }

    public void ShowTargetFrame(NPC target)
    {
        targetFrame.SetActive(true);

        healthStat.Initialize(target.MyHealth.MyCurrentValue, target.MyHealth.MyMaxValue);

        portraitFrame.sprite = target.MyPortrait;

        target.healthChanged += new HealthChanged(UpdateTargetFrame);

        target.characterRemoved += new CharacterRemoved(HideTargetFrame);
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

    public void UpdateStackSize(IClickable clickable)//Updates the stacksize on a clickable slot
    {
        if (clickable.MyCount >1)//If our slot has more than one tem on it
        {
            clickable.MyStackText.text = clickable.MyCount.ToString();

            clickable.MyStackText.color = Color.white;

            clickable.MyIcon.color = Color.white;
        }
        else if (clickable.MyCount == 0)//If the slot is empty, then we need to hide the icon
        {
            clickable.MyIcon.color = new Color(0, 0, 0, 0);

            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }
        else//If it only has 1 item on it
        {
            clickable.MyStackText.color = new Color(0, 0, 0, 0);

            clickable.MyIcon.color = Color.white;
        }
    }
}
