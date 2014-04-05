using UnityEngine;


public class Speeder : MonoBehaviour 
{
	public float v;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tmp = transform.localPosition;
		tmp.y = tmp.y +  v * Time.deltaTime;
		transform.localPosition = tmp;
	}

}


