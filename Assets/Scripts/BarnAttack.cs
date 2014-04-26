using UnityEngine;
using System.Collections;

public class BarnAttack : MonoBehaviour 
{
	private BearController controller;
	public BoxCollider2D attackbox;
	public Vector2 spawnoffset;
	
	Rigidbody2D rbody;
	// Use this for initialization
	void Start () {
		var parent = transform.parent.gameObject;
		rbody = parent.rigidbody2D;
		rbody.fixedAngle = true;
		controller = parent.GetComponent<BearController> ();

		attackbox.enabled = false;
	}

	public void Attack() 
	{
		var state = controller.state;

		if (state == CharState.Idle || state == CharState.Moving) {
			StartAttack();
		}
	}

	void StartAttack() {
		attackbox.enabled = true;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log (other);
	}
}