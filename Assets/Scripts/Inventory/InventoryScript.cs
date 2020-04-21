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
        get { return bags.Count < 5; }
    }

    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;

            foreach (Bag bag in bags)
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

            foreach (Bag bag  in bags)
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

    public SlotScript FromSlot { get
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

    private void Awake()
    {
        Bag bag = (Bag)Instantiate(items[0]);

        bag.Initialize(20);

        bag.Use();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Bag bag = (Bag)Instantiate(items[0]);

            bag.Initialize(20);

            bag.Use();
        }

        else if (Input.GetKeyDown(KeyCode.K))//for debug
        {
            Debug.Log("pressed K");

            Bag bag = (Bag)Instantiate(items[0]);

            bag.Initialize(20);

            AddItem(bag);
        }

        else if (Input.GetKeyDown(KeyCode.L))
        {
            HealthPotion potion = (HealthPotion)Instantiate(items[1]);

            AddItem(potion);
        }

        else if (Input.GetKeyDown(KeyCode.H))
        {
            AddItem((Armor)Instantiate(items[2]));

            AddItem((Armor)Instantiate(items[3]));

            AddItem((Armor)Instantiate(items[4]));

            AddItem((Armor)Instantiate(items[5]));
        }
    }

    public void AddBag(Bag bag)//Equips a bag to the inventory
    {
        foreach (BagButton bagButton in bagButtons)
        {
            if(bagButton.MyBag == null)
            {
                bagButton.MyBag = bag;

                bags.Add(bag);

                bag.MyBagButton = bagButton;

                break;
            }
        }
    }

    public void AddBag(Bag bag, BagButton bagButton)//Overload func
    {
        bags.Add(bag);

        bagButton.MyBag = bag;
    }

    public void RemoveBag(Bag bag)//Removes the bag from the inventory
    {
        bags.Remove(bag);

        Destroy(bag.MyBagScript.gameObject);
    }

    public void SwapBags(Bag oldBag, Bag newBag)
    {
        int newSlotCount = (MyEmptySlotCount - oldBag.Slots) + newBag.Slots;

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

    public void AddItem(Item item)//Add an item to the inventory
    {
        if (item.MyStackSize > 0)
        {
            if (PlaceInStack(item))
            {
                return;
            }
        }

        PlaceInEmpty(item);
    }

    private void PlaceInEmpty(Item item)//Places an item on an empty slot in the game
    {
        foreach (Bag bag in bags)//Checks all bags
        {
            if (bag.MyBagScript.AddItem(item))//Tries to add the item
            {
                OnItemCountChanged(item);

                return;//possible to add the item
            }
        }
    }

    private bool PlaceInStack(Item item)//Tries to stack an item on another
    {
        foreach (Bag bag in bags)//Checks all bags
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

    public void OpenClose()//Open and closes all bags
    {

        bool closedBag = bags.Find(x => !x.MyBagScript.IsOpen);//Checks if any bags are closed

        //if closed bag == true, then open all closed bags. 
        //If closed bag == false, then close all open bags

        foreach (Bag bag in bags)
        {
            if(bag.MyBagScript.IsOpen != closedBag)
            {
                bag.MyBagScript.OpenClose();
            }
        }
    }

    public Stack<IUseable> GetUseables(IUseable type)
    {
        Stack<IUseable> useables = new Stack<IUseable>();

        foreach (Bag bag in bags)
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

    public void OnItemCountChanged(Item item)
    {
        if (itemCountChangedEvent != null)
        {
            itemCountChangedEvent.Invoke(item);
        }
    }
    
}
