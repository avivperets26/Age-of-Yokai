using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//This script allows navigating through any scroll view using keys.
//This script must be attached to the scroll view
public class AutoScroll : MonoBehaviour {

	//Currently Selected object
	GameObject selectedObject;

	//Scroll view's content object
	GameObject contentObject;

	//The highest and lowest point of the scroll view
	Vector2 yboundaries;

	void Start () {

		//Getting content object
		contentObject = transform.GetChild(0).GetChild(0).gameObject;

		//Getting boundaries
		Vector3 [] corners = new Vector3 [4];
		this.gameObject.GetComponent<RectTransform>().GetWorldCorners(corners);

		yboundaries = new Vector2 (corners[0].y, corners[2].y);
	}

	void Update () {

		//Getting currently selected object
		selectedObject = EventSystem.current.currentSelectedGameObject;

		//For each child object to the content object
		foreach (Transform tr in contentObject.transform) {
			//If the object in question is the selected object
			if (tr.gameObject == selectedObject) {
				//Setting position adjustment value
				var adj = 0f;

				//While we exceed the boundaries in any direction, adjusting the position in the opposite direction (scrolling)
				while (tr.position.y < yboundaries[0] || tr.position.y > yboundaries[1]) {
					if (tr.position.y < yboundaries[0]) {
						adj += 0.01f;
					} else if (tr.position.y > yboundaries[1]) {
						adj -= 0.01f;
					}

					var pos = contentObject.transform.position;
					contentObject.transform.position = new Vector2 (pos.x, pos.y + adj);
				}
				
				//Breaking the loop since we already found what we were looking for.
				break;
			}
		}
	}

}


//(c) Cination - Tsenkilidis Alexandros