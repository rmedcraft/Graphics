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
    public Vector3 translate = new Vector3(0, 0, -6);
    public Vector3 rotateDeg = new Vector3(0, 0, 0);
    public Vector3 scale = new Vector3(1, 1, 1);
    public bool autoSpin = true;
    public Vector3 spinSpeedDegPerSec = new Vector3(0, 45, 0);

    [Header("Grid Transform")]
    public Vector3 translateGrid = new Vector3(0, 0, 0);
    public Vector3 rotateGrid = new Vector3(0, 0, 0);
    public Vector3 scaleGrid = new Vector3(1, 1, 1);

    [Header("Primitives")]
    public bool showCube = true;
    public float cubeSize = 2f;
    public bool showAxes = true;
    public float axesLength = 2f;
    public bool showGridXZ = true;
    public float gridExtent = 8f;
    public float gridStep = 1f;

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
            float halfH = orthoHeight * 0.5f;
            float halfW = halfH * aspect;
            return Mat4.Ortho(-halfW, +halfW, -halfH, +halfH, nearPlane, farPlane);
        }
    }

    public List<Line3> CollectGridAndAxes() {
        var lines = new List<Line3>();
        if (showAxes) {
            lines.AddRange(CGWirePrims.Axes(axesLength));
        }
        if (showGridXZ) {
            lines.AddRange(CGWirePrims.GridXZ(gridExtent, gridStep));
        }
        return lines;
    }

    public List<Line3> CollectCube() {
        var lines = new List<Line3>();

        if (showCube) lines.AddRange(CGWirePrims.Cube(cubeSize));
        return lines;
    }
}