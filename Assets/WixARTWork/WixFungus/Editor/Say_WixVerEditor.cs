using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rotorz.ReorderableList;

namespace Fungus {

    [CustomEditor(typeof(Say_WixVer))]
    public class Say_WixVerEditor : SayEditor {

        protected SerializedProperty animationNameProp;
        protected SerializedProperty loopProp;
        protected SerializedProperty timeScaleProp;

        protected override void OnEnable() {
            base.OnEnable();
            animationNameProp = serializedObject.FindProperty("animationName");
            loopProp = serializedObject.FindProperty("loop");
            timeScaleProp = serializedObject.FindProperty("timeScale");
        }

        public override void DrawCommandGUI() {
            serializedObject.Update();

            bool showProfile = false;

            ObjectField(characterProp,
                        new GUIContent("角色", "Character that is speaking"),
                        new GUIContent("<無>"),
                        Character.activeCharacters);

            Say_WixVer t = target as Say_WixVer;

            // Only show portrait selection if...
            if (t.character != null &&                  // 角色已選擇
                t.character.spineProfile != null)       // 角色擁有spine立繪
            {
                showProfile = true;
            }

            if (showProfile) {
                Spine.SkeletonData sd = t.character.spineProfile.skeletonDataAsset.GetAnimationStateData().SkeletonData;
                //string[] animations = new string[t.character.spineProfile.skeleton.Data.Animations.Count + 1];
                string[] animations = new string[sd.Animations.Count + 1];

                animations[0] = "<未選取>";
                int animationIndex = 0;
                for (int i = 0; i < animations.Length - 1; i++) {
                    string name = sd.Animations.Items[i].Name;
                    animations[i + 1] = name;
                    if (name == animationNameProp.stringValue) animationIndex = i + 1;
                }

                animationIndex = EditorGUILayout.Popup("動畫", animationIndex, animations);

                string selectedAnimationName = animationIndex == 0 ? null : animations[animationIndex];
                if(t.animationName != selectedAnimationName) {
                    t.animationName = selectedAnimationName;
                    animationNameProp.stringValue = selectedAnimationName;
                }

                EditorGUILayout.PropertyField(loopProp);
                EditorGUILayout.PropertyField(timeScaleProp);

            }
            //else {
            //    if (!t.extendPrevious) {
            //        t.portrait = null;
            //    }
            //}

            EditorGUILayout.PropertyField(storyTextProp);

            EditorGUILayout.PropertyField(descriptionProp);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(extendPreviousProp);

            GUILayout.FlexibleSpace();

            if (GUILayout.Button(new GUIContent("Tag Help", "View available tags"), new GUIStyle(EditorStyles.miniButton))) {
                showTagHelp = !showTagHelp;
            }
            EditorGUILayout.EndHorizontal();

            if (showTagHelp) {
                DrawTagHelpLabel();
            }

            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(voiceOverClipProp,
                                          new GUIContent("Voice Over Clip", "Voice over audio to play when the text is displayed"));

            EditorGUILayout.PropertyField(showAlwaysProp);

            if (showAlwaysProp.boolValue == false) {
                EditorGUILayout.PropertyField(showCountProp);
            }

            GUIStyle centeredLabel = new GUIStyle(EditorStyles.label);
            centeredLabel.alignment = TextAnchor.MiddleCenter;
            GUIStyle leftButton = new GUIStyle(EditorStyles.miniButtonLeft);
            leftButton.fontSize = 10;
            leftButton.font = EditorStyles.toolbarButton.font;
            GUIStyle rightButton = new GUIStyle(EditorStyles.miniButtonRight);
            rightButton.fontSize = 10;
            rightButton.font = EditorStyles.toolbarButton.font;

            EditorGUILayout.PropertyField(fadeWhenDoneProp);
            EditorGUILayout.PropertyField(waitForClickProp);
            EditorGUILayout.PropertyField(setSayDialogProp);

            //if (showPortraits && t.portrait != null) {
            //    Texture2D characterTexture = t.portrait.texture;
            //    float aspect = (float)characterTexture.width / (float)characterTexture.height;
            //    Rect previewRect = GUILayoutUtility.GetAspectRect(aspect, GUILayout.Width(100), GUILayout.ExpandWidth(true));
            //    if (characterTexture != null) {
            //        GUI.DrawTexture(previewRect, characterTexture, ScaleMode.ScaleToFit, true, aspect);
            //    }
            //}

            serializedObject.ApplyModifiedProperties();
        }
    }
}