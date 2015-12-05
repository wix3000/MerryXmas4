using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

namespace SSG {

    [CustomEditor(typeof(SSGPlayer_Controller))]
    public class SSGPlayer_ControllerEditer : Editor {

        ReorderableList list;

        SerializedProperty playerModel;
        SerializedProperty interval;

        void OnEnable() {

            playerModel = serializedObject.FindProperty("playerModel");
            interval = serializedObject.FindProperty("interval");

            list = new ReorderableList(serializedObject, serializedObject.FindProperty("skillList"), true, true, true, true);
            list.elementHeight = EditorGUIUtility.singleLineHeight * 2f + 4f;
            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2f;

                EditorGUI.PrefixLabel(new Rect(rect.x, rect.y, 28f, EditorGUIUtility.singleLineHeight), new GUIContent("名稱"));
                EditorGUI.PropertyField(
                    new Rect(rect.x + 28f, rect.y, rect.width * 0.7f - 28f, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("skillName"), GUIContent.none);
                EditorGUI.PrefixLabel(new Rect(rect.x + rect.width * 0.7f + 8f, rect.y, 28f, EditorGUIUtility.singleLineHeight), new GUIContent("消耗"));
                EditorGUI.PropertyField(
                    new Rect(rect.x + rect.width * 0.7f + 36f, rect.y, rect.width * 0.3f - 36f, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("cost"), GUIContent.none);
                
                for (int i = 0; i < 6; i++) {
                    EditorGUI.PropertyField(
                        new Rect(rect.x + rect.width / 6f * i, rect.y + EditorGUIUtility.singleLineHeight + 2f, rect.width / 6f, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative("key" + (i + 1)), GUIContent.none);
                }
                
            };

            list.drawHeaderCallback = (Rect rect) => {
                EditorGUI.LabelField(rect, "技能組合");
            };
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUILayout.PropertyField(playerModel, new GUIContent("Player Model"));
            EditorGUILayout.PropertyField(interval, new GUIContent("按鍵間隔"));
            EditorGUILayout.Separator();
            list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }


    }
}
