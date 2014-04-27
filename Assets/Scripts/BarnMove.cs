using UnityEngine;
using System.Collections;

public class BarnMove : MonoBehaviour {
	private float maxspeed = 3.0f;
	private float accel = 6.0f;
	private float jumpaccel = 25.0f;
	
	private Timer jumpTimer = new Timer();
	[HideInInspector]
	public bool jumpEnd;
	private float jumpCutoff = 0.14f;
	private float grabCutoff = 2.8f;
	public float wallslidedrag = 5;
	private bool onGround = true;
	private bool onWall = false;
	private Vector2 grabPosition;
	private int maxJumps = 2;
	private int numJumps;
	private Timer grabTimer = new Timer();
	private float wallpushamt = 50.0f;

	private BearController controller;
	private WallGrabCollider wallGrabCollider;
	[HideInInspector]
	public GameObject grabbedWall;

	public AudioClip sfxrun;
	public AudioClip sfxhit;
	public AudioClip sfxgrab;
	public AudioClip sfxtele;


	Rigidbody2D rbody;

	private BarnAnimation animator;

	// Use this for initialization
	void Start () {
		rbody = rigidbody2D;
		rbody.fixedAngle = true;

		animator = GetComponent<BarnAnimation>();
		controller = GetComponent<BearController>();
		wallGrabCollider = GetComponentInChildren<WallGrabCollider> ();
	}

	public void Move(int dir) {
		switch (controller.state) {
		case CharState.Idle:
			//controller.state = CharState.Moving;
			//audio.PlayOneShot(sfxrun);
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
			audio.PlayOneShot(sfxrun);
			if (Mathf.Abs (rbody.velocity.x) < maxspeed || 
					Mathf.Sign (dir) != Mathf.Sign (rbody.velocity.x)) {

					rbody.AddForce (new Vector2 ((float)dir * accel, 0.0f));

				if (rbody.velocity.x < 0){
					FaceRight();
				}else if (rbody.velocity.x > 0){
					FaceLeft();
				}

				if (Mathf.Abs (rbody.velocity.x) > 1.0f){
					animator.SetIsRunning(true);
				}else {
					animator.SetIsRunning(false);
				}

				if (dir == 0){
					controller.state = CharState.Idle;
				}
			}
			break;
		case CharState.Jumping:
			if (Mathf.Abs (rbody.velocity.x) < maxspeed/2.0f || 
			    Mathf.Sign (dir) != Mathf.Sign (rbody.velocity.x)) {
				rbody.AddForce (new Vector2 ((float)dir * accel/2.0f, 0.0f));
			}

			if (dir < 0){
				FaceRight();
			}else if (dir > 0){
				FaceLeft();
			}
			
			if (wallGrabCollider.onWall && onWall){
				AttachToWall();
			}
			break;
		case CharState.WallGrab:
			//rbody.
			//rbody.velocity = new Vector2(0,0.2f);
			if (grabTimer.GetElapsedTimeSecs() > grabCutoff) {
				rbody.drag = wallslidedrag;
				rbody.AddForce (new Vector2 (0.0f, -3.0f) );
				controller.state = CharState.WallSlide;
			}

			if (dir < 0){
				FaceRight();
			}else if (dir > 0){
				FaceLeft();
			}

			break;
		case CharState.WallSlide:
			//rbody.
			//rbody.velocity = new Vector2(0,0.2f);
			if (!wallGrabCollider.onWall){
				rbody.drag = 0;
				controller.state = CharState.Idle;
			}
			break;
		default:
			break;
		}
	}

	public void JumpStart() {
		if (controller.state == CharState.Idle || controller.state == CharState.Moving) {
			onGround = false;
			numJumps = 1;
			jumpTimer.Start ();
			controller.state = CharState.Jumping;
			rbody.AddForce (new Vector2 (0.0f, 2.4f*jumpaccel) );
		} else if (controller.state == CharState.WallGrab || controller.state == CharState.WallSlide){
			if (transform.localScale.x > 0){
				rbody.AddForce (new Vector2 (wallpushamt, 2.0f*jumpaccel) );
			}else {
				rbody.AddForce (new Vector2 (-wallpushamt, 2.0f*jumpaccel) );
			}

			jumpTimer.Start ();
			controller.state = CharState.Jumping;
			rbody.drag = 0;

			wallGrabCollider.onWall = false;
			onWall = false;
		}
		else if (controller.state == CharState.Jumping && numJumps++ < maxJumps) {
			jumpTimer.Start ();
			animator.DoubleJump();
			if (jumpTimer.GetElapsedTimeSecs() < jumpCutoff) {
//				if ( rbody.velocity.y < 0 ) {
					rbody.velocity = new Vector2(rbody.velocity.x, 0.0f);
//				}
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

	public void Lunge(Vector2 vec) {
		if ( transform.localScale.x < 0 ) {
			vec.x *= -1.0f;
		}
		rbody.AddForce (vec);
	}

	public void AttachToWall(){
		grabTimer.Start();
		rbody.drag = 1000000;
		controller.state = CharState.WallGrab;
		numJumps = maxJumps - 1;
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
		case CharState.WallGrab:
			animator.Animate("WallGrab");
			break;
		default:
			break;
		}
	}

	private void FaceLeft(){
		if (transform.localScale.x > 0) {
			transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
		}
	}
	
	private void FaceRight(){
		if (transform.localScale.x < 0) {
			transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{

		var normal = col.contacts[0].normal;
		// get its elevation angle in degrees:
		var angle = Mathf.Rad2Deg * Mathf.Asin(normal.y);

		// if normal points below -limAngle, collision is from above:
		if (angle < 0) {
				} else // if angle > limAngle, collision is from below:
		if (angle > 3 && angle < 177) {
						onGround = true;
						rbody.drag = 0;
						//Debug.Log ("onGroundl");
						controller.state = CharState.Moving;
				} else // if angle > limAngle, collision is from below:
		if (((angle > 179 && angle < 181) || (angle > -1 && angle < 1)) && col.transform.tag != "Player") {
				onWall = true;
				grabbedWall = col.gameObject;
				audio.PlayOneShot(sfxgrab);
				if (transform.localScale.x > 0) {
						FaceRight ();
				} else {
						FaceLeft ();
				}
		}
		else { // otherwise collision is lateral:
		}

	}

	public void LetGoWall() {
		if (onWall) {
			onWall = false;
			wallGrabCollider.onWall = false;
			controller.state = CharState.Idle;
			rbody.drag = 0;
		}
	}

	void OnCollisionExit2D(Collision2D col)
	{
		if (onWall && col.transform.tag != "Player"){
			onWall = false;
		}
		if (col.gameObject == grabbedWall) {
			grabbedWall = null;
		}
	}
}
