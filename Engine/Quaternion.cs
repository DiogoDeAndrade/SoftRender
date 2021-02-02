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

        public static Quaternion identity = new Quaternion(0, 0, 0, 1);
    }
}
