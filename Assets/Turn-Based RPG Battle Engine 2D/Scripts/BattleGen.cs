using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using ClassDB;

//This script generates various lists
public class BattleGen : MonoBehaviour {

	//Reference used by other scripts to access the active instance of BattleGen
	public static BattleGen core;

	GameObject actionsWindow;
	GameObject optionPrefab;

	//The list of main options
	public List<actionInfo> mainActions = new List<actionInfo>();

	//Generates main options menu
	public void mainGen () {

		//Getting UI
		actionsWindow = ObjectDB.core.battleUIActionsWindow;
		optionPrefab = ObjectDB.core.battleUIOptionPrefab;

		//Emptying actions window
		FunctionDB.core.emptyWindow(actionsWindow);

		//Used for setting focus
		bool focusSet = false;
				
		//Clearing current actions window
		BattleManager.core.curActions.Clear();

		//Spawning options and assigning listeners
		foreach (actionInfo ai in mainActions) {

			//Creating object
			GameObject g = Instantiate (optionPrefab, actionsWindow.transform);

			//Managing button
			Button b = g.GetComponent<Button>();

			//Setting focus
			if (!focusSet) {
				EventSystem.current.SetSelectedGameObject(g);
				focusSet = true;

			}
			
			//Getting functions list
			var fl = ai.functions;

			foreach (string f in fl) {
				//Listeners
				b.onClick.AddListener ( delegate { this.Invoke (f, 0f); } );
			}

			//Setting button text
			g.transform.GetChild(0).gameObject.GetComponent<Text>().text = ai.name;

			//Adding element to current action list
			curActionInfo cai = new curActionInfo();
			cai.actionObject = g;
			cai.description = ai.description;
			cai.turnPointCost = -1;

			BattleManager.core.curActions.Add (cai);

		}

	}


	//Spawning characters
	public void charGen (List<GameObject> teamSpawns, List<int> charTeam, int team) {

		//Character info prefab and window
		var charInfoPrefab = ObjectDB.core.battleUICharacterInfoPrefab;
		var charInfoWindow = ObjectDB.core.battleUICharacterInfoWindow;

		//Removing children before respawning
		foreach (GameObject spawn in teamSpawns) {
			foreach (Transform t in spawn.transform) {
				Destroy (t.gameObject);
			}
		}

		//A counter is kept in order to ensure that there are available spawn points
		var counter = 0;

		foreach (int charId in charTeam) {

			//Getting character
			var character = Database.core.characters[FunctionDB.core.findCharacterIndexById (charId)];

			//Getting animation controller
			var controller = character.animationController;

			//Instantiating character prefab if there is a slot available
			if (teamSpawns.Count > counter) {

				GameObject instance = Instantiate (ObjectDB.core.battlerPrefab, teamSpawns[counter].transform);
				//Setting animator
				instance.GetComponent<Animator>().runtimeAnimatorController = controller;
				//Setting GameObject's name
				instance.name = character.name;

				//Setting sprite direction
				var spriteRotation = team == 1 ? 180 : 0;
				instance.transform.Rotate (0 , spriteRotation, 0);

				//Adding element to instances
				characterInfo info = new characterInfo();
				info.characterId = charId;
				info.instanceObject = instance;
				info.spawnPointObject = teamSpawns[counter];

				BattleManager.core.characters.Add (info);
				
				//Incrementing character
				counter++;

				//Spawning UI
				//We only need to spawn UI for the player team
				if (team == 0) {

					//Instatiating object
					GameObject g = Instantiate (charInfoPrefab, charInfoWindow.transform);

					var charIndx = FunctionDB.core.findBattleManagerCharactersIndexById (charId);

					BattleManager.core.characters[charIndx].uiObject = g;

					//Setting data
					Transform gt = g.transform;
					GameObject iconObject = gt.GetChild(0).gameObject;
					GameObject nameObject = gt.GetChild(1).gameObject;
					GameObject attributeSlot1 = gt.GetChild(2).gameObject;
					GameObject attributeSlot2 = gt.GetChild(3).gameObject;

					//Icon
					iconObject.GetComponent<Image>().sprite = character.icon;

					//Name
					nameObject.GetComponent<Text>().text = character.name;

					//Attribute1
					var attributes = character.characterAttributes;

					if (attributes.Count >= 2) {
						attributeSlot1.GetComponent<Text>().text = attributes[0].name + " " + attributes[0].curValue.ToString() + " / " + attributes[0].maxValue.ToString();
						attributeSlot2.GetComponent<Text>().text = attributes[1].name + " " + attributes[1].curValue.ToString() + " / " + attributes[1].maxValue.ToString();
					} else {
						Debug.Log ("The default configuration requires at least 2 attributes per character. Please add more attributes or change the configuration.");
					}
					
				}

			} else {
				Debug.Log ("Character with id" + charId.ToString() + " was not spawned due to the lack of spawn points.");
			}
		}
	}

