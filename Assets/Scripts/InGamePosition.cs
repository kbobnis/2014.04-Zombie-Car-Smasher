using UnityEngine;

public class InGamePosition : MonoBehaviour {


	public float x, y;

	public static float tileW, tileH;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		UpdatePosition();
	}

	private void UpdatePosition(){
		transform.position = new Vector3(x*tileW, y*tileH, 0);
	}

}


