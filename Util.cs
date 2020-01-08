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

        public static Vector3 RandInUnitDisk()
        {
            Vector3 p;
            do
            {
                p = 2.0f * new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), 0) - new Vector3(1.0f, 1.0f, 0.0f);
            } while (p.LengthSquared() >= 1.0f);
            return p;
        }
    }
}