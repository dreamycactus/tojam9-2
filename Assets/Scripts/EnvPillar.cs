using UnityEngine;
using System.Collections;


public class EnvPillar : MonoBehaviour {
	enum PillarState { Rising, Falling, WaitingDown, WaitingUp };
	private Vector2 origPos;
	private PillarState state;
	public float risespeed = 1000.0f;
	public float fallspeed = 0.01f;
	public float[] pillarIntervals = new float[2]{5.0f,5.0f};
	public Timer timer = new Timer();
	private BoxCollider2D box;
	private Rigidbody2D rbody;
	private GameObject[] players;

	// Use this for initialization
	void Start () {
		state = PillarState.WaitingUp;
		origPos = new Vector2 (transform.position.x, gameObject.transform.position.y);
		box = GetComponent<BoxCollider2D> ();
		rbody = GetComponent<Rigidbody2D> ();
		players = GameObject.FindGameObjectsWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (timer.GetElapsedTimeSecs ());
		Debug.Log (state);
		transform.position = new Vector3 (origPos.x, transform.position.y, 0.0f);
		switch(state) {
		case PillarState.Rising:
			ShakePlayersOff();
			if ( transform.position.y < origPos.y ) {
				rbody.AddForce(new Vector2(0.0f,risespeed));
			} else {
				state = PillarState.WaitingUp;
				timer.Start ();
			}
			break;
		case PillarState.Falling:
			ShakePlayersOff();
			rbody.AddForce(new Vector2(0.0f,-fallspeed));
			break;
		case PillarState.WaitingUp:
			if (!timer.IsRunning() ) {
				timer.Start();
			}
			rbody.isKinematic=true;
			if (timer.GetElapsedTimeSecs() > pillarIntervals[0] ) {
				state = PillarState.Falling;
				rbody.isKinematic=false;
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

	void ShakePlayersOff() {
		foreach (GameObject go in players) {
			var bm = go.GetComponent<BarnMove>();
			if (bm.grabbedWall == gameObject) {
				bm.LetGoWall();
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player") {
			var normal = other.contacts [0].normal;
			// get its elevation angle in degrees:
			var angle = Mathf.Rad2Deg * Mathf.Asin (normal.y);
			if (angle > 89 && angle < 91) {
					other.gameObject.GetComponent<BearController> ().Die ();
					other.gameObject.GetComponent<BearController> ().Respawn (new Vector2(0,0));
			}
		} else if (other.gameObject.tag == "Platform" && state == PillarState.Falling) {
			state = PillarState.WaitingDown;
			timer.Stop ();
			timer.Start ();
		}
	}
}
