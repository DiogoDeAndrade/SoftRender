
using System;

namespace SoftRender.Engine
{
    // Basic implementation of a 3x3 matrix
    public struct Matrix3x3
    {
        float[,] m;

        public static Matrix3x3 zero { get { return new Matrix3x3(0); } }
        public static Matrix3x3 identity { get { return new Matrix3x3(1); } }

        static public Matrix3x3 Rotate(Quaternion rotation)
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

            return new Matrix3x3(1 - 2 * (yy + zz), 2 * (xy + zw), 2 * (xz - yw),
                                 2 * (xy - zw), 1 - 2 * (xx + zz), 2 * (yz + xw),
                                 2 * (xz + yw), 2 * (yz - xw), 1 - 2 * (xx + yy));
        }
        static public Matrix3x3 Scale(Vector3 scale)
        {
            return new Matrix3x3(scale.x, 0, 0,
                                 0, scale.y, 0,
                                 0, 0, scale.z);
        }
        static public Matrix3x3 RS(Quaternion rotation, Vector3 scale)
        {
            var rotationMatrix = Rotate(rotation);
            var scalingMatrix = Scale(scale);

            return scalingMatrix * rotationMatrix;
        }

        public Matrix3x3(float diagonal = 1.0f)
        {
            m = new float[3, 3];
            m[0, 0] = diagonal; m[0, 1] = 0; m[0, 2] = 0;
            m[1, 0] = 0; m[1, 1] = diagonal; m[1, 2] = 0;
            m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = diagonal;
        }

        public Matrix3x3(float m00, float m01, float m02,
                         float m10, float m11, float m12,
                         float m20, float m21, float m22)
        {
            m = new float[3, 3];
            m[0, 0] = m00; m[0, 1] = m01; m[0, 2] = m02;
            m[1, 0] = m10; m[1, 1] = m11; m[1, 2] = m12;
            m[2, 0] = m20; m[2, 1] = m21; m[2, 2] = m22;
        }

        public Matrix3x3(Vector3 v1,
                         Vector3 v2,
                         Vector3 v3)
        {
            m = new float[3, 3];
            m[0, 0] = v1.x; m[0, 1] = v1.y; m[0, 2] = v1.z;
            m[1, 0] = v2.x; m[1, 1] = v2.y; m[1, 2] = v2.z;
            m[2, 0] = v3.x; m[2, 1] = v3.y; m[2, 2] = v3.z;
        }

        public float this[int line, int column]
        {
            get => m[line, column];
            set => m[line, column] = value;
        }

        public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
        {
            Matrix3x3 ret = new Matrix3x3(0);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    float val = 0.0f;
                    for (int d = 0; d < 3; d++)
                        val = val + a[i, d] * b[d, j];
                    ret[i, j] = val;
                }
            }
            return ret;
        }

        public static Vector3 operator *(Matrix3x3 a, Vector3 v)
        {
            Vector3 ret = new Vector3(
                v.x * a.m[0, 0] + v.y * a.m[1, 0] + v.z * a.m[2, 0],
                v.x * a.m[0, 1] + v.y * a.m[1, 1] + v.z * a.m[2, 1],
                v.x * a.m[0, 2] + v.y * a.m[1, 2] + v.z * a.m[2, 2]);

            return ret;
        }

        public static Vector3 operator *(Vector3 v, Matrix3x3 a)
        {
            Vector3 ret = new Vector3(
                v.x * a.m[0, 0] + v.y * a.m[0, 1] + v.z * a.m[0, 2],
                v.x * a.m[1, 0] + v.y * a.m[1, 1] + v.z * a.m[1, 2],
                v.x * a.m[2, 0] + v.y * a.m[2, 1] + v.z * a.m[2, 2]);

            return ret;
        }

        public static Vector4 operator *(Matrix3x3 a, Vector4 v)
        {
            Vector4 ret = new Vector4(
                v.x * a.m[0, 0] + v.y * a.m[1, 0] + v.z * a.m[2, 0],
                v.x * a.m[0, 1] + v.y * a.m[1, 1] + v.z * a.m[2, 1],
                v.x * a.m[0, 2] + v.y * a.m[1, 2] + v.z * a.m[2, 2],
                0);

            return ret;
        }

        public static Vector4 operator *(Vector4 v, Matrix3x3 a)
        {
            Vector4 ret = new Vector4(
                v.x * a.m[0, 0] + v.y * a.m[0, 1] + v.z * a.m[0, 2],
                v.x * a.m[1, 0] + v.y * a.m[1, 1] + v.z * a.m[1, 2],
                v.x * a.m[2, 0] + v.y * a.m[2, 1] + v.z * a.m[2, 2],
                0);

            return ret;
        }


        public Matrix3x3 inverse
        {
            get
            {
                float det = 0.0f;
                for (int i = 0; i < 3; i++)
                    det += (m[0,i] * (m[1,(i + 1) % 3] * m[2,(i + 2) % 3] - m[1,(i + 2) % 3] * m[2,(i + 1) % 3]));

                var ret = new Matrix3x3();

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        ret.m[i, j] = ((m[(i + 1) % 3, (j + 1) % 3] * m[(i + 2) % 3, (j + 2) % 3]) - (m[(i + 1) % 3, (j + 2) % 3] * m[(i + 2) % 3, (j + 1) % 3])) / det;
                    }
                }

                return ret;
            }
        }
    }
}