	//Generating skills list
	void skillGen () {

		//Emptying list
		FunctionDB.core.emptyWindow(actionsWindow);

		//Getting character id
		var characterId = BattleManager.core.activeCharacterId;

		//Getting skill list
		var charIndex = FunctionDB.core.findCharacterIndexById (characterId);
		var character = Database.core.characters[charIndex];
		var skillList = character.skills;

		//Used to set focus on the first element instantiated
		bool focusSet = false;
		
		//Clearing current actions window
		BattleManager.core.curActions.Clear();

		foreach (int s in skillList) {

			//Getting skill data
			var skillIndex = FunctionDB.core.findSkillIndexById (s);
			var skill = Database.core.skills[skillIndex];

			if (skill.unlocked) {
				//Getting function data
				var functionsToCall = skill.functionsToCall;
				
				//Instantiating gameObject
				GameObject t = Instantiate (optionPrefab, actionsWindow.transform);

				//Set skill text
				t.transform.GetChild(0).gameObject.GetComponent<Text>().text = skill.name;
				
				//Getting button component
				Button b = t.GetComponent<Button>();

				//Setting focus
				if (!focusSet) {
					EventSystem.current.SetSelectedGameObject(t);
					focusSet = true;
				}

				//Set new listeners for each function
				//current turn points
				var curTp = BattleManager.core.turnPoints;

				//Setting listeners
				b.onClick.AddListener (delegate {
					
					if ((curTp - skill.turnPointCost) >= 0) {
						BattleManager.core.functionQueue = new List<callInfo>(functionsToCall);
						BattleManager.core.StartCoroutine(BattleManager.core.methodCaller ());
					} else {
						BattleManager.core.startWarningRoutine ("Insufficient turn points", 2f);
					}
					
				});

				//Adding element to current action list
				curActionInfo cai = new curActionInfo();
				cai.actionObject = t;
				cai.description = skill.description;
				cai.turnPointCost = skill.turnPointCost;

				BattleManager.core.curActions.Add (cai);
			}

			
		}

		//Instantiating back button
		instantiateBackButton(focusSet);

	}

	public void itemGen () {

		//Emptying list
		FunctionDB.core.emptyWindow(actionsWindow);

		//Getting character id
		var characterId = BattleManager.core.activeCharacterId;

		//Getting character
		var charIndex = FunctionDB.core.findCharacterIndexById (characterId);
		var character = Database.core.characters[charIndex];

		//Used for setting focus
		bool focusSet = false;

		//Clearing current actions window
		BattleManager.core.curActions.Clear();

		//For each item that the player possesses
		foreach (characterItemInfo itemInfo in character.items) {
			
			//If player has at least some of quantity of the item
			if (itemInfo.quantity > 0) {

				//Spawn Item

				//Item id
				var itemId = itemInfo.id;

				//Getting item
				var itemIndex = FunctionDB.core.findItemIndexById (itemId);
				var item = Database.core.items[itemIndex];

				//functions to call list
				var functionsToCall = item.functionsToCall;

				//Spawning option
				GameObject t = Instantiate (optionPrefab, actionsWindow.transform);

				//Setting item text
				t.transform.GetChild(0).gameObject.GetComponent<Text>().text = item.name;
				
				//Getting button component
				Button b = t.GetComponent<Button>();

				//Setting focus
				if (!focusSet) {
					EventSystem.current.SetSelectedGameObject(t);
					focusSet = true;
				}

				//current turn points
				var curTp = BattleManager.core.turnPoints;

				//Setting listeners
				b.onClick.AddListener (delegate {

					if ((curTp - item.turnPointCost) >= 0) {
						BattleManager.core.functionQueue = new List<callInfo>(functionsToCall);
						BattleManager.core.StartCoroutine(BattleManager.core.methodCaller ());
					} else {
						BattleManager.core.startWarningRoutine ("Insufficient turn points", 2f);
					}
					
				});

				//Adding element to current action list
				curActionInfo cai = new curActionInfo();
				cai.actionObject = t;
				cai.description = item.description;
				cai.turnPointCost = item.turnPointCost;

				BattleManager.core.curActions.Add (cai);
			}
			

		}

		//Instantiating back button
		instantiateBackButton(focusSet);

	}

