using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EllipseDrawer)), CanEditMultipleObjects]
public class EllipseDrawerEditor : Editor
{
    SerializedProperty ellipsesList;

    List<EllipseData> ellipseDataList;

    void OnEnable()
    {
        ellipsesList = serializedObject.FindProperty("ellipses");
        ellipseDataList = new List<EllipseData>();

        for (int i = 0; i < ellipsesList.arraySize; i++)
        {
            ellipseDataList.Add(new EllipseData(0f, 0f, 0f, 0f, 0f));
        }
    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginVertical(new GUIStyle());
        EditorGUILayout.PropertyField(ellipsesList, new GUIContent("List of Ellipses"));

        if (ellipsesList.isExpanded)
            EditorGUILayout.PropertyField(ellipsesList.FindPropertyRelative("Array.size"), new GUIContent("Count", "Amount of ellipses to be drawn"));

        EditorGUI.indentLevel += 1;
        for (int i = 0; i < ellipsesList.arraySize; i++)
        {       
            if (ellipsesList.isExpanded)
            {
                DrawEllipseConfiguration(i);

                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }
        }
        EditorGUI.indentLevel -= 1;

        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }

    void DrawEllipseConfiguration (int index)
    {
        SerializedProperty ellipseConfig = ellipsesList.GetArrayElementAtIndex(index);
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

        EllipseData ellipseData = new EllipseData(ellipseDataList[index].minRadius, ellipseDataList[index].maxRadius, ellipseDataList[index].semi_major_axis, ellipseDataList[index].semi_minor_axis, ellipseDataList[index].eccentricity);
        ellipseData.Update(ref minRadius, ref maxRadius, ref semi_major_axis, ref semi_minor_axis, ref eccentricity);

        //Debug.Log(ellipseData.ToString());

        EditorGUILayout.PropertyField(ellipseConfig, new GUIContent("Ellipse " + (index + 1)));

        if (ellipseConfig.isExpanded)
        {
            EditorGUILayout.PropertyField(lineMaterial);
            EditorGUILayout.PropertyField(lineColour);
            EditorGUILayout.PropertyField(widthCurve);
            EditorGUILayout.PropertyField(widthMultiplier);
            EditorGUILayout.PropertyField(segments);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Ellipse", EditorStyles.boldLabel);

            EditorGUI.indentLevel += 1;
       
            EditorGUILayout.HelpBox("Set two parameters > 0 to define the ellipse", MessageType.Info, true);

            if (!ellipseData.minRadiusHidden)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(minRadius, new GUIContent("Min Radius", "Minimum Radius From Focus Point (Perihelion)"));
                if (EditorGUI.EndChangeCheck())
                {
                    ellipseData.minRadius = minRadius.floatValue;
                    ellipseData.semi_major_axis_hidden = ellipseData.semi_minor_axis_hidden = ellipseData.minRadius > 0f;

                    ellipseData.maxRadiusHidden = ellipseData.minRadius > 0f && ellipseData.eccentricity > 0f;
                    ellipseData.eccentricityHidden = ellipseData.minRadius > 0f && ellipseData.maxRadius > 0f;
                }
            }

            if (!ellipseData.maxRadiusHidden)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(maxRadius, new GUIContent("Max Radius", "Maximum Radius From Focus Point (Aphelion)"));
                if (EditorGUI.EndChangeCheck())
                {
                    ellipseData.maxRadius = maxRadius.floatValue;
                    ellipseData.semi_major_axis_hidden = ellipseData.semi_minor_axis_hidden = ellipseData.maxRadius > 0f;

                    ellipseData.minRadiusHidden = ellipseData.maxRadius > 0f && ellipseData.eccentricity > 0f;
                    ellipseData.eccentricityHidden = ellipseData.maxRadius > 0f && ellipseData.minRadius > 0f;
                }
            } 

