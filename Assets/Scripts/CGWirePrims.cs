using UnityEngine;
using System.Collections.Generic;
using MedGraphics;
public class Line3 {
    public Vec3 a, b;
    public Line3(Vec3 a, Vec3 b) {
        this.a = a;
        this.b = b;
    }
}

public class CGWirePrims {

    public static List<Line3> Axes(float length = 1f) {
        float s = length * 0.5f;

        Vec3[] v = new Vec3[] {
            new Vec3(-1, -1, -1), // v0
            new Vec3(1, -1, -1), // v1
            new Vec3(1, 1, -1), // v2
            new Vec3(-1, 1, -1), // v3
            new Vec3(-1, 1, 1), // v4
            new Vec3(1, -1, 1), // v5
            new Vec3(1, 1, 1), // v6
            new Vec3(-1, 1, 1) // v7
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
}
