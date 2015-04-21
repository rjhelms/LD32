using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleScreenController : MonoBehaviour
{

	public Text PressAnyKeyText;
	public float WaitTime = 1f;
	public GameObject MusicSource;

	private float readyToStartTime;
	private bool ready;
	// Use this for initialization
	void Start ()
	{
		// destroy the game object, if there's one here.
		GameObject controller = GameObject.Find ("GameController");
		if (controller != null) {
			Destroy (controller);
		}

		DontDestroyOnLoad (MusicSource);
		readyToStartTime = Time.time + WaitTime;
		ready = false;
		PressAnyKeyText.enabled = false;

		if (!MusicSource.GetComponent<AudioSource> ().isPlaying) {
			MusicSource.GetComponent<AudioSource> ().Play ();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Time.time > readyToStartTime) {
			ready = true;
			PressAnyKeyText.enabled = true;
		}
		if (ready & Input.anyKey) {
			Application.LoadLevel ("Map1");
		}

	}
}
