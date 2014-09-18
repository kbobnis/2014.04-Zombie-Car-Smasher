using UnityEngine;

public class InGamePosition : MonoBehaviour {

	public float x, y, z;



	public static float tileW, tileH;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position = UpdatePosition();
	}

	public Vector3 UpdatePosition(){
		return new Vector3(x*tileW, y*tileH, z);
	}



}


