using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendorWindow : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    public void Open()
    {
        canvasGroup.alpha = 1;

        canvasGroup.blocksRaycasts = true;
    }

    public void Close()
    {
        canvasGroup.alpha = 0;

        canvasGroup.blocksRaycasts = false;
    }
}
