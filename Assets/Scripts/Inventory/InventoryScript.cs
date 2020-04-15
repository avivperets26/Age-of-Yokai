using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{

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

    private List<Bag> bags = new List<Bag>();

    [SerializeField]
    private BagButton[] bagButtons;

    [SerializeField]//To test items while developing
    private Item[] items;

    public bool CanAddBag
    {
        get { return bags.Count < 5; }
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

        else if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("pressed K");

            Bag bag = (Bag)Instantiate(items[0]);

            bag.Initialize(20);

            AddItem(bag);
        }
    }

    public void AddBag(Bag bag)
    {
        foreach (BagButton bagButton in bagButtons)
        {
            if(bagButton.MyBag == null)
            {
                bagButton.MyBag = bag;

                bags.Add(bag);

                break;
            }
        }
    }

    public void AddItem(Item item)
    {
        foreach (Bag bag in bags)
        {
            if (bag.MyBagScript.AddItem(item))
            {
                return;
            }
        }
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

}
