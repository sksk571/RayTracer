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
            Vector3 reflected = VectorUtil.Reflect(rIn.Direction, hit.Normal);
            Vector3 outwardNormal;
            float niOverNt;
            if (Vector3.Dot(rIn.Direction, hit.Normal) > 0)
            {
                outwardNormal = -hit.Normal;
                niOverNt = refIndex;
            }
            else
            {
                outwardNormal = hit.Normal;
                niOverNt = 1.0f / refIndex;
            }
            Vector3 refracted = VectorUtil.Refract(rIn.Direction, outwardNormal, niOverNt);
            if (refracted != Vector3.Zero)
            {
                scattered = new Ray(hit.P, refracted);
            }
            else
            {
                scattered = new Ray(hit.P, reflected);
                //return false;
            }
            return true;
        }
    }
}