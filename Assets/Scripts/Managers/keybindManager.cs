using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class keybindManager : MonoBehaviour
{
    private static keybindManager instance;

    public static keybindManager Instance//Singeltone
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<keybindManager>();
            }
            return instance;
        }
    }

    public Dictionary<string , KeyCode> Keybinds { get; set; }

    public Dictionary<string, KeyCode> ActionBinds { get; private set; }

    private string bindName;

    void Start()
    {
        Keybinds = new Dictionary<string, KeyCode>();

        ActionBinds = new Dictionary<string, KeyCode>();

        BindKey("UP", KeyCode.W);
        BindKey("LEFT", KeyCode.A);
        BindKey("DOWN", KeyCode.S);
        BindKey("RIGHT", KeyCode.D);

        BindKey("ACT1", KeyCode.Alpha1);
        BindKey("ACT2", KeyCode.Alpha2);
        BindKey("ACT3", KeyCode.Alpha3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BindKey(string key, KeyCode keyBind)
    {
        Dictionary<string, KeyCode> currentDictionary = Keybinds;

        if (key.Contains("ACT"))
        {
            currentDictionary = ActionBinds;
        }
        else if (!currentDictionary.ContainsValue(keyBind))
        {
            currentDictionary.Add(key, keyBind);

            UIManager.MyInstance.UpdateKetText(key, keyBind);
        }
        else if (currentDictionary.ContainsValue(keyBind))
        {
            string myKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key;

            currentDictionary[myKey] = KeyCode.None;

            UIManager.MyInstance.UpdateKetText(key, KeyCode.None);
        }

        currentDictionary[key] = keyBind;

        UIManager.MyInstance.UpdateKetText(key, keyBind);

        bindName = string.Empty;

    }
}
