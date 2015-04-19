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

	public int Score;
	public int Lives;
	public int[] Ammo = {100, -1, -1};
	public Text[] AmmoText;
	public Text ScoreText;
	public Image WeaponSelectorImage;

	public Vector3[] WeaponSelectorPositions;

	public Transform CommieContainer;
	public Transform CivilianContainer;
	public Transform CapitalistContainer;
	public Transform ProjectileContainer;
	public Transform BureaucratContainer;

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

		for (int i = 0; i < AmmoText.Length; i++) {
			if (Ammo [i] > 99)
				Ammo [i] = 99;

			if (Ammo [i] < 0)
				Ammo [i] = 0;

			AmmoText [i].text = (Ammo [i]).ToString ();
		}
		
		ScoreText.text = Score.ToString ();
	}

	void Initialize ()
	{
		AmmoText = new Text[3];
		AmmoText [0] = GameObject.Find ("LeafletValue").GetComponent<Text> ();
		AmmoText [1] = GameObject.Find ("MoneyValue").GetComponent<Text> ();
		AmmoText [2] = GameObject.Find ("MegaphoneValue").GetComponent<Text> ();
		ScoreText = GameObject.Find ("ScoreValue").GetComponent<Text> ();
		WeaponSelectorImage = GameObject.Find ("WeaponSelector").GetComponent<Image> ();

		WeaponSelectorPositions = new Vector3[3];
		WeaponSelectorPositions [0] = WeaponSelectorImage.rectTransform.localPosition;
		WeaponSelectorPositions [1] = WeaponSelectorPositions [0] + new Vector3 (36, 0, 0);
		WeaponSelectorPositions [2] = WeaponSelectorPositions [1] + new Vector3 (36, 0, 0);

		CommieContainer = GameObject.Find ("Commies").transform;
		CivilianContainer = GameObject.Find ("Civilians").transform;
		CapitalistContainer = GameObject.Find ("Capitalists").transform;
		ProjectileContainer = GameObject.Find ("Projectiles").transform;
		BureaucratContainer = GameObject.Find ("Bureaucrats").transform;
	}
}