// Draws wire primitives using our own pipeline: Model -> Projection -> NDC -> Viewport -> Pixel
// Renders via GL.LINES in screen space (GL.LoadPixelMatrix). Attach to the Camera.
using UnityEngine;
using System.Collections.Generic;
using MedGraphics;

namespace CG {
    [RequireComponent(typeof(Camera))]
    public class CG_Draw : MonoBehaviour {
        private Material lineMat;
        private CG_ModelingTransformsDemo demo;
        private Camera cam;

        void Awake() {
            cam = GetComponent<Camera>();
            demo = GetComponent<CG_ModelingTransformsDemo>();
            EnsureMaterial();
        }

        void EnsureMaterial() {
            if (lineMat != null) return;
            var shader = Shader.Find("Hidden/Internal-Colored");
            lineMat = new Material(shader);
            lineMat.hideFlags = HideFlags.HideAndDontSave;
            // basic state
            lineMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            lineMat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            lineMat.SetInt("_ZWrite", 0);

            // Add this line:
            lineMat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
        }

        void OnRenderObject() {
            if (demo == null) return;
            EnsureMaterial();

            int w = Screen.width;
            int h = Screen.height;

            var m = demo.BuildModelMatrix();
            var p = demo.BuildProjectionMatrix(w, h);

            // viewport in pixels
            float vx = demo.vpX * w;
            float vy = demo.vpY * h;
            float vw = demo.vpW * w; if (vw < 1f) vw = 1f;
            float vh = demo.vpH * h; if (vh < 1f) vh = 1f;

            var prims = demo.CollectPrims();
            GL.Color(Color.white); // or any bright color, alpha=1

            lineMat.SetPass(0);
            GL.PushMatrix();
            GL.LoadPixelMatrix(0, w, h, 0); // 2D pixel space, (0,0)=top-left

            DrawLinesTransformed(prims, m, p, vx, vy, vw, vh);

            GL.PopMatrix();
        }

        void DrawLinesTransformed(List<Line3> lines, Mat4 m, Mat4 p, float vx, float vy, float vw, float vh) {
            // --- CUBE (non-axis, non-grid) ---
            GL.Begin(GL.LINES);
            GL.Color(new Color(1, 1, 1, 1)); // white
            for (int i = 0; i < lines.Count; i++) {
                var ln = lines[i];
                DrawLineObject(ln.a, ln.b, m, p, vx, vy, vw, vh);
            }
            GL.End();
        }

        bool IsAxis(Line3 ln, int axis) // 0=x,1=y,2=z
        {
            if (axis == 0) return IsSame(ln.a, new Vec3(0, 0, 0)) && (ln.b.y == 0 && ln.b.z == 0);
            if (axis == 1) return IsSame(ln.a, new Vec3(0, 0, 0)) && (ln.b.x == 0 && ln.b.z == 0);
            return IsSame(ln.a, new Vec3(0, 0, 0)) && (ln.b.x == 0 && ln.b.y == 0);
        }

        bool IsSame(Vec3 a, Vec3 b) {
            return (MathUtils.Abs(a.x - b.x) < 1e-6f) && (MathUtils.Abs(a.y - b.y) < 1e-6f) && (MathUtils.Abs(a.z - b.z) < 1e-6f);
        }

        void DrawLineObject(Vec3 aObj, Vec3 bObj, Mat4 m, Mat4 p, float vx, float vy, float vw, float vh) {
            Vec4 aClip = p * m * Vec4.FromPoint(aObj);
            Vec4 bClip = p * m * Vec4.FromPoint(bObj);

            var pm = p * m;         // composite once


            // perspective divide -> NDC
            Vec3 aNdc = aClip.Homogenized();
            Vec3 bNdc = bClip.Homogenized();

            if (BothOutside(aNdc, bNdc)) return;

            Vector2 aPix = NdcToPixel(aNdc, vx, vy, vw, vh);
            Vector2 bPix = NdcToPixel(bNdc, vx, vy, vw, vh);

            GL.Vertex3(aPix.x, aPix.y, 0);
            GL.Vertex3(bPix.x, bPix.y, 0);
        }

        bool BothOutside(Vec3 a, Vec3 b) {
            if (a.x < -1 && b.x < -1) return true;
            if (a.x > 1 && b.x > 1) return true;
            if (a.y < -1 && b.y < -1) return true;
            if (a.y > 1 && b.y > 1) return true;
            if (a.z < -1 && b.z < -1) return true;
            if (a.z > 1 && b.z > 1) return true;
            return false;
        }

        Vector2 NdcToPixel(Vec3 ndc, float vx, float vy, float vw, float vh) {
            float sx = (ndc.x * 0.5f + 0.5f) * vw + vx;
            float syUp = (ndc.y * 0.5f + 0.5f) * vh + vy; // origin bottom-left
            float sy = (vy + vh) - (syUp - vy);           // flip to top-left
            return new Vector2(sx, sy);
        }
    }
}