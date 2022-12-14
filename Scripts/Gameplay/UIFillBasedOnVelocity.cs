using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFillBasedOnVelocity : MonoBehaviour {

    public Rigidbody2D objectToMeasure;
    public float maxVelocity = 10.0f;

    private Image image;

	// Use this for initialization
	void Start () {

        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {

        image.fillAmount = objectToMeasure.velocity.magnitude / maxVelocity;
	}
}
