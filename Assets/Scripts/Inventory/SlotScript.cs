using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler,IClickable
{
    private Stack<Item> items = new Stack<Item>();//A stack for all items on this slot

    [SerializeField]
    private Image icon;//A a reference to the slots icon

    public bool IsEmpty//Checks if the item is empty
    {
        get
        {
            return items.Count == 0;
        }
    }

    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return items.Peek();
            }

            return null;
        }
    }

    public Image MyIcon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }
        
    

    public int MyCount
    {
        get { return items.Count; }
    }

    public bool AddItem(Item item)//Adds an Item to the slot
    {
        items.Push(item);

        icon.sprite = item.MyIcon;

        icon.color = Color.white;

        item.MySlot = this;

        return true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }

    public void RemoveItem(Item item)
    {
        if (!IsEmpty)
        {
            items.Pop();

            UIManager.MyInstance.UpdateStackSize(this);
        }
    }

    public void UseItem()
    {
        if (MyItem is IUseable)
        {
            (MyItem as IUseable).Use();
        }
        
    }

    public bool StackItem(Item item)
    {

        if (!IsEmpty && item.name == MyItem.name && items.Count < MyItem.MyStackSize)
        {
            items.Push(item);

            item.MySlot = this;

            return true;
        }

        return false;

    }
}
