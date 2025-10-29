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
    public Vector3 translate = new Vector3(0, 0, 15);
    public Vector3 rotateDeg = new Vector3(35, 0, 0);
    public Vector3 scale = new Vector3(1, 1, 1);
    public bool autoSpin = true;
    public Vector3 spinSpeedDegPerSec = new Vector3(0, 45, 0);

    [Header("Grid Transform")]
    public Vector3 translateGrid = new Vector3(0, -1, 15);
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

    [Header("Assignment Controls Rodrigues")]
    public Vector3 axisDir = new Vector3(1f, 1f, 1f); // arbitrary axis (normalized in builder)
    [Range(0f, 360f)] public float axisAngleDeg = 0f;
    public bool usePivot = false;
    public Vector3 pivotPoint = new Vector3(0f, 0f, 0f);

    [Header("Four Cubes (equal displacement)")]
    [Range(0.1f, 10f)] public float cubeDisplacement = 3f;
    public bool useAssignmentLayout = true; // toggle assignment mode

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) usePerspective = false;
        if (Input.GetKeyDown(KeyCode.Alpha2)) usePerspective = true;
        if (Input.GetKeyDown(KeyCode.A)) autoSpin = !autoSpin;

        if (autoSpin && !useAssignmentLayout) {
            rotateDeg.x += spinSpeedDegPerSec.x * Time.deltaTime;
            rotateDeg.y += spinSpeedDegPerSec.y * Time.deltaTime;
            rotateDeg.z += spinSpeedDegPerSec.z * Time.deltaTime;
        }
        if (useAssignmentLayout) {
            rotateDeg = new Vector3(0, 0, 0);
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
    public Mat4 BuildGridMatrix() {
        var t = Mat4.Translation(translateGrid.x, translateGrid.y, translateGrid.z);
        var rx = Mat4.RotationX(rotateGrid.x);
        var ry = Mat4.RotationY(rotateGrid.y);
        var rz = Mat4.RotationZ(rotateGrid.z);
        var s = Mat4.Scaling(scaleGrid.x, scaleGrid.y, scaleGrid.z);
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

    public List<Line3> BuildStackedCubes() {
        // TODO: student implementation
        var cubes = new List<Line3>();
        var baseCube = CGWirePrims.Cube(cubeSize);
        float d = cubeDisplacement;

        // Example local angles (you may expose as inspector fields instead)
        float t = Time.time;
        float ax = t * 90f; // for +X cube
        float ay = t * 70f; // for -X cube
        float az = t * 50f; // for +Y cube
        axisAngleDeg = t * 90f;

        // Rodrigues inputs (axis normalized inside builder)
        Vec3 axis = new Vec3(axisDir);
        float aDeg = axisAngleDeg;

        var stack = new CGTransformStack();

        // +X cube X-rotation
        stack.LoadIdentity();
        stack.Translate(+d, 0, 0);
        stack.RotateX(ax);
        cubes.AddRange(TransformLines(baseCube, stack.Current()));

        // -X cube Y-rotation
        // TODO:
        stack.LoadIdentity();
        stack.Translate(-d, 0, 0);
        stack.RotateY(ay);
        cubes.AddRange(TransformLines(baseCube, stack.Current()));

        // +Y cube Z-rotation
        // TODO:

        stack.LoadIdentity();
        stack.Translate(0, +d, 0);
        stack.RotateZ(az);
        cubes.AddRange(TransformLines(baseCube, stack.Current()));

        // -Y cube Rodrigues (with optional pivot)

        // TODO: LoadIdentity matrix and translate like before
        stack.LoadIdentity();
        stack.Translate(0, -d, 0);
        if (usePivot) {
            stack.RotateAxisRodriguesPivot(axis, aDeg, new Vec3(pivotPoint));
        } else {
            stack.RotateAxisRodrigues(axis, aDeg);
        }
        cubes.AddRange(TransformLines(baseCube, stack.Current()));

        return cubes;
    }

    public List<Line3> TransformLines(List<Line3> lines, Mat4 transform) {
        var result = new List<Line3>();
        foreach (var line in lines) {
            Vec4 a = transform * Vec4.FromPoint(line.a);
            Vec4 b = transform * Vec4.FromPoint(line.b);
            result.Add(new Line3(a.XYZ(), b.XYZ()));
        }
        return result;
    }

    public List<Line3> CollectGrid() {
        var lines = new List<Line3>();
        if (showGridXZ) {
            lines.AddRange(CGWirePrims.GridXZ(gridExtent, gridStep));
        }
        return lines;
    }

    public List<Line3> CollectAxes() {
        var lines = new List<Line3>();
        if (showAxes) {
            lines.AddRange(CGWirePrims.Axes(axesLength));
        }
        return lines;
    }

    public List<Line3> CollectCube() {
        var lines = new List<Line3>();

        if (showCube) {
            if (useAssignmentLayout) {
                lines.AddRange(BuildStackedCubes());
            } else {
                lines.AddRange(CGWirePrims.Cube(cubeSize));
            }
        }
        return lines;
    }

}