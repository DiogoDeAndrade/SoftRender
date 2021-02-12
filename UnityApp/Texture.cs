using SoftRender.Engine;

namespace SoftRender.UnityApp
{
    public class Texture : Resource
    {
        Bitmap bitmap;

        public Texture(string name) : base(name)
        {

        }

        public bool Load(string filename)
        {
            bitmap = new Bitmap();
            return bitmap.Load(filename);
        }

        public static implicit operator Bitmap(Texture t) => t.bitmap;
    }
}
