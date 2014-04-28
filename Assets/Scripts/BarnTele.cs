using UnityEngine;
using System.Collections;

public enum TeleState { Tele, None }
public class BarnTele : MonoBehaviour 
{
	Rigidbody2D rbody;
	public float teleTime = 0.005f;
	public float teleamt = 20.0f;
	Timer timer = new Timer();
	Vector2 inputvec;
	[HideInInspector]
	public TeleState state;
	void Start () 
	{
		rbody = rigidbody2D;
		rbody.fixedAngle = true;
		state = TeleState.None;
	}

	void Update() 
	{
		if (state == TeleState.Tele) {
			if (timer.GetElapsedTimeSecs () < teleTime) {
				rbody.AddForce (inputvec*teleamt);					
			} else if (timer.GetElapsedTimeSecs () > teleTime * 3.0 ){
				//GetComponent<SpriteRenderer> ().color = new Color (256, 256, 256, 256);
				state = TeleState.None;
			}
		}
	}

	public void Teleport(Vector2 vec) {
		if (state == TeleState.None) {
			timer.Start ();
			state = TeleState.Tele;
			GetComponent<BarnMove>().audio.PlayOneShot(GetComponent<BarnMove>().sfxtele);
		}
		inputvec = vec;
		inputvec.y *= 1.5f;
		//GetComponent<SpriteRenderer> ().color = new Color (256, 256, 256, 0);
	}
}