using System.Collections;
using System;
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
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    private ActionButton[] actionButtons;//A reference to all action buttons

    [SerializeField]
    private GameObject targetFrame;

    private Stat healthStat;

    [SerializeField]
    private Image portraitFrame;

    [SerializeField]
    private CanvasGroup keybindMenu;//A reference to the keybiind menu

    private GameObject[] keybindButtons;//A reference to all the keybind buttons on the menu

    private void Awake()
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
    }

    void Start()//Use it to initialization
    {
        healthStat = targetFrame.GetComponentInChildren<Stat>();//Look for the Stat script in the TargetFrane Object children to initialize it.

        SetUseable(actionButtons[0],SpellBook.MyInstance.GetSpell("Fireball"));
        SetUseable(actionButtons[1], SpellBook.MyInstance.GetSpell("Frostbolt"));
        SetUseable(actionButtons[2], SpellBook.MyInstance.GetSpell("Thunderbolt"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenCloseMenu();
        }
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

    public void OpenCloseMenu()
    {
        keybindMenu.alpha = keybindMenu.alpha > 0 ? 0 : 1; //If alpha is >0 than replace to 1

        keybindMenu.blocksRaycasts =keybindMenu.blocksRaycasts == true ? false : true;

        Time.timeScale = Time.timeScale > 0 ? 0 : 1;
    }

    public void UpdateKetText(string key, KeyCode code)//Updates the text on a keybindbutton after the key has been changed
    {
        Text tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<Text>();

        tmp.text = code.ToString();
    }

    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x => x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    public void SetUseable(ActionButton btn, IUseable useable)
    {
        btn.MyButton.image.sprite = useable.MyIcon;

        btn.MyButton.image.color = Color.white;

        btn.MyUseable = useable;

    }
}
