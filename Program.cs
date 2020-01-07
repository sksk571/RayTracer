using System;
using System.IO;
using System.Numerics;

namespace RayTracer
{
    class Program
    {
        static Random rand = new Random();

        static void Main(string[] args)
        {
            using (var writer = File.CreateText(@"./out/result.ppm"))
            {
                int nx = 200;
                int ny = 100;
                int ns = 100;
                writer.Write($"P3\n{nx} {ny}\n255\n");

                IHittable world = new HittableList(new[]
                { 
                    new Sphere(new Vector3(0,0,-1), 0.5f, new Lambertian(new Vector3(0.8f, 0.3f, 0.3f))),
                    new Sphere(new Vector3(0,-100.5f,-1), 100f, new Lambertian(new Vector3(0.8f, 0.8f, 0.0f))),
                    new Sphere(new Vector3(1,0,-1), 0.5f, new Metal(new Vector3(0.8f, 0.6f, 0.2f))),
                    new Sphere(new Vector3(-1,0,-1), 0.5f, new Metal(new Vector3(0.8f, 0.8f, 0.8f))),
                });
                Camera cam = new Camera();
    
                for (int j = ny - 1; j >= 0; --j)
                    for (int i = 0; i < nx; ++i)
                    {
                        Vector3 color = Vector3.Zero;
                        for (int s = 0; s < ns; ++s)
                        {
                            float u = ((float)i + Util.Rand()) / nx;
                            float v = ((float)j + Util.Rand()) / ny;
                            Ray r = cam.GetRay(u, v);
                            color += Color(r, world, 0);
                        }
                        color /= ns;
                        // gamma correction
                        color = new Vector3((float)Math.Sqrt(color.X), (float)Math.Sqrt(color.Y), (float)Math.Sqrt(color.Z));

                        int ir = (int)(255 * color.X);
                        int ig = (int)(255 * color.Y);
                        int ib = (int)(255 * color.Z);
                        writer.WriteLine("{0} {1} {2}", ir, ig, ib);
                    }
            }
        }

        static Vector3 Color(in Ray r, IHittable world, int depth)
        {
            HitRecord hit = new HitRecord();
            if (world.Hit(r, 0.001f, float.MaxValue, ref hit))
            {
                Ray scattered;
                Vector3 attenuation;
                if (depth < 50 && hit.Material.Scatter(r, hit, out attenuation, out scattered))
                {
                    return attenuation * Color(scattered, world, depth + 1);
                }
                else return Vector3.Zero;
           }

            Vector3 unitDirection = Vector3.Normalize(r.Direction);
            float t = 0.5f * (unitDirection.Y + 1.0f);
            return Vector3.Lerp(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0.5f, 0.7f, 1.0f), t);
        }
    }
}
