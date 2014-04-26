using UnityEngine;
using System.Collections;

public class BarnMove : MonoBehaviour {
	public enum CharState { Idle, Moving, Jumping, Falling, WallGrab, WallSlide };

	[HideInInspector]
	public CharState state {
		get;
		set;
	}

	private float maxspeed = 2.0f;
	private float accel = 6.0f;
	private float jumpaccel = 20.0f;
	
	private Timer jumpTimer = new Timer();
	private float jumpCutoff = 0.2f;

	Rigidbody2D rbody;
	// Use this for initialization
	void Start () {
		rbody = rigidbody2D;
		rbody.fixedAngle = true;
		state = CharState.Idle;
	}

	public void Move(int dir) {
		switch (state) {
		case CharState.Idle:
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

	public void Jump() {
		Debug.Log (state);
//		rbody.AddForce (new Vector2 (0.0f, jumpaccel) );
		if (state == CharState.Idle || state == CharState.Moving) {
				jumpTimer.Start ();
				state = CharState.Jumping;
				rbody.AddForce (new Vector2 (0.0f, jumpaccel) );
		} else if (state == CharState.Jumping ) {
			if (jumpTimer.GetElapsedTimeSecs() < jumpCutoff) {
				rbody.AddForce (new Vector2 (0.0f, jumpaccel) );
			}
			jumpTimer.Stop();
		}
	}
	
	// Update is called once per frame
	void Update () {
		 
	}
}
