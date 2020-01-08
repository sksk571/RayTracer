using System;
using System.Numerics;

namespace RayTracer
{
    class Camera
    {
        private Vector3 origin;
        private Vector3 lowerLeftCorner;
        private Vector3 horizontal;
        private Vector3 vertical;

        private float lensRadius;
        private Vector3 u;
        private Vector3 v;
        private Vector3 w;

        public Camera(Vector3 vLookFrom, Vector3 vLookAt, Vector3 vUp, float vfov, float aspect, float aperture)
        {
            float theta = vfov * (float)Math.PI/180f;
            float halfHeight = (float)Math.Tan(theta / 2);
            float halfWidth = aspect * halfHeight;
            float focusDist = (vLookFrom - vLookAt).Length();

            this.lensRadius = aperture / 2f;

            this.w = Vector3.Normalize(vLookFrom - vLookAt);
            this.u = Vector3.Normalize(Vector3.Cross(vUp, w));
            this.v = Vector3.Cross(w, u);
            this.origin = vLookFrom;
            this.lowerLeftCorner = vLookFrom - focusDist*(halfWidth*u - halfHeight*v-w);
            this.horizontal = 2*focusDist*halfWidth*u;
            this.vertical = 2*focusDist*halfHeight*v;
        }

        public Ray GetRay(float s, float t)
        {
            //Vector3 rd = lensRadius * Util.RandInUnitDisk();
            //Vector3 offset = u * rd.X + v * rd.Y;
            Vector3 offset = Vector3.Zero;
            return new Ray(origin + offset, lowerLeftCorner + s * horizontal + t * vertical - origin - offset);
        }
    }
}