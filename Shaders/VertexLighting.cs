using SoftRender.Engine;
using SoftRender.UnityApp;
using System.Collections.Generic;

namespace SoftRender.Shaders
{
    public class VertexLighting : Shader
    {
        Matrix4x4   cameraClipMatrix;
        Matrix4x4   objectWorldMatrix;
        Matrix4x4   objectClipMatrix;
        List<Light> lights;
        Material    material;

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
            this.material = material;
            lights = Light.allLights;
        }

        private void VertexShader(FatVertex src, ref FatVertex dest)
        {
            Vector3 worldPos = (objectWorldMatrix * src.position).xyz;
            dest.position = objectClipMatrix * src.position;
            dest.normal = (objectWorldMatrix * new Vector4(src.normal, 0)).xyz;

            Color lighting = Color.black;
            foreach (var light in lights)
            {
                // Only point light supported
                if (light.type != LightType.Point) continue;

                var toLight = light.transform.position - worldPos;
                var attenuation = 10 / toLight.magnitudeSquared;

                lighting += Vector3.Dot(toLight.normalized, dest.normal) * light.intensity * light.color * material.baseColor * attenuation;
            }

            dest.color0 = lighting.saturated;
        }

        Color PixelShader(FatVertex src)
        {
            return src.color0;
        }
    }
}
