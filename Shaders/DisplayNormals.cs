using SoftRender.Engine;
using SoftRender.UnityApp;
using Mathlib;

namespace SoftRender.Shaders
{
    public class DisplayNormals : Shader
    {
        Matrix4x4 cameraClipMatrix;
        Matrix4x4 objectWorldMatrix;
        Matrix4x4 objectClipMatrix;

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
            objectWorldMatrix = Renderer.current.transform.localToWorldMatrix;
            objectClipMatrix = objectWorldMatrix * cameraClipMatrix;
        }

        private void VertexShader(FatVertex src, ref FatVertex dest)
        {
            dest.position = objectClipMatrix * src.position;
            dest.normal = (objectWorldMatrix * new Vector4(src.normal, 0)).xyz;
        }

        Color PixelShader(FatVertex src)
        {
            return (Color)(src.normal.normalized * 0.5f + 0.5f);
        }
    }
}
