
namespace SoftRender.Engine
{
    // Basic implementation of a 4x4 matrix
    public struct Matrix4x4
    {
        float[,] m;

        public static Matrix4x4 zero { get { return new Matrix4x4(0); } }
        public static Matrix4x4 identity { get { return new Matrix4x4(1); } }

        static public Matrix4x4 Translate(Vector3 position)
        {
            return new Matrix4x4(1, 0, 0, 0,
                                 0, 1, 0, 0,
                                 0, 0, 1, 0,
                                 position.x, position.y, position.z, 1);
        }
        static public Matrix4x4 Rotate(Quaternion rotation)
        {
            float xx = rotation.x * rotation.x;
            float xy = rotation.x * rotation.y;
            float xz = rotation.x * rotation.z;
            float xw = rotation.x * rotation.w;

            float yy = rotation.y * rotation.y;
            float yz = rotation.y * rotation.z;
            float yw = rotation.y * rotation.w;

            float zz = rotation.z * rotation.z;
            float zw = rotation.z * rotation.w;

            return new Matrix4x4(1 - 2 * (yy + zz), 2 * (xy + zw), 2 * (xz - yw), 0,
                                 2 * (xy - zw), 1 - 2 * (xx + zz), 2 * (yz + xw), 0,
                                 2 * (xz + yw), 2 * (yz - xw), 1 - 2 * (xx + yy), 0,
                                 0, 0, 0, 1);
        }
        static public Matrix4x4 Scale(Vector3 scale)
        {
            return new Matrix4x4(scale.x, 0, 0, 0,
                                 0, scale.y, 0, 0,
                                 0, 0, scale.z, 0,
                                 0, 0, 0, 1);
        }
        static public Matrix4x4 PRS(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            var translationMatrix = Translate(position);
            var rotationMatrix = Rotate(rotation);
            var scalingMatrix = Scale(scale);

            return scalingMatrix * rotationMatrix * translationMatrix;
        }

        static public Matrix4x4 Ortographic(int viewportWidth, int viewportHeight, float nearPlane = 0, float farPlane = 1)
        {
            return new Matrix4x4(2.0f / viewportWidth, 0, 0, -1.0f / viewportWidth,
                                 0, 2.0f / viewportHeight, 0, -1.0f / viewportHeight,
                                 0, 0, 1 / (farPlane - nearPlane), 0,
                                 0, 0, 0, 1); ;
        }


        public Matrix4x4(float diagonal = 1.0f)
        {
            m = new float[4, 4];
            m[0, 0] = diagonal; m[0, 1] = 0; m[0, 2] = 0; m[0, 3] = 0;
            m[1, 0] = 0; m[1, 1] = diagonal; m[1, 2] = 0; m[1, 3] = 0;
            m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = diagonal; m[2, 3] = 0;
            m[3, 0] = 0; m[3, 1] = 0; m[3, 2] = 0; m[3, 3] = diagonal;
        }

        public Matrix4x4(float m00, float m01, float m02, float m03,
                         float m10, float m11, float m12, float m13,
                         float m20, float m21, float m22, float m23,
                         float m30, float m31, float m32, float m33)
        {
            m = new float[4, 4];
            m[0, 0] = m00; m[0, 1] = m01; m[0, 2] = m02; m[0, 3] = m03;
            m[1, 0] = m10; m[1, 1] = m11; m[1, 2] = m12; m[1, 3] = m13;
            m[2, 0] = m20; m[2, 1] = m21; m[2, 2] = m22; m[2, 3] = m23;
            m[3, 0] = m30; m[3, 1] = m31; m[3, 2] = m32; m[3, 3] = m33;
        }

        public float this[int line, int column]
        {
            get => m[line, column];
            set => m[line, column] = value;
        }

