using UnityEngine;
using System.Collections;

public class BearController : MonoBehaviour {

	private BarnMove barnMove;
	// Use this for initialization
	void Start()
	{
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
	}

	int AxisRound(float val) 
	{
		if (Mathf.Abs(val) < 0.3) {
			return 0; 
		}
		return (int)Mathf.Sign(val);
	}

}
