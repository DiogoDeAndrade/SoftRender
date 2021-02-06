using System;

namespace SoftRender.Engine
{
    public enum CullMode { Off, Front, Back };
    public delegate bool CullFunction(FatVertex p1, FatVertex p2, FatVertex p3);

    public enum ZTest { Less, Greater, LEqual, GEqual, Equal, NotEqual, Always };
    public delegate bool DepthFunction(float depthBufferValue, float candidateDepth);

    public class Material
    {
        CullMode        _cullMode;
        CullFunction    cullFunction;

        ZTest           _depthTest;
        DepthFunction   depthFunction;

        public bool     isWireframe = false;
        public Color    baseColor = Color.magenta;

        public CullMode cullMode
        {
            get { return _cullMode; }
            set
            {
                _cullMode = value;
                switch (_cullMode)
                {
                    case CullMode.Off:
                        cullFunction = (p1, p2, p3) => { return false; };
                        break;
                    case CullMode.Front:
                        cullFunction = (p1, p2, p3) => { return GetFaceNormal(p1.position.xyz, p2.position.xyz, p3.position.xyz).z > 0.0f; };
                        break;
                    case CullMode.Back:
                        cullFunction = (p1, p2, p3) => { return GetFaceNormal(p1.position.xyz, p2.position.xyz, p3.position.xyz).z < 0.0f; };
                        break;
                    default:
                        cullFunction = (p1, p2, p3) => { return false; };
                        break;
                }
            }
        }
        public ZTest depthTest
        {
            get { return _depthTest; }
            set
            {
                _depthTest = value;
                switch (_depthTest)
                {
                    case ZTest.Less:
                        depthFunction = (current, newDepth) => newDepth < current;
                        break;
                    case ZTest.Greater:
                        depthFunction = (current, newDepth) => newDepth > current;
                        break;
                    case ZTest.LEqual:
                        depthFunction = (current, newDepth) => newDepth <= current;
                        break;
                    case ZTest.GEqual:
                        depthFunction = (current, newDepth) => newDepth >= current;
                        break;
                    case ZTest.Equal:
                        depthFunction = (current, newDepth) => newDepth == current;
                        break;
                    case ZTest.NotEqual:
                        depthFunction = (current, newDepth) => newDepth != current;
                        break;
                    case ZTest.Always:
                        depthFunction = (current, newDepth) => true;
                        break;
                    default:
                        depthFunction = (current, newDepth) => newDepth < current;
                        break;
                }
            }
        }

        public Material()
        {
            cullMode = CullMode.Back;
            depthTest = ZTest.Less;
        }

        public CullFunction GetCullFunction() => cullFunction;
        public DepthFunction GetDepthFunction() => depthFunction;

        static Vector3 GetFaceNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            return Vector3.Cross(v2 - v1, v3 - v1);
        }
    }
}
