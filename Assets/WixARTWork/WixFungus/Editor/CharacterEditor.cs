using UnityEditor;
using UnityEngine;
using System.Collections;
using Rotorz.ReorderableList;
using System.Collections.Generic;

namespace Fungus
{
	[CustomEditor (typeof(Character))]
	public class CharacterEditor : Editor
	{
		protected SerializedProperty nameTextProp;
		protected SerializedProperty nameColorProp;
        protected SerializedProperty textColorProp;
        protected SerializedProperty soundEffectProp;
		protected SerializedProperty portraitsProp;
		protected SerializedProperty portraitsFaceProp;
		protected SerializedProperty descriptionProp;
        protected SerializedProperty offsetionProp;
        protected SerializedProperty spineProfile;

        protected virtual void OnEnable()
		{
			nameTextProp    = serializedObject.FindProperty ("nameText");
			nameColorProp   = serializedObject.FindProperty ("nameColor");
            textColorProp   = serializedObject.FindProperty("textColor");
            soundEffectProp = serializedObject.FindProperty ("soundEffect");
			portraitsProp   = serializedObject.FindProperty ("portraits");
			portraitsFaceProp = serializedObject.FindProperty ("portraitsFace");
			descriptionProp = serializedObject.FindProperty ("description");
            offsetionProp   = serializedObject.FindProperty("offset");
            spineProfile    = serializedObject.FindProperty("spineProfile");
        }

		public override void OnInspectorGUI() 
		{
			serializedObject.Update();

			Character t = target as Character;

			EditorGUILayout.PropertyField(nameTextProp,     new GUIContent("名稱", "顯示於傳統對話框上的名稱"));
			EditorGUILayout.PropertyField(nameColorProp,    new GUIContent("代表色", "傳統對話框的名稱顏色或泡泡對話框的底圖顏色"));
            EditorGUILayout.PropertyField(textColorProp,    new GUIContent("文字色", "對話框內文字的顏色"));
            EditorGUILayout.PropertyField(soundEffectProp,  new GUIContent("文字音效", "角色說話時的音效，將會蓋過對話框設定"));
            EditorGUILayout.PropertyField(offsetionProp,    new GUIContent("偏移量", "用於泡泡對話框的基點偏移量"));
            EditorGUILayout.PropertyField(spineProfile,     new GUIContent("Spine立繪", "Spine製的立繪，使用animation"));
            EditorGUILayout.PropertyField(descriptionProp,  new GUIContent("描述", "提供角色的備註"));
            
            if (t.portraits != null &&
			    t.portraits.Count > 0)
			{
				t.profileSprite = t.portraits[0];
			}
			else
			{
				t.profileSprite = null;
			}
			
			if (t.profileSprite != null)
			{
				Texture2D characterTexture = t.profileSprite.texture;
				float aspect = characterTexture.width / (float)characterTexture.height;
				Rect previewRect = GUILayoutUtility.GetAspectRect(aspect, GUILayout.Width(100), GUILayout.ExpandWidth(true));
				if (characterTexture != null)
					GUI.DrawTexture(previewRect,characterTexture,ScaleMode.ScaleToFit,true,aspect);
			}

			ReorderableListGUI.Title(new GUIContent("頭像", "用於顯示在傳統對話框上的角色圖片"));
			ReorderableListGUI.ListField(portraitsProp);

			EditorGUILayout.HelpBox("所有的頭像應使用相同的的尺寸，以避免定位和銜接上的問題。", MessageType.Info);

			EditorGUILayout.Separator();

			string[] facingArrows = {
				"正面",
				"<--向左",
				"向右-->",
			};
			portraitsFaceProp.enumValueIndex = EditorGUILayout.Popup("Portraits Face", portraitsFaceProp.enumValueIndex, facingArrows);

			EditorGUILayout.Separator();

			EditorUtility.SetDirty(t);

			serializedObject.ApplyModifiedProperties();
		}
	}
}