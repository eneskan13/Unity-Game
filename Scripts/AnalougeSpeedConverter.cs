using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalougeSpeedConverter : MonoBehaviour {

    static float minAngle = -2506.0f;
    static float maxAngle = -2688.0f;
    static AnalougeSpeedConverter thisSpeedo;




    // Use this for initialization
    void Start() {

        thisSpeedo = this;
    }

    public static void ShowSpeed(float speed, float min, float max)
    {
        float ang = Mathf.Lerp(minAngle, maxAngle, Mathf.InverseLerp(min, max, speed));
        thisSpeedo.transform.eulerAngles = new Vector3(0, 0, ang);


    }
}
