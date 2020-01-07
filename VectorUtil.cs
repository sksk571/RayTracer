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

        public static Vector3 Refract(in Vector3 v, in Vector3 normal, float refIndex)
        {
            Vector3 uv = Vector3.Normalize(v);
            Vector3 outwardNormal;
            float dt = Vector3.Dot(uv, normal);
            float niOverNt;
            if (dt > 0)
            {
                outwardNormal = -normal;
                niOverNt = refIndex;
            }
            else
            {
                outwardNormal = normal;
                niOverNt = 1.0f / refIndex;
            }

            float discriminant = 1.0f - niOverNt*niOverNt*(1-dt*dt);
            if (discriminant > 0)
            {
                return niOverNt * (uv - outwardNormal * dt) - outwardNormal * (float)Math.Sqrt(discriminant);
            }
            return Vector3.Zero;
        }

        public static float Schlick(in Vector3 v, in Vector3 normal, float refIndex)
        {
            float dt = Vector3.Dot(v, normal);
            float cosine;
            if (dt > 0)
            {
                cosine = refIndex*dt/v.Length();
            }
            else
            {
                cosine = -dt/v.Length();
            }
            float r0 = (1-refIndex) / (1+refIndex);
            r0 = r0 * r0;
            return r0 + (1-r0) * (float)Math.Pow((1-cosine), 5);
        }
    }
}