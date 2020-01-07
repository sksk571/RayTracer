using System;
using System.Numerics;

namespace RayTracer
{
    class Util
    {
        // NOT THREADSAFE!
        private static Random rand = new Random();

        public static float Rand()
        {
            return (float)rand.NextDouble();
        }
        
        public static Vector3 RandInUnitSphere()
        {
            Vector3 p;
            do
            {
                p = 2.0f * new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble()) - Vector3.One;
            } while (p.LengthSquared() >= 1.0f);
            return p;
        }
    }
}