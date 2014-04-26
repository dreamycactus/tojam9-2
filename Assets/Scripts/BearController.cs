using UnityEngine;
using System.Collections;
public enum CharState { Idle, Moving, Jumping, Falling, WallGrab, WallSlide };
public class BearController : MonoBehaviour {
//	public enum CharState { Idle, Moving, Jumping, Falling, WallGrab, WallSlide };
	
	[HideInInspector]
	public CharState state {
		get;
		set;
	}
	private Animator animator;
	private BarnMove barnMove;
	private BarnAttack barnAttack;
	// Use this for initialization
	void Start()
	{
		animator = this.GetComponent<Animator>();
		barnMove = GetComponent<BarnMove> ();
		state = CharState.Idle;
		barnAttack = GetComponentInChildren<BarnAttack> ();
	}

	void HandleInput()
	{
		Input.GetJoystickNames ();
		var idx = Input.GetAxis ("Hor1");
		var idy = Input.GetAxis("Ver1");

		if (transform.localScale.x > 0) {
			FlipSprite();
		}
		
		barnMove.Move(AxisRound(idx));
		var ijmpdown = Input.GetButtonDown ("Jump1");
		var ijmpstate = Input.GetButton ("Jump1");

		if (ijmpdown) {
				barnMove.JumpStart();
		} else if (ijmpstate) {
				barnMove.Jump();
		}

		var iattackdown = Input.GetButtonDown ("joy1x");
		if (iattackdown) {
			Debug.Log ("hi");
			barnAttack.Attack();
		}
	}

	// Update is called once per frame
	void Update()
	{
		HandleInput();
	}

	int AxisRound(float val) 
	{
		if (Mathf.Abs(val) < 0.3) {
			return 0; 
		}
		return (int)Mathf.Sign(val);
	}
	private void FlipSprite(){
		transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
	}
}
