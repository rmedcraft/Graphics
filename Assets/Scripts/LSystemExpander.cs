using System.Collections.Generic;
using UnityEngine;
using MedGraphics;

public class LSystemExpander {

    float iterations;
    public float MAX_LENGTH = 1.0f;

    [Range(0.5f, 1.5f)] private string axiom = "F-F-F-F";
    private Dictionary<string, string> rule = new Dictionary<string, string>(){
        { "F", "F-F+F+FF-F-F+F" }
    };

    List<Vec3> verts = new List<Vec3>();
    public List<Line3> edges = new List<Line3>();

    float ANGLE_IN_DEGREES = 90f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public LSystemExpander(float iterations) {
        this.iterations = iterations;
    }

    public string Expand() {
        var startString = axiom;
        for (var i = 0; i < iterations; i++) {
            var newString = "";

            // go through all the characters in the current string
            foreach (char c in startString) {
                if (rule.ContainsKey(c + "")) {
                    newString += rule[c + ""];
                } else {
                    newString += c;
                }
            }

            // store the new string for the next iteration / to be returned
            startString = newString;
        }
        return startString;
    }

    public void Turtle(string derivedString) {
        float xStart = 0.0f;
        float yStart = 0.0f;
        float xEnd = 0.0f;
        float yEnd = 0.0f;
        float angle = 0.0f;

        for (var i = 0; i < derivedString.Length; i++) {
            if (derivedString[i] == 'F') {
                verts.Add(new Vec3(xStart, yStart, 0));
                xEnd = xStart + MAX_LENGTH * Mathf.Sin(angle * Mathf.Deg2Rad);
                yEnd = yStart + MAX_LENGTH * Mathf.Cos(angle * Mathf.Deg2Rad);
                xStart = xEnd;
                yStart = yEnd;
            } else if (derivedString[i] == '+') {
                angle += ANGLE_IN_DEGREES;
            } else if (derivedString[i] == '-') {
                angle -= ANGLE_IN_DEGREES;
            }
        }

        for (var i = 0; i < verts.Count - 1; i++) {
            edges.Add(new Line3(verts[i], verts[i + 1]));
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
