using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{

	private static GameController _instance;

	public static GameController Instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<GameController> ();
				DontDestroyOnLoad (_instance.gameObject);
			}
			return _instance;
		}
	}

	public bool Running;
	public bool Starting;
	public bool Winning;

	public int Score;
	public int HitPoints;
	public int Lives;
	public int[] Ammo = {100, -1, -1};
	public Text[] AmmoText;
	public Text ScoreText;
	public Text LevelTitleText;
	public Text LevelDescriptionText;
	public Text CountdownText;
	public Text CivilianText;
	public Text ClearText;
	public Text TimeText;
	public Text PowerupText;
	public Text PlayerConvertedText;
	public Text EnemyConvertedText;
	public Text AnyKeyText;

	public Image WeaponSelectorImage;
	public Image HealthBarImage;
	public Image TitleImage;
	public Image LevelEndImage;

	public GameObject CivilianPrefab;
	public GameObject CommiePrefab;
	public GameObject CapitalistPrefab;
	public GameObject BureaucratPrefab;

	public Vector3[] WeaponSelectorPositions;

	public Transform CommieContainer;
	public Transform CivilianContainer;
	public Transform CapitalistContainer;
	public Transform ProjectileContainer;
	public Transform BureaucratContainer;
	public Transform PlayerTransform;

	public AudioClip[] PlayerWeaponSounds;

	public AudioClip PowerUpSound;
	public AudioClip PlayerHitSound;
	public AudioClip CommieSound;
	public AudioClip CivilianSound;
	public AudioClip EnragedSound;
	public AudioClip Blip;

	public string LevelTitle;
	public string LevelDescription;

	public AudioSource SFXSource;

	public int Countdown = 3;
	public float CountdownSpeed = 1f;
	public float WinTickSpeed = 0.5f;

	public int CivilianCount;
	public int PowerupCount;
	public int OriginalPowerupCount;

	public int PlayerConversions;
	public int EnemyConversions;

	public int CurrentLevel;

	private float nextCount;
	private float levelStartTime;
	private float levelEndTime;
	private int winState = 0;

	void Awake ()
	{
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad (this);
		} else {
			if (this != _instance)
				Destroy (this.gameObject);
		}
	}

	void StartingUpdate ()
	{
		if (Time.unscaledTime > nextCount) {
			Countdown--;
			nextCount = Time.unscaledTime + CountdownSpeed;
			CountdownText.text = Countdown.ToString ();
			SFXSource.PlayOneShot (Blip);
		}
		if (Countdown < 0) {
			Starting = false;
			TitleImage.enabled = false;
			LevelTitleText.enabled = false;
			LevelDescriptionText.enabled = false;
			CountdownText.enabled = false;
			Resume ();
		}
		if (PlayerTransform.GetComponent<PlayerController> ().MyCamera != null)
			PlayerTransform.GetComponent<PlayerController> ().CentreCamera ();
	}

	void RunningUpdate ()
	{
		if (HitPoints > 32) {
			HitPoints = 32;
		}
		if (HitPoints <= 0) {
			Debug.Log ("You died");
			Pause ();
		}
		for (int i = 0; i < AmmoText.Length; i++) {
			if (Ammo [i] > 99)
				Ammo [i] = 99;
			if (Ammo [i] < 0)
				Ammo [i] = 0;
		}
		UpdateUI ();
		if (CivilianCount <= 0) {
			Win ();
		}
	}

	void WinningUpdate ()
	{
		if (Time.unscaledTime > nextCount && winState < 5) {
			winState++;
			nextCount = Time.unscaledTime + WinTickSpeed;
			SFXSource.PlayOneShot (Blip);
			switch (winState) {
			case 1:
				TimeText.enabled = true;
				break;
			case 2:
				PowerupText.enabled = true;
				break;
			case 3:
				PlayerConvertedText.enabled = true;
				break;
			case 4:
				EnemyConvertedText.enabled = true;
				break;
			case 5:
				AnyKeyText.enabled = true;
				break;
			}
		} else
			if (winState == 5) {
			if (Input.anyKey) {
				CurrentLevel++;
				Application.LoadLevel ("Map1");
			}
		}
	}

	void Update ()
	{
		if (Starting) {
			StartingUpdate ();
		} else if (Running) {
			RunningUpdate ();
		} else if (Winning) {
			WinningUpdate ();
		}
	}
	
	void UpdateUI ()
	{
		for (int i = 0; i < Ammo.Length; i++) {
			AmmoText [i].text = Ammo [i].ToString ();
		}
		CivilianText.text = CivilianCount.ToString ();
		ScoreText.text = Score.ToString ();
		HealthBarImage.rectTransform.sizeDelta = new Vector2 (HitPoints * 2, 8);
	}

	public void Initialize ()
	{
		if (Application.loadedLevelName.Contains ("Map")) {
			AmmoText = new Text[3];
			AmmoText [0] = GameObject.Find ("LeafletValue").GetComponent<Text> ();
			AmmoText [1] = GameObject.Find ("MoneyValue").GetComponent<Text> ();
			AmmoText [2] = GameObject.Find ("MegaphoneValue").GetComponent<Text> ();
			ScoreText = GameObject.Find ("ScoreValue").GetComponent<Text> ();
			LevelTitleText = GameObject.Find ("LevelTitleValue").GetComponent<Text> ();
			LevelDescriptionText = GameObject.Find ("LevelDescriptionValue").GetComponent<Text> ();
			CountdownText = GameObject.Find ("CountdownValue").GetComponent<Text> ();
			CivilianText = GameObject.Find ("RemainingCivilianValue").GetComponent<Text> ();
			ClearText = GameObject.Find ("ClearTitle").GetComponent<Text> ();
			TimeText = GameObject.Find ("TimeValue").GetComponent<Text> ();
			PowerupText = GameObject.Find ("PowerupValue").GetComponent<Text> ();
			PlayerConvertedText = GameObject.Find ("PlayerConvertedValue").GetComponent<Text> ();
			EnemyConvertedText = GameObject.Find ("EnemyConvertedValue").GetComponent<Text> ();
			AnyKeyText = GameObject.Find ("PressAnyKey").GetComponent<Text> ();

			WeaponSelectorImage = GameObject.Find ("WeaponSelector").GetComponent<Image> ();
			HealthBarImage = GameObject.Find ("HealthBar").GetComponent<Image> ();
			TitleImage = GameObject.Find ("TitleImage").GetComponent<Image> ();
			LevelEndImage = GameObject.Find ("LevelEndImage").GetComponent<Image> ();

			WeaponSelectorPositions = new Vector3[3];
			WeaponSelectorPositions [0] = WeaponSelectorImage.rectTransform.localPosition;
			WeaponSelectorPositions [1] = WeaponSelectorPositions [0] + new Vector3 (36, 0, 0);
			WeaponSelectorPositions [2] = WeaponSelectorPositions [1] + new Vector3 (36, 0, 0);
			
			CommieContainer = GameObject.Find ("Commies").transform;
			CivilianContainer = GameObject.Find ("Civilians").transform;
			CapitalistContainer = GameObject.Find ("Capitalists").transform;
			ProjectileContainer = GameObject.Find ("Projectiles").transform;
			BureaucratContainer = GameObject.Find ("Bureaucrats").transform;
			PlayerTransform = GameObject.Find ("Player(Clone)").transform;
			
			SFXSource = GameObject.Find ("SoundFX").GetComponent<AudioSource> ();
			
			LevelTitleText.text = LevelTitle;
			LevelDescriptionText.text = LevelDescription;

			Countdown = 3;
			PowerupCount = 0;
			PlayerConversions = 0;
			EnemyConversions = 0;

			CountdownText.text = Countdown.ToString ();
			
			UpdateUI ();
			
			nextCount = Time.unscaledTime + CountdownSpeed;
			levelStartTime = Time.time;

			LevelEndImage.enabled = false;
			ClearText.enabled = false;
			TimeText.enabled = false;
			PowerupText.enabled = false;
			PlayerConvertedText.enabled = false;
			EnemyConvertedText.enabled = false;
			AnyKeyText.enabled = false;

			Starting = true;
			Winning = false;
			Pause ();
			
		}
	}

	public void Pause ()
	{
		Running = false;
		Time.timeScale = 0;
	}
	public void Resume ()
	{
		Running = true;
		Time.timeScale = 1;
	}

	public void BureaucratHit ()
	{
		SFXSource.PlayOneShot (PlayerHitSound);
		Score -= 100;
		Ammo [0] -= 10;
		Ammo [1] -= 10;
		Ammo [2] -= 10;
		HitPoints -= 5;
	}

	public void Win ()
	{
		Winning = true;
		winState = 0;
		levelEndTime = Time.time;
		int levelDuration = Mathf.RoundToInt (levelEndTime - levelStartTime);
		int winTimeMinutes = Mathf.FloorToInt (levelDuration / 60);
		int winTimeSeconds = levelDuration % 60;
		TimeText.text = "Time: " + winTimeMinutes + ":" + winTimeSeconds.ToString ("D2");
		PowerupText.text = "Powerups Collected: " + PowerupCount + " / " + OriginalPowerupCount;
		PlayerConvertedText.text = "Player Conversions: " + PlayerConversions;
		EnemyConvertedText.text = "Enemy Conversions: " + EnemyConversions;

		LevelEndImage.enabled = true;
		ClearText.enabled = true;

		nextCount = Time.unscaledTime + WinTickSpeed;
		SFXSource.PlayOneShot (Blip);

		Debug.Log ("level won, time: " + winTimeMinutes + ":" + winTimeSeconds.ToString ("D2"));
		Pause ();
	}
	
}