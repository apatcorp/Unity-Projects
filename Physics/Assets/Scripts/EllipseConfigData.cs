using UnityEngine;
using UnityEditor;

[System.Serializable]
public class EllipseConfigData
{
    [SerializeField]
    public Color lineColour;
    [SerializeField]
    public AnimationCurve widthCurve;
    [SerializeField]
    public float widthMultiplier;
    [SerializeField]
    public int segments;
    [SerializeField]
    public float minRadius;
    [SerializeField]
    public float maxRadius;
    [SerializeField]
    public float semi_major_axis;
    [SerializeField]
    public float semi_minor_axis;
    [SerializeField]
    public float eccentricity;
    [SerializeField]
    public float semi_latus_rectum;
    [SerializeField]
    public bool minRadiusHidden, maxRadiusHidden;
    [SerializeField]
    public bool smaHidden, smiHidden;
    [SerializeField]
    public bool eccentricityHidden;

    public EllipseConfigData(SerializedProperty ellipseData)
    {
        lineColour = ellipseData.FindPropertyRelative("lineColour").colorValue == Color.black ? Color.white : ellipseData.FindPropertyRelative("lineColour").colorValue;
        widthCurve = ellipseData.FindPropertyRelative("widthCurve").animationCurveValue;
        widthMultiplier = ellipseData.FindPropertyRelative("widthMultiplier").floatValue == 0 ? 1 : ellipseData.FindPropertyRelative("widthMultiplier").floatValue;
        segments = ellipseData.FindPropertyRelative("segments").intValue == 0 ? 100 : ellipseData.FindPropertyRelative("segments").intValue;
    }

    public void UpdateLineRenderConfigs(SerializedProperty colour, SerializedProperty animCurve, SerializedProperty widthMultiplier, SerializedProperty segments)
    {
        lineColour = colour.colorValue;
        widthCurve = animCurve.animationCurveValue;
        this.widthMultiplier = widthMultiplier.floatValue;
        this.segments = segments.intValue;
    }

    public void HideElements(ref SerializedProperty minRadius, ref SerializedProperty maxRadius, ref SerializedProperty semi_major_axis, ref SerializedProperty semi_minor_axis, ref SerializedProperty eccentricity)
    {
        HideMinRadius(ref minRadius);
        HideMaxRadius(ref maxRadius);
        HideSMA(ref semi_major_axis);
        HideSMI(ref semi_minor_axis);
        HideEccentricity(ref eccentricity);
    }

    void HideMinRadius(ref SerializedProperty minRadius)
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