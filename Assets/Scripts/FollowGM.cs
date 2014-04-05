using UnityEngine;
using System.Collections;

public class FollowGM : MonoBehaviour {

	public GameObject FollowWhom;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (FollowWhom != null){
			Vector3 tmp = FollowWhom.transform.position;
			Vector3 tmp2 = transform.position;

			float gmHeight = FollowWhom.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
			transform.position = new Vector3(tmp2.x, tmp.y + gmHeight/2, tmp2.z);
		}
	}
}
