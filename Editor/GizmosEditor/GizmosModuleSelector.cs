using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace GizmosSystem
{
    public class GizmosModuleSelector : EditorWindow
    {
        private List<Type> moduleTypes;
        private string searchQuery = "";
        private Action<Type> onSelectCallback;

        private GUIStyle headerLabelStyle;
        private GUIStyle buttonStyle;

        private Vector2 scrollPos;

        private const float MinWindowWidth = 250;
        private const float MinWindowHeight = 300;
        private const float ButtonHeight = 20;

        private void OnEnable()
        {
            minSize = new Vector2(MinWindowWidth, MinWindowHeight);
            InitializeStyles();
        }

        private void InitializeStyles()
        {
            headerLabelStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 12
            };

            buttonStyle = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleLeft,
                padding = new RectOffset(13, 0, 0, 0)
            };
        }

        private void OnGUI()
        {
            DrawSearchBar();
            DrawModuleList();
        }

        public static void ShowWindow(Action<Type> onSelect)
        {
            var window = CreateInstance<GizmosModuleSelector>();
            window.titleContent = new GUIContent("Select Gizmo Module");
            window.onSelectCallback = onSelect;
            window.InitializeModules();
            window.ShowUtility();
        }

        private void InitializeModules()
        {
            var baseType = typeof(GizmoModuleBase);

            moduleTypes = TypeCache.GetTypesDerivedFrom(baseType)
                .Where(t => !t.IsAbstract && t.IsClass && t.GetConstructor(Type.EmptyTypes) != null)
                .OrderBy(t => t.Name)
                .ToList();
        }

        private void DrawSearchBar()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginChangeCheck();
                searchQuery = GUILayout.TextField(searchQuery, GUI.skin.FindStyle("SearchTextField"));
                if (EditorGUI.EndChangeCheck())
                    Repaint();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
        }

        private void DrawModuleList()
        {
            if (moduleTypes == null || moduleTypes.Count == 0)
            {
                EditorGUILayout.HelpBox("No modules found.", MessageType.Info);
                return;
            }

            Rect headerRect = GUILayoutUtility.GetRect(0, 20, GUILayout.ExpandWidth(true));

            Color headerColor = EditorGUIUtility.isProSkin
                ? new Color(0.22f, 0.22f, 0.22f) // Темная тема
                : new Color(0.76f, 0.76f, 0.76f); // Светлая тема

            EditorGUI.DrawRect(headerRect, headerColor);
            GUI.Label(headerRect, "Module", headerLabelStyle);

            var filteredModules = FilterModules(searchQuery);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            {
                foreach (var type in filteredModules)
                {
                    Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent(type.Name), buttonStyle, GUILayout.Height(ButtonHeight), GUILayout.ExpandWidth(true));

                    if (buttonRect.Contains(Event.current.mousePosition))
                    {
                        Color hoverColor = EditorGUIUtility.isProSkin
                            ? new Color(0.26f, 0.48f, 0.78f, 0.5f) // Темная тема
                            : new Color(0.26f, 0.48f, 0.78f, 0.3f); // Светлая тема

                        EditorGUI.DrawRect(buttonRect, hoverColor); 
                    }

                    GUI.Label(buttonRect, type.Name, buttonStyle);

                    if (GUI.Button(buttonRect, type.Name, buttonStyle))
                    {
                        onSelectCallback?.Invoke(type);
                        // Close(); // Закрываем окно после выбора
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private List<Type> FilterModules(string query)
        {
            return moduleTypes
                .Where(t => string.IsNullOrEmpty(query) ||
                            t.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }
    }
}
