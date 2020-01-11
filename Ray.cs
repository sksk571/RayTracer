using System;
using System.Numerics;

namespace RayTracer
{
    struct Ray
    {
        private readonly Vector3 a;
        private readonly Vector3 b;

        public Ray(Vector3 a, Vector3 b)
        {
            this.a = a;
            this.b = b;
        }

        public Vector3 Origin => a;
        public Vector3 Direction => b;
        public Vector3 PointAtParameter(float t) => a + t*b;
    }

    struct Rays
    {
        public Rays(int n)
        {
            N = n;
            OriginX = new float[n];
            OriginY = new float[n];
            OriginZ = new float[n];

            DirectionX = new float[n];
            DirectionY = new float[n];
            DirectionZ = new float[n];
        }

        public readonly int N;

        public readonly float[] OriginX;
        public readonly float[] OriginY;
        public readonly float[] OriginZ;

        public readonly float[] DirectionX;
        public readonly float[] DirectionY;
        public readonly float[] DirectionZ;

        public void PointAtParameter(float[] t, float[] pX, float[] pY, float[] pZ)
        {
            int n = t.Length;
            int vectorSize = Vector<float>.Count;
            int nV = n / vectorSize;

            for (int i = 0, offset = 0; i < nV; ++i, offset += vectorSize)
            {
                Vector<float> aX = new Vector<float>(OriginX, offset);
                Vector<float> aY = new Vector<float>(OriginY, offset);
                Vector<float> aZ = new Vector<float>(OriginZ, offset);

                Vector<float> bX = new Vector<float>(DirectionX, offset);
                Vector<float> bY = new Vector<float>(DirectionY, offset);
                Vector<float> bZ = new Vector<float>(DirectionZ, offset);

                // a + t*b
                Vector<float> tV = new Vector<float>(t, offset);
                Vector.Add(aX, Vector.Multiply(tV, bX)).CopyTo(pX, offset);
                Vector.Add(aY, Vector.Multiply(tV, bY)).CopyTo(pY, offset);
                Vector.Add(aZ, Vector.Multiply(tV, bZ)).CopyTo(pZ, offset);
            }
        }
    }
}