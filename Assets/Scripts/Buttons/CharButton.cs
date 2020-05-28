using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharButton : MonoBehaviour,IPointerClickHandler, IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField]
    private ArmorType armorType;

    private Armor equippedArmor;

    [SerializeField]
    private Image icon;

    public Armor MyEquippedArmor
    {
        get
        {
            return equippedArmor;
        }

        set
        {
            equippedArmor = value;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMoveable is Armor)
            {
                Armor tmp = (Armor)HandScript.MyInstance.MyMoveable;

                if (tmp.MyArmorType == armorType)
                {
                    EquipArmor(tmp);
                }

                UIManager.MyInstance.RefreshToolTip(tmp);
            }
            else if (HandScript.MyInstance.MyMoveable == null && MyEquippedArmor != null)
            {
                HandScript.MyInstance.TakeMoveable(MyEquippedArmor);

                CharacterPanel.MyInstance.MySelectedButton = this;

                icon.color = Color.grey;
            }
        }
    }

    public void EquipArmor(Armor armor)
    {
        armor.Remove();

        if (MyEquippedArmor != null)
        {

            if (MyEquippedArmor != armor)
            {
                armor.MySlot.AddItem(MyEquippedArmor);
            }          

            UIManager.MyInstance.RefreshToolTip(MyEquippedArmor);
        }
        else
        {
            UIManager.MyInstance.HideTooltip();
        }

        icon.enabled = true;

        icon.sprite = armor.MyIcon;

        icon.color = Color.white;

        this.equippedArmor = armor;// A reference to the equipped armor

        this.MyEquippedArmor.MyCharButton = this;

        if(HandScript.MyInstance.MyMoveable == (armor as IMoveable))
        {
            HandScript.MyInstance.Drop();
        }       
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (MyEquippedArmor != null)
        {
            UIManager.MyInstance.ShowTooltip(new Vector2(0, 0), transform.position, MyEquippedArmor);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }

    public void DequipArmor()
    {
        icon.color = Color.white;

        icon.enabled = false;

        equippedArmor.MyCharButton = null;

        equippedArmor = null;
    }
}