            if (!ellipseData.semi_major_axis_hidden)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(semi_major_axis, new GUIContent("Semi-Major Axis", "The major axis of an ellipse is its longest diameter: the semi-major axis is one half of the major axis"));
                if (EditorGUI.EndChangeCheck())
                {
                    ellipseData.semi_major_axis = semi_major_axis.floatValue;
                    ellipseData.maxRadiusHidden = ellipseData.minRadiusHidden = ellipseData.semi_major_axis > 0f;

                    ellipseData.semi_minor_axis_hidden = ellipseData.semi_major_axis > 0f && ellipseData.eccentricity > 0f;
                    ellipseData.eccentricityHidden = ellipseData.semi_major_axis > 0f && ellipseData.semi_minor_axis > 0f;
                }
            }

            if (!ellipseData.semi_minor_axis_hidden)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(semi_minor_axis, new GUIContent("Semi-Minor Axis", "The minor axis of an ellipse is its shortest diameter: the semi-minor axis is one half of the minor axis"));
                if (EditorGUI.EndChangeCheck())
                {
                    ellipseData.semi_minor_axis = semi_minor_axis.floatValue;
                    ellipseData.maxRadiusHidden = ellipseData.minRadiusHidden = ellipseData.semi_minor_axis > 0f;

                    ellipseData.semi_major_axis_hidden = ellipseData.semi_minor_axis > 0f && ellipseData.eccentricity > 0f;
                    ellipseData.eccentricityHidden = ellipseData.semi_minor_axis > 0f && ellipseData.semi_major_axis > 0f;
                }
            }
            
            if (!ellipseData.eccentricityHidden)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(eccentricity, new GUIContent("Eccentricity", "Eccentricity [0, 1] is a measure of how much the conic section deviates from being circular (circle = 0, hyperbola = 1"));
                if (EditorGUI.EndChangeCheck())
                {
                    ellipseData.eccentricity = eccentricity.floatValue;

                    ellipseData.minRadiusHidden = ellipseData.eccentricity > 0f && (ellipseData.maxRadius > 0f || ellipseData.semi_major_axis > 0f || ellipseData.semi_minor_axis > 0f);
                    ellipseData.maxRadiusHidden = ellipseData.eccentricity > 0f && (ellipseData.minRadius > 0f || ellipseData.semi_major_axis > 0f || ellipseData.semi_minor_axis > 0f);
                    ellipseData.semi_major_axis_hidden = ellipseData.eccentricity > 0f || (ellipseData.semi_minor_axis > 0f || ellipseData.minRadius > 0f || ellipseData.maxRadius > 0f);
                    ellipseData.semi_minor_axis_hidden = ellipseData.eccentricity > 0f || (ellipseData.semi_major_axis > 0f || ellipseData.minRadius > 0f || ellipseData.maxRadius > 0f);

                }
            }

            if (ellipseData.minRadius > 0f && ellipseData.maxRadius > 0f)
            {
                semi_major_axis.floatValue = EllipseCalculation.Semi_Major_Axis_1(ellipseData.minRadius, ellipseData.maxRadius);
                semi_minor_axis.floatValue = EllipseCalculation.Semi_Minor_Axis_1(ellipseData.minRadius, ellipseData.maxRadius);
                semi_latus_rectum.floatValue = EllipseCalculation.Semi_Latus_Rectum(semi_major_axis.floatValue, semi_minor_axis.floatValue);
                eccentricity.floatValue = EllipseCalculation.Eccentricity_1(semi_major_axis.floatValue, semi_minor_axis.floatValue);

            }
            else if (ellipseData.minRadius > 0f && ellipseData.eccentricity > 0f)
            {              
                semi_major_axis.floatValue = EllipseCalculation.Semi_Major_Axis_5(ellipseData.minRadius, ellipseData.eccentricity);
                maxRadius.floatValue = EllipseCalculation.Max_Radius_1(semi_minor_axis.floatValue, ellipseData.eccentricity);
                semi_minor_axis.floatValue = EllipseCalculation.Semi_Minor_Axis_2(semi_major_axis.floatValue, ellipseData.eccentricity);
                semi_latus_rectum.floatValue = EllipseCalculation.Semi_Latus_Rectum(semi_major_axis.floatValue, semi_minor_axis.floatValue);
             
            }
            else if (ellipseData.maxRadius > 0f && ellipseData.eccentricity > 0f)
            {
                semi_major_axis.floatValue = EllipseCalculation.Semi_Major_Axis_6(ellipseData.maxRadius, ellipseData.eccentricity);
                minRadius.floatValue = EllipseCalculation.Min_Radius_1(semi_major_axis.floatValue, ellipseData.eccentricity);
                semi_minor_axis.floatValue = EllipseCalculation.Semi_Minor_Axis_2(semi_major_axis.floatValue, ellipseData.eccentricity);
                semi_latus_rectum.floatValue = EllipseCalculation.Semi_Latus_Rectum(semi_major_axis.floatValue, semi_minor_axis.floatValue);
            }
            else if (ellipseData.semi_major_axis > 0f && ellipseData.semi_minor_axis > 0f)
            {
                eccentricity.floatValue = EllipseCalculation.Eccentricity_1(semi_major_axis.floatValue, semi_minor_axis.floatValue);
                maxRadius.floatValue = EllipseCalculation.Max_Radius_1(ellipseData.semi_major_axis, eccentricity.floatValue);
                minRadius.floatValue = EllipseCalculation.Min_Radius_1(ellipseData.semi_major_axis, eccentricity.floatValue);
                semi_latus_rectum.floatValue = EllipseCalculation.Semi_Latus_Rectum(semi_major_axis.floatValue, semi_minor_axis.floatValue);
                
            }
            else if (ellipseData.semi_major_axis > 0f && ellipseData.eccentricity > 0f)
            {
                maxRadius.floatValue = EllipseCalculation.Max_Radius_1(ellipseData.semi_major_axis, ellipseData.eccentricity);
                semi_minor_axis.floatValue = EllipseCalculation.Semi_Minor_Axis_2(ellipseData.semi_major_axis, ellipseData.eccentricity);
                minRadius.floatValue = EllipseCalculation.Min_Radius_1(ellipseData.semi_major_axis, ellipseData.eccentricity);
                semi_latus_rectum.floatValue = EllipseCalculation.Semi_Latus_Rectum(semi_major_axis.floatValue, semi_minor_axis.floatValue);
            }
            else if (ellipseData.semi_minor_axis > 0f && ellipseData.eccentricity > 0f)
            {
                semi_major_axis.floatValue = EllipseCalculation.Semi_Major_Axis_2(ellipseData.semi_minor_axis, ellipseData.eccentricity);
                maxRadius.floatValue = EllipseCalculation.Max_Radius_1(semi_major_axis.floatValue, ellipseData.eccentricity);
                minRadius.floatValue = EllipseCalculation.Min_Radius_1(semi_major_axis.floatValue, ellipseData.eccentricity);
                semi_latus_rectum.floatValue = EllipseCalculation.Semi_Latus_Rectum(semi_major_axis.floatValue, semi_minor_axis.floatValue);
            }

            ellipseDataList[index] = ellipseData;

            EditorGUI.indentLevel -= 1;
        }
    }

    class EllipseData
    {
        public float minRadius = 0f, maxRadius = 0f;
        public float semi_major_axis = 0f, semi_minor_axis = 0f;
        public float eccentricity = 0f;

        public bool minRadiusHidden, maxRadiusHidden;
        public bool semi_major_axis_hidden, semi_minor_axis_hidden;
        public bool eccentricityHidden;

        public EllipseData(float minRadius, float maxRadius, float semi_major_axis, float semi_minor_axis, float eccentricity)
        {
            this.minRadius = minRadius;
            this.maxRadius = maxRadius;
            this.semi_major_axis = semi_major_axis;
            this.semi_minor_axis = semi_minor_axis;
            this.eccentricity = eccentricity;

            minRadiusHidden = (semi_minor_axis > 0f || semi_major_axis > 0f) || (maxRadius > 0f && eccentricity > 0f);
            maxRadiusHidden = (semi_minor_axis > 0f || semi_major_axis > 0f) || (minRadius > 0f && eccentricity > 0f);
            semi_major_axis_hidden = (minRadius > 0f || maxRadius > 0f) || (semi_minor_axis > 0f && eccentricity > 0f);
            semi_minor_axis_hidden = (minRadius > 0f || maxRadius > 0f) || (semi_major_axis > 0f && eccentricity > 0f);
            eccentricityHidden = (minRadius > 0f && maxRadius > 0f) || (semi_major_axis > 0f && semi_minor_axis > 0f);

            if (minRadiusHidden) minRadius = 0f;
            if (maxRadiusHidden) maxRadius = 0f;
            if (semi_major_axis_hidden) semi_major_axis = 0f;
            if (semi_minor_axis_hidden) semi_minor_axis = 0f;
            if (eccentricityHidden) eccentricity = 0f;
        }

        public void Update (ref SerializedProperty minRadius, ref SerializedProperty maxRadius, ref SerializedProperty semi_major_axis, ref SerializedProperty semi_minor_axis, ref SerializedProperty eccentricity)
        {
            if (minRadiusHidden)
                minRadius.floatValue = 0f;
          
            if (maxRadiusHidden)
                maxRadius.floatValue = 0f;

            if (semi_major_axis_hidden)
                semi_major_axis.floatValue = 0f;

            if (semi_minor_axis_hidden)
                semi_minor_axis.floatValue = 0f;

            if (eccentricityHidden)
                eccentricity.floatValue = 0f;
        }

        public override string ToString()
        {
            return "Min Radius: " + minRadius + " | Min Radius Hidden: " + minRadiusHidden + "\t" +
                    "Max Radius: " + maxRadius + " | Max Radius Hidden: " + maxRadiusHidden + "\n" +
                    "Semi-Major Axis: " + semi_major_axis + " | Semi-Major Axis Hidden: " + semi_major_axis_hidden + "\t" +
                    "Semi-Minor Axis: " + semi_minor_axis + " | Semi-Minor Axis Hidden: " + semi_minor_axis_hidden + "\t" +
                    "Eccentricity : " + eccentricity + " | Eccentricity Hidden: " + eccentricityHidden;
        }
    }
}
