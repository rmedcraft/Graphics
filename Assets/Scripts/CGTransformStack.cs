using UnityEngine;
using MedGraphics;
using System.Collections.Generic;
public class CGTransformStack {
    List<Mat4> stack = new List<Mat4>();

    private Mat4 current;

    public CGTransformStack() {
        current = Mat4.Identity();
    }

    public Mat4 Current() {
        return current;
    }

    public void LoadIdentity() {
        current = Mat4.Identity();
    }

    public void Push() {
        stack.Add(current);
    }

    public Mat4 Pop() {
        if (stack.Count <= 0) {
            return Mat4.Identity();
        }

        current = stack[stack.Count - 1];
        stack.RemoveAt(stack.Count - 1);
        return current;
    }

    public void Mult(Mat4 m) {
        current *= m;
    }

    public void Translate(float x, float y, float z) {
        current = current.Translate(x, y, z);
    }

    public void Scale(float x, float y, float z) {
        current = current.Scale(x, y, z);
    }


    public void RotateX(float degrees) {
        current = current.RotateX(degrees);
    }
    public void RotateY(float degrees) {
        current = current.RotateY(degrees);
    }
    public void RotateZ(float degrees) {
        current = current.RotateZ(degrees);
    }
}