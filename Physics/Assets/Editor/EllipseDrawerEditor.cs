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

    //Dictionary<int, EllipseConfigData> ellipseConfigDataDictionary = new Dictionary<int, EllipseConfigData>();
    [SerializeField]
    List<EllipseConfigData> ellipseConfigList = new List<EllipseConfigData>();


    void OnEnable()
    {
        ellipsesList = serializedObject.FindProperty("ellipses");
        //Debug.Log("START");
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


        /*if (!ellipseConfigDataDictionary.TryGetValue(ellipseConfig.displayName.GetHashCode(), out ellipseConfigData))
        {
            ellipseConfigData = new EllipseConfigData(ellipseConfig);
            ellipseConfigDataDictionary.Add(ellipseConfig.displayName.GetHashCode(), ellipseConfigData);
            Debug.Log(ellipseConfig.displayName);
        }*/
        //ellipseConfigData = ellipseConfigList[index];
        if (EllipseData.ellipseConfigList.Count > 0)
        {
            ellipseConfigData = EllipseData.ellipseConfigList[index];
        } else
        {
            Debug.Log(ellipseConfig.displayName);
            ellipseConfigData = new EllipseConfigData(ellipseConfig);
            EllipseData.ellipseConfigList.Add(ellipseConfigData);
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

            ellipseConfigData.UpdateLineRenderConfigs(lineColour.colorValue, widthCurve.animationCurveValue, widthMultiplier.floatValue, segments.intValue);
        }
    }

    public class EllipseConfigData
    {
        public Color lineColour;
        public AnimationCurve widthCurve;
        public float widthMultiplier;
        public int segments;

        public float minRadius;
        public float maxRadius;
        public float semi_major_axis;
        public float semi_minor_axis;
        public float eccentricity;
        public float semi_latus_rectum;

        public bool minRadiusHidden, maxRadiusHidden;
        public bool smaHidden, smiHidden;
        public bool eccentricityHidden;

        public EllipseConfigData(SerializedProperty ellipseData)
        {
            lineColour = ellipseData.FindPropertyRelative("lineColour").colorValue == Color.black ? Color.white : ellipseData.FindPropertyRelative("lineColour").colorValue;
            widthCurve = ellipseData.FindPropertyRelative("widthCurve").animationCurveValue;
            widthMultiplier = ellipseData.FindPropertyRelative("widthMultiplier").floatValue == 0 ? 1 : ellipseData.FindPropertyRelative("widthMultiplier").floatValue;
            segments = ellipseData.FindPropertyRelative("segments").intValue == 0 ? 100 : ellipseData.FindPropertyRelative("segments").intValue;
        }

        public void UpdateLineRenderConfigs(Color colour, AnimationCurve animCurve, float widthMultiplier, int segments)
        {
            lineColour = colour;
            widthCurve = animCurve;
            this.widthMultiplier = widthMultiplier;
            this.segments = segments;
        }

        public void HideElements(ref SerializedProperty minRadius, ref SerializedProperty maxRadius, ref SerializedProperty semi_major_axis, ref SerializedProperty semi_minor_axis, ref SerializedProperty eccentricity)
        {
            HideMinRadius(ref minRadius);
            HideMaxRadius(ref maxRadius);
            HideSMA(ref semi_major_axis);
            HideSMI(ref semi_minor_axis);
            HideEccentricity(ref eccentricity);
        }

        void HideMinRadius (ref SerializedProperty minRadius)
        {
            bool minRadiusHiddenBefore = minRadiusHidden;

            minRadiusHidden = (maxRadius > 0f && eccentricity > 0f) || (semi_major_axis > 0f || semi_minor_axis > 0f);
            if (!minRadiusHidden)
            {
                if (minRadiusHiddenBefore)
                {
                    this.minRadius = 0f;
                    minRadius.floatValue = this.minRadius;
                }
            }
        }

        void HideMaxRadius(ref SerializedProperty maxRadius)
        {
            bool maxRadiusHiddenBefore = maxRadiusHidden;

            maxRadiusHidden = (minRadius > 0f && eccentricity > 0f) || (semi_major_axis > 0f || semi_minor_axis > 0f);
            if (!maxRadiusHidden)
            {
                if (maxRadiusHiddenBefore)
                {
                    this.maxRadius = 0f;
                    maxRadius.floatValue = 0f;
                }
            }
        }

        void HideSMA(ref SerializedProperty semi_major_axis)
        {
            bool smaHiddenBefore = smaHidden;

            smaHidden = (semi_minor_axis > 0f && eccentricity > 0f) || (maxRadius > 0f || minRadius > 0f);
            if (!smaHidden)
            {
                if (smaHiddenBefore)
                {
                    this.semi_major_axis = 0f;
                    semi_major_axis.floatValue = 0f;
                }
            }
        }

        void HideSMI(ref SerializedProperty semi_minor_axis)
        {
            bool smiHiddenBefore = smiHidden;

            smiHidden = (semi_major_axis > 0f && eccentricity > 0f) || (maxRadius > 0f || minRadius > 0f);
            if (!smiHidden)
            {
                if (smiHiddenBefore)
                {
                    this.semi_minor_axis = 0f;
                    semi_minor_axis.floatValue = 0f;
                }
            }
        }

        void HideEccentricity(ref SerializedProperty eccentricity)
        {
            bool eccentricityHiddenBefore = eccentricityHidden;

            eccentricityHidden = (maxRadius > 0f && minRadius > 0f) || (semi_major_axis > 0f && semi_minor_axis > 0f);
            if (!eccentricityHidden)
            {
                if (eccentricityHiddenBefore)
                {
                    this.eccentricity = 0f;
                    eccentricity.floatValue = 0f;
                }
            }
        }

        public bool CaclulateHiddenElementsValue(ref SerializedProperty minRadius, ref SerializedProperty maxRadius, ref SerializedProperty semi_major_axis, ref SerializedProperty semi_minor_axis, ref SerializedProperty eccentricity, ref SerializedProperty semi_latus_rectum)
        {
            bool succesful = false;

            if (this.minRadius > 0f && this.maxRadius > 0f)
            {
                minRadius.floatValue = this.minRadius;
                maxRadius.floatValue = this.maxRadius;
                semi_major_axis.floatValue = EllipseCalculation.Semi_Major_Axis_1(minRadius.floatValue, maxRadius.floatValue);
                semi_minor_axis.floatValue = EllipseCalculation.Semi_Minor_Axis_1(minRadius.floatValue, maxRadius.floatValue);
                semi_latus_rectum.floatValue = EllipseCalculation.Semi_Latus_Rectum(semi_major_axis.floatValue, semi_minor_axis.floatValue);
                eccentricity.floatValue = EllipseCalculation.Eccentricity_1(semi_major_axis.floatValue, semi_minor_axis.floatValue);

                succesful = true;
            }
            else if (this.minRadius > 0f && this.eccentricity > 0f)
            {
                minRadius.floatValue = this.minRadius;
                eccentricity.floatValue = this.eccentricity;
                semi_major_axis.floatValue = EllipseCalculation.Semi_Major_Axis_5(minRadius.floatValue, eccentricity.floatValue);
                semi_minor_axis.floatValue = EllipseCalculation.Semi_Minor_Axis_2(semi_major_axis.floatValue, eccentricity.floatValue);
                maxRadius.floatValue = EllipseCalculation.Max_Radius_1(semi_minor_axis.floatValue, eccentricity.floatValue);
                semi_latus_rectum.floatValue = EllipseCalculation.Semi_Latus_Rectum(semi_major_axis.floatValue, semi_minor_axis.floatValue);

                succesful = true;
            }
            else if (this.maxRadius > 0f && this.eccentricity > 0f)
            {
                maxRadius.floatValue = this.maxRadius;
                eccentricity.floatValue = this.eccentricity;
                semi_major_axis.floatValue = EllipseCalculation.Semi_Major_Axis_6(maxRadius.floatValue, eccentricity.floatValue);
                minRadius.floatValue = EllipseCalculation.Min_Radius_1(semi_major_axis.floatValue, eccentricity.floatValue);
                semi_minor_axis.floatValue = EllipseCalculation.Semi_Minor_Axis_2(semi_major_axis.floatValue, eccentricity.floatValue);
                semi_latus_rectum.floatValue = EllipseCalculation.Semi_Latus_Rectum(semi_major_axis.floatValue, semi_minor_axis.floatValue);

                succesful = true;
            }
            else if (this.semi_major_axis > 0f && this.semi_minor_axis > 0f)
            {
                semi_major_axis.floatValue = this.semi_major_axis;
                semi_minor_axis.floatValue = this.semi_minor_axis;
                eccentricity.floatValue = EllipseCalculation.Eccentricity_1(semi_major_axis.floatValue, semi_minor_axis.floatValue);
                maxRadius.floatValue = EllipseCalculation.Max_Radius_1(semi_major_axis.floatValue, eccentricity.floatValue);
                minRadius.floatValue = EllipseCalculation.Min_Radius_1(semi_major_axis.floatValue, eccentricity.floatValue);
                semi_latus_rectum.floatValue = EllipseCalculation.Semi_Latus_Rectum(semi_major_axis.floatValue, semi_minor_axis.floatValue);

                succesful = true;
            }
            else if (this.semi_major_axis > 0f && this.eccentricity > 0f)
            {
                semi_major_axis.floatValue = this.semi_major_axis;
                eccentricity.floatValue = this.eccentricity;
                maxRadius.floatValue = EllipseCalculation.Max_Radius_1(semi_major_axis.floatValue, eccentricity.floatValue);
                semi_minor_axis.floatValue = EllipseCalculation.Semi_Minor_Axis_2(semi_major_axis.floatValue, eccentricity.floatValue);
                minRadius.floatValue = EllipseCalculation.Min_Radius_1(semi_major_axis.floatValue, eccentricity.floatValue);
                semi_latus_rectum.floatValue = EllipseCalculation.Semi_Latus_Rectum(semi_major_axis.floatValue, semi_minor_axis.floatValue);

                succesful = true;
            }
            else if (this.semi_minor_axis > 0f && this.eccentricity > 0f)
            {
                semi_minor_axis.floatValue = this.semi_minor_axis;
                eccentricity.floatValue = this.eccentricity;
                semi_major_axis.floatValue = EllipseCalculation.Semi_Major_Axis_2(semi_minor_axis.floatValue, eccentricity.floatValue);
                maxRadius.floatValue = EllipseCalculation.Max_Radius_1(semi_major_axis.floatValue, eccentricity.floatValue);
                minRadius.floatValue = EllipseCalculation.Min_Radius_1(semi_major_axis.floatValue, eccentricity.floatValue);
                semi_latus_rectum.floatValue = EllipseCalculation.Semi_Latus_Rectum(semi_major_axis.floatValue, semi_minor_axis.floatValue);

                succesful = true;
            }

            return succesful;
        }
    }
}


[System.Serializable]
public static class EllipseData
{
    [SerializeField]
    public static List<EllipseDrawerEditor.EllipseConfigData> ellipseConfigList = new List<EllipseDrawerEditor.EllipseConfigData>();
}
