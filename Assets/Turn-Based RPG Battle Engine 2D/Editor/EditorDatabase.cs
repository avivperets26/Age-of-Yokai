using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using ClassDB;

//This script is responsible for the visual appearance of the Database script in the inspector.

[CustomEditor(typeof(Database))]
public class EditorDatabase : Editor {

	private static SerializedObject soTarget;

	private static Database myTarget;

	private string toolBarStr;

	private int modeBarInt = 0;
	private bool view;

	private static SerializedProperty class1;
	private static SerializedProperty class2;
	private static SerializedProperty class3;


	public static Vector2 scroll = Vector2.zero;
	public static Vector2 scroll2 = Vector2.zero;
	public static Vector2 scroll3 = Vector2.zero;

	private void OnEnable() {

		myTarget = (Database) target;
		soTarget = new SerializedObject (target);

		class1 = soTarget.FindProperty ("items");
		class2 = soTarget.FindProperty ("skills");
		class3 = soTarget.FindProperty ("characters");

	}

	public override void OnInspectorGUI() {

		soTarget.Update ();

		EditorGUI.BeginChangeCheck ();
		modeBarInt = GUILayout.Toolbar (modeBarInt, new string[] { "Main view", "Simple view" });
		GUILayout.Space (15);

		switch (modeBarInt) {
		case 0:
			view = true;
			break;
		case 1:
			view = false;
			break;
		}

		if (EditorGUI.EndChangeCheck()) {
			soTarget.ApplyModifiedProperties ();
		}

		if (view) {
			
			EditorGUI.BeginChangeCheck ();
			
			myTarget.tab = GUILayout.Toolbar (myTarget.tab, new string[] { "Items", "Skills", "Characters" });
			
			switch (myTarget.tab) {
			case 0:
				toolBarStr = "Items";
				break;
			case 1:
				toolBarStr = "Skills";
				break;
			case 3:
				toolBarStr = "Characters";
				break;
			default:
				toolBarStr = "Characters";
				break;
			}

			if (EditorGUI.EndChangeCheck ()) {
				soTarget.ApplyModifiedProperties ();
				GUI.FocusControl (null);
			}

			EditorGUI.BeginChangeCheck ();

			switch (toolBarStr) {

			case "Items":

				EditorGUI.indentLevel = 1;
				GUILayout.BeginVertical (EditorStyles.helpBox);
				
				Show (class1);

				GUILayout.Space (5);
				GUILayout.EndVertical ();
				EditorGUI.indentLevel = 0;

				break;

			case "Skills":

				EditorGUI.indentLevel = 1;
				GUILayout.BeginVertical (EditorStyles.helpBox);

				Show (class2);

				GUILayout.Space (5);
				GUILayout.EndVertical ();
				EditorGUI.indentLevel = 0;

				break;
				
			case "Characters":

				EditorGUI.indentLevel = 1;
				GUILayout.BeginVertical (EditorStyles.helpBox);

				Show (class3);

				GUILayout.Space (5);
				GUILayout.EndVertical ();
				EditorGUI.indentLevel = 0;

				break;

			default:
				break;
			}

			if (EditorGUI.EndChangeCheck ()) {
				soTarget.ApplyModifiedProperties ();
			}
				
		} else {
			
			EditorGUI.indentLevel = 1;
			EditorGUILayout.BeginVertical (EditorStyles.helpBox);

			base.DrawDefaultInspector ();

			GUILayout.Space (10);
			EditorGUILayout.EndVertical ();
			GUILayout.Space (15);
		}

	}

