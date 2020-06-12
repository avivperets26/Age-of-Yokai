using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A collection of custom classes used across the asset

namespace ClassDB {
	
	[System.Serializable]
	public class item {
		public string name;
		public int id;
		public string description;

		//The amount of turn points it costs to use the item.
		public int turnPointCost;
		
		//A list of functions to be called when the item is used
		public List<callInfo> functionsToCall = new List<callInfo>(); 

	}

	[System.Serializable]
	public class characterItemInfo {
		public int id;
		public float quantity;
	}

	[System.Serializable]
	public class skill {

		public string name;
		public int id;
		public string description;

		//Can the skill be used?
		public bool unlocked;

		//The amount of turn points it costs to use the skill
		public int turnPointCost;

		//A list of functions to be called when the skill is used
		public List<callInfo> functionsToCall = new List<callInfo>();
	}

	[System.Serializable]
	public class character {

		public string name;
		public int id;
		public string description;
		public Sprite icon;

		//Animator component
		public RuntimeAnimatorController animationController;

		//A list of all available skills
		public List<int> skills = new List<int>();

		//A list of items available to the player by ids
		public List<characterItemInfo> items = new List<characterItemInfo>();

		//A list of character attributes
		public List<characterAttribute> characterAttributes = new List<characterAttribute>();

		//A list of A.I functions
		public List<callInfo> aiFunctions = new List<callInfo>();

		//Is the character active
		public bool isActive;
		
	}

	[System.Serializable]
	public class characterAttribute {
		public string name;
		public int id;

		//The maximum value that the attribute can have
		public float maxValue;

		//The current value the attribute has
		public float curValue;
	}

	[System.Serializable]
	public class callInfo {
		public string functionName;
		public List<string> parametersArray = new List<string>();
		public bool waitForPreviousFunction;
		public bool isCoroutine;
		public bool isRunning;
	}

	[System.Serializable]
	public class characterInfo {
		public GameObject instanceObject;
		public GameObject uiObject;
		public GameObject spawnPointObject;
		public string currentAnimation;
		public int characterId;
	}
	
	//Used by main action menu, not skills and items.
	[System.Serializable]
	public class actionInfo {
		public string name;
		public string description;

		//The functions to be called when the action is selected
		public List<string> functions = new List<string>();
	}

	//Used to display info about elements of the current action list
	[System.Serializable]
	public class curActionInfo {
		public GameObject actionObject;
		public string description;
		public int turnPointCost;
	}

	
	[System.Serializable]
	public class audioInfo {
		public int id;
		public AudioClip clip;
	}

	[System.Serializable]
	public class spriteInfo {
		public int id;
		public Sprite sprite;
	}
	
}

//(c) Cination - Tsenkilidis Alexandros