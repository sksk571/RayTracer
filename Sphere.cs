using System;
using System.Numerics;

namespace RayTracer
{
    class Sphere : IHittable
    {
        private readonly Vector3 center;
        private readonly float radius;

        public Sphere(Vector3 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public bool Hit(in Ray r, float tMin, float tMax, ref HitRecord hit)
        {
            Vector3 oc = r.Origin - center;
            float a = Vector3.Dot(r.Direction, r.Direction);
            float b = 2.0f * Vector3.Dot(oc, r.Direction);
            float c = Vector3.Dot(oc, oc) - radius * radius;
            float discriminant = b*b - 4*a*c;

            if (discriminant < 0.0f)
                return false;
            else
            {
                float t = (-b - (float)Math.Sqrt(discriminant)) / (2.0f * a);
                // if (t < tMin || t > tMax)
                //     t = (-b + (float)Math.Sqrt(discriminant)) / (2.0f * a);
                if (t >= tMin && t <= tMax)
                {
                    hit.T = t;
                    hit.P = r.PointAtParameter(t);
                    hit.Normal = (hit.P - center) / radius;
                    return true;
                }
                return false;
            }
        }
    }
}