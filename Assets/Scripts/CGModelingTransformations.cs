using UnityEngine;
using System.Collections.Generic;
using MedGraphics;
using System;

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
    public Vector3 spinSpeedDegPerSec = new Vector3(0, 45, 6);

    [Header("Grid Transform")]
    public Vector3 translateGrid = new Vector3(0, -1, 8);
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

    [Header("Camera (OpenGL LookAt, RHS)")]
    public Vector3 camEye = new Vector3(0, 0, 1);
    public Vector3 camTarget = new Vector3(0, 0, 0);
    public Vector3 worldUp = new Vector3(0, 1, 0); // reference world up

    [Header("Chaikin Subdivision (XY plane)")]
    public bool showChaikin = true;
    [Range(0, 6)] public int chaikinLevels = 0;
    public float chaikinSquareSize = 2f;

    [Header("Quadratic Bezier (XY plane)")]
    public bool showBezier = true;
    public bool showBezierControlPolygon = false;
    public bool bezierClosed = true; // keep true for a loop around the square
    public float bezierSquareSize = 2f; // size of the 4-pt control loop
    [Range(2, 64)] public int bezierSamplesPerSegment = 12;

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

    public Mat4 BuildViewMatrix() {
        return MathUtils.LookAtRH(new Vec3(camEye), new Vec3(camTarget), new Vec3(worldUp));
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
        axisAngleDeg = t * 90f % 360;

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

    public List<Line3> CollectChaikin() {
        var lines = new List<Line3>();
        if (showChaikin) lines.AddRange(BuildChaikinCurve());
        return lines;
    }

    public List<Line3> CollectCube() {
        var lines = new List<Line3>();

        // if (showCube) {
        //     if (useAssignmentLayout) {
        //         lines.AddRange(BuildStackedCubes());
        //     } else {
        //         lines.AddRange(CGWirePrims.Cube(cubeSize));
        //     }
        // }

        // lines.AddRange(CollectChaikin());
        if (showBezier) {
            lines.AddRange(BuildBezierCurve());
        }
        return lines;
    }

    List<Line3> BuildChaikinCurve() {
        // Guarantee a usable square side length so Chaikin never runs on degenerate loop
        // (degenerate = control points collapse to one spot so no edges remain to subdivide)
        float sideLength = Mathf.Max(0.1f, chaikinSquareSize);
        // Half the side lets us offset from the origin to place a centered square easily
        float halfSide = sideLength * 0.5f;
        //Note Vec3 and Vec4 are our implementations of vector objects not the Unity
        var control = new List<Vec3>(4){
            new Vec3(-halfSide, -halfSide, 0f),
            new Vec3(halfSide, -halfSide, 0f),
            new Vec3(halfSide, halfSide, 0f),
            new Vec3(-halfSide, halfSide, 0f)
        };
        // "refined" holds the subdivided control loop after the requested Chaikin levels
        var refined = ApplyChaikin(control, chaikinLevels);
        var result = new List<Line3>(refined.Count);
        for (int i = 1; i < refined.Count; i++) {
            Vec3 a = refined[i];
            Vec3 b = refined[(i + 1) % refined.Count];
            result.Add(new Line3(a, b));
        }
        return result;
    }

    public List<Vec3> ApplyChaikin(List<Vec3> points, float levels) {
        var current = new List<Vec3>(points);

        if (levels == 0) {
            return current;
        }

        for (var i = 0; i < levels; i++) {
            var count = current.Count;
            var next = new List<Vec3>();

            for (var j = 0; j < count; j++) {
                var p0 = current[j];
                var p1 = current[(j + 1) % count];

                var q = 0.75f * p0 + 0.25f * p1;
                var r = 0.25f * p0 + 0.75f * p1;

                next.Add(q);
                next.Add(r);
            }

            current = next;
        }

        return current;
    }

    // Function EvaluateQuadraticBezier(P0, P1, P2, t):
    //     A = (1 - t) * P0 + t * P1
    //     B = (1 - t) * P1 + t * P2
    //     return (1 - t) * A + t * B
    // End
    public Vec3 EvaluateQuadraticBezier(Vec3 P0, Vec3 P1, Vec3 P2, float t) {
        var a = (1 - t) * P0 + t * P1;
        var b = (1 - t) * P1 + t * P2;
        return (1 - t) * a + t * b;
    }

    // Procedure SampleQuadraticSegment(P0, P1, P2, steps, dest, includeFirstPoint):
    //     for s = 0 .. steps-1:
    //     if (includeFirstPoint == false) and (s == 0): continue
    //     t = (steps == 1) ? 0 : s / (steps - 1)
    //     dest.append( EvaluateQuadraticBezier(P0, P1, P2, t) )
    // End

    public void SampleQuadraticSegment(Vec3 P0, Vec3 P1, Vec3 P2, int steps, List<Vec3> dest, bool includeFirstPoint) {
        for (var i = 0; i < steps; i++) {
            if (!includeFirstPoint && i == 0) {
                continue;
            }

            var t = (steps == 1) ? 0 : (float)i / (steps - 1);
            dest.Add(EvaluateQuadraticBezier(P0, P1, P2, t));
        }
    }

    // Function SampleBezier(Control[], samplesPerSegment, closed):
    //     if Control.length < 3: return copy(Control)
    //     steps = max(2, samplesPerSegment)
    //     count = Control.length
    //     sampled = []
    //     segmentCount = closed ? count : (count - 2)
    //     for i = 0 .. segmentCount-1:
    //     P0 = Control[i]
    //     P1 = closed ? Control[(i+1) % count] : Control[i+1]
    //     P2 = closed ? Control[(i+2) % count] : Control[i+2]
    //     includeFirst = (i == 0)
    //     SampleQuadraticSegment(P0, P1, P2, steps, sampled, includeFirst)
    //     return sampled
    // End

    public List<Vec3> SampleBezier(List<Vec3> control, int samplesPerSegment) {
        if (control.Count < 3) {
            return new List<Vec3>(control);
        }

        var steps = (int)MathUtils.Max(2, samplesPerSegment);
        var count = control.Count;
        var sampled = new List<Vec3>();
        var segmentCount = count;

        for (var i = 0; i < segmentCount; i++) {
            var P0 = control[i];
            var P1 = control[(i + 1) % count];
            var P2 = control[(i + 2) % count];

            var includeFirst = (i == 0);
            SampleQuadraticSegment(P0, P1, P2, steps, sampled, includeFirst);
        }

        return sampled;
    }

    // Procedure BuildBezierCurve():
    //     side = clamp(bezierSquareSize, 0.1, +inf)
    //     h = side / 2
    //     Control = [(-h,-h,0), (h,-h,0), (h,h,0), (-h,h,0)]
    //     Points = SampleBezier(Control, bezierSamplesPerSegment, bezierClosed)
    //     if Points.length < 2: Points = Control
    //     return BuildPolyline(Points, bezierClosed) # converts to connected Line3 segments
    // End

    public List<Line3> BuildBezierCurve() {
        var side = Mathf.Clamp(bezierSquareSize, 0.1f, Mathf.Infinity);
        var h = side / 2;

        var control = new List<Vec3> {
            new Vec3(-h, -h, 0),
            new Vec3(h, -h, 0),
            new Vec3(h, h, 0),
            new Vec3(-h, h, 0),
        };

        var points = SampleBezier(control, bezierSamplesPerSegment);
        if (points.Count < 2) {
            points = control;
        }
        return BuildPolyline(points, control, bezierClosed);
    }

    public List<Line3> BuildPolyline(List<Vec3> points, List<Vec3> control, bool isClosed) {
        var polyline = new List<Line3>();

        // check if a vector exists in a list
        static bool hasVec(List<Vec3> list, Vec3 test) {
            foreach (Vec3 v in list) {
                if (v.Equals(test)) {
                    return true;
                }
            }
            return false;
        }

        var endpoints = new List<Vec3>();

        for (var i = 0; i < points.Count - 1; i++) {
            var line = new Line3(points[i], points[i + 1]);
            if (!hasVec(control, points[i])) {
                polyline.Add(line);
            } else {
                // store endpoints for closing the polyline
                endpoints.Add(points[i + 1]);
            }
        }

        // close polyline by connecting control points to the associated bezier endpoint
        if (isClosed) {
            for (var i = 0; i < endpoints.Count; i++) {
                polyline.Add(new Line3(endpoints[i], control[i]));
            }
        }
        return polyline;
    }
}