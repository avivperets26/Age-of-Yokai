using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void ItemCountChanged(Item item);

public class InventoryScript : MonoBehaviour
{
    public event ItemCountChanged itemCountChangedEvent;

    private static InventoryScript instance;

    public static InventoryScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }

            return instance;
        }
    }

    private SlotScript fromSlot;

    private List<Bag> bags = new List<Bag>();

    [SerializeField]
    private BagButton[] bagButtons;

    [SerializeField]//To test items while developing
    private Item[] items;

    public bool CanAddBag
    {
        get { return MyBags.Count < 5; }
    }

    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;

            foreach (Bag bag in MyBags)
            {
                count += bag.MyBagScript.MyEmptySlotCount;
            }
            return count;
        }
    }

    public int MyTotalSlotCount
    {
        get
        {
            int count = 0;

            foreach (Bag bag  in MyBags)
            {
                count += bag.MyBagScript.MySlots.Count;
            }

            return count;
        }
    }

    public int MyFullSlotCount
    {
        get
        {
            return MyTotalSlotCount - MyEmptySlotCount;
        }
    }

    public SlotScript FromSlot
    {
        get
        {
            return fromSlot;
        }

        set
        {
            fromSlot = value;

            if (value!= null)
            {
                fromSlot.MyIcon.color = Color.grey;
            }
        }
    }

    public List<Bag> MyBags
    {
        get
        {
            return bags;
        }
    }

    private void Awake()
    {
        Bag bag = (Bag)Instantiate(items[3]);

        bag.Initialize(20);

        bag.Use();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Bag bag = (Bag)Instantiate(items[3]);

            bag.Initialize(20);

            AddItem(bag);
        }

        else if (Input.GetKeyDown(KeyCode.K))//for debug
        {
            Bag bag = (Bag)Instantiate(items[3]);

            bag.Initialize(20);

            AddItem(bag);
        }

        else if (Input.GetKeyDown(KeyCode.P))
        {
            HealthPotion potion = (HealthPotion)Instantiate(items[4]);

            AddItem(potion);
        }

        else if (Input.GetKeyDown(KeyCode.H))
        {
            AddItem((Armor)Instantiate(items[1]));

            AddItem((Armor)Instantiate(items[2]));

            AddItem((Armor)Instantiate(items[5]));

            AddItem((Armor)Instantiate(items[6]));

            AddItem((Armor)Instantiate(items[7]));           

            AddItem((Armor)Instantiate(items[0]));

            AddItem((Armor)Instantiate(items[8]));//Armor 1
        }
    }

    public void AddBag(Bag bag)//Equips a bag to the inventory
    {
        foreach (BagButton bagButton in bagButtons)
        {
            if(bagButton.MyBag == null)
            {
                bagButton.MyBag = bag;

                MyBags.Add(bag);

                bag.MyBagButton = bagButton;

                bag.MyBagScript.transform.SetSiblingIndex(bagButton.MyBagIndex);

                break;
            }
        }
    }

    public void AddBag(Bag bag, BagButton bagButton)//Overload func
    {
        MyBags.Add(bag);

        bagButton.MyBag = bag;

        bag.MyBagScript.transform.SetSiblingIndex(bagButton.MyBagIndex);
    }

    public void AddBag(Bag bag, int bagIndex)//Second Overload func
    {
        bag.SetUpScript();

        MyBags.Add(bag);

        bag.MyBagButton = bagButtons[bagIndex];

        bagButtons[bagIndex].MyBag = bag;
    }

    public void RemoveBag(Bag bag)//Removes the bag from the inventory
    {
        MyBags.Remove(bag);

        Destroy(bag.MyBagScript.gameObject);
    }

    public void SwapBags(Bag oldBag, Bag newBag)
    {
        int newSlotCount = (MyEmptySlotCount - oldBag.MySlotCount) + newBag.MySlotCount;

        if (newSlotCount - MyFullSlotCount >= 0)
        {
            //Do swaping
            List<Item> bagItems = oldBag.MyBagScript.GetItems();

            RemoveBag(oldBag);

            newBag.MyBagButton = oldBag.MyBagButton;

            newBag.Use();

            foreach (Item item in bagItems)
            {
                if (item != newBag)//To make sure we dont get dubplicate bags
                {
                    AddItem(item);
                }
            }

            AddItem(oldBag);

            HandScript.MyInstance.Drop();

            MyInstance.fromSlot = null;
        }
    }
    //Add an item to the inventory
    public bool AddItem(Item item)
    {
        if (item.MyStackSize > 0)
        {
            if (PlaceInStack(item))
            {
                return true;
            }
        }

        return PlaceInEmpty(item);
    }
    
    private bool PlaceInEmpty(Item item)//Places an item on an empty slot in the game
    {
        foreach (Bag bag in MyBags)//Checks all bags
        {
            if (bag.MyBagScript.AddItem(item))//Tries to add the item
            {
                OnItemCountChanged(item);

                return true;//possible to add the item
            }
        }
        return false;
    }

    private bool PlaceInStack(Item item)//Tries to stack an item on another
    {
        foreach (Bag bag in MyBags)//Checks all bags
        {
            foreach (SlotScript slots in bag.MyBagScript.MySlots)//Checks all slots on the current bag
            {
                if (slots.StackItem(item))//Tries to stack the item
                {
                    OnItemCountChanged(item);

                    return true;//It was possible to stack the item
                }
            }
        }

        return false;// It wasn't possible to stack the item
    }

    public void PlaceInSpecific(Item item, int slotIndex, int bagIndex)
    {
        bags[bagIndex].MyBagScript.MySlots[slotIndex].AddItem(item);

    }
    //public void PlaceInSpecific(Item item, int slotIndex, int bagIndex)
    //{
    //    Debug.Log("In PlaceInSpecific method at InventoryScript");
    //    bags[bagIndex].MyBagScript.MySlots[slotIndex].AddItem(item);
    //    Debug.Log("Out PlaceInSpecific method at InventoryScript");
    //}

    public void OpenClose()//Open and closes all bags
    {

        bool closedBag = MyBags.Find(x => !x.MyBagScript.IsOpen);//Checks if any bags are closed

        //if closed bag == true, then open all closed bags. 
        //If closed bag == false, then close all open bags

        foreach (Bag bag in MyBags)
        {
            if(bag.MyBagScript.IsOpen != closedBag)
            {
                bag.MyBagScript.OpenClose();
            }
        }
    }

    public List<SlotScript> GetAllItems()
    {
        List<SlotScript> slots = new List<SlotScript>();

        foreach (Bag bag in MyBags)
        {
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                if (!slot.IsEmpty)
                {
                    slots.Add(slot);
                }
            }
        }

        return slots;
    }

    public Stack<IUseable> GetUseables(IUseable type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();

        foreach (Bag bag in MyBags)
        {
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                if (!slot.IsEmpty && slot.MyItem.GetType() == type.GetType())
                {
                    foreach (Item item in slot.MyItems)
                    {
                        useables.Push(item as IUseable);
                    }
                }
            }
        }

        return useables;
    }

    public IUseable GetUseable(string type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();

        foreach (Bag bag in MyBags)
        {
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                if (!slot.IsEmpty && slot.MyItem.MyTitle == type)
                {
                    return (slot.MyItem as IUseable);
                }
            }
        }

        return null;
    }

    public int GetItemCount(string type)
    {
        int itemCount = 0;

        foreach (Bag bag in MyBags)
        {
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                if (!slot.IsEmpty && slot.MyItem.MyTitle == type)
                {
                    itemCount += slot.MyItems.Count;
                }
            }
        }
        return itemCount;
    }

    public Stack<Item> GetItems(string type, int count)
    {
        Stack<Item> items = new Stack<Item>();

        foreach (Bag bag in MyBags)
        {
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                if (!slot.IsEmpty && slot.MyItem.MyTitle == type)
                {
                    foreach (Item item in slot.MyItems)
                    {
                        items.Push(item);

                        if (items.Count == count)
                        {
                            return items;
                        }
                    }
                }
            }
        }

        return items;
    }

    public void RemoveItem(Item item)
    {
        foreach (Bag bag in MyBags)
        {
            foreach (SlotScript slot in bag.MyBagScript.MySlots)
            {
                if (!slot.IsEmpty && slot.MyItem.MyTitle == item.MyTitle)
                {
                    slot.RemoveItem(item);

                    break;
                }
            }
        }
    }

    public void OnItemCountChanged(Item item)
    {
        if (itemCountChangedEvent != null)
        {
            itemCountChangedEvent.Invoke(item);
        }
    }
    
}
