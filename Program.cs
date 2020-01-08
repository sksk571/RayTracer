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

                IHittable world = RandomScene();
                Camera cam = new Camera(new Vector3(4,2f,2.5f), new Vector3(0,1,0), new Vector3(0,1,0), 45.0f, ((float)nx) / ny, 0.1f);

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

        static IHittable RandomScene()
        {
            int n = 488;
            IHittable[] list = new IHittable[n];
            list[0] = new Sphere(new Vector3(0, -1000f, 0), 1000, new Lambertian(new Vector3(0.5f, 0.5f, 0.5f)));
            int i = 1;
            for (int a = -11; a < 11; ++a)
                for (int b = -11; b < 11; ++b)
                {
                    float chooseMat = Util.Rand();
                    Vector3 center = new Vector3(a + 0.9f * Util.Rand(), 0.2f, b + 0.9f*Util.Rand());
                    //if ((center - new Vector3(4, 0.2f, 0)).Length() > 0.9f)
                    {
                        if (chooseMat < 0.8f)
                        {
                            list[i++] = new Sphere(center, 0.2f, new Lambertian(new Vector3(Util.Rand() * Util.Rand(),Util.Rand() * Util.Rand(),Util.Rand() * Util.Rand())));
                        }
                        else if (chooseMat < 0.95f)
                        {
                            list[i++] = new Sphere(center, 0.2f, 
                                new Metal(new Vector3(0.5f*(1 + Util.Rand()), 0.5f*(1 + Util.Rand()), 0.5f*(1 + Util.Rand())), 0.5f*(1 + Util.Rand())));
                        }
                        else
                        {
                            list[i++] = new Sphere(center, 0.2f, new Dielectric(1.5f));
                        }
                    }
                }
            list[i++] = new Sphere(new Vector3(0, 1, 0), 1.0f, new Dielectric(1.5f));
            list[i++] = new Sphere(new Vector3(-2, 1, 0), 1.0f, new Lambertian(new Vector3(0.4f, 0.2f, 0.1f)));
            list[i++] = new Sphere(new Vector3(2, 1, 0), 1.0f, new Metal(new Vector3(0.7f, 0.6f, 0.5f), 0.0f));
            return new HittableList(list);
        }
    }
}
