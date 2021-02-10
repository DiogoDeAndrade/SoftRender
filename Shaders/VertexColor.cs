using SoftRender.Engine;
using SoftRender.UnityApp;

namespace SoftRender.Shaders
{
    public class VertexColor : Shader
    {
        Matrix4x4 cameraClipMatrix;

        public override FragmentProgram GetFragmentProgram()
        {
            return VertexColorFragment;            
        }

        public override VertexProgram GetVertexProgram()
        {
            return VertexColorProgram;
        }

        public override void Setup(Material material)
        {
            cameraClipMatrix = Camera.current.GetClipMatrix();
        }

        private void VertexColorProgram(FatVertex src, ref FatVertex dest)
        {
            dest.position = cameraClipMatrix * src.position;
            dest.color0 = src.color0;
        }

        Color VertexColorFragment(FatVertex src)
        {
            return src.color0;
        }
    }
}
