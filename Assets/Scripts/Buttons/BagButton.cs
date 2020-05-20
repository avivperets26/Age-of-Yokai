using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagButton : MonoBehaviour,IPointerClickHandler
{

    private Bag bag;//A reference to the bag item

    [SerializeField]
    private Sprite full, empty;//Sprites to indicate if the bag is full or empty

    [SerializeField]
    private int bagIndex;

    public Bag MyBag {//A property for accessing the specific bag
        get
        {
            return bag;
        }

        set
        {
            if (value != null)
            {
                GetComponent<Image>().sprite = full;
            }
            else
            {
                GetComponent<Image>().sprite = empty;
            }
            bag = value;
        }
    }

    public int MyBagIndex { get => bagIndex; set => bagIndex = value; }

    public void OnPointerClick(PointerEventData eventData)//If we click the specific bag button
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.MyInstance.FromSlot != null && HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is Bag)
            {
                if (MyBag != null)
                {
                    InventoryScript.MyInstance.SwapBags(MyBag, HandScript.MyInstance.MyMoveable as Bag);
                }
                else
                {
                    Bag tmp = (Bag)HandScript.MyInstance.MyMoveable;

                    tmp.MyBagButton = this;

                    tmp.Use();

                    MyBag = tmp; ;

                    HandScript.MyInstance.Drop();

                    InventoryScript.MyInstance.FromSlot = null;
                }
            }    

            else if (Input.GetKey(KeyCode.LeftShift))
            {
                HandScript.MyInstance.TakeMoveable(MyBag);
            }
            else if (bag != null)//If we have a bag equipped
            {
                bag.MyBagScript.OpenClose();//Open or Close the bag
            }
        }       
    }

    public void RemovBag()//Removes the bag from the bagbar
    {
        InventoryScript.MyInstance.RemoveBag(MyBag);

        MyBag.MyBagButton = null;

        foreach (Item item in MyBag.MyBagScript.GetItems())
        {
            InventoryScript.MyInstance.AddItem(item);
        }

        MyBag = null;
    }
}
