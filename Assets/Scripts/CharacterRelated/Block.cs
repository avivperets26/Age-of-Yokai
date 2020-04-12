using System;
using UnityEngine;

[Serializable]//A class for paring up blocks
public class Block
{
    //A pair of blocks
    [SerializeField]
    private GameObject first, second;

    /// <summary>
    /// Deactivates the pair
    /// </summary>
    public void Deactivate()
    {
        first.SetActive(false);
        second.SetActive(false);
    }

    /// <summary>
    /// Activates the pair
    /// </summary>
    public void Activate()
    {
        first.SetActive(true);
        second.SetActive(true);
    }
}
