using UnityEngine;
using MedGraphics;
public class TestScript : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        Vec3 testVec = new(1, 0, 0);
        print(testVec.ToString());

        testVec.AddScalar(1);
        print(testVec.ToString());

        print(testVec.Magnitude());
        testVec.Normalize();
        print(testVec.Magnitude());
        print(testVec.ToString());

        Vec3 otherVec = new(1, 1, 1);

        otherVec.Cross(testVec);
        print(otherVec.ToString());

        Mat4 identity = Mat4.Identity();
        // print(identity.ToString());

        Mat4 testMat = new Mat4(new float[,]{
            {2f, 2f, 1f, 1f },
            {2f, 2f, 1f, 1f },
            {2f, 2f, 2f, 2f },
            {2f, 2f, 2f, 2f },
        });
        print(testMat);

        Mat4 testMat2 = new Mat4(new float[,]{
            {0f, 1f, 1f, 0f },
            {1f, 1f, 2f, 2f },
            {2f, 2f, 1f, 1f },
            {1f, 0f, 3f, 1f },
        });
        print(testMat2);

        Mat4 mult = testMat * testMat2;
        print(mult);
    }

    // Update is called once per frame
    void Update() {

    }
}
