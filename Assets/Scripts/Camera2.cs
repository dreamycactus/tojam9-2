using UnityEngine;
using System.Collections;

public class Camera2 : MonoBehaviour
{
	private ArrayList players;
	private Vector3 desiredPos;
	public float border = 0.1f;
		// Use this for initialization
		void Start ()
		{
			players = new ArrayList();
			var ps = GameObject.FindGameObjectsWithTag ("Player");
			players.AddRange (ps);
		}
	
		// Update is called once per frame
		void Update ()
		{
			float minx = Screen.width;
			float miny = Screen.height;
			float maxx = 0.0f;
			float maxy = 0.0f;
				
			foreach (GameObject go in players) {
				if (go.transform.position.x < minx) {
					minx = go.transform.position.x;
				}
				if ( go.transform.position.y < miny) {
					miny = go.transform.position.y;
				}
				if (go.transform.position.x > maxx) {
					maxx = go.transform.position.x;
				}
				if ( go.transform.position.y > maxy) {
					maxy = go.transform.position.y;
				}
			}

			minx -= border; miny -= border;
			maxx += border; maxy += border;
			float zoom = Mathf.Max (maxx - minx, maxy - miny);
			Camera.main.transform.position = new Vector3 ((maxx + minx) / 2.0f, (maxy + miny) / 2.0f, -10.0f);
			Debug.Log (Camera.main.transform.position);
			Camera.main.orthographicSize = Mathf.Lerp (Camera.main.orthographicSize, zoom/1.5f, Time.deltaTime * 5.0f);
			
		}
}

