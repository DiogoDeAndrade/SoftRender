namespace SoftRender.Engine
{
    // Basic implementation of a Quaternion
    public struct Quaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static Quaternion operator *(Quaternion a, Quaternion b) => new Quaternion(a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y, 
                                                                                          a.w * b.y - a.x * b.z + a.y * b.w + a.z * b.x, 
                                                                                          a.w * b.z + a.x * b.y - a.y * b.x + a.z * b.w,
                                                                                          a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z);
        public static Quaternion operator -(Quaternion a) => a.inversed;

        public override string ToString()
        {
            return $"({x:F3},{y:F3},{z:F3},{w:F3})";
        }

        public Quaternion inversed
        {
            get
            {
                float inv_norm = 1 / magnitude;
                return new Quaternion(-x * inv_norm, -y * inv_norm, -z * inv_norm, w * inv_norm);
            }
        }

        public float magnitude
        {
            get => Mathf.Sqrt(x * x + y * y + z * z + w * w);
        }

        public Vector3 eulerAngles
        {
            get
            {
                Vector3 ret = new Vector3();
                // pitch (x-axis rotation)
                float sinr_cosp = 2 * (w * x + y * z);
                float cosr_cosp = 1 - 2 * (x * x + y * y);
                ret.x = Mathf.Atan2(sinr_cosp, cosr_cosp);

                // yaw (y-axis rotation)
                float sinp = 2 * (w * y - z * x);
                if (Mathf.Abs(sinp) >= 1)
                    ret.y = Mathf.CopySign(Mathf.PI / 2, sinp); // use 90 degrees if out of range
                else
                    ret.y = Mathf.Asin(sinp);

                // roll (z-axis rotation)
                float siny_cosp = 2 * (w * z + x * y);
                float cosy_cosp = 1 - 2 * (y * y + z * z);
                ret.z = Mathf.Atan2(siny_cosp, cosy_cosp);
                
                return ret * Mathf.Rad2Deg;
            }
        }

        public static Quaternion AngleAxis(float angle, Vector3 axis)
        {
            float ang = -angle * Mathf.Deg2Rad;
            float ang2 = ang * 0.5f;
            float sin_ang2 = Mathf.Sin(ang2);

            float x = -axis.x * sin_ang2;
            float y = -axis.y * sin_ang2;
            float z = -axis.z * sin_ang2;
            float w = Mathf.Cos(ang2);

            return new Quaternion(x, y, z, w);
        }

        public static Quaternion Euler(float x, float y, float z)
        {
            Quaternion rx = Quaternion.AngleAxis(x, Vector3.right);
            Quaternion ry = Quaternion.AngleAxis(y, Vector3.up);
            Quaternion rz = Quaternion.AngleAxis(z, Vector3.forward);

            return rz * ry * rx;
        }

        public static Quaternion identity = new Quaternion(0, 0, 0, 1);
    }
}
