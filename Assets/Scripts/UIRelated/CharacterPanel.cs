using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanel : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    public void OpenClose()
    {
        if (canvasGroup.alpha <= 0)
        {
            canvasGroup.blocksRaycasts = true;

            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.blocksRaycasts = false;

            canvasGroup.alpha = 0;
        }
    }
}
