using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClassDB;

public class Database : MonoBehaviour {

	public static Database core;

	//A list of all in-game characters
	public List<character> characters = new List<character>();

	//A list of all in-game items
	public List<item> items = new List<item>();

	//A list of all in-game skills
	public List<skill> skills = new List<skill>();

	//Used by "EditorDatabase.cs" to determine which tab is currently selected
	[HideInInspector] public int tab;

	void Awake () { if (core == null) { core = this; } }

}


//(c) Cination - Tsenkilidis Alexandros