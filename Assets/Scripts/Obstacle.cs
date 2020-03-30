using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IComparable<Obstacle>
{
    public SpriteRenderer MySpriteRenderer { get; set; }//The Obstacle spriteRendrer

    private Color defaultColor;
    private Color fadedColor;

    public int CompareTo(Obstacle other)//Compare to, that is used for sorting the obstacle, so that we can pick
    {
        if (MySpriteRenderer.sortingOrder > other.MySpriteRenderer.sortingOrder)
        {
            return 1;//If this obstacles has a higher sortorder
        }

        else if(MySpriteRenderer.sortingOrder < other.MySpriteRenderer.sortingOrder)
        {
            return -1;//If this obstales has a lower sortorder
        }

        return 0;//If both obstacles has an equel sortorder
    }

    // Start is called before the first frame update
    void Start()
    {
        MySpriteRenderer = GetComponent<SpriteRenderer>();//Creates a refernce to the spriterendere

        defaultColor = MySpriteRenderer.color;

        fadedColor = defaultColor;

        fadedColor.a = 0.7f;
    }

    public void FadeOut()
    {
        Debug.Log("FadeOut");
        MySpriteRenderer.color = fadedColor;
    }

    public void FadeIn()
    {
        Debug.Log("Fadein");
        MySpriteRenderer.color = defaultColor;
    }
}

