using SoftRender.Engine;
using SoftRender.UnityApp;
using System.Collections.Generic;

namespace SoftRender.Shaders
{
    public class PixelLighting : Shader
    {
        Matrix4x4   cameraClipMatrix;
        Matrix4x4   objectWorldMatrix;
        Matrix4x4   objectClipMatrix;
        Material    material;
        List<Light> lights;

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
            dest.position = objectClipMatrix * src.position;
            dest.normal = (objectWorldMatrix * new Vector4(src.normal, 0)).xyz;
            // Tangent stores world position of the vertex
            dest.tangent = (objectWorldMatrix * src.position).xyz;
        }

        Color PixelShader(FatVertex src)
        {
            Color lighting = Color.black;
            foreach (var light in lights)
            {
                // Only point light supported
                if (light.type != LightType.Point) continue;

                // Remember: tangent stores world position of the vertex
                var toLight = light.transform.position - src.tangent;
                var attenuation = 10 / toLight.magnitudeSquared;

                lighting += Vector3.Dot(toLight.normalized, src.normal.normalized) * light.intensity * material.baseColor * light.color * attenuation;
            }

            return lighting.saturated;
        }
    }
}
