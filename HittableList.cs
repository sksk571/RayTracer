namespace RayTracer
{
    class HittableList : IHittable
    {
        private readonly IHittable[] list;

        public HittableList(IHittable[] list)
        {
            this.list = list;
        }

        public bool Hit(in Ray ray, float tMin, float tMax, ref HitRecord hit)
        {
            bool hitAnything = false;
            float closest = tMax;
            for (int i = 0; i < list.Length; ++i)
            {
                if (list[i].Hit(ray, tMin, closest, ref hit))
                {
                    hitAnything = true;
                    closest = hit.T;
                }
            }
            return hitAnything;
        }
    }
}