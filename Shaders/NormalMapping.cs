using SoftRender.Engine;
using SoftRender.UnityApp;
using System.Collections.Generic;
using Mathlib;

namespace SoftRender.Shaders
{
    public class NormalMapping : Shader
    {
        Matrix4x4   cameraClipMatrix;
        Matrix4x4   objectWorldMatrix;
        Matrix4x4   objectClipMatrix;
        Vector3     cameraPosition;
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
            cameraPosition = Camera.current.transform.position;
            lights = Light.allLights;
        }

        private void VertexShader(FatVertex src, ref FatVertex dest)
        {
            dest.position = objectClipMatrix * src.position;
            dest.normal = (objectWorldMatrix * new Vector4(src.normal, 0)).xyz;
            // color1 stores world position of the vertex
            dest.color1 = (objectWorldMatrix * src.position).xyz;

            dest.tangent = (objectWorldMatrix * new Vector4(src.tangent, 0)).xyz;
            dest.binormal = (objectWorldMatrix * new Vector4(src.binormal, 0)).xyz;

            dest.uv0 = src.uv0;
        }

        Color PixelShader(FatVertex src)
        {
            src.normal.Normalize();
            src.tangent.Normalize();
            src.binormal.Normalize();

            Matrix3x3 tbn = new Matrix3x3(src.normal, src.binormal, src.tangent);
            Vector3   normalMap = PointSample(material.normal, src.uv0) * 2.0f - 1.0f;
            Vector3   worldNormal = normalMap * tbn;

            Vector3 toCamera = (cameraPosition - (Vector3)src.color1).normalized;
            Vector3 worldPos = src.color1;

            Color lighting = Color.black;
            foreach (var light in lights)
            {
                // Only point light supported
                if (light.type != LightType.Point) continue;

                // Only point light supported
                if (light.type != LightType.Point) continue;

                // Remember: color1 stores world position of the vertex
                var toLight = light.transform.position - worldPos;
                var magnitude = toLight.magnitude;
                toLight /= magnitude;
                var attenuation = 10 / (magnitude * magnitude);

                // Add diffuse light
                lighting += Vector3.Dot(toLight.normalized, worldNormal) * light.intensity * light.color * material.baseColor;

                if (Vector3.Dot(toLight, worldNormal) > 0)
                {
                    // Add specular light
                    Vector3 h = (toCamera+ toLight).normalized;
                    lighting += Mathf.Pow(Vector3.Dot(worldNormal, h), material.specPower) * light.color * light.intensity * material.specIntensity;
                }
            }

            return lighting.saturated;
        }
    }
}
