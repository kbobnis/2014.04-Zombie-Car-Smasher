using UnityEngine;


public class Speeder : MonoBehaviour 
{
	public float v;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<InGamePosition>().y += v * Time.deltaTime;
	}

}


