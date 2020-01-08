using System;
using System.Numerics;

namespace RayTracer
{
    class Util
    {
        [ThreadStatic]
        private static Random rand;// = new Random();

        public static void InitRandom(int seed)
        {
            rand = new Random(seed);
        }

        public static float Rand()
        {
            return (float)rand.NextDouble();
        }
        
        public static Vector3 RandInUnitSphere()
        {
            Vector3 p;
            do
            {
                p = 2.0f * new Vector3(Rand(), Rand(), Rand()) - Vector3.One;
            } while (p.LengthSquared() >= 1.0f);
            return p;
        }

        public static Vector3 RandInUnitDisk()
        {
            Vector3 p;
            do
            {
                p = 2.0f * new Vector3(Rand(), Rand(), 0) - new Vector3(1.0f, 1.0f, 0.0f);
            } while (p.LengthSquared() >= 1.0f);
            return p;
        }
    }
}