﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    private static HandScript instance;

    public static HandScript MyInstance//Singelton Design Pattern
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandScript>();
            }
            return instance;
        }
    }

    public IMoveable MyMoveable { get; set; }// The current moveable

    private Image icon;// The icon of the item, that we acre moving around atm.

    [SerializeField]
    private Vector3 offset;// An offset to move the icon away from the mouse

    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponent<Image>();//Creates a reference to the image on the hand
    }

    // Update is called once per frame
    void Update()
    {
        icon.transform.position = Input.mousePosition+offset;//Makes sure that the icon follows the hand

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && MyInstance.MyMoveable != null)
        {
            DeleteItem();
        }
       
    }

    public void TakeMoveable(IMoveable moveable)// Take a moveable in the hand, so that we can move it around
    {
        this.MyMoveable = moveable;

        icon.sprite = moveable.MyIcon;

        icon.enabled = true;
    }

    public IMoveable Put()
    {
        IMoveable tmp = MyMoveable;

        MyMoveable = null;

        icon.enabled = false;

        return tmp;
    }

    public void Drop()
    {
        MyMoveable = null;

        icon.enabled = false;

        InventoryScript.MyInstance.FromSlot = null;
    }

    public void DeleteItem()//Deletes an item from the inventory
    {

        if (MyMoveable is Item)
        {
            Item item = (Item)MyMoveable;

            if (item.MySlot != null)
            {
                item.MySlot.Clear();
            }
            else if (item.MyCharButton != null)
            {
                item.MyCharButton.DequipArmor();
            }
           
        }

        Drop();

        InventoryScript.MyInstance.FromSlot = null;
    }
}
