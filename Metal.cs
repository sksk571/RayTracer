using System.Numerics;

namespace RayTracer
{
    class Metal : Material
    {
        private readonly Vector3 albedo;

        public Metal(Vector3 albedo)
        {
            this.albedo = albedo;
        }

        public override bool Scatter(in Ray rIn, in HitRecord hit, out Vector3 attenuation, out Ray scattered)
        {
            Vector3 reflected = Reflect(Vector3.Normalize(rIn.Direction), hit.Normal);
            scattered = new Ray(hit.P, reflected);
            attenuation = albedo;
            return Vector3.Dot(scattered.Direction, hit.Normal) > 0;
        }

        private Vector3 Reflect(in Vector3 vector, in Vector3 normal)
        {
            return vector - 2 * Vector3.Dot(vector, normal) * normal;
        }
    }
}