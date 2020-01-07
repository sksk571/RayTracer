using System.Numerics;

namespace RayTracer
{
    class Lambertian : Material
    {
        private readonly Vector3 albedo;

        public Lambertian(Vector3 albedo)
        {
            this.albedo = albedo; 
        }

        public override bool Scatter(in Ray rIn, in HitRecord hit, out Vector3 attenuation, out Ray scattered)
        {
            Vector3 target = hit.P + hit.Normal + Util.RandInUnitSphere();
            scattered = new Ray(hit.P, target - hit.P);
            attenuation = albedo;
            return true;
        }
    }
}