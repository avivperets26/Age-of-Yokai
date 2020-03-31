using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Spell
{
    [SerializeField]
    private string name;//The spell name

    [SerializeField]
    private int damage;//The spell damage

    [SerializeField]
    private Sprite icon;//The spell icon

    [SerializeField]
    private float speed;//The spell speed

    [SerializeField]
    private float castTime; //The spell cast time

    [SerializeField]
    private GameObject spellPrefab;//The spell prefab

    [SerializeField]
    private Color barColor;//The spell color

    public string MyName { get => name;}//Property for accessing the spell name
    public int MyDamage { get => damage;}//Property for reading the damage
    public Sprite MyIcon { get => icon;}//Property for reading the icon
    public float MySpeed { get => speed; }//Property for reading the speed
    public float MyCastTime { get => castTime; }//Property for reading the castTime
    public GameObject MySpellPrefab { get => spellPrefab; }//Property for reading the spellPrefab
    public Color MyBarColor { get => barColor;}//porperty for reading the color
}
