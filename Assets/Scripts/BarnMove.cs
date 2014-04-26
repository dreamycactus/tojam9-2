using UnityEngine;
using System.Collections;

public class BarnMove : MonoBehaviour {
	public float maxspeed = 10.0f;
	public float accel = 0.2f;

	Rigidbody2D rbody;
	// Use this for initialization
	void Start () {
		rbody = rigidbody2D;
		rbody.fixedAngle = true;
	}

	public void Move(int dir) {
		rbody.AddForce (new Vector2 ( (float)dir * 0.2f, 0.0f) );
	}
	
	// Update is called once per frame
	void Update () {
		 
	}
}
