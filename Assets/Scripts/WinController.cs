using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinController : MonoBehaviour
{

	public Text WinText;
	public Text FlavourText;
	public Text ScoreText;
	public Text[] AmmoTexts;
	public SpriteRenderer[] AmmoSprites;
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
		AmmoTexts [0].enabled = false;
		AmmoTexts [1].enabled = false;
		AmmoTexts [2].enabled = false;
		AmmoSprites [0].enabled = false;
		AmmoSprites [1].enabled = false;
		AmmoSprites [2].enabled = false;
		RestartText.enabled = false;
		nextUpdate = Time.time + UpdateSpeed;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time > nextUpdate && updateState < 7) {
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
				AmmoTexts [0].enabled = true;
				AmmoTexts [1].enabled = true;
				AmmoTexts [2].enabled = true;
				AmmoSprites [0].enabled = true;
				AmmoSprites [1].enabled = true;
				AmmoSprites [2].enabled = true;
				break;
			case 4:
				MyController.Score += MyController.Ammo [0] * 100;
				MyController.Ammo [0] = 0;
				break;
			case 5:
				MyController.Score += MyController.Ammo [1] * 100;
				MyController.Ammo [1] = 0;
				break;
			case 6:
				MyController.Score += MyController.Ammo [2] * 100;
				MyController.Ammo [2] = 0;
				break;
			case 7:
				RestartText.enabled = true;
				break;
			}
		} else if (updateState == 7) {
			if (Input.anyKey) {
				Destroy (MyController.gameObject);
				Destroy (GameObject.Find ("MusicSource"));
				Application.LoadLevel (0);
			}
		}

		UpdateUI ();
	}

	void UpdateUI ()
	{
		ScoreText.text = "Final Score: " + MyController.Score.ToString ();
		AmmoTexts [0].text = MyController.Ammo [0].ToString ();
		AmmoTexts [1].text = MyController.Ammo [1].ToString ();
		AmmoTexts [2].text = MyController.Ammo [2].ToString ();
	}
}