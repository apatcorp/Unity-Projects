using System;

public class ComplexNumber {

    // cartesian
    float real = 0f;
    float imaginary = 0f;

    // polar
    float magnitude = 0f;
    float angleInRadians = 0f;

    public float Real
    {
        get
        {
            return real;
        }

        set
        {
            PolarCoords polarCoords = CartesianCoords.ToPolar(value, imaginary);

            real = value;
            magnitude = polarCoords.magnitude;
            angleInRadians = polarCoords.angleInRadias;
        }
    }

    public float Imaginary
    {
        get
        {
            return imaginary;
        }

        set
        {
            PolarCoords polarCoords = CartesianCoords.ToPolar(real, value);

            imaginary = value;
            magnitude = polarCoords.magnitude;
            angleInRadians = polarCoords.angleInRadias;
        }
    }

    public float Magnitude
    {
        get
        {
            return magnitude;
        }

        set
        {
            CartesianCoords cartesianCoords = PolarCoords.ToCartesian(value, angleInRadians);

            magnitude = value;
            real = cartesianCoords.real;
            angleInRadians = cartesianCoords.imaginary;
        }
    }

    public float AngleInRadians
    {
        get
        {
            return angleInRadians;
        }

        set
        {
            CartesianCoords cartesianCoords = PolarCoords.ToCartesian(magnitude, value);

            angleInRadians = value;
            real = cartesianCoords.real;
            angleInRadians = cartesianCoords.imaginary;
        }
    }

    public float AngleInDegrees
    {
        get
        {
            return angleInRadians * RadiansToDegree;
        }

        set
        {
            CartesianCoords cartesianCoords = PolarCoords.ToCartesian(magnitude, (value * DegreeToRadians));

            angleInRadians = value;
            real = cartesianCoords.real;
            angleInRadians = cartesianCoords.imaginary;
        }
    }

    public ComplexNumber ComplexConjugated
    {
        get
        {
            return new ComplexNumber(real, -imaginary);
        }  
    }

    public void ComplexConjugate ()
    {
        Imaginary = -Imaginary;
    }

    float DegreeToRadians
    {
        get
        {
            return (float)(Math.PI / 180);
        }
    }

    float RadiansToDegree
    {
        get
        {
            return (float)(180 / Math.PI);
        }
    }

    public ComplexNumber(float real, float imaginary)
    {
        this.real = real;
        this.imaginary = imaginary;

        PolarCoords polarCoords = CartesianCoords.ToPolar(real, imaginary);
        magnitude = polarCoords.magnitude;
        angleInRadians = polarCoords.angleInRadias;
    }

    public ComplexNumber(float magnitude, float angle, Angle representation)
    {
        this.magnitude = magnitude;
        angleInRadians = (representation == Angle.RADIANS) ? angle: angle * DegreeToRadians;

        CartesianCoords cartesianCoords = PolarCoords.ToCartesian(magnitude, angleInRadians);
        real = cartesianCoords.real;
        imaginary = cartesianCoords.imaginary;
    }

    public static ComplexNumber operator +(ComplexNumber c1, ComplexNumber c2)
    {
         return new ComplexNumber(c1.real + c2.real, c1.imaginary + c2.imaginary);
    }

    public static ComplexNumber operator -(ComplexNumber c1, ComplexNumber c2)
    {
        return new ComplexNumber(c1.real - c2.real, c1.imaginary - c2.imaginary);
    }

    public static ComplexNumber operator *(ComplexNumber c1, ComplexNumber c2)
    {
        return new ComplexNumber(c1.magnitude * c2.magnitude, c1.angleInRadians + c2.angleInRadians, Angle.RADIANS);
    }

    public static ComplexNumber operator /(ComplexNumber c1, ComplexNumber c2)
    {
        return new ComplexNumber(c1.magnitude / c2.magnitude, c1.angleInRadians - c2.angleInRadians, Angle.RADIANS);
    }

    public static bool operator ==(ComplexNumber c1, ComplexNumber c2)
    {
        return c1.real == c2.real && c1.imaginary == c2.imaginary;
    }

