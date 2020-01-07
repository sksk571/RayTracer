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

        public Camera(Vector3 vLookFrom, Vector3 vLookAt, Vector3 vUp, float vfov, float aspect)
        {
            float theta = vfov * (float)Math.PI/180f;
            float halfHeight = (float)Math.Tan(theta / 2);
            float halfWidth = aspect * halfHeight;

            Vector3 w = Vector3.Normalize(vLookFrom - vLookAt);
            Vector3 u = Vector3.Normalize(Vector3.Cross(vUp, w));
            Vector3 v = Vector3.Cross(w, u);
            this.origin = vLookFrom;
            this.lowerLeftCorner = vLookFrom-halfWidth*u-halfHeight*v-w;
            this.horizontal = 2*halfWidth*u;
            this.vertical = 2*halfHeight*v;
        }

        public Ray GetRay(float u, float v) => new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical - origin);
    }
}