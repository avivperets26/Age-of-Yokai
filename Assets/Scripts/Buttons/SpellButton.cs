using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellButton : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// The name of the spell that the button will create
    /// </summary>
    [SerializeField]
    private string spellName;

    //When the button is pressed
    public void OnPointerClick(PointerEventData eventData)
    {
        //If we clicked with the left mouse
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //Then give a spell to the hand.
            HandScript.MyInstance.TakeMoveable(SpellBook.MyInstance.GetSpell(spellName));
        }
    }
}