	//This function generates a target list
	//Note: This function does not include complex filter, please feel free to add your own if necessary
	public void targetGen (bool genPlayerTeam, bool genEnemyTeam, int targetLimit) {

		//Emptying list
		FunctionDB.core.emptyWindow(actionsWindow);

		//Used for setting focus
		bool focusSet = false;

		//Setting targetLimit
		BattleManager.core.targetLimit = targetLimit;

		//Creating a list of ids to generate
		List<int> toGen = new List<int>();

		//Adding players team
		if (genPlayerTeam) {
			toGen.AddRange(BattleManager.core.activePlayerTeam);
		}

		//Adding enemy team
		if (genEnemyTeam) {
			toGen.AddRange(BattleManager.core.activeEnemyTeam);
		}

		//Excluding invalid characters
		for (int i = 0; i < toGen.Count; i++) {
			//Getting character
			var character = Database.core.characters[FunctionDB.core.findCharacterIndexById(toGen[i])];

			//If the character is not active, remove from list
			if (!character.isActive) {
				toGen.RemoveAt(i);
			}
		}

		//Cleaning action target List
		BattleManager.core.actionTargets.Clear();

		//Clearing current actions window
		BattleManager.core.curActions.Clear();

		foreach (int charId in toGen) {

			//Getting character
			var charIndex = FunctionDB.core.findCharacterIndexById (charId);
			var character = Database.core.characters[charIndex];

			//Spawning option
			GameObject t = Instantiate (optionPrefab, actionsWindow.transform);

			//Setting char name
			t.transform.GetChild(0).gameObject.GetComponent<Text>().text = character.name;
			
			//Setting focus
			if (!focusSet) {
				EventSystem.current.SetSelectedGameObject(t);
				focusSet = true;
			}

			//Getting button component
			Button b = t.GetComponent<Button>();

			b.onClick.AddListener (delegate {

				//Making sure that the character is not already in the target list
				var selectMore = BattleManager.core.targetLimit > BattleManager.core.actionTargets.Count;
				if (!BattleManager.core.actionTargets.Exists(x => x == charId) && selectMore) {
					BattleManager.core.actionTargets.Add (charId);
					selectMore = BattleManager.core.targetLimit > BattleManager.core.actionTargets.Count;
				} 
				
				if (!selectMore) {
					mainGen();
				}
				
			});

			//Adding element to current action list
			curActionInfo cai = new curActionInfo();
			cai.actionObject = t;
			cai.description = character.description;
			cai.turnPointCost = -1;

			BattleManager.core.curActions.Add (cai);

		}

		instantiateFinishSelectionButton();

	}

	//Terminates character selection
	void instantiateFinishSelectionButton () {

		//Instatiating back button
		GameObject backButtonObject = Instantiate (optionPrefab, actionsWindow.transform);
		backButtonObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Finish selection";

		//Getting Button
		Button backButton = backButtonObject.GetComponent<Button>();

		//Setting listeners
		backButton.onClick.AddListener ( delegate { 
					
			//Returning to main menu
			mainGen();

			//Terminating selection process
			BattleManager.core.targetLimit = BattleManager.core.actionTargets.Count;
			
		} );

		//Adding element to current action list
		curActionInfo cai = new curActionInfo();
		cai.actionObject = backButtonObject;
		cai.description = "Finish selection";
		cai.turnPointCost = -1;

		BattleManager.core.curActions.Add (cai);

	}

	//This function instantiates the back button
	void instantiateBackButton (bool focusSet) {

		//Instatiating back button
		GameObject backButtonObject = Instantiate (optionPrefab, actionsWindow.transform);
		backButtonObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Back";

		//Getting Button
		Button backButton = backButtonObject.GetComponent<Button>();

		//Setting listeners
		backButton.onClick.AddListener ( delegate { mainGen(); } );

		//Adding element to current action list
		curActionInfo cai = new curActionInfo();
		cai.actionObject = backButtonObject;
		cai.description = "Back";
		cai.turnPointCost = -1;

		BattleManager.core.curActions.Add (cai);

		//Setting focus
		if (!focusSet) {
			EventSystem.current.SetSelectedGameObject(backButtonObject);
			focusSet = true;
		}

	}

	//This function nullifies turn points, ending the turn
	void endTurn () {
		BattleMethods.core.subtractTurnPoints(-1, -1);
	}

	void Awake () { if (core == null) { core = this; } }
}


//(c) Cination - Tsenkilidis Alexandros