using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoseController : MonoBehaviour
{
	
	public Text LoseText;
	public Text FlavourText;
	public Text ScoreText;
	public Text RestartText;
	
	public GameController MyController;
	
	public AudioClip Blip;
	
	public float UpdateSpeed = .5f;
	
	private int updateState = 0;
	private float nextUpdate;
	
	// Use this for initialization
	void Start ()
	{
		Time.timeScale = 1;
		MyController = GameObject.Find ("GameController").GetComponent<GameController> ();
		FlavourText.enabled = false;
		ScoreText.enabled = false;
		RestartText.enabled = false;
		nextUpdate = Time.time + UpdateSpeed;
		ScoreText.text = "Final Score: " + MyController.Score.ToString ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time > nextUpdate && updateState < 3) {
			nextUpdate = Time.time + UpdateSpeed;
			updateState++;
			MyController.SFXSource.PlayOneShot (Blip);
			switch (updateState) {
			case 1:
				FlavourText.enabled = true;
				break;
			case 2:
				ScoreText.enabled = true;
				break;
			case 3:
				RestartText.enabled = true;
				break;
			}
		} else if (updateState == 3) {
			if (Input.anyKey) {
				Destroy (MyController.gameObject);
				Destroy (GameObject.Find ("MusicSource"));
				Application.LoadLevel (0);
			}
		}
	}

}