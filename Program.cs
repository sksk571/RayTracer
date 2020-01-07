using System;
using System.IO;
using System.Numerics;

namespace RayTracer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var writer = File.CreateText(@"./out/result.ppm"))
            {
                int nx = 200;
                int ny = 100;
                writer.Write($"P3\n{nx} {ny}\n255\n");
                Vector3 lowerLeftCorner = new Vector3(-2.0f, -1.0f, -1.0f);
                Vector3 horizontal = new Vector3(4.0f, 0.0f, 0.0f);
                Vector3 vertical = new Vector3(0.0f, 2.0f, 0.0f);
                Vector3 origin = new Vector3(0.0f, 0.0f, 0.0f);
                for (int j = ny - 1; j >= 0; --j)
                    for (int i = 0; i < nx; ++i)
                    {
                        float u = ((float)i) / nx;
                        float v = ((float)j) / ny;
                        Ray r = new Ray(origin, lowerLeftCorner + u * horizontal + v * vertical);
                        Vector3 color = Color(r);
                        int ir = (int)(255 * color.X);
                        int ig = (int)(255 * color.Y);
                        int ib = (int)(255 * color.Z);
                        writer.WriteLine("{0} {1} {2}", ir, ig, ib);
                    }
            }
        }

        static Vector3 Color(in Ray r)
        {
            if (HitSphere(new Vector3(0, 0, -1), 0.5f, r))
                return new Vector3(1, 0, 0);
            Vector3 unitDirection = Vector3.Normalize(r.Direction);
            float t = 0.5f * (unitDirection.Y + 1.0f);
            return Vector3.Lerp(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.5f, 0.7f, 1.0f), t);
        }

        static bool HitSphere(in Vector3 center, float radius, in Ray r)
        {
            Vector3 oc = r.Origin - center;
            float a = Vector3.Dot(r.Direction, r.Direction);
            float b = 2.0f * Vector3.Dot(oc, r.Direction);
            float c = Vector3.Dot(oc, oc) - radius * radius;
            float discriminant = b*b - 4*a*c;
            return discriminant > 0;
        }
    }
}
