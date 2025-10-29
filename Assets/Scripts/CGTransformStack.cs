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

    public void Translate(float x, float y, float z) {
        current *= Mat4.Translation(x, y, z);
    }

    public void Scale(float x, float y, float z) {
        current *= Mat4.Scaling(x, y, z);
    }


    public void RotateX(float degrees) {
        current *= Mat4.RotationX(degrees);
    }
    public void RotateY(float degrees) {
        current *= Mat4.RotationY(degrees);
    }
    public void RotateZ(float degrees) {
        current *= Mat4.RotationZ(degrees);
    }

    // TODO[PA-Rodrigues]: apply Rodrigues rotation about axis
    public void RotateAxisRodrigues(Vec3 axis, float angleDeg) {
        current *= Mat4.RotationRodrigues(axis, angleDeg);
    }

    // TODO[PA-Rodrigues]: Rodrigues with pivot (sandwich)
    public void RotateAxisRodriguesPivot(Vec3 axis, float angleDeg, Vec3 pivot) {
        current *= Mat4.RotationRodriguesAroundPivot(axis, angleDeg, pivot);
    }
}