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
    ElliptcalMotionData ellipseData = new ElliptcalMotionData();

    void OnEnable()
    {
        ellipticalMotion = serializedObject.FindProperty("ellipse");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();

        DrawEllipseConfiguration();

        EditorGUI.indentLevel -= 1;

        EditorGUILayout.EndVertical();

        serializedObject.Update();

        serializedObject.ApplyModifiedProperties();
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

        ellipseData.useOrbitalPeriod = EditorGUILayout.Toggle(new GUIContent("Use Orbital Period"), ellipseData.useOrbitalPeriod);
           
        if (ellipseData.useOrbitalPeriod)
        { 
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
        }


        ellipseData.HideElements(ref minRadius, ref maxRadius, ref semi_major_axis, ref semi_minor_axis, ref eccentricity, ref orbitalPeriod);

        ellipseData.CaclulateHiddenElementsValue(ref minRadius, ref maxRadius, ref semi_major_axis, ref semi_minor_axis, ref eccentricity, ref semi_latus_rectum, ref orbitalPeriod);
    }

    public class ElliptcalMotionData
    {
        public float minRadius;
        public float maxRadius;
        public float semi_major_axis;
        public float semi_minor_axis;
        public float eccentricity;
        public float semi_latus_rectum;
        public float orbitalPeriod;

        public bool minRadiusHidden, maxRadiusHidden;
        public bool smaHidden, smiHidden;
        public bool eccentricityHidden;
        public bool orbitalPeriodHidden;

        public bool useOrbitalPeriod;

        public void HideElements(ref SerializedProperty minRadius, ref SerializedProperty maxRadius, ref SerializedProperty semi_major_axis, ref SerializedProperty semi_minor_axis, ref SerializedProperty eccentricity, ref SerializedProperty orbitalPeriod)
        {
            HideMinRadius(ref minRadius);
            HideMaxRadius(ref maxRadius);
            HideSMA(ref semi_major_axis);
            HideSMI(ref semi_minor_axis);
            HideEccentricity(ref eccentricity);
            HideOrbitalPeriod(ref orbitalPeriod);
        }

        void HideMinRadius (ref SerializedProperty minRadius)
        {
            bool minRadiusHiddenBefore = minRadiusHidden;

            minRadiusHidden = (maxRadius > 0f && eccentricity > 0f && orbitalPeriod > 0f) || (maxRadius > 0f && eccentricity > 0f) ||  (semi_major_axis > 0f || semi_minor_axis > 0f);
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

            maxRadiusHidden = (minRadius > 0f && eccentricity > 0f) || (minRadius > 0f && orbitalPeriod > 0f) || (eccentricity > 0f && orbitalPeriod > 0f) || (semi_major_axis > 0f || semi_minor_axis > 0f);
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

            smaHidden = (semi_minor_axis > 0f && eccentricity > 0f) || (semi_minor_axis > 0f && orbitalPeriod > 0f) || (eccentricity > 0f && orbitalPeriod > 0f) || (maxRadius > 0f || minRadius > 0f);
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

            smiHidden = (semi_major_axis > 0f && eccentricity > 0f) || (semi_major_axis > 0f && orbitalPeriod > 0f) || (eccentricity > 0f && orbitalPeriod > 0f) || (maxRadius > 0f || minRadius > 0f);
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

            eccentricityHidden = (maxRadius > 0f && minRadius > 0f) || (maxRadius > 0f && orbitalPeriod > 0f) || (minRadius  > 0f && orbitalPeriod > 0f) || 
                                (semi_major_axis > 0f && semi_minor_axis > 0f) ||(semi_major_axis > 0f && orbitalPeriod > 0f) || (semi_minor_axis > 0f && orbitalPeriod > 0f);
            if (!eccentricityHidden)
            {
                if (eccentricityHiddenBefore)
                {
                    this.eccentricity = 0f;
                    eccentricity.floatValue = 0f;
                }
            }
        }

        void HideOrbitalPeriod(ref SerializedProperty orbitalPeriod)
        {
            bool orbitalPeriodHiddenBefore = orbitalPeriodHidden;

            orbitalPeriodHidden = (maxRadius > 0f && minRadius > 0f) || (maxRadius > 0f && eccentricity > 0f) || (minRadius > 0f && eccentricity > 0f) ||
                                (semi_major_axis > 0f && semi_minor_axis > 0f) || (semi_major_axis > 0f && eccentricity > 0f) || (semi_minor_axis > 0f && eccentricity > 0f);
            if (!orbitalPeriodHidden)
            {
                if (orbitalPeriodHiddenBefore)
                {
                    this.orbitalPeriod = 0f;
                    orbitalPeriod.floatValue = 0f;
                }
            }
        }

        public bool CaclulateHiddenElementsValue(ref SerializedProperty minRadius, ref SerializedProperty maxRadius, ref SerializedProperty semi_major_axis, ref SerializedProperty semi_minor_axis, ref SerializedProperty eccentricity, ref SerializedProperty semi_latus_rectum, ref SerializedProperty orbitalperiod)
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