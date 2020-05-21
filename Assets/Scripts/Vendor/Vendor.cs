using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendor : MonoBehaviour,IInteractable
{
    [SerializeField]
    private VendorItem[] items;

    [SerializeField]
    private VendorWindow vendorWindow;

    public void Interact()
    {
        vendorWindow.Open();
    }

    public void StopInteract()
    {
        vendorWindow.Close();
    }

}
