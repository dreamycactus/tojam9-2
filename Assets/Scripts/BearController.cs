using UnityEngine;
using System.Collections;

public class BearController : MonoBehaviour {
	
	private BarnAnimation animator;

	private BarnMove barnMove;
	// Use this for initialization
	void Start()
	{
		animator = GetComponent<BarnAnimation>();
		barnMove = GetComponent<BarnMove> ();
	}

	void HandleInput()
	{
		Input.GetJoystickNames ();
		var idx = Input.GetAxis ("Hor1");
		var idy = Input.GetAxis("Ver1");
		
		barnMove.Move(AxisRound(idx));
		if (idy < -0.5f) {
			barnMove.Jump ();
		}
	}

	// Update is called once per frame
	void Update()
	{
		HandleInput();
		
		HandleAnimation();
	}

	int AxisRound(float val) 
	{
		if (Mathf.Abs(val) < 0.3) {
			return 0; 
		}
		return (int)Mathf.Sign(val);
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

	private void HandleAnimation(){
		var idx = Input.GetAxis ("Horizontal");
		var idy = Input.GetAxis("Vertical");
		
		if (idx > 0.1){
			FaceLeft();
			animator.Animate("Walk");
		}
		else if (idx < -0.1){
			FaceRight();
			animator.Animate("Walk");
		}
		else {
			animator.Animate("Idle");
		}
	}
}
