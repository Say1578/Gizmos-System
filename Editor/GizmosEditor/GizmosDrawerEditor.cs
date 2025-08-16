using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace GizmosSystem
{
    [CustomEditor(typeof(GizmosDrawer))]
    public class GizmosDrawerEditor : Editor
    {
        private SerializedProperty modulesProp;
        private GUIStyle titleStyle;
        private GUIStyle descStyle;
        private GUIStyle buttonStyle;

        private const float WideButtonWidth = 160;
        private const float RemoveButtonMax = 100f;

        private void OnEnable()
        {
            modulesProp = serializedObject.FindProperty("gizmoModules");
            InitializeStyle();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawCustomHeader();
            DrawModulesArea();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawCustomHeader()
        {
            EditorGUILayout.LabelField("Gizmos Modules", titleStyle);
            EditorGUILayout.Space();
        }

        private void DrawModulesArea()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(
                        modulesProp.arraySize == 0
                            ? "No modules found"
                            : $"Modules: {modulesProp.arraySize}",
                        EditorStyles.boldLabel
                    );

                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button("Add Module", GUILayout.MinWidth(1), GUILayout.MaxWidth(WideButtonWidth)))
                    {
                        ShowAddModuleMenu();
                    }
                }

                EditorGUILayout.Space();

                for (int i = 0; i < modulesProp.arraySize; i++)
                {
                    var element = modulesProp.GetArrayElementAtIndex(i);
                    DrawModuleElement(element, i);
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawModuleElement(SerializedProperty element, int index)
        {
            string typeName = element.managedReferenceValue != null
                ? element.managedReferenceValue.GetType().Name
                : "Null Module";

            EditorGUILayout.BeginVertical("HelpBox");
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(typeName, EditorStyles.boldLabel);

                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button("Remove", GUILayout.MinWidth(1), GUILayout.MaxWidth(RemoveButtonMax)))
                    {
                        modulesProp.DeleteArrayElementAtIndex(index);
                        return;
                    }
                }

                if (element.managedReferenceValue != null)
                {
                    EditorGUI.indentLevel++;
                    DrawProperties(element);
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawProperties(SerializedProperty prop)
        {
            var iterator = prop.Copy();
            var end = iterator.GetEndProperty(true);

            iterator.NextVisible(true); // Первая видимая проперти
            while (!SerializedProperty.EqualContents(iterator, end))
            {
                if (iterator.propertyPath != "m_Script")
                    EditorGUILayout.PropertyField(iterator, true);

                iterator.NextVisible(false);
            }
        }

        private void ShowAddModuleMenu()
        {
            GizmosModuleSelector.ShowWindow(AddModuleOfType);
        }

        private void AddModuleOfType(Type type)
        {
            Undo.RecordObject(target, "Add Gizmo Module");

            serializedObject.Update();

            int newIndex = modulesProp.arraySize;
            modulesProp.InsertArrayElementAtIndex(newIndex);
            var element = modulesProp.GetArrayElementAtIndex(newIndex);
            element.managedReferenceValue = Activator.CreateInstance(type);

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }

        private void InitializeStyle()
        {
            titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = EditorStyles.boldLabel.fontSize
            };

            descStyle = new GUIStyle(EditorStyles.label)
            {
                wordWrap = true,
                richText = true,
                fontSize = EditorStyles.label.fontSize,
                normal =
                {
                    textColor = EditorGUIUtility.isProSkin
                        ? new Color(0.85f, 0.85f, 0.85f)
                        : new Color(0.15f, 0.15f, 0.15f)
                }
            };

            buttonStyle = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = EditorStyles.label.fontSize,
                richText = false
            };
        }
    }
}