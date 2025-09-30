using UnityEngine;

public class Mat4 {

    public float[,] matrix = new float[4, 4];
    public static Mat4 Identity() {
        Mat4 m = new Mat4();
        for (int i = 0; i < 4; i++) {
            m.matrix[i, i] = 1f;
        }
        return m;
    }

    // default constructor, sets the matrix to the identity
    public Mat4() { }

    public Mat4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33) {
        matrix = new float[,]{
            { m00, m01, m02, m03},
            { m10, m11, m12, m13},
            { m20, m21, m22, m23},
            { m30, m31, m32, m33},
        };
    }


    public Mat4(float[] row0, float[] row1, float[] row2, float[] row3) {
        if (row0.Length != 4 || row1.Length != 4 || row2.Length != 4 || row3.Length != 4) {
            Debug.LogError("Error Creating Mat4: Incorrect Dimensions");

            // default to identity matrix
            matrix = Identity().matrix;
            return;
        }

        matrix = new float[,]{
            { row0[0], row0[1], row0[2], row0[3]},
            { row1[0], row1[1], row1[2], row1[3]},
            { row2[0], row2[1], row2[2], row2[3]},
            { row3[0], row3[1], row3[2], row3[3]},
        };
    }

    public Mat4(float[,] matrix) {
        if (matrix.GetLength(0) != 4 || matrix.GetLength(1) != 4) {
            Debug.LogError("Error Creating Mat4: Incorrect Dimensions");

            // default to identity matrix
            this.matrix = Identity().matrix;
            return;
        }

        // fill out the 2d array 
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                this.matrix[i, j] = matrix[i, j];
            }
        }
    }

    public Mat4 Translate(float tx, float ty, float tz) {
        Mat4 trans = new Mat4(
            1, 0, 0, tx,
            0, 1, 0, ty,
            0, 0, 1, tz,
            0, 0, 0, 1
        );

        return trans * this;
    }

    public Mat4 RotateX(float degrees) {
        float radians = Mathf.Deg2Rad * degrees;

        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        Mat4 rotateX = new Mat4(
            1, 0, 0, 0,
            0, cos, -sin, 0,
            0, sin, cos, 0,
            0, 0, 0, 1
        );

        return rotateX * this;
    }

    public Mat4 RotateY(float degrees) {
        float radians = Mathf.Deg2Rad * degrees;

        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        Mat4 rotateY = new Mat4(
            cos, 0, sin, 0,
            0, 1, 0, 0,
            -sin, 0, cos, 0,
            0, 0, 0, 1
        );

        return rotateY * this;
    }
    public Mat4 RotateZ(float degrees) {
        float radians = Mathf.Deg2Rad * degrees;

        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        Mat4 rotateZ = new Mat4(
            cos, -sin, 0, 0,
            sin, cos, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1
        );

        return rotateZ * this;
    }

    public Mat4 Scale(float sx, float sy, float sz) {
        Mat4 scale = new Mat4(
            sx, 0, 0, 0,
            0, sy, 0, 0,
            0, 0, sz, 0,
            0, 0, 0, 1
        );

        return scale * this;
    }

    public static Mat4 operator *(Mat4 a, Mat4 b) {
        float[,] output = new float[4, 4];

        // get each row of a
        for (int i = 0; i < 4; i++) {
            // get each column of b
            float[] aRow = a.GetRow(i);
            for (int j = 0; j < 4; j++) {
                // multiply a.getRow(i) * b.getColumn(j)
                float[] bCol = b.GetColumn(j);

                float dotProduct = 0;
                for (int r = 0; r < aRow.Length; r++) {
                    dotProduct += aRow[r] * bCol[r];
                }

                // put the dot product of the row and column in the right position in the matrix
                output[i, j] = dotProduct;
            }
        }

        return new Mat4(output);
    }

    public float[] GetRow(int index) {
        float[] output = new float[4];

        for (int i = 0; i < 4; i++) {
            output[i] = matrix[index, i];
        }
        return output;
    }


    public float[] GetColumn(int index) {
        float[] output = new float[4];

        for (int i = 0; i < 4; i++) {
            output[i] = matrix[i, index];
        }
        return output;
    }

    public static Vec3 operator *(Mat4 a, Vec3 b) {
        float[] output = new float[3];

        // loop through each row of the matrix, treat as though the last row doesnt matter since we're multiplying a vec3
        // todo: might be useful to know if the w coordinate is anything other than 1
        for (int i = 0; i < 3; i++) {
            float[] rowA = a.GetRow(i);

            float dotProduct = 0;
            for (int j = 0; j < 3; j++) {
                dotProduct += rowA[j] * b.GetComponent(j);
            }

            // treat the last coordinate of the vec3 as 1, add the last index of row3 * 1 to the dot product
            dotProduct += rowA[3];
            output[i] = dotProduct;
        }

        return new Vec3(output[0], output[1], output[2]);
    }

    // Creates a deep copy of the matrix 
    public Mat4 Clone() {
        return new Mat4(matrix);
    }

    public override string ToString() {
        string output = "{\n";
        for (int i = 0; i < matrix.GetLength(0); i++) {
            output += "\t";
            for (int j = 0; j < matrix.GetLength(1); j++) {
                output += matrix[i, j] + ", ";
            }
            output += "\n";
        }
        output += "}";

        return output;
    }
}