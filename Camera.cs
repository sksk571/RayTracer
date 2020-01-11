using System;
using System.Numerics;

namespace RayTracer
{
    class Camera
    {
        private Vector3 origin;
        private Vector3V originV;
        private Vector3 lowerLeftCorner;
        private Vector3V lowerLeftCornerV;
        private Vector3 horizontal;
        private Vector3V horizontalV;
        private Vector3 vertical;
        private Vector3V verticalV;

        private float lensRadius;
        private Vector<float> lensRadiusV;

        private Vector3 u;
        private Vector3V uV;
        private Vector3 v;
        private Vector3V vV;
        private Vector3 w;
        private Vector3V wV;

        public Camera(Vector3 vLookFrom, Vector3 vLookAt, Vector3 vUp, float vfov, float aspect, float aperture)
        {
            float theta = vfov * (float)Math.PI/180f;
            float halfHeight = (float)Math.Tan(theta / 2);
            float halfWidth = aspect * halfHeight;
            float focusDist = (vLookFrom - vLookAt).Length();

            this.lensRadius = aperture / 2f;
            this.lensRadiusV = new Vector<float>(lensRadius);

            this.w = Vector3.Normalize(vLookFrom - vLookAt);
            this.u = Vector3.Normalize(Vector3.Cross(vUp, w));
            this.v = Vector3.Cross(w, u);

            this.wV = new Vector3V(w);
            this.uV = new Vector3V(u);
            this.vV = new Vector3V(v);

            this.origin = vLookFrom;
            this.lowerLeftCorner = vLookFrom - focusDist*halfWidth*u - focusDist*halfHeight*v - focusDist*w;
            this.horizontal = 2*focusDist*halfWidth*u;
            this.vertical = 2*focusDist*halfHeight*v;

            this.originV = new Vector3V(origin);
            this.lowerLeftCornerV = new Vector3V(lowerLeftCorner);
            this.horizontalV = new Vector3V(horizontal);
            this.verticalV = new Vector3V(vertical);
        }

        public Ray GetRay(float s, float t)
        {
            Vector3 offset = Vector3.Zero;
            if (lensRadius > 0.0f)
            {
                Vector3 rd = lensRadius * Util.RandInUnitDisk();
                offset = u * rd.X + v * rd.Y;            
            }

            return new Ray(origin + offset, lowerLeftCorner + s * horizontal + t * vertical - origin - offset);
        }

        public void GetRays(float[] s, float[] t, Rays r)
        {
            int n = s.Length;
            int vectorSize = Vector<float>.Count;
            int nV = n / vectorSize;

            for (int i = 0; i < n; i += vectorSize)
            {
                // offset = (u * lensRadius * rd.X + v * lensRadius * rd.Y)
                Vector3V rd = Util.RandInUnitDiskV();
                Vector3V offset = uV * rd.X + vV * rd.Y;
               
                Vector3V direction = lowerLeftCornerV + new Vector<float>(s, i) * horizontalV + new Vector<float>(t, i) * verticalV - originV - offset;

                //
                Vector3V origin = originV + offset;

                origin.CopyTo(r.OriginX, r.OriginY, r.OriginZ, i);
                direction.CopyTo(r.DirectionX, r.DirectionY, r.DirectionZ, i);
            }
        }
    }
}