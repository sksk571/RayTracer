using System.Numerics;

namespace RayTracer
{
    class Dielectric : Material
    {
        private readonly float refIndex;

        public Dielectric(float refIndex)
        {
            this.refIndex = refIndex;
        }

        public override bool Scatter(in Ray rIn, in HitRecord hit, out Vector3 attenuation, out Ray scattered)
        {
            attenuation = new Vector3(1.0f, 1.0f, 1.0f);
            Vector3 refracted = VectorUtil.Refract(rIn.Direction, hit.Normal, refIndex);
            if (refracted != Vector3.Zero &&
                Util.Rand() > VectorUtil.Schlick(rIn.Direction, hit.Normal, refIndex))
            {
                scattered = new Ray(hit.P, refracted);
            }
            else
            {
                Vector3 reflected = VectorUtil.Reflect(rIn.Direction, hit.Normal);
                scattered = new Ray(hit.P, reflected);
            }
            return true;
        }
    }
}