
public class MathUtils {
    /*
        Calculates the square root of a number n with the Newton-Raphson Method
        n: the number to take the square root of
        l: the tolerance level of the answer, the lower l is, the closer the answer will be at the expense of speed, defaults to 0.0001
    */
    public static float Sqrt(float n, float l = 0.0001f) {
        float x = n;
        //root = 0.5 * (X + (N / X)) where X is any guess which can be assumed to be N or 1. 

        while (true) {
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
}
