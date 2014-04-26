using UnityEngine;
using System.Collections;

public class BarnMove : MonoBehaviour {
	private float maxspeed = 2.0f;
	private float accel = 6.0f;
	private float jumpaccel = 25.0f;
	
	private Timer jumpTimer = new Timer();
	private float jumpCutoff = 0.14f;
	private bool onGround = true;
	private int maxJumps = 2;
	private int numJumps;

	private BearController controller;

	Rigidbody2D rbody;

	private BarnAnimation animator;

	// Use this for initialization
	void Start () {
		rbody = rigidbody2D;
		rbody.fixedAngle = true;

		animator = GetComponent<BarnAnimation>();

		controller = GetComponent<BearController>();
	}

	public void Move(int dir) {
		switch (controller.state) {
		case CharState.Idle:
			//controller.state = CharState.Moving;
			if (Mathf.Abs (rbody.velocity.x) < maxspeed) {
					rbody.AddForce (new Vector2 ((float)dir * accel, 0.0f));

					if (dir < 0){
						controller.state = CharState.Moving;
						FaceRight();
					}else if (dir > 0){
						controller.state = CharState.Moving;
						FaceLeft();
					}
			}
			break;
		case CharState.Moving:
			if (Mathf.Abs (rbody.velocity.x) < maxspeed || 
					Mathf.Sign (dir) != Mathf.Sign (rbody.velocity.x)) {

					rbody.AddForce (new Vector2 ((float)dir * accel, 0.0f));

					if (dir == 0){
						controller.state = CharState.Idle;
					}
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
			rbody.AddForce (new Vector2 (0.0f, 2.4f*jumpaccel) );
		} else if (controller.state == CharState.Jumping && numJumps++ < maxJumps) {
			jumpTimer.Start ();
			if (jumpTimer.GetElapsedTimeSecs() < jumpCutoff) {
				if ( rbody.velocity.y < 0 ) {
					rbody.velocity = new Vector2(rbody.velocity.x, 0.0f);
				}
				rbody.AddForce (new Vector2 (0.0f, 2.0f*jumpaccel) );
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
		HandleAnimation();
	}

	private void HandleAnimation(){

		switch (controller.state) {
		case CharState.Idle:
			animator.Animate("Idle");
			break;
		case CharState.Moving:
			animator.Animate("Walk");
			break;
		case CharState.Jumping:
			animator.Animate("Jump");
			break;
		default:
			break;
		}
	}

	private void FaceLeft(){
		if (transform.localScale.x < 0) {
			transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
		}
	}
	
	private void FaceRight(){
		if (transform.localScale.x > 0) {
			transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
		}
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
