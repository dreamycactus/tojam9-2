using UnityEngine;
using System.Collections;

public class BearController : MonoBehaviour {

	private Animator animator;
	private BarnMove barnMove;
	// Use this for initialization
	void Start()
	{
		animator = this.GetComponent<Animator>();
		barnMove = GetComponent<BarnMove> ();
	}

	void HandleInput()
	{
		var idx = Input.GetAxis ("Horizontal");
		var idy = Input.GetAxis("Vertical");

		if (transform.localScale.x > 0) {
			FlipSprite();
		}
		
		barnMove.Move(AxisRound(idx) );
			
//		else
//		{
//			
//		}
	}

	// Update is called once per frame
	void Update()
	{
		HandleInput();

	}

	int AxisRound(float val) 
	{
		Debug.Log (Mathf.Sign (val));
		if (Mathf.Abs(val) < 0.3) {
			return 0; 
		}

		return Mathf.Sign (val);
	}
	private void FlipSprite(){
		transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
	}
}
