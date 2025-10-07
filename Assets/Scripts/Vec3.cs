using UnityEngine;

namespace MedGraphics {

    public class Vec3 {
        public float x;
        public float y;
        public float z;
        public static Vec3 Zero() {
            return new Vec3(0, 0, 0);
        }

        public override string ToString() {
            return "<" + x + ", " + y + ", " + z + ">";
        }

        public Vec3(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vec3() {
            this.x = 1;
            this.y = 1;
            this.z = 1;
        }


        public static Vec3 operator +(Vec3 a, Vec3 b) {
            return new Vec3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vec3 operator -(Vec3 a, Vec3 b) {
            return new Vec3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vec3 operator -(Vec3 a) {
            return new Vec3(-a.x, -a.y, -a.z);
        }

        public static Vec3 operator *(Vec3 a, Vec3 b) {
            return new Vec3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vec3 operator *(Vec3 b, float s) {
            return new Vec3(s * b.x, s * b.y, s * b.z);
        }

        public static Vec3 operator *(float s, Vec3 b) {
            return new Vec3(s * b.x, s * b.y, s * b.z);
        }


        public static Vec3 operator /(Vec3 b, float s) {
            return new Vec3(b.x / s, b.y / s, b.z / s);
        }

        public Vec3 Clone() {
            return new Vec3(x, y, z);
        }

        // calculates the magnitude of this vector squared. When comparing the magnitude of two vectors this should be used as its slightly faster to calculate
        public float SqrMagnitude() {
            return (x * x) + (y * y) + (z * z);
        }

        public float Magnitude() {
            return MathUtils.Sqrt(SqrMagnitude());
        }

        public Vec3 Normalize() {
            float mag = Magnitude();
            if (mag == 0) {
                x = 1;
                return this;
            }
            MultiplyScalar(1 / mag);
            return this;
        }

        // sets this vector equal to the sum of this vector and the given vector
        public Vec3 Add(Vec3 vec) {
            x += vec.x;
            y += vec.y;
            z += vec.z;
            return this;
        }

        //  Adds the scalar value n to this vector's x, y and z values. 
        public Vec3 AddScalar(float n) {
            x += n;
            y += n;
            z += n;
            return this;
        }

        // Sets this vector to the sum of a and b
        public Vec3 AddVectors(Vec3 a, Vec3 b) {
            x = a.x + b.x;
            y = a.y + b.y;
            z = a.z + b.z;
            return this;
        }

        // Multiplies the elements of this vector by the elements of a
        public Vec3 Multiply(Vec3 a) {
            x *= a.x;
            y *= a.y;
            z *= a.z;
            return this;
        }

        // Multiplies each element of this vector by n
        public Vec3 MultiplyScalar(float n) {
            x *= n;
            y *= n;
            z *= n;
            return this;
        }

        // Sets this vector equal to the component-wise multiplication of a and b
        public Vec3 MultiplyVectors(Vec3 a, Vec3 b) {
            x = a.x * b.x;
            y = a.y * b.y;
            z = a.z * b.z;
            return this;
        }

        // returns the dot product of this vector and a
        public float Dot(Vec3 a) {
            return (x * a.x) + (y * a.y) + (z * a.z);
        }

        // returns the dot product of a and b
        public float DotVectors(Vec3 a, Vec3 b) {
            return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
        }

        // sets this vector equal to the cross product of this vector and a
        public Vec3 Cross(Vec3 a) {
            x = y * a.z - z * a.y;
            y = z * a.x - x * a.z;
            z = x * a.y - y * a.x;
            return this;
        }

        // sets this vector equal to the cross product of a and b
        public Vec3 CrossVectors(Vec3 a, Vec3 b) {
            x = a.y * b.z - a.z * b.y;
            y = a.z * b.x - a.x * b.z;
            z = a.x * b.y - a.y * b.x;
            return this;
        }

        // inverts this vector
        public Vec3 Negate() {
            x = -x;
            y = -y;
            z = -z;
            return this;
        }

        // sets this vector to a vector with the same direction as this one, but with magnitude f
        public Vec3 SetMagnitude(float f) {
            Normalize();
            MultiplyScalar(f);
            return this;
        }

        // sets the x, y, and z components of this vector to f
        public Vec3 SetScalar(float f) {
            x = y = z = f;
            return this;
        }

        // returns the manhattan distance of this vector from (0, 0, 0)
        public float ManhattanLength() {
            return x + y + z;
        }

        // returns the euclidean distance between this vector and a
        public float DistanceTo(Vec3 a) {
            return Mathf.Sqrt(Mathf.Pow(a.x - x, 2) + Mathf.Pow(a.y - y, 2) + Mathf.Pow(a.z - z, 2));
        }

        // returns whether two vectors are equal
        public bool Equals(Vec3 a) {
            return a.x == x && a.y == y && a.z == z;
        }

        // gets the given vector element as though its an array
        public float GetComponent(int index) {
            if (index == 0) {
                return x;
            } else if (index == 1) {
                return y;
            } else if (index == 2) {
                return z;
            }
            return -1;
        }

        // converts the components of this vector to an array and returns it. 
        public float[] ToArray() {
            return new float[] { x, y, z };
        }
    }

    public class Vec4 {
        public float x, y, z, w;

        public Vec4(float x, float y, float z, float w) { this.x = x; this.y = y; this.z = z; this.w = w; }

        public static Vec4 FromPoint(Vec3 p) => new Vec4(p.x, p.y, p.z, 1f);

        public static Vec4 From3DVector(Vec3 v) => new Vec4(v.x, v.y, v.z, 0f);

        public Vec3 XYZ() => new Vec3(x, y, z);

        public Vec3 Homogenized() {
            if (w > -1e-8f && w < 1e-8f) return new Vec3(x, y, z);
            float invW = 1.0f / w;
            return new Vec3(x * invW, y * invW, z * invW);
        }

        public float GetComponent(int index) {
            if (index == 0) {
                return x;
            } else if (index == 1) {
                return y;
            } else if (index == 2) {
                return z;
            } else if (index == 3) {
                return w;
            }
            return -1;
        }

        public override string ToString() {
            return "(" + x + ", " + y + ", " + z + ", " + w + ")";
        }
    }
}
