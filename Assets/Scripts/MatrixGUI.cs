using UnityEngine;   // MonoBehaviour + GUI only
using MedGraphics;
public class MatrixGUI : MonoBehaviour {
    // Editable components for A and B

    [SerializeField] float a00 = 1f, a01 = 0f, a02 = 0f, a03 = 0f;
    [SerializeField] float a10 = 0f, a11 = 1f, a12 = 0f, a13 = 0f;
    [SerializeField] float a20 = 0f, a21 = 0f, a22 = 1f, a23 = 0f;
    [SerializeField] float a30 = 0f, a31 = 0f, a32 = 0f, a33 = 1f;


    [SerializeField] float b00 = 1f, b01 = 1f, b02 = 1f, b03 = 1f;
    [SerializeField] float b10 = 1f, b11 = 1f, b12 = 1f, b13 = 1f;
    [SerializeField] float b20 = 1f, b21 = 1f, b22 = 1f, b23 = 1f;
    [SerializeField] float b30 = 1f, b31 = 1f, b32 = 1f, b33 = 1f;

    [SerializeField] float cx = 1f, cy = 2f, cz = 3f;


    Rect win = new Rect(12, 12, 920, 550);

    void OnGUI() {
        win = GUI.Window(42, win, DrawWindow, "Matrix Display (Mat4)");
    }

    void DrawWindow(int id) {
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Label("Matrix A");
        GUILayout.BeginHorizontal();
        TextField(ref a00);
        TextField(ref a01);
        TextField(ref a02);
        TextField(ref a03);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        TextField(ref a10);
        TextField(ref a11);
        TextField(ref a12);
        TextField(ref a13);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        TextField(ref a20);
        TextField(ref a21);
        TextField(ref a22);
        TextField(ref a23);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        TextField(ref a30);
        TextField(ref a31);
        TextField(ref a32);
        TextField(ref a33);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        GUILayout.Label("Matrix B");
        GUILayout.BeginHorizontal();
        TextField(ref b00);
        TextField(ref b01);
        TextField(ref b02);
        TextField(ref b03);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        TextField(ref b10);
        TextField(ref b11);
        TextField(ref b12);
        TextField(ref b13);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        TextField(ref b20);
        TextField(ref b21);
        TextField(ref b22);
        TextField(ref b23);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        TextField(ref b30);
        TextField(ref b31);
        TextField(ref b32);
        TextField(ref b33);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();


        GUILayout.EndHorizontal();


        GUILayout.Space(6);

        GUILayout.Label("Vector C");
        Slider(ref cx, "Bx", -5f, 5f);
        Slider(ref cy, "By", -5f, 5f);
        Slider(ref cz, "Bz", -5f, 5f);

        // Build matrices and compute with your custom math
        var A = new Mat4(a00, a01, a02, a03, a10, a11, a12, a13, a20, a21, a22, a23, a30, a31, a32, a33);
        var B = new Mat4(b00, b01, b02, b03, b10, b11, b12, b13, b20, b21, b22, b23, b30, b31, b32, b33);
        var C = new Vec4(cx, cy, cz, 1);

        var matMult = A * B;
        var aVec = A * C;
        var bVec = B * C;

        GUILayout.Space(8);
        GUILayout.Label("Results");

        GUILayout.BeginHorizontal();
        GUILayout.Label("A = " + A.ToString());
        GUILayout.Label("B = " + B.ToString());
        GUILayout.Label("A × B  = " + matMult.ToString());
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("A × C  = " + aVec.ToString());
        GUILayout.Label("B × C  = " + bVec.ToString());
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        GUILayout.Label("Translate A by C\n" + (A * Mat4.Translation(C.x, C.y, C.z)).ToString());
        GUILayout.Label("Scale A by C\n" + A.Scale(C.x, C.y, C.z).ToString());

        GUILayout.Label("Rotate A by 45 in X\n" + A.RotateX(45).ToString());
        GUILayout.Label("Rotate A by 45 in Y\n" + A.RotateY(45).ToString());
        GUILayout.Label("Rotate A by 45 in Z\n" + A.RotateZ(45).ToString());
        GUILayout.EndHorizontal();

        GUILayout.Space(6);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Reset")) {
            a00 = 1f; a01 = 0f; a02 = 0f; a03 = 0f;
            a10 = 0f; a11 = 1f; a12 = 0f; a13 = 0f;
            a20 = 0f; a21 = 0f; a22 = 1f; a23 = 0f;
            a30 = 0f; a31 = 0f; a32 = 0f; a33 = 1f;

            b00 = 1f; b01 = 1f; b02 = 1f; b03 = 1f;
            b10 = 1f; b11 = 1f; b12 = 1f; b13 = 1f;
            b20 = 1f; b21 = 1f; b22 = 1f; b23 = 1f;
            b30 = 1f; b31 = 1f; b32 = 1f; b33 = 1f;

            cx = 1f; cy = 2f; cz = 3f;
        }
        if (GUILayout.Button("Randomize")) {
            a00 = Rand(-3f, 3f); a01 = Rand(-3f, 3f); a02 = Rand(-3f, 3f); a03 = Rand(-3f, 3f);
            a10 = Rand(-3f, 3f); a11 = Rand(-3f, 3f); a12 = Rand(-3f, 3f); a13 = Rand(-3f, 3f);
            a20 = Rand(-3f, 3f); a21 = Rand(-3f, 3f); a22 = Rand(-3f, 3f); a23 = Rand(-3f, 3f);
            a30 = Rand(-3f, 3f); a31 = Rand(-3f, 3f); a32 = Rand(-3f, 3f); a33 = Rand(-3f, 3f);

            b00 = Rand(-3f, 3f); b01 = Rand(-3f, 3f); b02 = Rand(-3f, 3f); b03 = Rand(-3f, 3f);
            b10 = Rand(-3f, 3f); b11 = Rand(-3f, 3f); b12 = Rand(-3f, 3f); b13 = Rand(-3f, 3f);
            b20 = Rand(-3f, 3f); b21 = Rand(-3f, 3f); b22 = Rand(-3f, 3f); b23 = Rand(-3f, 3f);
            b30 = Rand(-3f, 3f); b31 = Rand(-3f, 3f); b32 = Rand(-3f, 3f); b33 = Rand(-3f, 3f);

            cx = Rand(-3f, 3f); cy = Rand(-3f, 3f); cz = Rand(-3f, 3f);
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        // Drag by title bar
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }

    // --- UI helpers (no Mathf used) ---
    static void Slider(ref float v, string label, float min, float max) {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label + "  " + F(v), GUILayout.Width(110));
        v = GUILayout.HorizontalSlider(v, min, max);
        GUILayout.EndHorizontal();
    }

    static void TextField(ref float v) {
        GUILayout.BeginHorizontal();
        v = Mathf.Round(float.Parse(GUILayout.TextField(v.ToString())) * 100) / 100;
        GUILayout.EndHorizontal();
    }


    // Lightweight number -> string (round ~3 decimals) without format strings
    static string F(float value) {
        float sign = value < 0f ? -1f : 1f;
        float a = value * sign;
        int scaled = (int)(a * 1000f + 0.5f);
        float r = (scaled / 1000f) * sign;
        return r.ToString();
    }

    static float Rand(float min, float max) => min + (max - min) * Random.value; // UI-only
}