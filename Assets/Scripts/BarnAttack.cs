using UnityEngine;
using System.Collections;

public enum AttackState { Start, Middle, End, None }
public class BarnAttack : MonoBehaviour 
{
	[HideInInspector]
	public AttackState state = AttackState.None;

	private BearController controller;
	public BoxCollider2D attackbox;
	public Vector2 spawnoffset;

	public float[] attackInterval;
	public float attackTotal = 2.0f;
	private Timer attackTimer = new Timer();
	
	Rigidbody2D rbody;
	// Use this for initialization
	void Start () {
		var parent = transform.parent.gameObject;
		rbody = parent.rigidbody2D;
		rbody.fixedAngle = true;
		controller = parent.GetComponent<BearController> ();
//		attackInterval = new float[]{0.5f, 1.5f};
		attackbox.enabled = false;
		gameObject.tag = "Attack";
	}

	public void Attack() 
	{
		var cstate = controller.state;
		if (cstate == CharState.Idle || cstate == CharState.Moving) {
			state = AttackState.Start;
			attackTimer.Start ();
			StartAttack();
		}
	}

	void StartAttack() {
		controller.isAttacking = true;
	}
	void Update() {
		var time = attackTimer.GetElapsedTimeSecs ();
		if (time < attackTotal) {
			if (time < attackInterval [0]) {

			} else if (time < attackInterval [1]) {
//					if (attackbox.collider.bounds.Intersects
				attackbox.enabled = true;
				state = AttackState.Middle;
			} else if (time < attackTotal) {
				state = AttackState.End;
			} else {
				EndAttack();
			}
		} else {
			EndAttack();
		}
	}

	public void EndAttack()
	{
		controller.isAttacking = false;
		attackbox.enabled = false;
		attackTimer.Stop ();
	}

//	void OnGUI() {
//		Render_Colored_Rectangle (0, 0, 100, 100, 128, 0, 0);
////			GUI.Box(new Rect(0, 0, 100, 100), "This is a title");
//	}

                       
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Attack") {
			Debug.Log ("Attack hit");
		}
		Debug.Log ("hit" + other);
		if (other.gameObject.tag == "Player" && other.gameObject != transform.parent) {
			other.gameObject.GetComponent<BearController>().isAlive = false;
			Debug.Log("Kill");
		}
	}
}
