using System.Numerics;

namespace RayTracer
{
    class Camera
    {
        private Vector3 origin;
        private Vector3 lowerLeftCorner;
        private Vector3 horizontal;
        private Vector3 vertical;

        public Camera()
        {
            this.lowerLeftCorner = new Vector3(-2.0f, -1.0f, -1.0f);
            this.horizontal = new Vector3(4.0f, 0.0f, 0.0f);
            this.vertical = new Vector3(0.0f, 2.0f, 0.0f);
            this.origin = new Vector3(0.0f, 0.0f, 0.0f);
        }

        public Ray GetRay(float u, float v) => new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical - origin);
    }
}