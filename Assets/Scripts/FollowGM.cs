using UnityEngine;
using System.Collections;

public class FollowGM : MonoBehaviour {

	public GameObject FollowWhom;
	public Vector3 Offset = new Vector3();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (FollowWhom != null){
			float y = FollowWhom.GetComponent<InGamePosition>().y;
			GetComponent<InGamePosition>().y = y + Offset.y;
		}
	}
}
