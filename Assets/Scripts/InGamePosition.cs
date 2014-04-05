using UnityEngine;

public class InGamePosition : MonoBehaviour {


	public float x, y;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		UpdatePosition();
	}

	private void UpdatePosition(){
		transform.position = new Vector3(x, y, 0);
	}

}


