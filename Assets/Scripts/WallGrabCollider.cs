using UnityEngine;
using System.Collections;

public class WallGrabCollider : MonoBehaviour {

	public bool onWall = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col)
	{

		if (col.transform.tag != "Player"){
			onWall = true;
		}
		
	}
	
	void OnTriggerExit2D(Collider2D col)
	{
		
		if (onWall && col.transform.tag != "Player"){
			onWall = false;
		}
		
	}
}
