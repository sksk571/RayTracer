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
            this.lowerLeftCorner = vLookFrom - focusDist*halfWidth*u - focusDist*halfHeight*v - focusDist*w;
            this.horizontal = 2*focusDist*halfWidth*u;
            this.vertical = 2*focusDist*halfHeight*v;
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

            float[] rdX = new float[n];
            float[] rdY = new float[n];
            Util.RandInUnitDisk(rdX, rdY);

            for (int i = 0, offset = 0; i < nV; ++i, offset += vectorSize)
            {
                // offset = (u * lensRadius * rd.X + v * lensRadius * rd.Y)
                var offX = Vector.Add(new Vector<float>(v.X), Vector.Multiply(u.X, Vector.Multiply(lensRadius, new Vector<float>(rdX, offset))));
                var offY = Vector.Add(new Vector<float>(u.Y), Vector.Multiply(v.Y, Vector.Multiply(lensRadius, new Vector<float>(rdY, offset))));

                //
                var originX = Vector.Add(offX, new Vector<float>(origin.X));
                var originY = Vector.Add(offY, new Vector<float>(origin.Y));
                var originZ = new Vector<float>(origin.Z);
                
                // lowerLeftCorner + s * horizontal + t * vertical - origin
                var directionX = Vector.Subtract(Vector.Subtract(Vector.Add(Vector.Add(Vector.Multiply(horizontal.X, new Vector<float>(s, offset)), Vector.Multiply(vertical.X, new Vector<float>(t, offset))), new Vector<float>(lowerLeftCorner.X)), new Vector<float>(origin.X)), offX);
                var directionY = Vector.Subtract(Vector.Subtract(Vector.Add(Vector.Add(Vector.Multiply(horizontal.Y, new Vector<float>(s, offset)), Vector.Multiply(vertical.Y, new Vector<float>(t, offset))), new Vector<float>(lowerLeftCorner.Y)), new Vector<float>(origin.Y)), offY);
                var directionZ = Vector.Subtract(Vector.Add(Vector.Add(Vector.Multiply(horizontal.Z, new Vector<float>(s, offset)), Vector.Multiply(vertical.Z, new Vector<float>(t, offset))), new Vector<float>(lowerLeftCorner.Z)), new Vector<float>(origin.Z));

                originX.CopyTo(r.OriginX, offset);
                originY.CopyTo(r.OriginY, offset);
                originZ.CopyTo(r.OriginZ, offset);

                directionX.CopyTo(r.DirectionX, offset);
                directionY.CopyTo(r.DirectionY, offset);
                directionZ.CopyTo(r.DirectionZ, offset);
            }
        }
    }
}