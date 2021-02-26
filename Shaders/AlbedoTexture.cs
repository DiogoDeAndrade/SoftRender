using SoftRender.Engine;
using SoftRender.UnityApp;
using Mathlib;

namespace SoftRender.Shaders
{
    public class AlbedoTexture : Shader
    {
        Matrix4x4 cameraClipMatrix;
        Matrix4x4 objectClipMatrix;
        Material  material;

        public override FragmentProgram GetFragmentProgram()
        {
            return PixelShader;            
        }

        public override VertexProgram GetVertexProgram()
        {
            return VertexShader;
        }

        public override void Setup(Material material)
        {
            cameraClipMatrix = Camera.current.GetClipMatrix();
            objectClipMatrix = Renderer.current.transform.localToWorldMatrix * cameraClipMatrix;
            this.material = material;
        }

        private void VertexShader(FatVertex src, ref FatVertex dest)
        {
            dest.position = objectClipMatrix * src.position;
            dest.uv0 = src.uv0;
        }

        Color PixelShader(FatVertex src)
        {
            return PointSample(material.albedo, src.uv0);
        }
    }
}
