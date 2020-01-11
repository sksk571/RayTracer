using System.Numerics;

namespace RayTracer
{
    struct HitRecord
    {
        public float T;
        public Vector3 P;
        public Vector3 Normal;
        public Material Material;
    }

    struct Hits
    {
        public Hits(int n)
        {
            N = n;
            T = new float[n];
            PX = new float[n];
            PY = new float[n];
            PZ = new float[n];
            NormalX = new float[n];
            NormalY = new float[n];
            NormalZ = new float[n];
        }

        public readonly int N;

        public readonly float[] T;
        public readonly float[] PX;
        public readonly float[] PY;
        public readonly float[] PZ;

        public readonly float[] NormalX;
        public readonly float[] NormalY;
        public readonly float[] NormalZ;
    }

    interface IHittable
    {
        bool Hit(in Ray ray, float tMin, float tMax, ref HitRecord hit);
        void Hit(in Rays rays, float tMin, float tMax, in Hits hits);
    }
}