        public static Matrix4x4 operator *(Matrix4x4 a, Matrix4x4 b)
        {
            Matrix4x4 ret = new Matrix4x4(0);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    float val = 0.0f;
                    for (int d = 0; d < 4; d++)
                        val += a[i, d] * b[d, j];
                    ret[i, j] = val;
                }
            }
            return ret;
        }

        public static Vector3 operator *(Matrix4x4 a, Vector3 v)
        {
            Vector3 ret = new Vector3(
                v.x * a.m[0, 0] + v.y * a.m[1, 0] + v.z * a.m[2, 0] + a.m[3, 0],
                v.x * a.m[0, 1] + v.y * a.m[1, 1] + v.z * a.m[2, 1] + a.m[3, 1],
                v.x * a.m[0, 2] + v.y * a.m[1, 2] + v.z * a.m[2, 2] + a.m[3, 2]);

            return ret;
        }

        public static Vector4 operator* (Matrix4x4 a, Vector4 v)
        {
            Vector4 ret = new Vector4(
                v.x * a.m[0, 0] + v.y * a.m[1, 0] + v.z * a.m[2, 0] + v.w * a.m[3, 0],
                v.x * a.m[0, 1] + v.y * a.m[1, 1] + v.z * a.m[2, 1] + v.w * a.m[3, 1],
                v.x * a.m[0, 2] + v.y * a.m[1, 2] + v.z * a.m[2, 2] + v.w * a.m[3, 2],
                v.x * a.m[0, 3] + v.y * a.m[1, 3] + v.z * a.m[2, 3] + v.w * a.m[3, 3]);

            return ret;
        }


        public Matrix4x4 inverse
        {
            get
            {
                float[] tmp = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                float[] src = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                float[] dst = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                // Initialize with transpose of source
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                        src[i * 4 + j] = m[j, i];

                // Calculate pairs for first 8 elements (cofactors) 
                tmp[0] = src[10] * src[15];
                tmp[1] = src[11] * src[14];
                tmp[2] = src[9] * src[15];
                tmp[3] = src[11] * src[13];
                tmp[4] = src[9] * src[14];
                tmp[5] = src[10] * src[13];
                tmp[6] = src[8] * src[15];
                tmp[7] = src[11] * src[12];
                tmp[8] = src[8] * src[14];
                tmp[9] = src[10] * src[12];
                tmp[10] = src[8] * src[13];
                tmp[11] = src[9] * src[12];

                // calculate first 8 elements 
                dst[0] = tmp[0] * src[5] + tmp[3] * src[6] + tmp[4] * src[7];
                dst[0] -= tmp[1] * src[5] + tmp[2] * src[6] + tmp[5] * src[7];
                dst[1] = tmp[1] * src[4] + tmp[6] * src[6] + tmp[9] * src[7];
                dst[1] -= tmp[0] * src[4] + tmp[7] * src[6] + tmp[8] * src[7];
                dst[2] = tmp[2] * src[4] + tmp[7] * src[5] + tmp[10] * src[7];
                dst[2] -= tmp[3] * src[4] + tmp[6] * src[5] + tmp[11] * src[7];
                dst[3] = tmp[5] * src[4] + tmp[8] * src[5] + tmp[11] * src[6];
                dst[3] -= tmp[4] * src[4] + tmp[9] * src[5] + tmp[10] * src[6];
                dst[4] = tmp[1] * src[1] + tmp[2] * src[2] + tmp[5] * src[3];
                dst[4] -= tmp[0] * src[1] + tmp[3] * src[2] + tmp[4] * src[3];
                dst[5] = tmp[0] * src[0] + tmp[7] * src[2] + tmp[8] * src[3];
                dst[5] -= tmp[1] * src[0] + tmp[6] * src[2] + tmp[9] * src[3];
                dst[6] = tmp[3] * src[0] + tmp[6] * src[1] + tmp[11] * src[3];
                dst[6] -= tmp[2] * src[0] + tmp[7] * src[1] + tmp[10] * src[3];
                dst[7] = tmp[4] * src[0] + tmp[9] * src[1] + tmp[10] * src[2];
                dst[7] -= tmp[5] * src[0] + tmp[8] * src[1] + tmp[11] * src[2];

                // calculate pairs for second 8 elements (cofactors) 
                tmp[0] = src[2] * src[7];
                tmp[1] = src[3] * src[6];
                tmp[2] = src[1] * src[7];
                tmp[3] = src[3] * src[5];
                tmp[4] = src[1] * src[6];
                tmp[5] = src[2] * src[5];
                tmp[6] = src[0] * src[7];
                tmp[7] = src[3] * src[4];
                tmp[8] = src[0] * src[6];
                tmp[9] = src[2] * src[4];
                tmp[10] = src[0] * src[5];
                tmp[11] = src[1] * src[4];

                // calculate second 8 elements (cofactors) 
                dst[8] = tmp[0] * src[13] + tmp[3] * src[14] + tmp[4] * src[15];
                dst[8] -= tmp[1] * src[13] + tmp[2] * src[14] + tmp[5] * src[15];
                dst[9] = tmp[1] * src[12] + tmp[6] * src[14] + tmp[9] * src[15];
                dst[9] -= tmp[0] * src[12] + tmp[7] * src[14] + tmp[8] * src[15];
                dst[10] = tmp[2] * src[12] + tmp[7] * src[13] + tmp[10] * src[15];
                dst[10] -= tmp[3] * src[12] + tmp[6] * src[13] + tmp[11] * src[15];
                dst[11] = tmp[5] * src[12] + tmp[8] * src[13] + tmp[11] * src[14];
                dst[11] -= tmp[4] * src[12] + tmp[9] * src[13] + tmp[10] * src[14];
                dst[12] = tmp[2] * src[10] + tmp[5] * src[11] + tmp[1] * src[9];
                dst[12] -= tmp[4] * src[11] + tmp[0] * src[9] + tmp[3] * src[10];
                dst[13] = tmp[8] * src[11] + tmp[0] * src[8] + tmp[7] * src[10];
                dst[13] -= tmp[6] * src[10] + tmp[9] * src[11] + tmp[1] * src[8];
                dst[14] = tmp[6] * src[9] + tmp[11] * src[11] + tmp[3] * src[8];
                dst[14] -= tmp[10] * src[11] + tmp[2] * src[8] + tmp[7] * src[9];
                dst[15] = tmp[10] * src[10] + tmp[4] * src[8] + tmp[9] * src[9];
                dst[15] -= tmp[8] * src[9] + tmp[11] * src[10] + tmp[5] * src[8];

                // calculate determinant 
                float det = src[0] * dst[0] + src[1] * dst[1] + src[2] * dst[2] + src[3] * dst[3];

                // calculate inverse
                det = 1 / det;
                for (int i = 0; i < 16; i++)
                    dst[i] *= det;

                // Load to target
                Matrix4x4 ret = new Matrix4x4(0);

                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                        ret.m[i, j] = src[i * 4 + j];

                return ret;
            }
        }
    }
}
