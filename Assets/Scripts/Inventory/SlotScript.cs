using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler,IClickable
{
    private ObservableStack<Item> items = new ObservableStack<Item>();//A stack for all items on this slot

    [SerializeField]
    private Image icon;//A a reference to the slots icon

    [SerializeField]
    private Text stackSize;

    public bool IsEmpty//Checks if the item is empty
    {
        get
        {
            return items.Count == 0;
        }
    }

    public bool IsFull
    {
        get
        {
            if (IsEmpty|| MyCount < MyItem.MyStackSize)
            {
                return false;
            }

            return true;
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

    public Text MyStackText
    {
        get
        {
            return stackSize;
        }
    }

    private void Awake()
    {
        //Assigns all the event on our observable stack to the updateSlot function
        items.OnPop += new UpdateStackEvent(UpdateSlot);

        items.OnPush += new UpdateStackEvent(UpdateSlot);

        items.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    public bool AddItem(Item item)//Adds an Item to the slot
    {
        items.Push(item);

        icon.sprite = item.MyIcon;

        icon.color = Color.white;

        item.MySlot = this;

        return true;
    }


    public bool AddItems(ObservableStack<Item> newItems)//Add a stack of items to the slot
    {
        if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
        {
            int count = newItems.Count;

            for (int i = 0; i < count; i++)
            {
                if (IsFull)
                {
                    return false;
                }

                AddItem(newItems.Pop());
            }

            return true;
        }

        return false;
    }
    public void OnPointerClick(PointerEventData eventData)//When the slot is clicked
    {
        if (eventData.button == PointerEventData.InputButton.Left)//If we dont have somthing to move yet
        {
            if (InventoryScript.MyInstance.FromSlot == null && !IsEmpty)
            {
                HandScript.MyInstance.TakeMoveable(MyItem as IMoveable);

                InventoryScript.MyInstance.FromSlot = this;
            }

            else if (InventoryScript.MyInstance.FromSlot != null)//If we have somthiing to move
            {
                //We will try to do different things to place the item back into the inventory
                if (PutItemBack() || MergeItems(InventoryScript.MyInstance.FromSlot) ||SwapItems(InventoryScript.MyInstance.FromSlot) || AddItems(InventoryScript.MyInstance.FromSlot.items))
                {
                    HandScript.MyInstance.Drop();

                    InventoryScript.MyInstance.FromSlot = null;
                }
            }

        }

        if (eventData.button == PointerEventData.InputButton.Right)//If we Rightclick on the slot
        {
            UseItem();
        }
    }

    public void RemoveItem(Item item)//Removes the item from the slot
    {
        if (!IsEmpty)
        {
            items.Pop();
        }
    }

    public void Clear()
    {
        if (items.Count > 0)
        {
            items.Clear();
        }
    }

    public void UseItem()//Uses the item if it useable
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

    public bool PutItemBack()
    {
        if(InventoryScript.MyInstance.FromSlot == this)
        {
            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;

            return true;
        }
        else
        {
            return false;
        }
    }

    private bool SwapItems(SlotScript from)//Swap two itmes in the inventory
    {
        if (IsEmpty)
        {
            return false;
        }
        else if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount+MyCount > MyItem.MyStackSize)
        {
            //Copy all the itmes we need to swap from A
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.items);

            //Clear slot A
            from.items.Clear();

            //All itmes from slot B and copy them into A
            from.AddItems(items);

            //Clear B
            items.Clear();

            //Move the items from A copy to B
            AddItems(tmpFrom);

            return true;
        }

        return false;
    }

    private bool MergeItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;

        }
        else if (from.MyItem.GetType() == MyItem.GetType() && !IsFull)
        {
            //How many free slots do we have in the stack
            int free = MyItem.MyStackSize - MyCount;

            for (int i = 0; i < free; i++)
            {
                AddItem(from.items.Pop());
            }

            return true;
        }

        return false;
    }

    private void UpdateSlot()//Update the slot
    {
        UIManager.MyInstance.UpdateStackSize(this);
    }
}
