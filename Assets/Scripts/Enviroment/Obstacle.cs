using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IComparable<Obstacle>
{
    /// <summary>
    /// The obstacles spriterenderer 
    /// </summary>
    public SpriteRenderer MySpriteRenderer { get; set; }

    /// <summary>
    /// Color to use the the obstacle isn't faded
    /// </summary>
    private Color defaultColor;

    /// <summary>
    /// Color to use the the obstacle is faded out
    /// </summary>
    private Color fadedColor;

    /// <summary>
    /// Compare to, that is used for sorting the obstacles, so that we can pick the on with the lowest sortorder
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Obstacle other)
    {
        if (MySpriteRenderer.sortingOrder > other.MySpriteRenderer.sortingOrder)
        {
            return 1; //If this obstacles has a higher sortorder 
        }
        else if (MySpriteRenderer.sortingOrder < other.MySpriteRenderer.sortingOrder)
        {
            return -1; //If this obstacles has a lower sortorder 
        }

        return 0; //If both obstacles ha an equal sortorder
    }

    // Use this for initialization
    void Start()
    {
        //Creates a reference to the spriterendere
        MySpriteRenderer = GetComponent<SpriteRenderer>();

        //Creates the colors
        defaultColor = MySpriteRenderer.color;
        fadedColor = defaultColor;
        fadedColor.a = 0.7f;
    }

    /// <summary>
    /// Fades out the obstacle
    /// </summary>
    public void FadeOut()
    {
        MySpriteRenderer.color = fadedColor;
    }

    /// <summary>
    /// Fades in the obstacle
    /// </summary>
    public void FadeIn()
    {
        MySpriteRenderer.color = defaultColor;
    }

}
