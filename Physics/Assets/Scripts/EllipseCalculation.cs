using UnityEngine;

public static class EllipseCalculation
{
    // calculating semi-major axis
    public static float Semi_Major_Axis_1(float radius_min, float radius_max)
    {
        return (radius_min + radius_max) / 2f;
    }

    public static float Semi_Major_Axis_2(float semi_minor_axis, float eccentricity)
    {
        return semi_minor_axis / (Mathf.Sqrt(1 - (eccentricity * eccentricity)));
    }

    public static float Semi_Major_Axis_3(float semi_latus_rectum, float eccentricity)
    {
        return semi_latus_rectum / (1 - (eccentricity * eccentricity));
    }

    public static float Semi_Major_Axis_4(float semi_minor_axis, float semi_latus_rectum)
    {
        return (semi_minor_axis * semi_minor_axis) / semi_latus_rectum;
    }

    public static float Semi_Major_Axis_5(float radius_min, float eccentricity)
    {
        return radius_min / (1 - eccentricity);
    }

    public static float Semi_Major_Axis_6(float radius_max, float eccentricity)
    {
        return radius_max / (1 + eccentricity);
    }


    // calculating semi-minor axis
    public static float Semi_Minor_Axis_1(float radius_min, float radius_max)
    {
        return Mathf.Sqrt(radius_min * radius_max);
    }

    public static float Semi_Minor_Axis_2(float semi_major_axis, float eccentricity)
    {
        return semi_major_axis * Mathf.Sqrt((1 - (eccentricity * eccentricity)));
    }

    public static float Semi_Minor_Axis_3(float semi_latus_rectum, float semi_major_axis)
    {
        return Mathf.Sqrt(semi_major_axis * semi_latus_rectum);
    }

    
    // calculating semi-latus rectum
    public static float Semi_Latus_Rectum(float semi_major_axis, float semi_minor_axis)
    {
        return (semi_minor_axis * semi_minor_axis) / semi_major_axis;
    }


    // calculating minimum and maximum radius
    public static float Min_Radius_1(float semi_major_axis, float eccentricity)
    {
        return semi_major_axis * (1 - eccentricity);
    }

    public static float Min_Radius_2(float semi_major_axis, float radius_max)
    {
        return 2f * semi_major_axis - radius_max;
    }

    public static float Min_Radius_3(float semi_minor_axis, float radius_max)
    {
        return (semi_minor_axis * semi_minor_axis) / radius_max;
    }

    public static float Max_Radius_1(float semi_major_axis, float eccentricity)
    {
        return semi_major_axis * (1 + eccentricity);
    }

    public static float Max_Radius_2(float semi_major_axis, float radius_min)
    {
        return 2f * semi_major_axis - radius_min;
    }

    public static float Max_Radius_3(float semi_minor_axis, float radius_min)
    {
        return (semi_minor_axis * semi_minor_axis) / radius_min;
    }


    // calculating eccentricity
    public static float Eccentricity_1(float semi_major_axis, float semi_minor_axis)
    {
        return Mathf.Sqrt(1 - ((semi_minor_axis * semi_minor_axis) / (semi_major_axis * semi_major_axis)));
    }

    public static float Eccentricity_2(float semi_major_axis, float semi_latus_rectum)
    {
        return Mathf.Sqrt(1 - (semi_latus_rectum / semi_major_axis));
    }

    public static float Eccentricity_3(float semi_major_axis, float radius_max)
    {
        return (radius_max / semi_major_axis) - 1f;
    }

    public static float Eccentricity_4(float semi_major_axis, float radius_min)
    {
        return 1f - (radius_min / semi_major_axis);
    }


    public static class Orbiting
    {
        public static float Semi_Major_Axis_5(float orbital_period, float G, float orbiting_mass, float central_mass)
        {
            return Mathf.Pow(((orbital_period * orbital_period) * (G * (central_mass + orbiting_mass)) / 4f * (Mathf.PI * Mathf.PI)), (1f / 3f));
        }

        public static float CalculateOrbitalPeriod(float semi_major_axis, float G, float orbiting_mass, float central_mass)
        {
            return 2f * Mathf.PI * Mathf.Sqrt(Mathf.Pow(semi_major_axis, 3) / (G * (central_mass + orbiting_mass)));
        }

        public static float CalculateRadiusAtAngle (float angleInRadians, float semi_latus_rectum, float eccentricity)
        {
           return semi_latus_rectum / (1 + eccentricity * Mathf.Cos(angleInRadians));
        }
    }
}

