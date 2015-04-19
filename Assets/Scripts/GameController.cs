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
		InitializeText ();
	}

	void OnLevelWasLoaded (int level)
	{
		InitializeText ();
	}

	void Update ()
	{
		for (int i = 0; i < AmmoText.Length; i++) {
			AmmoText [i].text = (Ammo [i]).ToString ();
		}
		
		ScoreText.text = Score.ToString ();
	}

	void InitializeText ()
	{
		AmmoText = new Text[2];
		AmmoText [0] = GameObject.Find ("LeafletValue").GetComponent<Text> ();
		AmmoText [1] = GameObject.Find ("MoneyValue").GetComponent<Text> ();
		ScoreText = GameObject.Find ("ScoreValue").GetComponent<Text> ();
	}
}