using System;
using System.Numerics;

namespace RayTracer
{
    static class VectorUtil
    {
        public static Vector3 Reflect(in Vector3 v, in Vector3 normal)
        {
            return v - 2 * Vector3.Dot(v, normal) * normal;
        }

        public static Vector3 Refract(in Vector3 v, in Vector3 normal, float niOverNt)
        {
            Vector3 uv = Vector3.Normalize(v);
            float dt = Vector3.Dot(uv, normal);
            float discriminant = 1.0f - niOverNt*niOverNt*(1-dt*dt);
            if (discriminant > 0)
            {
                return niOverNt * (uv - normal * dt) - normal * (float)Math.Sqrt(discriminant);
            }
            return Vector3.Zero;
        }
    }
}