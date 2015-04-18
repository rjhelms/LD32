using UnityEngine;
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
}
