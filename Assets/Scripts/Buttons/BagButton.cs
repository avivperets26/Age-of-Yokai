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

    public void OnPointerClick(PointerEventData eventData)//If we click the specific bag button
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Input.GetKey(KeyCode.LeftShift))
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
