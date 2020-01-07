using System.Numerics;

namespace RayTracer
{
    struct Ray
    {
        private readonly Vector3 a;
        private readonly Vector3 b;

        public Ray(Vector3 a, Vector3 b)
        {
            this.a = a;
            this.b = b;
        }

        public Vector3 Origin => a;
        public Vector3 Direction => b;
        public Vector3 PointAtParameter(float t) => a + t*b;
    }
}