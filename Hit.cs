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

    interface IHittable
    {
        bool Hit(in Ray ray, float tMin, float tMax, ref HitRecord hit);
    }
}