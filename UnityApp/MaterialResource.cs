using SoftRender.Engine;

namespace SoftRender.UnityApp
{
    public class MaterialResource : Resource
    {
        public Material material;

        public MaterialResource(string name, Material mat) : base(name)
        {
            material = mat;
        }
    }
}
