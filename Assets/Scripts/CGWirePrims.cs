using UnityEngine;
using System.Collections.Generic;
using MedGraphics;
public class Line3 {
    public Vec3 a, b;
    public Line3(Vec3 a, Vec3 b) {
        this.a = a;
        this.b = b;
    }

    override
    public string ToString() {
        return a.ToString() + "\n" + b.ToString();
    }
}

public class CGWirePrims {

    public static List<Line3> Cube(float length = 1f) {
        float s = length * 0.5f;

        Vec3[] v = new Vec3[] {
            new Vec3(-s,-s,-s), new Vec3(+s,-s,-s),
            new Vec3(+s,+s,-s), new Vec3(-s,+s,-s),
            new Vec3(-s,-s,+s), new Vec3(+s,-s,+s),
            new Vec3(+s,+s,+s), new Vec3(-s,+s,+s)
        };

        int[,] edges = new int[,] {
            {0, 1}, {1, 2}, {2, 3}, {3, 0}, // bottom face
            {0, 4}, {1, 5}, {2, 6}, {3, 7}, // vertical edges connecting top and bottom
            {4, 5}, {5, 6}, {6, 7}, {7, 4}, // top face
        };

        var lines = new List<Line3>(12);
        for (int i = 0; i < edges.GetLength(0); i++) {
            lines.Add(new Line3(v[edges[i, 0]], v[edges[i, 1]]));
        }

        return lines;
    }

    public static List<Line3> Axes(float L = 5f) {
        return new List<Line3> {
            new Line3(new Vec3(0, 0, 0), new Vec3(L, 0, 0)), //  +X
            new Line3(new Vec3(0, 0, 0), new Vec3(-L, 0, 0)), // -X
            new Line3(new Vec3(0, 0, 0), new Vec3(0, L, 0)), //  +Y
            new Line3(new Vec3(0, 0, 0), new Vec3(0, -L, 0)), // -Y
            new Line3(new Vec3(0, 0, 0), new Vec3(0, 0, L)), //  +Z
            new Line3(new Vec3(0, 0, 0), new Vec3(0, 0, -L)) //  -Z
        };
    }

    public static List<Line3> GridXZ(float extent = 8f, float step = 1f) {
        // Starting input guards
        if (step <= 0) {
            step = 1f;
        }

        if (extent < step) {
            extent = step;
        }

        int N = (int)Mathf.Ceil(extent / step);
        float E = N * step;

        // create grid lines
        var lines = new List<Line3>(4 * N + 2);

        for (int i = -N; i <= N; i++) {
            lines.Add(new Line3(new Vec3(-E, 0, i * step), new Vec3(+E, 0, i * step)));
            lines.Add(new Line3(new Vec3(i * step, 0, -E), new Vec3(i * step, 0, +E)));
        }

        Debug.Log("Printing Lines");
        lines.ForEach((line) => {
            Debug.Log(line);
        });

        return lines;
    }
}
