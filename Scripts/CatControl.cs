using UnityEngine;
public class CatControl : MonoBehaviour {
	// Use this for initialization
	void Start () {
		GameManager.instance.bulletPlayerAnimator = GetComponent<Animator>();
	}
	
}
