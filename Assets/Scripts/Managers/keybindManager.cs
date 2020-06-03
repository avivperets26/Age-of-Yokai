using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyBindManager : MonoBehaviour
{
    /// <summary>
    /// A reference to the singleton instance
    /// </summary>
    private static KeyBindManager instance;

    /// <summary>
    /// Property for accessing the singleton instance
    /// </summary>
    public static KeyBindManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<KeyBindManager>();
            }

            return instance;
        }
    }

    /// <summary>
    /// A dictionary for all movement keybinds
    /// </summary>
    public Dictionary<string, KeyCode> Keybinds { get; private set; }

    /// <summary>
    /// A dictionary for all actionKeybinds
    /// </summary>
    public Dictionary<string, KeyCode> ActionBinds { get; private set; }

    /// <summary>
    /// The name of the keybind we are trying to set or change
    /// </summary>
    private string bindName;

    // Use this for initialization
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

    /// <summary>
    /// Binds a specific key
    /// </summary>
    /// <param name="key">Key to bind</param>
    /// <param name="keyBind">Keybind to set</param>
    public void BindKey(string key, KeyCode keyBind)
    {
        //Sets the default dictionary to the keybinds
        Dictionary<string, KeyCode> currentDictionary = Keybinds;

        if (key.Contains("ACT")) //If we are trying to bind an actionbutton, then we use the actionbinds dictionary instead
        {
            currentDictionary = ActionBinds;
        }
        if (!currentDictionary.ContainsKey(key))//Checks if the key is new
        {
            //If the key is new we add it
            currentDictionary.Add(key, keyBind);

            //We update the text on the button
            UIManager.MyInstance.UpdateKeyText(key, keyBind);
        }
        else if (currentDictionary.ContainsValue(keyBind)) //If we already have the keybind, then we need to change it to the new bind
        {
            //Looks for the old keybind
            string myKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key;

            //Unassigns the old keybind
            currentDictionary[myKey] = KeyCode.None;
            UIManager.MyInstance.UpdateKeyText(key, KeyCode.None);
        }

        //Makes sure the new key is bound
        currentDictionary[key] = keyBind;
        UIManager.MyInstance.UpdateKeyText(key, keyBind);
        bindName = string.Empty;
    }

    /// <summary>
    /// Function for setting a keybind, this is called whenever a keybind button is clicked on the keybind menu
    /// </summary>
    /// <param name="bindName"></param>
    public void KeyBindOnClick(string bindName)
    {
        this.bindName = bindName;
    }


    private void OnGUI()
    {
        if (bindName != string.Empty)//Checks if we are going to save a keybind
        {
            Event e = Event.current; //Listens for an event

            if (e.isKey) //If the event is a key, then we change the keybind
            {
                BindKey(bindName, e.keyCode);
            }
        }
    }
}
