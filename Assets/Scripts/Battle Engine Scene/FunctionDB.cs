using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClassDB;
using UnityEngine.UI;

//This script contains functions used across multiple scripts
public class FunctionDB : MonoBehaviour {

	public static FunctionDB core;

	//The following functions are used to find certain element indexes in specified lists by given criteria
	public int findCharacterIndexById (int seekId) {
		return Database.core.characters.FindIndex(x => x.id == seekId);
	}

	public int findSkillIndexById (int seekId) {
		return Database.core.skills.FindIndex(x => x.id == seekId);
	}

	public int findItemIndexById (int seekId) {
		return Database.core.items.FindIndex(x => x.id == seekId);
	}

	public int findAttributeIndexById (int seekId, character c) {
		return c.characterAttributes.FindIndex(x => x.id == seekId);
	}

	public int findFunctionQueueIndexByCallInfo (callInfo ci) {
		return BattleManager.core.functionQueue.FindIndex(x => x == ci);
	}

	public int findFunctionQueueIndexByName (string s) {
		return BattleManager.core.functionQueue.FindIndex(x => x.functionName == s);
	}

	//Getting battle manager characters list index by id
	public int findBattleManagerCharactersIndexById (int id) {
		return BattleManager.core.characters.FindIndex(x => x.characterId == id);
	}

	//Getting audio clip by id
	public AudioClip findAudioClipById (int id) {

		foreach (audioInfo info in ObjectDB.core.AudioClips) {

			if (info.id == id) {
				return info.clip;
			}
		}

		return null;
	}

	//Getting sprite by id
	public Sprite findSpriteById (int id) {

		foreach (spriteInfo info in ObjectDB.core.Sprites) {

			if (info.id == id) {
				return info.sprite;
			}
		}

		return null;
	}

	//Getting character instance object by id
	public GameObject findCharInstanceById (int id) {

		foreach (characterInfo info in BattleManager.core.characters) {
			if (info.characterId == id) {
				return info.instanceObject;
			}
		}

		return null;
	}

	
	//Getting character spawn point by id
	public GameObject findCharSpawnById (int id) {

		foreach (characterInfo info in BattleManager.core.characters) {
			if (info.characterId == id) {
				return info.spawnPointObject;
			}
		}

		return null;
	}

	//This function returns a list of all child objects of a transform
	public List<GameObject> childObjects (Transform tL) {
		var l = new List<GameObject>();
		foreach (Transform t in tL) {
			l.Add(t.gameObject);
		}
		return l;
	}

	//This function sets character animations
	public void setAnimation (int characterId, string animationName) {

		//Getting character
		int characterIndex = findBattleManagerCharactersIndexById (characterId);
		characterInfo character = BattleManager.core.characters[characterIndex];

		//Setting animation
		character.currentAnimation = animationName;
	}

	//Checking animation status
	public bool checkAnimation (int characterId, string animationName) {

		//Getting character
		int characterIndex = findBattleManagerCharactersIndexById (characterId);
		var character = BattleManager.core.characters[characterIndex];
		var instance = character.instanceObject;
		Animator charAnimator = instance.GetComponent<Animator>();

		//Returning status
		return charAnimator.GetBool(animationName);

	}

	//This function deletes all children of a gameobject
	public void emptyWindow (GameObject w) {
		foreach (Transform i in w.transform) {
			Destroy(i.gameObject);
		}
	}

	
	//This method is used to get the first active character in a characters list
	public int activeCharacter (List<int> l, int startingIndex, int inc) {

		for (int e = startingIndex + inc; e < l.Count; e++) {
			if (Database.core.characters[FunctionDB.core.findCharacterIndexById(l[e])].isActive) {
				return e;
			}
		}

		return -1;
	}


	Coroutine audioTransitionRoutine;

	//Changing current audio
	public void changeAudio (int audioSourceIndex, int audioId) {
		if (audioTransitionRoutine != null) {
			StopCoroutine (audioTransitionRoutine);
		}

		audioTransitionRoutine = StartCoroutine (audioTransition(audioSourceIndex, audioId));
	}

	//This variable will be used by BattleManager's musicManager to determine whether an audio transition is in progress.
	[HideInInspector] public bool audioTransitioning;

