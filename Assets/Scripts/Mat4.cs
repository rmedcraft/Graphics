using UnityEngine;

namespace MedGraphics {

    public class Mat4 {

        public float[,] matrix = new float[4, 4];
        public static Mat4 Identity() {
            Mat4 m = new Mat4();
            for (int i = 0; i < 4; i++) {
                m.matrix[i, i] = 1f;
            }
            return m;
        }

        // default constructor, sets the matrix to all zeros
        public Mat4() { }

        public Mat4(float f) {
            matrix = new float[,]{
                {f, f, f, f},
                {f, f, f, f},
                {f, f, f, f},
                {f, f, f, f}
            };
        }

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

        public static Mat4 Translation(float tx, float ty, float tz) {
            return new Mat4(
                1, 0, 0, tx,
                0, 1, 0, ty,
                0, 0, 1, tz,
                0, 0, 0, 1
            );
        }

        public static Mat4 RotationX(float degrees) {
            float radians = Mathf.Deg2Rad * degrees;

            float cos = Mathf.Cos(radians);
            float sin = Mathf.Sin(radians);

            return new Mat4(
                1, 0, 0, 0,
                0, cos, -sin, 0,
                0, sin, cos, 0,
                0, 0, 0, 1
            );
        }

        public static Mat4 RotationY(float degrees) {
            float radians = Mathf.Deg2Rad * degrees;

            float cos = Mathf.Cos(radians);
            float sin = Mathf.Sin(radians);

            return new Mat4(
                cos, 0, sin, 0,
                0, 1, 0, 0,
                -sin, 0, cos, 0,
                0, 0, 0, 1
            );
        }

        public static Mat4 RotationZ(float degrees) {
            float radians = Mathf.Deg2Rad * degrees;

            float cos = Mathf.Cos(radians);
            float sin = Mathf.Sin(radians);

            return new Mat4(
                cos, -sin, 0, 0,
                sin, cos, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1
            );
        }

        public static Mat4 Scaling(float sx, float sy, float sz) {
            return new Mat4(
                sx, 0, 0, 0,
                0, sy, 0, 0,
                0, 0, sz, 0,
                0, 0, 0, 1
            );
        }


        public Mat4 Scale(float sx, float sy, float sz) {
            return Scaling(sx, sy, sz) * this;
        }

        public Mat4 RotateX(float degrees) {
            return RotationX(degrees) * this;
        }
        public Mat4 RotateY(float degrees) {
            return RotationY(degrees) * this;
        }
        public Mat4 RotateZ(float degrees) {
            return RotationZ(degrees) * this;
        }

        public Mat4 Translate(float tx, float ty, float tz) {
            return Translation(tx, ty, tz) * this;
        }

        public static Mat4 Ortho(float l, float r, float b, float t, float n, float f) {
            var rl = r - l;
            var tb = t - b;
            var fn = f - n;

            return new Mat4(
                2 / rl, 0, 0, -(r + l) / rl,
                0, 2 / tb, 0, -(t + b) / tb,
                0, 0, -2 / fn, -(f + n) / fn,
                0, 0, 0, 1
            );
        }

        public static Mat4 Perspective(float fovYDegrees, float aspect, float n, float far) {
            float fovYRad = fovYDegrees * Mathf.Deg2Rad;
            float f = 1f / Mathf.Tan(fovYRad / 2f);
            float nf = far - n;
            float sum = far + n;
            float twice = 2 * far * n;


            return new Mat4(
                f / aspect, 0, 0, 0,
                0, f, 0, 0,
                0, 0, -sum / nf, -twice / nf,
                0, 0, -1, 0
            );
        }


        public Mat4 RotateAroundAxis(Vec3 n, float angle) {
            return RotationRodrigues(n, angle) * this;
        }

        public static Mat4 RotationRodrigues(Vec3 n, float angle) {
            n.Normalize();

            float rad = Mathf.Deg2Rad * angle;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);

            return new Mat4(
                cos + n.x * n.x * (1 - cos), n.x * n.y * (1 - cos) - n.z * sin, n.x * n.z * (1 - cos) + n.y * sin, 0,
                n.y * n.x * (1 - cos) + n.z * sin, cos + n.y * n.y * (1 - cos), n.y * n.z * (1 - cos) - n.x * sin, 0,
                n.z * n.x * (1 - cos) - n.y * sin, n.z * n.y * (1 - cos) + n.x * sin, cos + n.z * n.z * (1 - cos), 0,
                0, 0, 0, 1
            );
        }

        public static Mat4 RotationRodriguesAroundPivot(Vec3 axis, float angleDeg, Vec3 pivot) {
            var rotationMat = RotationRodrigues(axis, angleDeg);
            var trans = Translation(pivot.x, pivot.y, pivot.z);
            var transNeg = Translation(-pivot.x, -pivot.y, -pivot.z);

            return trans * rotationMat * transNeg;
        }


        // multiples a and b
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

        public static Vec4 operator *(Mat4 a, Vec4 b) {
            float[] output = new float[4];

            // loop through each row of the matrix, treat as though the last row doesnt matter since we're multiplying a vec3
            // todo: might be useful to know if the w coordinate is anything other than 1
            for (int i = 0; i < 4; i++) {
                float[] rowA = a.GetRow(i);

                float dotProduct = 0;
                for (int j = 0; j < 4; j++) {
                    dotProduct += rowA[j] * b.GetComponent(j);
                }

                output[i] = dotProduct;
            }

            return new Vec4(output[0], output[1], output[2], output[3]);
        }


        // gets the row at a specified index as an array
        public float[] GetRow(int index) {
            float[] output = new float[4];

            for (int i = 0; i < 4; i++) {
                output[i] = matrix[index, i];
            }
            return output;
        }

        // gets the column at a specified index as an array
        public float[] GetColumn(int index) {
            float[] output = new float[4];

            for (int i = 0; i < 4; i++) {
                output[i] = matrix[i, index];
            }
            return output;
        }

        public bool Equals(Mat4 a) {
            for (int r = 0; r < 4; r++) {
                for (int c = 0; c < 4; c++) {
                    if (a.matrix[r, c] != this.matrix[r, c]) {
                        return false;
                    }
                }
            }
            return true;
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
                    output += Mathf.Round(matrix[i, j] * 10f) * .1 + ", ";
                }
                output += "\n";
            }
            output += "}";

            return output;
        }
    }
}