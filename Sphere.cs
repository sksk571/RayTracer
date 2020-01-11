using System;
using System.Numerics;

namespace RayTracer
{
    class Sphere : IHittable
    {
        private readonly Vector3 center;
        private readonly Vector3V centerV;
        private readonly float radius;
        private readonly Vector<float> radiusV;
        private readonly Material material;

        public Sphere(Vector3 center, float radius, Material material)
        {
            this.center = center;
            this.centerV = new Vector3V(center);
            this.radius = radius;
            this.radiusV = new Vector<float>(radius);
            this.material = material;
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
                    hit.Material = material;
                    return true;
                }
                return false;
            }
        }

        public void Hit(in Rays rays, float tMin, float tMax, in Hits hits)
        {
            int n = rays.N;
            int vectorSize = Vector<float>.Count;
            int nV = n / vectorSize;

            var tMinV = new Vector<float>(tMin);
            var tMaxV = new Vector<float>(tMax);

            for (int i = 0; i< n; i += vectorSize)
            {
                Vector3V oc = new Vector3V(rays.OriginX, rays.OriginY, rays.OriginZ, i) - centerV;
                Vector3V direction = new Vector3V(rays.DirectionX, rays.DirectionY, rays.DirectionZ, i);
                Vector<float> a = Vector3V.Dot(direction, direction);
                Vector<float> b = 2.0f * Vector3V.Dot(oc, direction);
                Vector<float> c = Vector3V.Dot(oc, oc) - radiusV * radiusV;
                Vector<float> discriminant = b*b - 4*a*c;

                Vector<float> t = (-b -Vector.SquareRoot(discriminant)) / (2.0f * a);
                t = Vector.ConditionalSelect(
                    Vector.GreaterThan(discriminant, Vector<float>.Zero) & 
                    Vector.GreaterThanOrEqual(t, tMinV) & 
                    Vector.LessThanOrEqual(t, tMaxV), t, Vector<float>.Zero);
                t.CopyTo(hits.T, i);
            }

            rays.PointAtParameter(hits.T, hits.PX, hits.PY, hits.PZ);
        }
    }
}