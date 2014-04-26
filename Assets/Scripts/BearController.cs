﻿using UnityEngine;
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
		var idx = Input.GetAxis ("Horizontal");
		var idy = Input.GetAxis("Vertical");

		if (transform.localScale.x > 0) {
			FlipSprite();
		}
		
		barnMove.Move(AxisRound(idx) );

		animator.Animate("Walk");
			
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
		if (Mathf.Abs(val) < 0.3) {
			return 0; 
		}

		return (int)Mathf.Sign (val);
	}

	private void FlipSprite(){
		transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
	}
}
