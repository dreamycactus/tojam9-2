using UnityEngine;
using System.Collections;
public enum CharState { Idle, Moving, Jumping, Falling, WallGrab, WallSlide };
public enum InputMap { Axis1X, Axis1Y, ButX, ButY, ButA, ButB };
public class BearController : MonoBehaviour {

//	public enum CharState { Idle, Moving, Jumping, Falling, WallGrab, WallSlide };
	
	[HideInInspector]
	public CharState state {
		get;
		set;
	}

	[HideInInspector]
	public bool isAttacking;

	private BarnMove barnMove;
	private BarnAttack barnAttack;

	public string[] inputmap;

	[HideInInspector]
	public bool isAlive = true;
	// Use this for initialization
	void Start()
	{
		barnMove = GetComponent<BarnMove> ();
		state = CharState.Idle;
		barnAttack = GetComponentInChildren<BarnAttack> ();
		gameObject.tag = "Player";
	}

	void HandleInput()
	{
		if ( Input.GetButtonDown(inputmap[(int)InputMap.ButY]) && isAttacking &&
		    barnAttack.state == AttackState.End) {
			barnAttack.EndAttack();
			Debug.Log ("Cancel" + barnAttack.state);
		}
		if (!isAttacking) {
			Input.GetJoystickNames ();
			var idx = Input.GetAxis (inputmap[(int)InputMap.Axis1X]);
			var idy = Input.GetAxis (inputmap[(int)InputMap.Axis1Y]);

			barnMove.Move (AxisRound (idx));
			var ijmpdown = Input.GetButtonDown (inputmap[(int)InputMap.ButY]);
			var ijmpup = Input.GetButtonUp(inputmap[(int)InputMap.ButY]);
			var ijmpstate = Input.GetButton (inputmap[(int)InputMap.ButY]);

//			if (ijmpup && !barnMove.jumpEnd) {
//				barnMove.jumpEnd = true;
//			}
			if (ijmpdown) {
				barnMove.JumpStart ();
			} else if (ijmpstate) {
				barnMove.Jump ();
			}

			var iattackdown = Input.GetButtonDown (inputmap[(int)InputMap.ButX]);
			if (iattackdown && !isAttacking) {
				barnAttack.Attack ();
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (isAlive) {
			HandleInput ();
		}
	}

	int AxisRound(float val) 
	{
		if (Mathf.Abs(val) < 0.3) {
			return 0; 
		}
		return (int)Mathf.Sign(val);
	}

}