    public static bool operator !=(ComplexNumber c1, ComplexNumber c2)
    {
        return !(c1.real == c2.real && c1.imaginary == c2.imaginary);
    }

    public static bool operator <(ComplexNumber c1, ComplexNumber c2)
    {
        return c1.real < c2.real && c1.imaginary < c2.imaginary;
    }

    public static bool operator >(ComplexNumber c1, ComplexNumber c2)
    {
        return c1.real > c2.real && c1.imaginary > c2.imaginary;
    }

    public static bool operator <=(ComplexNumber c1, ComplexNumber c2)
    {
        return c1.real <= c2.real && c1.imaginary <= c2.imaginary;
    }

    public static bool operator >=(ComplexNumber c1, ComplexNumber c2)
    {
        return c1.real >= c2.real && c1.imaginary >= c2.imaginary;
    }

    public override bool Equals(object obj)
    {
        var item = obj as ComplexNumber;

        return item == null || real.Equals(item.real) && imaginary.Equals(item.imaginary);
    }

    public override int GetHashCode()
    {
        return real.GetHashCode() + imaginary.GetHashCode();
    }

    public override string ToString()
    {
        string resultCartesian = "", resultPolar = "", resultEuler = "";

        float real= (float) Math.Round(this.real, 2);
        float imaginary = (float)Math.Round(this.imaginary, 2);
        float magnitude = (float)Math.Round(this.magnitude, 2);
        float angleInDegrees = (float)Math.Round(angleInRadians * RadiansToDegree, 2);

        if (imaginary == 0)
        {
            resultCartesian = "[Cartesian: " + real + " ]";
        }
        else if (imaginary > 0)
        {
            resultCartesian = "[Cartesian: " + real + " + " + imaginary + "i ]";
        }
        else if (imaginary < 0)
        {
            resultCartesian = "[Cartesian: " + real + " - " + -imaginary + "i ]";
        }

        resultPolar = "[Polar: " + magnitude + " * (cos " + angleInDegrees + " + i * sin " + angleInDegrees + ") ]";
        resultEuler = "[Euler: " + magnitude + " * exp(i * " + angleInDegrees + ") ]";

        return resultCartesian + "\n" + resultPolar + "\n" + resultEuler;
    }

    private struct PolarCoords
    {
        public float magnitude { get; private set; }
        public float angleInRadias { get; private set; }

        public PolarCoords(float magnitude, float angleInRadias) : this()
        {
            this.magnitude = magnitude;
            this.angleInRadias = angleInRadias;
        }

        public static CartesianCoords ToCartesian(float magnitude, float angleInRadians)
        {
            float real = (float)(magnitude * Math.Cos(angleInRadians));
            float imagninary = (float)(magnitude * Math.Sin(angleInRadians));

            return new CartesianCoords(real, imagninary);
        }
    }

    private struct CartesianCoords
    {
        public float real { get; private set; }
        public float imaginary { get; private set; }

        public CartesianCoords(float real, float imaginary) : this()
        {
            this.real = real;
            this.imaginary = imaginary;
        }

        public static PolarCoords ToPolar(float real, float imaginary)
        {
            float magnitude = (float)Math.Sqrt((real * real) + (imaginary * imaginary));
            float angleInRadians = 0f;

            if (real > 0 && imaginary >= 0)
            {
                angleInRadians = (float)Math.Atan2(imaginary, real);
            }
            else if (real > 0 && imaginary < 0)
            {
                angleInRadians = (float)(Math.Atan2(imaginary, real) + 2 * Math.PI);
            }
            else if (real < 0 && imaginary > 0)
            {
                angleInRadians = (float)(Math.Atan2(imaginary, real));
            }
            else if (real < 0 && imaginary < 0)
            {
                angleInRadians = (float)(Math.Atan2(imaginary, real) + 2 * Math.PI);
            }
            else if (real == 0 && imaginary > 0)
            {
                angleInRadians = (float)(Math.PI / 2);
            }
            else if (real == 0 && imaginary < 0)
            {
                angleInRadians = (float)(3 * Math.PI / 2);
            }

            return new PolarCoords(magnitude, angleInRadians);
        }
    }
}

public enum Angle { DEGREES, RADIANS }