	//Audio transition enumerator
	//Allows smooth transitions between currently playing clip and clip in question.
	IEnumerator audioTransition (int audioSourceIndex, int audioId) {

		audioTransitioning = true;

		//Getting audiosources
		var audio = GetComponents<AudioSource>();

		//Save current source volume
		var curVol = audio[audioSourceIndex].volume;

		//Mute source gradually
		while (audio[audioSourceIndex].volume > 0) {
			audio[audioSourceIndex].volume -= 0.01f;
			yield return new WaitForEndOfFrame();
		}

		//stopping audio
		stopAudio (audioSourceIndex);

		//Setting and playing new clip
		setAudio (audioSourceIndex, audioId);
		playAudio (audioSourceIndex);

		//Turning volume back on
		while (audio[audioSourceIndex].volume < curVol) {
			audio[audioSourceIndex].volume += 0.01f;
			yield return new WaitForEndOfFrame();
		}

		audioTransitioning = false;

	}

	//Playing audio once
	public void playAudioOnce (int audioSourceIndex, int audioId) {

		//Getting clip
		var clip = FunctionDB.core.findAudioClipById (audioId);

		//Getting audiosource(s)
		var audio = GetComponents<AudioSource>();

		//stopping audio
		if (audio.Length > audioSourceIndex) {
			audio[audioSourceIndex].PlayOneShot(clip);
		} else {
			Debug.Log ("Invalid source index " + audioSourceIndex.ToString());
		}
	}
	
	//Setting an audio clip is required to play, resume and pause the audio further on
	public void setAudio (int audioId, int audioSourceIndex) {
		//Getting clip
		var clip = FunctionDB.core.findAudioClipById (audioId);
		
		//Checking clip's validity
		if (clip != null) {
			//Getting audioSource(s)
			var audio = GetComponents<AudioSource>();

			//Setting audio
			if (audio.Length > audioSourceIndex) {
				audio[audioSourceIndex].clip = clip;
			} else {
				Debug.Log ("Invalid source index " + audioSourceIndex.ToString());
			}
			
		} else {
			Debug.Log ("Invalid audio clip id " + audioId.ToString());
		}
	}
	
	//Playing set audio
	public void playAudio (int audioSourceIndex) {

		//Getting audiosource(s)
		var audio = GetComponents<AudioSource>();

		//Playing audio
		if (audio.Length > audioSourceIndex) {
			audio[audioSourceIndex].Play();
		} else {
			Debug.Log ("Invalid source index " + audioSourceIndex.ToString());
		}

	}

	//Stopping audio
	public void stopAudio (int audioSourceIndex) {

		//Getting audiosource(s)
		var audio = GetComponents<AudioSource>();

		//stopping audio
		if (audio.Length > audioSourceIndex) {
			audio[audioSourceIndex].Stop();
		} else {
			Debug.Log ("Invalid source index " + audioSourceIndex.ToString());
		}
		
	}

	public void pauseAudio (int audioSourceIndex) {
		//Getting audiosource(s)
		var audio = GetComponents<AudioSource>();

		//pausing audio
		if (audio.Length > audioSourceIndex) {
			audio[audioSourceIndex].Pause();
		} else {
			Debug.Log ("Invalid source index " + audioSourceIndex.ToString());
		}
	}

	
	//Destroying a gameObject after a specified amount of time
	public IEnumerator destroyAfterTime (GameObject g, float time) {
		yield return new WaitForSeconds (time);
		Destroy(g);
	}

	//This function makes one gameObject follow another.
	public IEnumerator follow (GameObject sourceObject, GameObject targetObject, float xAdjustment, float yAdjustment) {

		while (sourceObject != null && targetObject != null) {

			Vector3 temp1 = targetObject.transform.position;
			temp1 = new Vector3 (temp1.x + xAdjustment, temp1.y + yAdjustment, sourceObject.transform.position.z);

			sourceObject.transform.position = temp1;

			yield return new WaitForEndOfFrame();
		}
	}

	//This function is used to display on-screen values such as damage
	public IEnumerator displayValue (GameObject target, float value, float xAdjustment, float yAdjustment) {

		//Getting coordinates
		Vector3 coordinates = target.transform.position;

		//Adjusted coordinates
		Vector3 newCoordinates = new Vector3 (coordinates.x + xAdjustment, coordinates.y + yAdjustment, coordinates.z);

		//Getting UI body element
		GameObject body = ObjectDB.core.battleUIBody;

		//Spawning Object
		GameObject g = Instantiate (ObjectDB.core.battleUIValuePrefab, newCoordinates, Quaternion.identity, body.transform);

		//Setting text
		g.GetComponent<Text>().text = value.ToString();

		//Making value follow target
		StartCoroutine(follow (g, target, xAdjustment, yAdjustment));

		yield return new WaitForSeconds (1);

		Destroy (g);

	}


	void Awake () { if (core == null) { core = this; } }

}


//(c) Cination - Tsenkilidis Alexandros