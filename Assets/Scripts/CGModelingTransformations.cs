using UnityEngine;
using System.Collections.Generic;
using MedGraphics;

public class CG_ModelingTransformsDemo : MonoBehaviour {
    [Header("Projection")]
    public bool usePerspective = true;
    [Range(10f, 120f)] public float fovY = 60f;
    public float orthoHeight = 4f;
    public float nearPlane = 0.1f;
    public float farPlane = 100f;

    [Header("Viewport (normalized)")]
    [Range(0, 1)] public float vpX = 0f;
    [Range(0, 1)] public float vpY = 0f;
    [Range(0, 1)] public float vpW = 1f;
    [Range(0, 1)] public float vpH = 1f;

    [Header("Modeling Transform (T * Rz * Ry * Rx * S)")]

    public Vec3 translate = new Vec3(0, 0, 6);
    public Vec3 rotateDeg = new Vec3(0, 0, 0);
    public Vec3 scale = new Vec3();
    public bool autoSpin = true;
    public Vec3 spinSpeedDegPerSec = new Vec3(0, 45, 0);

    [Header("Primitives")]
    // public bool showAxes = true;
    // public float axesLen = 2f;
    public bool showCube = true;
    public float cubeSize = 2f;
    // public bool showGrid = true;
    // public int gridHalfSteps = 5;
    // public float gridStep = 1f;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) usePerspective = false;
        if (Input.GetKeyDown(KeyCode.Alpha2)) usePerspective = true;
        if (Input.GetKeyDown(KeyCode.A)) autoSpin = !autoSpin;

        if (autoSpin) {
            rotateDeg.x += spinSpeedDegPerSec.x * Time.deltaTime;
            rotateDeg.y += spinSpeedDegPerSec.y * Time.deltaTime;
            rotateDeg.z += spinSpeedDegPerSec.z * Time.deltaTime;
        }
    }

    public Mat4 BuildModelMatrix() {
        var t = Mat4.Translation(translate.x, translate.y, translate.z);
        var rx = Mat4.RotationX(rotateDeg.x);
        var ry = Mat4.RotationY(rotateDeg.y);
        var rz = Mat4.RotationZ(rotateDeg.z);
        var s = Mat4.Scaling(scale.x, scale.y, scale.z);
        // order: M = T * rz * ry * rx * s
        return t * rz * ry * rx * s;
    }

    public Mat4 BuildProjectionMatrix(int pixelW, int pixelH) {
        float aspect = (pixelH != 0) ? (pixelW / (float)pixelH) : 1f;
        if (usePerspective) {
            return Mat4.Perspective(fovY, aspect, nearPlane, farPlane);
        } else {
            float halfH = orthoHeight * .5f;
            float halfW = halfH * aspect;

            return Mat4.Ortho(-halfW, halfW, -halfH, halfH, nearPlane, farPlane);
        }
    }

    public List<Line3> CollectPrims() {
        var lines = new List<Line3>();

        if (showCube) lines.AddRange(CGWirePrims.Axes());
        return lines;
    }
}