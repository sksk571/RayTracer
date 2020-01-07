using System.Numerics;

namespace RayTracer
{
    class Metal : Material
    {
        private readonly Vector3 albedo;
        private readonly float fuzz;

        public Metal(Vector3 albedo, float fuzz)
        {
            this.albedo = albedo;
            this.fuzz = fuzz;
        }

        public override bool Scatter(in Ray rIn, in HitRecord hit, out Vector3 attenuation, out Ray scattered)
        {
            Vector3 reflected = VectorUtil.Reflect(Vector3.Normalize(rIn.Direction), hit.Normal);
            scattered = new Ray(hit.P, reflected + fuzz * Util.RandInUnitSphere());
            attenuation = albedo;
            return Vector3.Dot(scattered.Direction, hit.Normal) > 0;
        }
    }
}