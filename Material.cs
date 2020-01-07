using System;
using System.Numerics;

namespace RayTracer
{
    abstract class Material
    {
        public abstract bool Scatter(in Ray rIn, in HitRecord hit, out Vector3 attenuation, out Ray scattered);
    }
}