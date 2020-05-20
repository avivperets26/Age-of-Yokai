using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler,IClickable,IPointerEnterHandler,IPointerExitHandler
{
    private ObservableStack<Item> items = new ObservableStack<Item>();//A stack for all items on this slot

    [SerializeField]
    private Image icon;//A a reference to the slots icon

    [SerializeField]
    private Text stackSize;

    public BagScript MyBag { get; set; }//A reference to the bag that this slot belong to

    public bool IsEmpty//Checks if the item is empty
    {
        get
        {
            return MyItems.Count == 0;
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
                return MyItems.Peek();
            }

            return null;
        }
    }

    public Image MyIcon//The icon on the slot
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
        
    

    public int MyCount//The item count on the slot
    {
        get { return MyItems.Count; }
    }

    public Text MyStackText//The stack text
    {
        get
        {
            return stackSize;
        }
    }

    public ObservableStack<Item> MyItems { get => items; }

    private void Awake()
    {
        //Assigns all the event on our observable stack to the updateSlot function
        MyItems.OnPop += new UpdateStackEvent(UpdateSlot);

        MyItems.OnPush += new UpdateStackEvent(UpdateSlot);

        MyItems.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    public bool AddItem(Item item)//Adds an Item to the slot
    {
        MyItems.Push(item);

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
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.MyInstance.FromSlot == null && !IsEmpty)//If we dont have somthing to move yet
            {
                if (HandScript.MyInstance.MyMoveable != null)
                {
                    if (HandScript.MyInstance.MyMoveable is Bag)
                    {
                        if (MyItem is Bag)
                        {
                            InventoryScript.MyInstance.SwapBags(HandScript.MyInstance.MyMoveable as Bag, MyItem as Bag);
                        }
                    }
                    else if (HandScript.MyInstance.MyMoveable is Armor)
                    {
                        if (MyItem is Armor && (MyItem as Armor).MyArmorType == (HandScript.MyInstance.MyMoveable as Armor).MyArmorType)
                        {
                            (MyItem as Armor).Equip();

                            HandScript.MyInstance.Drop();
                        }
                    }
                }
                else
                {
                    HandScript.MyInstance.TakeMoveable(MyItem as IMoveable);

                    InventoryScript.MyInstance.FromSlot = this;
                }

            }
            else if (InventoryScript.MyInstance.FromSlot == null && IsEmpty)
            {

                if (HandScript.MyInstance.MyMoveable is Bag)
                {
                    Bag bag = (Bag)HandScript.MyInstance.MyMoveable;//Dequips a bag from the inventory

                    if (bag.MyBagScript != MyBag && InventoryScript.MyInstance.MyEmptySlotCount - bag.Slots > 0)
                    {
                        AddItem(bag);

                        //CharacterPanel.MyInstance.MySelectedButton.DequipArmor();

                        bag.MyBagButton.RemovBag();

                        HandScript.MyInstance.Drop();
                    }
                }
                else if (HandScript.MyInstance.MyMoveable is Armor)
                {
                    Armor armor = (Armor)HandScript.MyInstance.MyMoveable;

                    AddItem(armor);

                    HandScript.MyInstance.Drop();
                }
                

            }
            else if (InventoryScript.MyInstance.FromSlot != null)//If we have somthiing to move
            {
                //We will try to do different things to place the item back into the inventory
                if (PutItemBack() || MergeItems(InventoryScript.MyInstance.FromSlot) ||SwapItems(InventoryScript.MyInstance.FromSlot) || AddItems(InventoryScript.MyInstance.FromSlot.MyItems))
                {
                    HandScript.MyInstance.Drop();

                    InventoryScript.MyInstance.FromSlot = null;
                }
            }

        }

        if (eventData.button == PointerEventData.InputButton.Right && HandScript.MyInstance.MyMoveable == null)//If we Rightclick on the slot
        {
            UseItem();
        }
    }

    public void RemoveItem(Item item)//Removes the item from the slot
    {
        if (!IsEmpty)
        {
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
        }
    }

    public void Clear()
    {
        if (MyItems.Count > 0)
        {
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());

            MyItems.Clear();
        }
    }

    public void UseItem()//Uses the item if it useable
    {
        if (MyItem is IUseable)
        {
            (MyItem as IUseable).Use();
        }
        else if (MyItem is Armor)
        {
            (MyItem as Armor).Equip();
        }
        
    }

    public bool StackItem(Item item)//Stack two items
    {

        if (!IsEmpty && item.name == MyItem.name && MyItems.Count < MyItem.MyStackSize)
        {
            MyItems.Push(item);

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
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.MyItems);

            //Clear slot A
            from.MyItems.Clear();

            //All itmes from slot B and copy them into A
            from.AddItems(MyItems);

            //Clear B
            MyItems.Clear();

            //Move the items from A copy to B
            AddItems(tmpFrom);

            return true;
        }

        return false;
    }

    private bool MergeItems(SlotScript from)//Merges two identical stack of items
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
                AddItem(from.MyItems.Pop());
            }

            return true;
        }

        return false;
    }

    private void UpdateSlot()//Update the slot
    {
        UIManager.MyInstance.UpdateStackSize(this);
    }

    public void OnPointerEnter(PointerEventData eventData)//Show tooltip
    {
        if (!IsEmpty)//if there is an item on the inventory slot
        {
            UIManager.MyInstance.ShowTooltip(new Vector2(1, 0), transform.position, MyItem);
        }
    }

    public void OnPointerExit(PointerEventData eventData)//Hide tooltip
    {
        UIManager.MyInstance.HideTooltip();
    }
}
