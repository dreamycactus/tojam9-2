using UnityEngine;
using System.Collections;


public class EnvPillar : MonoBehaviour {
	enum PillarState { Rising, Falling, WaitingDown, WaitingUp };
	private Vector2 origPos;
	private PillarState state;
	public float risespeed = 0.2f;
	public float fallspeed = 0.7f;
	float[] pillarIntervals = new float[2]{1.0f,2.0f};
	public Timer timer = new Timer();
	private BoxCollider2D box;

	// Use this for initialization
	void Start () {
		state = PillarState.WaitingUp;
		origPos = new Vector2 (transform.position.x, gameObject.transform.position.y);
		box = GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		switch(state) {
		case PillarState.Rising:
			if ( box.center.y < origPos.y ) {
				box.center = box.center + new Vector2(0.0f, risespeed);
			} else {
				state = PillarState.WaitingUp;
			}
			break;
		case PillarState.Falling:
			box.center = box.center + new Vector2(0.0f, fallspeed);
			break;
		case PillarState.WaitingUp:
			if (!timer.IsRunning() ) {
				timer.Start();
			}
			if (timer.GetElapsedTimeSecs() > pillarIntervals[0] ) {
				state = PillarState.Falling;
			}
			break;
		case PillarState.WaitingDown:
			if (!timer.IsRunning() ) {
				timer.Start();
			}
			if (timer.GetElapsedTimeSecs() > pillarIntervals[1] ) {
				state = PillarState.Rising;
			}
			break;
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player") {
			var normal = other.contacts [0].normal;
			// get its elevation angle in degrees:
			var angle = Mathf.Rad2Deg * Mathf.Asin (normal.y);
			if (angle > -1 && angle < 1) {
					other.gameObject.GetComponent<BearController> ().Die ();
			}
		} else if (other.gameObject.tag == "Platform") {
			state = PillarState.WaitingDown;
		}
	}
}
