using System;

namespace SoftRender.Engine
{
    public enum CullMode { Off, Front, Back };
    public delegate bool CullFunction(FatVertex p1, FatVertex p2, FatVertex p3);

    public enum ZTest { Less, Greater, LEqual, GEqual, Equal, NotEqual, Always };
    public delegate bool DepthFunction(float depthBufferValue, float candidateDepth);
    public delegate bool TestAndSetDepthFunction(ref float[] data, int index, float candidateDepth);

    public enum BlendOp { Add, Sub, RevSub, Min, Max };
    public enum BlendFactor { One, Zero, SrcColor, SrcAlpha, DstColor, DstAlpha, OneMinusSrcColor, OneMinusSrcAlpha, OneMinusDstColor, OneMinusDstAlpha };
    public delegate Color32 BlendOpFunction(Color32 src, Color32 dest);

    delegate Color32 BlendFunction(Color32 src, Color32 dest, Color32 current);

    public class Material
    {
        CullMode        _cullMode;
        CullFunction    cullFunction;

        ZTest                   _depthTest;
        bool                    _depthWrite = true;
        DepthFunction           depthFunction;
        TestAndSetDepthFunction testAndSetDepthFunction;

        bool                    _blend = false;
        BlendOp                 _blendOp = BlendOp.Add;
        BlendFactor             _sourceBlend = BlendFactor.SrcAlpha;
        BlendFactor             _destBlend = BlendFactor.OneMinusSrcAlpha;
        BlendOpFunction         _blendOpFunction;

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
                SetDepthFunctions();
            }
        }

        public bool depthWrite
        {
            get { return _depthWrite; }
            set
            {
                _depthWrite = value;
                SetDepthFunctions();
            }
        }

        public bool blend
        {
            get => _blend;
            set
            {
                _blend = value;
                SetBlendFunctions();
            }
        }

        public BlendOp blendOp
        {
            get => _blendOp;
            set
            {
                _blendOp = value;
                SetBlendFunctions();
            }
        }

        public BlendFactor sourceBlend
        {
            get => _sourceBlend;
            set
            {
                _sourceBlend = value;
                SetBlendFunctions();
            }
        }

        public BlendFactor destBlend
        {
            get => _destBlend;
            set
            {
                _destBlend = value;
                SetBlendFunctions();
            }
        }

        public Material()
        {
            cullMode = CullMode.Back;
            depthTest = ZTest.Less;
            SetDepthFunctions();
            SetBlendFunctions();
        }

        public CullFunction GetCullFunction() => cullFunction;

        static Vector3 GetFaceNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            return Vector3.Cross(v2 - v1, v3 - v1);
        }

        #region Depth Functions

        public DepthFunction GetDepthFunction() => depthFunction;
        public TestAndSetDepthFunction GetTestAndSetDepthFunction() => testAndSetDepthFunction;

        bool ZTestLessWrite(ref float[] data, int index, float newDepth) { bool check = newDepth < data[index]; if (check) data[index] = newDepth; return check; }
        bool ZTestLessNoWrite(ref float[] data, int index, float newDepth) { bool check = newDepth < data[index]; return check; }
        bool ZTestGreaterWrite(ref float[] data, int index, float newDepth) { bool check = newDepth > data[index]; if (check) data[index] = newDepth; return check; }
        bool ZTestGreaterNoWrite(ref float[] data, int index, float newDepth) { bool check = newDepth > data[index]; return check; }
        bool ZTestLessEqualWrite(ref float[] data, int index, float newDepth) { bool check = newDepth <= data[index]; if (check) data[index] = newDepth; return check; }
        bool ZTestLessEqualNoWrite(ref float[] data, int index, float newDepth) { bool check = newDepth <= data[index]; return check; }
        bool ZTestGreaterEqualWrite(ref float[] data, int index, float newDepth) { bool check = newDepth >= data[index]; if (check) data[index] = newDepth; return check; }
        bool ZTestGreaterEqualNoWrite(ref float[] data, int index, float newDepth) { bool check = newDepth >= data[index]; return check; }
        bool ZTestEqualWrite(ref float[] data, int index, float newDepth) { bool check = newDepth == data[index]; if (check) data[index] = newDepth; return check; }
        bool ZTestEqualNoWrite(ref float[] data, int index, float newDepth) { bool check = newDepth == data[index]; return check; }
        bool ZTestNotEqualWrite(ref float[] data, int index, float newDepth) { bool check = newDepth != data[index]; if (check) data[index] = newDepth; return check; }
        bool ZTestNotEqualNoWrite(ref float[] data, int index, float newDepth) { bool check = newDepth != data[index]; return check; }
        bool ZTestAlwaysWrite(ref float[] data, int index, float newDepth) { data[index] = newDepth; return true; }
        bool ZTestAlwaysNoWrite(ref float[] data, int index, float newDepth) { return true; }

        void SetDepthFunctions()
        {
            switch (_depthTest)
            {
                case ZTest.Less:
                    depthFunction = (current, newDepth) => newDepth < current;
                    if (depthWrite) testAndSetDepthFunction = ZTestLessWrite;
                    else testAndSetDepthFunction = ZTestLessNoWrite;
                    break;
                case ZTest.Greater:
                    depthFunction = (current, newDepth) => newDepth > current;
                    if (depthWrite) testAndSetDepthFunction = ZTestGreaterWrite;
                    else testAndSetDepthFunction = ZTestGreaterNoWrite;
                    break;
                case ZTest.LEqual:
                    depthFunction = (current, newDepth) => newDepth <= current;
                    if (depthWrite) testAndSetDepthFunction = ZTestLessEqualWrite;
                    else testAndSetDepthFunction = ZTestLessEqualNoWrite;
                    break;
                case ZTest.GEqual:
                    depthFunction = (current, newDepth) => newDepth >= current;
                    if (depthWrite) testAndSetDepthFunction = ZTestGreaterEqualWrite;
                    else testAndSetDepthFunction = ZTestGreaterEqualNoWrite;
                    break;
                case ZTest.Equal:
                    depthFunction = (current, newDepth) => newDepth == current;
                    if (depthWrite) testAndSetDepthFunction = ZTestEqualWrite;
                    else testAndSetDepthFunction = ZTestEqualNoWrite;
                    break;
                case ZTest.NotEqual:
                    depthFunction = (current, newDepth) => newDepth != current;
                    if (depthWrite) testAndSetDepthFunction = ZTestNotEqualWrite;
                    else testAndSetDepthFunction = ZTestNotEqualNoWrite;
                    break;
                case ZTest.Always:
                    depthFunction = (current, newDepth) => true;
                    if (depthWrite) testAndSetDepthFunction = ZTestAlwaysWrite;
                    else testAndSetDepthFunction = ZTestAlwaysNoWrite;
                    break;
                default:
                    depthFunction = (current, newDepth) => newDepth < current;
                    if (depthWrite) testAndSetDepthFunction = ZTestLessWrite;
                    else testAndSetDepthFunction = ZTestLessNoWrite;
                    break;
            }

        }

        #endregion

        #region Blend Functions

        public BlendOpFunction GetBlendFunction() => _blendOpFunction;

        void SetBlendFunctions()
        {
            if (!blend)
            {
                _blendOpFunction = (src, dest) => src;
                return;
            }

            BlendFunction srcBlendFunction = GetBlendFactor(_sourceBlend);
            BlendFunction destBlendFunction = GetBlendFactor(_destBlend);

            switch (_blendOp)
            {
                case BlendOp.Add:
                    _blendOpFunction = (src, dest) => Color32.AddClamped(srcBlendFunction(src, dest, src), destBlendFunction(src, dest, dest));
                    break;
                case BlendOp.Sub:
                    _blendOpFunction = (src, dest) => Color32.SubClamped(srcBlendFunction(src, dest, src), destBlendFunction(src, dest, dest));
                    break;
                case BlendOp.RevSub:
                    _blendOpFunction = (src, dest) => Color32.SubClamped(destBlendFunction(src, dest, src), srcBlendFunction(src, dest, dest));
                    break;
                case BlendOp.Min:
                    _blendOpFunction = (src, dest) => Color32.Min(destBlendFunction(src, dest, src), srcBlendFunction(src, dest, dest));
                    break;
                case BlendOp.Max:
                    _blendOpFunction = (src, dest) => Color32.Max(destBlendFunction(src, dest, src), srcBlendFunction(src, dest, dest));
                    break;
                default:
                    _blendOpFunction = (src, dest) => Color32.AddClamped(srcBlendFunction(src, dest, src), destBlendFunction(src, dest, dest));
                    break;
            }
        }

        BlendFunction GetBlendFactor(BlendFactor type)
        {
            switch (type)
            {
                case BlendFactor.One:
                    return (src, dest, current) => current;
                case BlendFactor.Zero:
                    return (src, dest, current) => Color32.black;
                case BlendFactor.SrcColor:
                    return (src, dest, current) => new Color32((byte)((current.r * src.r) / 255), (byte)((current.g * src.g) / 255), (byte)((current.b * src.b) / 255), (byte)((current.a * src.a) / 255));
                case BlendFactor.SrcAlpha:
                    return (src, dest, current) => new Color32((byte)((current.r * src.a) / 255), (byte)((current.g * src.a) / 255), (byte)((current.b * src.a) / 255), (byte)((current.a * src.a) / 255));
                case BlendFactor.DstColor:
                    return (src, dest, current) => new Color32((byte)((current.r * dest.r) / 255), (byte)((current.g * dest.g) / 255), (byte)((current.b * dest.b) / 255), (byte)((current.a * dest.a) / 255));
                case BlendFactor.DstAlpha:
                    return (src, dest, current) => new Color32((byte)((current.r * dest.a) / 255), (byte)((current.g * dest.a) / 255), (byte)((current.b * dest.a) / 255), (byte)((current.a * dest.a) / 255));
                case BlendFactor.OneMinusSrcColor:
                    return (src, dest, current) => new Color32((byte)((current.r * (255 - src.r)) / 255), (byte)((current.g * (255 - src.g)) / 255), (byte)((current.b * (255 - src.b)) / 255), (byte)((current.a * (255 - src.a)) / 255));
                case BlendFactor.OneMinusSrcAlpha:
                    return (src, dest, current) => new Color32((byte)((current.r * (255 - src.a)) / 255), (byte)((current.g * (255 - src.a)) / 255), (byte)((current.b * (255 - src.a)) / 255), (byte)((current.a * (255 - src.a)) / 255));
                case BlendFactor.OneMinusDstColor:
                    return (src, dest, current) => new Color32((byte)((current.r * (255 - dest.r)) / 255), (byte)((current.g * (255 - dest.g)) / 255), (byte)((current.b * (255 - dest.b)) / 255), (byte)((current.a * (255 - dest.a)) / 255));
                case BlendFactor.OneMinusDstAlpha:
                    return (src, dest, current) => new Color32((byte)((current.r * (255 - dest.a)) / 255), (byte)((current.g * (255 - dest.a)) / 255), (byte)((current.b * (255 - dest.a)) / 255), (byte)((current.a * (255 - dest.a)) / 255));
                default:
                    return (src, dest, current) => src;
            }
        }
        #endregion

    }
}