	public static void Show (SerializedProperty list) {

		EditorGUILayout.PropertyField(list);

		if (list.isExpanded && list == class1) {
			
			GUILayout.Space (5);
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("Array.size"));
			GUILayout.Space (5);

			float height;

			if (list.arraySize == 0) {
				height = 10;
			} else {
				height = Screen.height;
			}

			scroll = EditorGUILayout.BeginScrollView (scroll, GUILayout.Height(height));

			for (int i = 0; i < list.arraySize; i++) {

				GUILayout.Space (10);

				EditorGUILayout.BeginHorizontal ();

				EditorGUILayout.PropertyField (list.GetArrayElementAtIndex (i), new GUIContent (myTarget.items [i].name));

				ShowButtons (list, i);

				EditorGUILayout.EndHorizontal ();

				if (list.arraySize > i && list.GetArrayElementAtIndex (i).isExpanded) {

					item instance = myTarget.items [i];

					SerializedProperty nameLabel;
					SerializedProperty idLabel;
					SerializedProperty descriptionLabel;
					SerializedProperty tpcLabel;
					SerializedProperty ftcLabel;

					GUILayout.BeginVertical (EditorStyles.helpBox);

					nameLabel = soTarget.FindProperty (string.Format ("items.Array.data[{0}].name", i));
					idLabel = soTarget.FindProperty (string.Format ("items.Array.data[{0}].id", i));
					descriptionLabel = soTarget.FindProperty (string.Format ("items.Array.data[{0}].description", i));
					ftcLabel = soTarget.FindProperty (string.Format ("items.Array.data[{0}].functionsToCall", i));
					tpcLabel = soTarget.FindProperty (string.Format ("items.Array.data[{0}].turnPointCost", i));

					EditorGUILayout.PropertyField (nameLabel);
					EditorGUILayout.PropertyField (idLabel);
					EditorGUILayout.PropertyField (descriptionLabel);
					EditorGUILayout.PropertyField (tpcLabel);
					EditorGUILayout.PropertyField (ftcLabel, true);

					
					GUILayout.Space (15);
					GUILayout.EndVertical ();

				}


			}

			EditorGUILayout.EndScrollView ();


			GUILayout.Space (10);

			if (list.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton)) {
				list.arraySize += 1;
			}



		} else if (list.isExpanded && list == class2){

			GUILayout.Space (5);
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("Array.size"));
			GUILayout.Space (5);

			float height;

			if (list.arraySize == 0) {
				height = 10;
			} else {
				height = Screen.height;
			}

			scroll2 = EditorGUILayout.BeginScrollView (scroll2, GUILayout.Height(height));

			for (int i = 0; i < list.arraySize; i++) {

				GUILayout.Space (10);

				EditorGUILayout.BeginHorizontal ();

				EditorGUILayout.PropertyField (list.GetArrayElementAtIndex (i), new GUIContent (myTarget.skills [i].name));

				ShowButtons (list, i);

				EditorGUILayout.EndHorizontal ();

				if (list.arraySize > i && list.GetArrayElementAtIndex (i).isExpanded) {

					skill instance = myTarget.skills [i];

					SerializedProperty nameLabel;
					SerializedProperty idLabel;
					SerializedProperty descriptionLabel;
					SerializedProperty unlockedLabel;
					SerializedProperty tcpLabel;
					SerializedProperty ftcLabel;

					GUILayout.BeginVertical (EditorStyles.helpBox);

					nameLabel = soTarget.FindProperty (string.Format ("skills.Array.data[{0}].name", i));
					idLabel = soTarget.FindProperty (string.Format ("skills.Array.data[{0}].id", i));
					descriptionLabel = soTarget.FindProperty (string.Format ("skills.Array.data[{0}].description", i));
					unlockedLabel = soTarget.FindProperty (string.Format ("skills.Array.data[{0}].unlocked", i));
					tcpLabel = soTarget.FindProperty (string.Format ("skills.Array.data[{0}].turnPointCost", i));
					ftcLabel = soTarget.FindProperty (string.Format ("skills.Array.data[{0}].functionsToCall", i));

					EditorGUILayout.PropertyField (nameLabel);
					EditorGUILayout.PropertyField (idLabel);
					EditorGUILayout.PropertyField (descriptionLabel);
					EditorGUILayout.PropertyField (unlockedLabel);
					EditorGUILayout.PropertyField (tcpLabel);
					EditorGUILayout.PropertyField (ftcLabel, true);

					
					GUILayout.Space (15);
					GUILayout.EndVertical ();

				}


			}

			EditorGUILayout.EndScrollView ();


			GUILayout.Space (10);
			if (list.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton)) {
				list.arraySize += 1;
			}

		} else if (list.isExpanded && list == class3) {

			GUILayout.Space (5);
			EditorGUILayout.PropertyField (list.FindPropertyRelative ("Array.size"));
			GUILayout.Space (5);

			float height;

			if (list.arraySize == 0) {
				height = 10;
			} else {
				height = Screen.height;
			}

			scroll3 = EditorGUILayout.BeginScrollView (scroll3 ,GUILayout.Height(height));

			for (int i = 0; i < list.arraySize; i++) {

				GUILayout.Space (10);

				EditorGUILayout.BeginHorizontal ();

				EditorGUILayout.PropertyField (list.GetArrayElementAtIndex (i), new GUIContent (myTarget.characters [i].name));

				ShowButtons (list, i);

				EditorGUILayout.EndHorizontal ();

				if (list.arraySize > i && list.GetArrayElementAtIndex (i).isExpanded) {

					character instance = myTarget.characters [i];

					SerializedProperty nameLabel;
					SerializedProperty idLabel;
					SerializedProperty descriptionLabel;
					SerializedProperty iconLabel;
					SerializedProperty aiLabel;
					SerializedProperty acLabel;
					SerializedProperty skillsLabel;
					SerializedProperty itemsLabel;
					SerializedProperty isActiveLabel;
					SerializedProperty charAttrLabel;


					GUILayout.BeginVertical (EditorStyles.helpBox);

					nameLabel = soTarget.FindProperty (string.Format ("characters.Array.data[{0}].name", i));
					idLabel = soTarget.FindProperty (string.Format ("characters.Array.data[{0}].id", i));
					descriptionLabel = soTarget.FindProperty (string.Format ("characters.Array.data[{0}].description", i));
					iconLabel = soTarget.FindProperty (string.Format ("characters.Array.data[{0}].icon", i));
					aiLabel = soTarget.FindProperty (string.Format ("characters.Array.data[{0}].aiFunctions", i));
					acLabel = soTarget.FindProperty (string.Format ("characters.Array.data[{0}].animationController", i));
					charAttrLabel = soTarget.FindProperty (string.Format ("characters.Array.data[{0}].characterAttributes", i));
					skillsLabel = soTarget.FindProperty (string.Format ("characters.Array.data[{0}].skills", i));
					itemsLabel = soTarget.FindProperty (string.Format ("characters.Array.data[{0}].items", i));
					isActiveLabel = soTarget.FindProperty (string.Format ("characters.Array.data[{0}].isActive", i));

					EditorGUILayout.PropertyField (nameLabel);
					EditorGUILayout.PropertyField (idLabel);
					EditorGUILayout.PropertyField (descriptionLabel);
					EditorGUILayout.PropertyField (iconLabel);
					EditorGUILayout.PropertyField (aiLabel, true);
					EditorGUILayout.PropertyField (acLabel, true);
					EditorGUILayout.PropertyField (charAttrLabel, true);
					EditorGUILayout.PropertyField (itemsLabel, true);
					EditorGUILayout.PropertyField (skillsLabel, true);
					EditorGUILayout.PropertyField (isActiveLabel, true);

					
					GUILayout.Space (15);
					GUILayout.EndVertical ();

				}


			}

			EditorGUILayout.EndScrollView ();


			GUILayout.Space (10);
			if (list.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton)) {
				list.arraySize += 1;
			}
		}

	}

	private static GUILayoutOption width = GUILayout.Width(35f);

	private static void ShowButtons (SerializedProperty list, int index) {

		if (GUILayout.Button(moveButtonContent, EditorStyles.miniButtonLeft, width)) {
			list.MoveArrayElement(index, index + 1);
		}

		if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonMid, width)) {
			list.InsertArrayElementAtIndex(index);
		}

		if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, width)) {
			
			int oldSize = list.arraySize;

			list.DeleteArrayElementAtIndex(index);

			if (list.arraySize == oldSize) {
				list.DeleteArrayElementAtIndex(index);
			}

		}
			
		list.serializedObject.ApplyModifiedProperties ();


	}

	private static GUIContent 
		moveButtonContent = new GUIContent("\u21b4", "move down"),
		duplicateButtonContent = new GUIContent("+", "duplicate"),
		deleteButtonContent = new GUIContent("-", "delete"),
		addButtonContent = new GUIContent("+", "add element");
}

//(c) Cination - Tsenkilidis Alexandros