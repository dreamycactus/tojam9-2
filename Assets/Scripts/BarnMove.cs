using UnityEngine;
using System.Collections;

public class BarnMove : MonoBehaviour {
	private float maxspeed = 2.0f;
	private float accel = 6.0f;
	private float jumpaccel = 20.0f;
	
	private Timer jumpTimer = new Timer();
	private float jumpCutoff = 0.2f;
	private bool onGround = true;
	private int maxJumps = 2;
	private int numJumps;

	private BearController controller;

	Rigidbody2D rbody;
	// Use this for initialization
	void Start () {
		rbody = rigidbody2D;
		rbody.fixedAngle = true;
		controller = GetComponent<BearController> ();

	}

	public void Move(int dir) {
		switch (controller.state) {
		case CharState.Idle:
			controller.state = CharState.Moving;
			if (Mathf.Abs (rbody.velocity.x) < maxspeed) {
					rbody.AddForce (new Vector2 ((float)dir * accel, 0.0f));
			}
			break;
		case CharState.Moving:
			if (Mathf.Abs (rbody.velocity.x) < maxspeed || 
					Mathf.Sign (dir) != Mathf.Sign (rbody.velocity.x)) {

					rbody.AddForce (new Vector2 ((float)dir * accel, 0.0f));
			}
			break;
		case CharState.Jumping:
			if (Mathf.Abs (rbody.velocity.x) < maxspeed/2.0f || 
			    Mathf.Sign (dir) != Mathf.Sign (rbody.velocity.x)) {
				
				rbody.AddForce (new Vector2 ((float)dir * accel, 0.0f));
			}
			break;
		default:
			break;
		}
	}

	public void JumpStart() {
		if (controller.state == CharState.Idle || controller.state == CharState.Moving) {
			numJumps = 1;
			jumpTimer.Start ();
			controller.state = CharState.Jumping;
			rbody.AddForce (new Vector2 (0.0f, jumpaccel) );
		} else if (controller.state == CharState.Jumping && numJumps++ < maxJumps) {
			jumpTimer.Start ();
			if (jumpTimer.GetElapsedTimeSecs() < jumpCutoff) {
				rbody.AddForce (new Vector2 (0.0f, jumpaccel) );
			}
		}
	}

	public void Jump() {
		if (controller.state == CharState.Jumping ) {
			if (jumpTimer.GetElapsedTimeSecs() < jumpCutoff) {
				rbody.AddForce (new Vector2 (0.0f, jumpaccel) );
			}
		}
	}

	// Update is called once per frame
	void Update () {
		 
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		var normal = col.contacts[0].normal;
		// get its elevation angle in degrees:
		var angle = Mathf.Rad2Deg * Mathf.Asin(normal.y);
		Debug.Log (angle);
		// if normal points below -limAngle, collision is from above:
		if (angle < 0){
		} 
		else // if angle > limAngle, collision is from below:
		if (angle > 0){
			onGround = true;
			Debug.Log ("onGround");
			controller.state = CharState.Idle;
		}
		else { // otherwise collision is lateral:
		}

	}
}
