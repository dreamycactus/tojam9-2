using UnityEngine;
using System.Collections;

public class BarnAnimation : MonoBehaviour {

	//character state used for tracking animations
	enum CharacterState {
		Idle = 0,
		Walk = 1,
		Jump = 2,
		Attack = 3
	}

	private CharacterState characterState = CharacterState.Idle;

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		animator.SetInteger("State", (int)characterState);
	}

	public void Animate(string state) {

		CharacterState parsed_state = (CharacterState) System.Enum.Parse( typeof( CharacterState ), state );

		characterState = parsed_state;

	}

	public int GetAnimState(){
		return (int)characterState;
	}

	public void SetIsRunning(bool toggle){
		animator.SetBool("Running", toggle);
	}

	public void DoubleJump(){
		animator.SetTrigger("DoubleJump");
	}

}
