
using MedGraphics;

public class MathUtils {
    /*
        Calculates the square root of a number n with the Newton-Raphson Method
        n: the number to take the square root of
        l: the tolerance level of the answer, the lower l is, the closer the answer will be at the expense of speed, defaults to 0.0001
    */
    public static float Sqrt(float n, float l = 0.0001f) {
        float x = n;

        while (true) {
            // root = 0.5 * (X + (N / X)) where X is any guess which can be assumed to be N or 1. 
            float root = 0.5f * (x + (n / x));

            if (Abs(x - root) < l) {
                return root;
            }

            x = root;
        }
    }

    public static float Abs(float n) {
        if (n < 0) {
            n *= -1;
        }
        return n;
    }

    public static Mat4 LookAtRH(Vec3 eye, Vec3 target, Vec3 worldUp) {
        Vec3 f = (target - eye).Normalize();
        Vec3 s = Vec3.CrossVectors(f, worldUp).Normalize();
        Vec3 u = Vec3.CrossVectors(s, f);

        return new Mat4(
            s.x, u.x, -f.x, 0,
            s.y, u.y, -f.y, 0,
            s.z, u.z, -f.z, 0,
            -Vec3.DotVectors(s, eye), -Vec3.DotVectors(u, eye), Vec3.DotVectors(f, eye), 1
        );
    }

    public static int Round(float n) {
        return (int)n;
    }

    public static int Ceil(float n) {
        int rounded = Round(n);
        if (n < rounded) {
            // if the round method already rounded up
            return rounded;
        }

        // if the round method rounded down
        float remainder = n - rounded;
        return (int)(1 + n - remainder);
    }

    public static int Floor(float n) {
        return Ceil(n - 1);
    }

    public static float Max(float a, float b) {
        return (a > b) ? a : b;
    }
}
