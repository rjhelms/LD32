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
	public Image WeaponSelectorImage;
	public Image HealthBarImage;
	public Image TitleImage;
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

	public int CivilianCount;
	public int PowerupCount;
	public int OriginalPowerupCount;

	private float nextCount;

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

	void Start ()
	{
		Initialize ();
	}

	void OnLevelWasLoaded (int level)
	{
		Initialize ();
	}

	void Update ()
	{
		if (Starting) {
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
			PlayerTransform.GetComponent<PlayerController> ().CentreCamera ();
		}

		if (Running) {
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

				AmmoText [i].text = (Ammo [i]).ToString ();
			}
		
			CivilianText.text = CivilianCount.ToString ();
			ScoreText.text = Score.ToString ();
			HealthBarImage.rectTransform.sizeDelta = new Vector2 (HitPoints * 2, 8);
		}
	}

	void Initialize ()
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
			WeaponSelectorImage = GameObject.Find ("WeaponSelector").GetComponent<Image> ();
			HealthBarImage = GameObject.Find ("HealthBar").GetComponent<Image> ();
			TitleImage = GameObject.Find ("TitleImage").GetComponent<Image> ();
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
			CountdownText.text = Countdown.ToString ();
			CivilianText.text = CivilianCount.ToString ();

			nextCount = Time.unscaledTime + CountdownSpeed;

			Starting = true;
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
	
}