using System;
using System.Numerics;

namespace RayTracer
{
    class Sphere : IHittable
    {
        private readonly Vector3 center;
        private readonly float radius;
        private readonly Material material;

        public Sphere(Vector3 center, float radius, Material material)
        {
            this.center = center;
            this.radius = radius;
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

            for (int i = 0, offset = 0; i < nV; ++i, offset += vectorSize)
            {
                Vector<float> ocX = Vector.Subtract(new Vector<float>(rays.OriginX, offset), new Vector<float>(center.X));
                Vector<float> ocY = Vector.Subtract(new Vector<float>(rays.OriginY, offset), new Vector<float>(center.Y));
                Vector<float> ocZ = Vector.Subtract(new Vector<float>(rays.OriginZ, offset), new Vector<float>(center.Z));

                var directionX = new Vector<float>(rays.DirectionX, offset);
                var directionY = new Vector<float>(rays.DirectionY, offset);
                var directionZ = new Vector<float>(rays.DirectionZ, offset);

                Vector<float> a = Vector.Add(Vector.Add(Vector.Multiply(directionX, directionX), Vector.Multiply(directionY, directionY)), Vector.Multiply(directionZ, directionZ));
                Vector<float> b = Vector.Multiply(2.0f, Vector.Add(Vector.Add(Vector.Multiply(ocX, directionX), Vector.Multiply(ocY, directionY)), Vector.Multiply(ocZ, directionZ)));
                Vector<float> c = Vector.Subtract(Vector.Add(Vector.Add(Vector.Multiply(ocX, ocX), Vector.Multiply(ocY, ocY)), Vector.Multiply(ocZ, ocZ)), new Vector<float>(radius * radius));
                Vector<float> discriminant = Vector.Multiply(Vector.Multiply(b, b), Vector.Multiply(4.0f, Vector.Multiply(a, c)));
                Vector<float> t = Vector.Divide(Vector.Subtract(Vector.Negate(b), Vector.SquareRoot(discriminant)), Vector.Multiply(2.0f, a));
                Vector<int> mask = Vector.BitwiseAnd(Vector.BitwiseAnd(Vector.GreaterThan(discriminant, Vector<float>.Zero), Vector.GreaterThanOrEqual(t, new Vector<float>(tMin))), Vector.LessThanOrEqual(t, new Vector<float>(tMax)));
                t = Vector.ConditionalSelect(mask, t, new Vector<float>(float.NaN));
                t.CopyTo(hits.T, offset);
                rays.PointAtParameter(hits.T, hits.PX, hits.PY, hits.PZ);
            }
        }
    }
}