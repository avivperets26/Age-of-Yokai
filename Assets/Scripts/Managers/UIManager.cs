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

    /// <summary>
    /// A reference to all the action buttons
    /// </summary>
    [SerializeField]
    private ActionButton[] actionButtons;

    [SerializeField]
    private GameObject targetFrame;

    private Stat healthStat;

    [SerializeField]
    private Image portraitFrame;

    /// <summary>
    /// A reference to the keybind menu
    /// </summary>
    [SerializeField]
    private CanvasGroup keybindMenu;

    [SerializeField]
    private CanvasGroup spellBook;

    /// <summary>
    /// A reference to all the kibind buttons on the menu
    /// </summary>
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
        if (Input.GetKeyDown(KeyCode.P))
        {
            OpenClose(spellBook);
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

    /// <summary>
    /// Updates the targetframe
    /// </summary>
    /// <param name="health"></param>
    public void UpdateTargetFrame(float health)
    {
        healthStat.MyCurrentValue = health;
    }

    /// <summary>
    /// Updates the text on a keybindbutton after the key has been changed
    /// </summary>
    /// <param name="key"></param>
    /// <param name="code"></param>
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
}
