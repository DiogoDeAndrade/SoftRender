using SoftRender.Engine;
using SoftRender.UnityApp;
using SoftRender.UnityApp.SceneManagement;
using System.Collections.Generic;

namespace SoftRender.Shaders
{
    public class PhongLightingWithFog : Shader
    {
        Matrix4x4   cameraClipMatrix;
        Matrix4x4   objectWorldMatrix;
        Matrix4x4   objectClipMatrix;
        Vector3     cameraPosition;
        Material    material;
        Scene       scene;
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

            scene = SceneManager.GetActiveScene();
        }

        private void VertexShader(FatVertex src, ref FatVertex dest)
        {
            dest.position = objectClipMatrix * src.position;
            dest.normal = (objectWorldMatrix * new Vector4(src.normal, 0)).xyz;
            // Tangent stores world position of the vertex
            dest.tangent = (objectWorldMatrix * src.position).xyz;
            // Binormal stores to camera vector
            dest.binormal = cameraPosition - dest.tangent;
        }

        Color PixelShader(FatVertex src)
        {
            src.normal.Normalize();

            float distToCamera = src.binormal.magnitude;
            src.binormal /= distToCamera;

            Color lighting = scene.ambientLight * material.baseColor;
            foreach (var light in lights)
            {
                // Remember: tangent stores world position of the vertex, and binormal stores the to camera vector
                var toLight = light.transform.position - src.tangent;
                var magnitude = toLight.magnitude;
                toLight /= magnitude;
                var attenuation = 10 / (magnitude * magnitude);

                // Add diffuse light
                lighting += Vector3.Dot(toLight.normalized, src.normal.normalized) * light.intensity * light.color * material.baseColor;

                if (Vector3.Dot(toLight, src.normal) > 0)
                {
                    // Add specular light
                    Vector3 h = (src.binormal + toLight).normalized;
                    lighting += Mathf.Pow(Vector3.Dot(src.normal, h), material.specPower) * light.color * light.intensity * material.specIntensity;
                }
            }

            float fogFactor = Mathf.Clamp01((distToCamera - scene.fogStart) / (scene.fogEnd - scene.fogStart));

            return Color.Lerp(lighting.saturated, scene.fogColor, fogFactor);
        }
    }
}
