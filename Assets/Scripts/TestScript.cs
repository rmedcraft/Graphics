using UnityEngine;

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
    }

    // Update is called once per frame
    void Update() {

    }
}
