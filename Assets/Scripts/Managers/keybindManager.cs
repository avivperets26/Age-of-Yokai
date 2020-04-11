using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeybindManager : MonoBehaviour
{
    private static KeybindManager instance;

    public static KeybindManager MyInstance//Singeltone
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<KeybindManager>();
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


        //Generates the default keybinds
        BindKey("UP", KeyCode.W);
        BindKey("LEFT", KeyCode.A);
        BindKey("DOWN", KeyCode.S);
        BindKey("RIGHT", KeyCode.D);

        BindKey("ACT1", KeyCode.Alpha1);
        BindKey("ACT2", KeyCode.Alpha2);
        BindKey("ACT3", KeyCode.Alpha3);
    }

    public void BindKey(string key, KeyCode keyBind)//Bind specific key
    {
        Dictionary<string, KeyCode> currentDictionary = Keybinds;

        if (key.Contains("ACT"))
        {
            currentDictionary = ActionBinds;
        }
        else if (!currentDictionary.ContainsKey(key))//Checks if the key is new
        {
            currentDictionary.Add(key, keyBind);//If the key is new we add it

            UIManager.MyInstance.UpdateKetText(key, keyBind);//We update the text on the button
        }
        else if (currentDictionary.ContainsValue(keyBind))//If we already have the keybind, then we need to change it to the new bind
        {
            string myKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key;//Looks for the old keybind

            //Unassigns the old keybind
            currentDictionary[myKey] = KeyCode.None;

            UIManager.MyInstance.UpdateKetText(key, KeyCode.None);
        }
        //Makes sure the new key is bound
        currentDictionary[key] = keyBind;

        UIManager.MyInstance.UpdateKetText(key, keyBind);

        bindName = string.Empty;

    }

    public void keyBindOnClick(string bindName)//Function for setting a keybind, this is called whenever a keybind button is clicked on the keybind menu
    {
        this.bindName = bindName;
    }

    private void OnGUI()
    {
        if(bindName != string.Empty)
        {
            Event e = Event.current;

            if (e.isKey)
            {
                BindKey(bindName, e.keyCode);
            }
        }
    }
}
