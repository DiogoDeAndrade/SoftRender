using System;
using SoftRender.Engine;

namespace SoftRender.UnityApp
{
    public enum CullMode { Off, Front, Back };
    public delegate bool CullFunction(FatVertex p1, FatVertex p2, FatVertex p3);

    public class Material
    {
        public bool     isWireframe = false;
        public Color    baseColor = Color.magenta;
        public CullMode cullMode = CullMode.Back;

        public CullFunction GetCullFunction() => GetCullFunction(cullMode);

        static public CullFunction GetCullFunction(CullMode mode)
        {
            switch (mode)
            {
                case CullMode.Off:
                    return (p1, p2, p3) => { return false; };
                case CullMode.Front:
                    return (p1, p2, p3) => { return GetFaceNormal(p1.position.xyz, p2.position.xyz, p3.position.xyz).z > 0.0f; };
                case CullMode.Back:
                    return (p1, p2, p3) => { return GetFaceNormal(p1.position.xyz, p2.position.xyz, p3.position.xyz).z < 0.0f; };
                default:
                    break;
            }

            return (p1, p2, p3) => { return false; };
        }
        static Vector3 GetFaceNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            return Vector3.Cross(v2 - v1, v3 - v1);
        }
    }
}
