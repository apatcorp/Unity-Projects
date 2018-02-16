using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
[CustomEditor(typeof(KeplerOrbitalMotion)), CanEditMultipleObjects]
public class KeplerOrbitMotionEditor : Editor
{
    [SerializeField]
    SerializedProperty keplerOrbitList;
    [SerializeField]
    SerializedProperty massMultiplier;
    [SerializeField]
    SerializedProperty radiusMultiplier;

    private void OnEnable()
    {
        keplerOrbitList = serializedObject.FindProperty("ellipseConfigurations");

        massMultiplier = serializedObject.FindProperty("massMultiplier");
        radiusMultiplier = serializedObject.FindProperty("radiusMultiplier");

    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        massMultiplier.intValue = EditorGUILayout.IntField(new GUIContent("Mass Multiplier"), massMultiplier.intValue);
        radiusMultiplier.intValue =  EditorGUILayout.IntField(new GUIContent("Radius Multiplier"), radiusMultiplier.intValue);

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical(new GUIStyle());
        EditorGUILayout.PropertyField(keplerOrbitList, new GUIContent("List of Planets"));

        keplerOrbitList.arraySize = 9;
        keplerOrbitList.isExpanded = true;

        if (keplerOrbitList.isExpanded)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(keplerOrbitList.FindPropertyRelative("Array.size"), new GUIContent("Count", "Amount of plante motions to be modeled"));
            EditorGUI.EndDisabledGroup();
        }

        EditorGUI.indentLevel += 1;

        for (int i = 0; i < keplerOrbitList.arraySize; i++)
        {
            if (keplerOrbitList.GetArrayElementAtIndex(i) == null)
            {
                keplerOrbitList.DeleteArrayElementAtIndex(i);
            }

            if (keplerOrbitList.isExpanded)
            {
                SerializedProperty ellipseConfig = keplerOrbitList.GetArrayElementAtIndex(i);
                DrawEllipseConfiguration(i, ellipseConfig);

                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }
        }
        EditorGUI.indentLevel -= 1;

        EditorGUILayout.EndVertical();
    }

    void DrawEllipseConfiguration(int index, SerializedProperty ellipseConfig)
    {
        SerializedProperty lineMaterial = ellipseConfig.FindPropertyRelative("lineMaterial");
        SerializedProperty lineColour = ellipseConfig.FindPropertyRelative("lineColour");
        SerializedProperty widthCurve = ellipseConfig.FindPropertyRelative("widthCurve");
        SerializedProperty widthMultiplier = ellipseConfig.FindPropertyRelative("widthMultiplier");
        SerializedProperty segments = ellipseConfig.FindPropertyRelative("segments");

        SerializedProperty ellipse = ellipseConfig.FindPropertyRelative("ellipse");
        SerializedProperty minRadius = ellipse.FindPropertyRelative("minRadius");
        SerializedProperty maxRadius = ellipse.FindPropertyRelative("maxRadius");
        SerializedProperty semi_major_axis = ellipse.FindPropertyRelative("semi_major_axis");
        SerializedProperty semi_minor_axis = ellipse.FindPropertyRelative("semi_minor_axis");
        SerializedProperty eccentricity = ellipse.FindPropertyRelative("eccentricity");
        SerializedProperty semi_latus_rectum = ellipse.FindPropertyRelative("semi_latus_rectum");

        EditorGUILayout.PropertyField(ellipseConfig, new GUIContent(System.Enum.GetName(typeof(Name), (index))));

        if (ellipseConfig.isExpanded)
        {
            EditorGUILayout.PropertyField(lineMaterial);
            EditorGUILayout.PropertyField(lineColour);
            EditorGUILayout.PropertyField(widthCurve);
            EditorGUILayout.PropertyField(widthMultiplier);
            EditorGUILayout.PropertyField(segments);

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
    }
}
