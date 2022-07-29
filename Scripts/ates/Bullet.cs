using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	float moveSpeed = 15f;

	Rigidbody2D rb;

	Cat target;
	Vector2 moveDirection;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		target = GameObject.FindObjectOfType<Cat>();
		moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
		rb.velocity = new Vector2 (moveDirection.x, moveDirection.y);
		Destroy (gameObject, 2f);
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.name.Equals ("Players")) {
			Debug.Log ("Hit!");
			Destroy (gameObject);
		}
	}

}
