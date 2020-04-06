using System;
using UnityEngine;

[Serializable]
public class Block
{
    [SerializeField]
    private GameObject first,second;

    public void Deactivete()
    {
        first.SetActive(false);
        second.SetActive(false);
    }

    public void Activete()
    {
        first.SetActive(true);
        second.SetActive(true);
    }
}
