using UnityEngine;   // MonoBehaviour + GUI only

public class VectorGUI : MonoBehaviour {
    // Editable components for A and B
    [SerializeField] float ax = 1f, ay = 2f, az = 3f;
    [SerializeField] float bx = 4f, by = 5f, bz = 6f;

    Rect win = new Rect(12, 12, 920, 520);

    void OnGUI() {
        win = GUI.Window(42, win, DrawWindow, "Vector Display (Vec3)");
    }

    void DrawWindow(int id) {
        GUILayout.BeginVertical();

        GUILayout.Label("Vector A");
        Slider(ref ax, "Ax", -5f, 5f);
        Slider(ref ay, "Ay", -5f, 5f);
        Slider(ref az, "Az", -5f, 5f);

        GUILayout.Space(6);

        GUILayout.Label("Vector B");
        Slider(ref bx, "Bx", -5f, 5f);
        Slider(ref by, "By", -5f, 5f);
        Slider(ref bz, "Bz", -5f, 5f);

        // Build vectors and compute with your custom math
        var A = new Vec3(ax, ay, az);
        var B = new Vec3(bx, by, bz);

        var add = A + B;
        var sub = A - B;
        var dot = new Vec3().DotVectors(A, B);
        var crs = new Vec3().CrossVectors(A, B);
        var magA = A.Magnitude();
        var magB = B.Magnitude();
        var nA = A.Normalize().Clone();
        var nB = B.Normalize().Clone();

        GUILayout.Space(8);
        GUILayout.Label("Results");
        GUILayout.Label("A      = " + V(A));
        GUILayout.Label("B      = " + V(B));
        GUILayout.Label("A + B  = " + V(add));
        GUILayout.Label("A - B  = " + V(sub));
        GUILayout.Label("dot    = " + F(dot));
        GUILayout.Label("A × B  = " + V(crs));
        GUILayout.Label("|A|    = " + F(magA) + "   |B| = " + F(magB));
        GUILayout.Label("Â      = " + V(nA));
        GUILayout.Label(" B̂     = " + V(nB));

        GUILayout.Space(6);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Reset")) { ax = 1f; ay = 2f; az = 3f; bx = 4f; by = 5f; bz = 6f; }
        if (GUILayout.Button("Randomize")) {
            ax = Rand(-3f, 3f); ay = Rand(-3f, 3f); az = Rand(-3f, 3f);
            bx = Rand(-3f, 3f); by = Rand(-3f, 3f); bz = Rand(-3f, 3f);
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

    // Lightweight number -> string (round ~3 decimals) without format strings
    static string F(float value) {
        float sign = value < 0f ? -1f : 1f;
        float a = value * sign;
        int scaled = (int)(a * 1000f + 0.5f);
        float r = (scaled / 1000f) * sign;
        return r.ToString();
    }

    static string V(Vec3 v) => "(" + F(v.x) + ", " + F(v.y) + ", " + F(v.z) + ")";

    static float Rand(float min, float max) => min + (max - min) * Random.value; // UI-only
}