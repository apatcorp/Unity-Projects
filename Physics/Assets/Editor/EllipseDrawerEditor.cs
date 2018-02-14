using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
[CustomEditor(typeof(EllipseDrawer)), CanEditMultipleObjects]
public class EllipseDrawerEditor : Editor
{
    [SerializeField]
    SerializedProperty ellipsesList;

    [SerializeField]
    List<EllipseConfigData> ellipseConfigList;


    void OnEnable()
    {
        ellipsesList = serializedObject.FindProperty("ellipses");
        EllipseDrawer drawer = (EllipseDrawer)target;
        ellipseConfigList = drawer.ellipseConfigData;

    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical(new GUIStyle());
        EditorGUILayout.PropertyField(ellipsesList, new GUIContent("List of Ellipses"));

        if (ellipsesList.isExpanded)
        {
            EditorGUILayout.PropertyField(ellipsesList.FindPropertyRelative("Array.size"), new GUIContent("Count", "Amount of ellipses to be drawn"));   
        }

        EditorGUI.indentLevel += 1;
        for (int i = 0; i < ellipsesList.arraySize; i++)
        {
            if (ellipsesList.GetArrayElementAtIndex(i) == null)
                ellipsesList.DeleteArrayElementAtIndex(i);
              
            if (ellipsesList.isExpanded)
            {
                SerializedProperty ellipseConfig = ellipsesList.GetArrayElementAtIndex(i);
                DrawEllipseConfiguration(i, ellipseConfig);

                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }
        }
        EditorGUI.indentLevel -= 1;

        EditorGUILayout.EndVertical();

        if (GUI.changed)
            EditorUtility.SetDirty(target);

        serializedObject.Update();

        serializedObject.ApplyModifiedProperties();
    }

