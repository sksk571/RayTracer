using System.Numerics;

namespace RayTracer
{
    struct Vector3V
    {
        public Vector3V(Vector3 v)
        {
            X = new Vector<float>(v.X);
            Y = new Vector<float>(v.Y);
            Z = new Vector<float>(v.Z);
        }

        public Vector3V(float[] x, float[] y, float[] z, int offset)
            : this(new Vector<float>(x, offset), new Vector<float>(y, offset), new Vector<float>(z, offset))
        {

        }

        public Vector3V(Vector<float> x, Vector<float> y, Vector<float> z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public readonly Vector<float> X;
        public readonly Vector<float> Y;
        public readonly Vector<float> Z;

        public void CopyTo(float[] x, float[] y, float[] z, int offset)
        {
            X.CopyTo(x, offset);
            Y.CopyTo(y, offset);
            Z.CopyTo(z, offset);
        }

        public static Vector3V Add(in Vector3V left, in Vector3V right)
        {
            return new Vector3V(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }
        public static Vector3V Sub(in Vector3V left, in Vector3V right)
        {
            return new Vector3V(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static Vector3V Mul(in Vector3V left, in Vector3V right)
        {
            return new Vector3V(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        }

        public static Vector3V Mul(in Vector3V left, in Vector<float> right)
        {
            return new Vector3V(left.X * right, left.Y * right, left.Z * right);
        }

        public static Vector3V Mul(in Vector<float> left, in Vector3V right)
        {
            return new Vector3V(left * right.X, left * right.Y, left * right.Z);
        }

        public static Vector<float> Dot(in Vector3V left, in Vector3V right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
        }

        public static Vector3V Normalize(in Vector3V v)
        {
            var length = Vector.SquareRoot(Dot(v, v));
            return new Vector3V(v.X / length, v.Y / length, v.Z / length);
        }

        public static Vector3V Lerp(in Vector3V left, in Vector3V right, Vector<float> t)
        {
            return (Vector<float>.One-t) * left + t * right;
        }

        public static Vector3V operator * (in Vector3V left, in Vector3V right)
        {
            return Mul(left, right);
        }
        public static Vector3V operator * (in Vector<float> left, in Vector3V right)
        {
            return Mul(left, right);
        }
        public static Vector3V operator * (in Vector3V left, in Vector<float> right)
        {
            return Mul(left, right);
        }
        public static Vector3V operator + (in Vector3V left, in Vector3V right)
        {
            return Add(left, right);
        }
        public static Vector3V operator - (in Vector3V left, in Vector3V right)
        {
            return Sub(left, right);
        }

        public override string ToString()
        {
            return $"X:{X}, Y:{Y}, Z:{Z}";
        }
    }    
}