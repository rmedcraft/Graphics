using UnityEngine;

public class Vec3 {
    public float x;
    public float y;
    public float z;

    Vec3(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vec3 Clone() {
        return new Vec3(x, y, z);
    }

    public float Magnitude() {
        return Mathf.Sqrt((x * x) + (y * y) + (z * z));
    }

    public Vec3 Normalize() {
        float mag = Magnitude();
        x /= mag;
        y /= mag;
        z /= mag;
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
}
