using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
[CustomEditor(typeof(EllipticalMotion)), CanEditMultipleObjects]
public class EllipticalMotionEditor : Editor
{
    [SerializeField]
    SerializedProperty ellipticalMotion;

    [SerializeField]
    ElliptcalMotionData ellipseData;

    void OnEnable()
    {
        ellipticalMotion = serializedObject.FindProperty("ellipse");
        ellipseData = ((EllipticalMotion)target).ellipseMotionData;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();

        DrawEllipseConfiguration();

        EditorGUI.indentLevel -= 1;

        EditorGUILayout.EndVertical();
    }

    void DrawEllipseConfiguration()
    {
        SerializedProperty minRadius = ellipticalMotion.FindPropertyRelative("minRadius");
        SerializedProperty maxRadius = ellipticalMotion.FindPropertyRelative("maxRadius");
        SerializedProperty semi_major_axis = ellipticalMotion.FindPropertyRelative("semi_major_axis");
        SerializedProperty semi_minor_axis = ellipticalMotion.FindPropertyRelative("semi_minor_axis");
        SerializedProperty eccentricity = ellipticalMotion.FindPropertyRelative("eccentricity");
        SerializedProperty semi_latus_rectum = ellipticalMotion.FindPropertyRelative("semi_latus_rectum");
        SerializedProperty orbitalPeriod = ellipticalMotion.FindPropertyRelative("orbitalPeriod");

        float currentMinRadius = ellipseData.minRadius;
        float currentMaxRadius = ellipseData.maxRadius;
        float currentSMA = ellipseData.semi_major_axis;
        float currentSMI = ellipseData.semi_minor_axis;
        float currentEccentricity = ellipseData.eccentricity;
        float currentOrbitalPeriod = ellipseData.orbitalPeriod;

        EditorGUILayout.LabelField("Ellipse", EditorStyles.boldLabel);

        EditorGUILayout.HelpBox("Set two parameters > 0 to define the ellipse", MessageType.Info, true);

        if (EditorGUILayout.DropdownButton(new GUIContent("Reset"), FocusType.Keyboard))
        {
            Reset();
        }

        /********** Min Radius *******/
        EditorGUI.BeginDisabledGroup(ellipseData.minRadiusHidden);
        EditorGUI.BeginChangeCheck();
        currentMinRadius = EditorGUILayout.FloatField(new GUIContent("Min Radius", "Minimum Radius From Focus Point (Perihelion)"), currentMinRadius);
        //EditorGUILayout.PropertyField(minRadius, new GUIContent("Min Radius", "Minimum Radius From Focus Point (Perihelion)"));
        if (EditorGUI.EndChangeCheck())
        {
            ellipseData.minRadius = currentMinRadius;
        }
        EditorGUI.EndDisabledGroup();
        /***************************/

        /********** Max Radius *******/
        EditorGUI.BeginDisabledGroup(ellipseData.maxRadiusHidden);
        EditorGUI.BeginChangeCheck();
        currentMaxRadius = EditorGUILayout.FloatField(new GUIContent("Max Radius", "Maximum Radius From Focus Point (Aphelion)"), currentMaxRadius);
        //EditorGUILayout.PropertyField(maxRadius, new GUIContent("Max Radius", "Maximum Radius From Focus Point (Aphelion)"));
        if (EditorGUI.EndChangeCheck())
        {
            ellipseData.maxRadius = currentMaxRadius;
            //ellipseData.maxRadius = maxRadius.floatValue;
        }
        EditorGUI.EndDisabledGroup();
        /***************************/

        /********** Semi-Major Axis *******/
        EditorGUI.BeginDisabledGroup(ellipseData.smaHidden);
        EditorGUI.BeginChangeCheck();
        currentSMA = EditorGUILayout.FloatField(new GUIContent("Semi-Major Axis", "The major axis of an ellipse is its longest diameter: the semi-major axis is one half of the major axis"), currentSMA);
        //EditorGUILayout.PropertyField(semi_major_axis, new GUIContent("Semi-Major Axis", "The major axis of an ellipse is its longest diameter: the semi-major axis is one half of the major axis"));
        if (EditorGUI.EndChangeCheck())
        {
            ellipseData.semi_major_axis = currentSMA;
        }
        EditorGUI.EndDisabledGroup();
        /***************************/

        /********** Semi-Minor Axis *******/
        EditorGUI.BeginDisabledGroup(ellipseData.smiHidden);
        EditorGUI.BeginChangeCheck();
        currentSMI = EditorGUILayout.FloatField(new GUIContent("Semi-Minor Axis", "The minor axis of an ellipse is its shortest diameter: the semi-minor axis is one half of the minor axis"), currentSMI);
        //EditorGUILayout.PropertyField(semi_minor_axis, new GUIContent("Semi-Minor Axis", "The minor axis of an ellipse is its shortest diameter: the semi-minor axis is one half of the minor axis"));
        if (EditorGUI.EndChangeCheck())
        {
            ellipseData.semi_minor_axis = currentSMI;
        }
        EditorGUI.EndDisabledGroup();
        /***************************/

        /********** Eccentricity *******/
        EditorGUI.BeginDisabledGroup(ellipseData.eccentricityHidden);
        EditorGUI.BeginChangeCheck();
        //EditorGUILayout.PropertyField(eccentricity, new GUIContent("Eccentricity", "Eccentricity [0, 1] is a measure of how much the conic section deviates from being circular (circle = 0, hyperbola = 1"));
        currentEccentricity = EditorGUILayout.Slider(new GUIContent("Eccentricity", "Eccentricity [0, 1] is a measure of how much the conic section deviates from being circular (circle = 0, hyperbola = 1"), currentEccentricity, 0f, 1f);
        if (EditorGUI.EndChangeCheck())
        {
            ellipseData.eccentricity = currentEccentricity;
        }
        EditorGUI.EndDisabledGroup();
        /***************************/

        /********** Orbital Period *******/
        EditorGUI.BeginDisabledGroup(ellipseData.orbitalPeriodHidden);
        EditorGUI.BeginChangeCheck();
        currentOrbitalPeriod = EditorGUILayout.FloatField(new GUIContent("Orbital Period", "Time it takes the orbiting object to complete one complete cycle"), currentOrbitalPeriod);
        if (EditorGUI.EndChangeCheck())
        {
            ellipseData.orbitalPeriod = currentOrbitalPeriod;
        }
        EditorGUI.EndDisabledGroup();
        /***************************/
        
        if (currentOrbitalPeriod > 0f)
        {
            EditorGUI.indentLevel += 1;
            ellipseData.orbitingMass = EditorGUILayout.FloatField(new GUIContent("Orbiting Mass", "The Mass of the orbiting object"), ellipseData.orbitingMass);
            ellipseData.centralMass = EditorGUILayout.FloatField(new GUIContent("Central Mass", "The Mass of the central object"), ellipseData.centralMass);
            ellipseData.G = EditorGUILayout.FloatField(new GUIContent("G", "Gravitational Constant"), ellipseData.G);
            EditorGUI.indentLevel -= 1;
        }

        

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();

        ellipseData.HideElements(ref minRadius, ref maxRadius, ref semi_major_axis, ref semi_minor_axis, ref eccentricity, ref orbitalPeriod);

        ellipseData.CaclulateHiddenElementsValue(ref minRadius, ref maxRadius, ref semi_major_axis, ref semi_minor_axis, ref eccentricity, ref semi_latus_rectum, ref orbitalPeriod);

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }

    void Reset()
    {
        if (ellipseData != null)
        {
            ellipseData.maxRadius = 0f;
            ellipseData.minRadius = 0f;
            ellipseData.eccentricity = 0f;
            ellipseData.semi_major_axis = 0f;
            ellipseData.semi_minor_axis = 0f;
            ellipseData.semi_latus_rectum = 0f;
            ellipseData.orbitalPeriod = 0f;
        }
    }
}