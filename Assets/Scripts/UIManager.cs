using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button[] actionButtons;

    private KeyCode action1, action2, action3;
    // Start is called before the first frame update
    void Start()
    {
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
    private void ActionButtonOnClick(int btnIndex)
    {
        actionButtons[btnIndex].onClick.Invoke();
    }
}
