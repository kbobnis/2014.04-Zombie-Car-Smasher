using UnityEngine;
using System.Collections;

public class PlaySingleSound : MonoBehaviour
{
	float sound_start = 0f;
	public AudioClip clip;
	bool sound_started = false;

	// Use this for initialization
	void Start ()
	{
		sound_started = false;
		gameObject.AddComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if( !sound_started ) {
			sound_started = true;
			sound_start = Time.realtimeSinceStartup;
//			audio.PlayOneShot( clip );
			audio.clip = clip;
			audio.rolloffMode = AudioRolloffMode.Linear;
			audio.Play();
		} else if( Time.realtimeSinceStartup - sound_start > clip.length ) {
			GameObject.Destroy( gameObject );
		}
	}

	public static void SpawnSound( AudioClip clip, Vector3 position )
	{
		GameObject go = new GameObject( "sound clip: " + clip.name );
		go.transform.position = position;
		PlaySingleSound play_sound = go.AddComponent<PlaySingleSound>();
		play_sound.clip = clip;
	}
}