    void DrawEllipseConfiguration(int index, SerializedProperty ellipseConfig)
    {
        EllipseConfigData ellipseConfigData;

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

        if (ellipseConfigList.Count > 0)
        {
            ellipseConfigData = ellipseConfigList[index];
        } else
        {
            Debug.Log(ellipseConfig.displayName);
            ellipseConfigData = new EllipseConfigData(ellipseConfig);
            ellipseConfigList.Add(ellipseConfigData);
        }

        lineColour.colorValue = ellipseConfigData.lineColour;
        widthCurve.animationCurveValue = ellipseConfigData.widthCurve;
        widthMultiplier.floatValue = ellipseConfigData.widthMultiplier;
        segments.intValue = ellipseConfigData.segments;

        float currentMinRadius = ellipseConfigData.minRadius;
        float currentMaxRadius = ellipseConfigData.maxRadius;
        float currentSMA = ellipseConfigData.semi_major_axis;
        float currentSMI = ellipseConfigData.semi_minor_axis;
        float currentEccentricity = ellipseConfigData.eccentricity;

        EditorGUILayout.PropertyField(ellipseConfig, new GUIContent(ellipseConfig.displayName));

        if (ellipseConfig.isExpanded)
        {
            EditorGUILayout.PropertyField(lineMaterial);
            EditorGUILayout.PropertyField(lineColour);
            EditorGUILayout.PropertyField(widthCurve);
            EditorGUILayout.PropertyField(widthMultiplier);
            EditorGUILayout.PropertyField(segments);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Ellipse", EditorStyles.boldLabel);

            EditorGUILayout.HelpBox("Set two parameters > 0 to define the ellipse", MessageType.Info, true);
           
            /********** Min Radius *******/
            EditorGUI.BeginDisabledGroup(ellipseConfigData.minRadiusHidden);
            EditorGUI.BeginChangeCheck();
            currentMinRadius = EditorGUILayout.FloatField(new GUIContent("Min Radius", "Minimum Radius From Focus Point (Perihelion)"), currentMinRadius);
            //EditorGUILayout.PropertyField(minRadius, new GUIContent("Min Radius", "Minimum Radius From Focus Point (Perihelion)"));
            if (EditorGUI.EndChangeCheck())
            {
                ellipseConfigData.minRadius = currentMinRadius;
            }
            EditorGUI.EndDisabledGroup();
            /***************************/

            /********** Max Radius *******/
            EditorGUI.BeginDisabledGroup(ellipseConfigData.maxRadiusHidden);
            EditorGUI.BeginChangeCheck();
            currentMaxRadius = EditorGUILayout.FloatField(new GUIContent("Max Radius", "Maximum Radius From Focus Point (Aphelion)"), currentMaxRadius);
            //EditorGUILayout.PropertyField(maxRadius, new GUIContent("Max Radius", "Maximum Radius From Focus Point (Aphelion)"));
            if (EditorGUI.EndChangeCheck())
            {
                ellipseConfigData.maxRadius = currentMaxRadius;
                //ellipseData.maxRadius = maxRadius.floatValue;
            }
            EditorGUI.EndDisabledGroup();
            /***************************/

            /********** Semi-Major Axis *******/
            EditorGUI.BeginDisabledGroup(ellipseConfigData.smaHidden);
            EditorGUI.BeginChangeCheck();
            currentSMA = EditorGUILayout.FloatField(new GUIContent("Semi-Major Axis", "The major axis of an ellipse is its longest diameter: the semi-major axis is one half of the major axis"), currentSMA);
            //EditorGUILayout.PropertyField(semi_major_axis, new GUIContent("Semi-Major Axis", "The major axis of an ellipse is its longest diameter: the semi-major axis is one half of the major axis"));
            if (EditorGUI.EndChangeCheck())
            {
                ellipseConfigData.semi_major_axis = currentSMA;
            }
            EditorGUI.EndDisabledGroup();
            /***************************/

            /********** Semi-Minor Axis *******/
            EditorGUI.BeginDisabledGroup(ellipseConfigData.smiHidden);
            EditorGUI.BeginChangeCheck();
            currentSMI = EditorGUILayout.FloatField(new GUIContent("Semi-Minor Axis", "The minor axis of an ellipse is its shortest diameter: the semi-minor axis is one half of the minor axis"), currentSMI);
            //EditorGUILayout.PropertyField(semi_minor_axis, new GUIContent("Semi-Minor Axis", "The minor axis of an ellipse is its shortest diameter: the semi-minor axis is one half of the minor axis"));
            if (EditorGUI.EndChangeCheck())
            {
                ellipseConfigData.semi_minor_axis = currentSMI;
            }
            EditorGUI.EndDisabledGroup();
            /***************************/

            /********** Eccentricity *******/
            EditorGUI.BeginDisabledGroup(ellipseConfigData.eccentricityHidden);
            EditorGUI.BeginChangeCheck();
            //EditorGUILayout.PropertyField(eccentricity, new GUIContent("Eccentricity", "Eccentricity [0, 1] is a measure of how much the conic section deviates from being circular (circle = 0, hyperbola = 1"));
            currentEccentricity = EditorGUILayout.Slider(new GUIContent("Eccentricity", "Eccentricity [0, 1] is a measure of how much the conic section deviates from being circular (circle = 0, hyperbola = 1"), currentEccentricity, 0f, 1f);
            if (EditorGUI.EndChangeCheck())
            {
                ellipseConfigData.eccentricity = currentEccentricity;
            }
            EditorGUI.EndDisabledGroup();
            /***************************/

            ellipseConfigData.HideElements(ref minRadius, ref maxRadius, ref semi_major_axis, ref semi_minor_axis, ref eccentricity);

            ellipseConfigData.CaclulateHiddenElementsValue(ref minRadius, ref maxRadius, ref semi_major_axis, ref semi_minor_axis, ref eccentricity, ref semi_latus_rectum);

            ellipseConfigData.UpdateLineRenderConfigs(lineColour, widthCurve, widthMultiplier, segments);
        }
    }
}
