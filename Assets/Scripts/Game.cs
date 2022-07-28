using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum STATE
{
	LEVEL      = 0,
	CRY_START  = 1,
	CRY_MIDDLE = 2,
	CRY_END    = 3,
	RESTART    = 4,

}

public class Game : MonoBehaviour
{
	public static Game game = null;
	private Player _player;
	private STATE _state = STATE.LEVEL;
	private AudioSource _audioSource;
	public AudioClip musicStartCry;
	public AudioClip musicEndCry;
	public GameObject playerPrefab;
	public Player GetPlayer() { return _player; }
	public AudioSource GetAudio() { return _audioSource; }
	void Awake()
	{
		_audioSource = gameObject.GetComponent<AudioSource>();
		if (game == null)
			game = this;
		else if(game != this)
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
	}
	void Initialize()
  {
		if(!_player)
		{
			GameObject instance = Instantiate(playerPrefab); 
			_player = instance.GetComponent<Player>();
			DontDestroyOnLoad(instance);
		}
		_player.Reset();
		GameObject respawn = GameObject.FindGameObjectWithTag("Respawn");
		_player.transform.position = respawn.transform.position;
		Camera.main.GetComponent<CameraController>().target = _player.transform;
		GameObject level = GameObject.FindGameObjectWithTag("Level");
		_audioSource.clip = level.GetComponent<Level>().music;
		_audioSource.loop = true;
		_audioSource.Play();
  }
  void Start()
  {
		//Initialize();
  }

  // Update is called once per frame
  void Update()
  {
		if(_state == STATE.CRY_START && !_audioSource.isPlaying)
		{
			_state = STATE.CRY_MIDDLE;
			Invoke("PlayCry", 0.3f);
		}
		if(_state == STATE.CRY_END && !_audioSource.isPlaying)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			_state = STATE.LEVEL;
		}
  }
	void OnEnable()
  {
    Debug.Log("OnEnable called");
    SceneManager.sceneLoaded += OnSceneLoaded;
  }
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
		Initialize();
		GameObject[] objects = GameObject.FindGameObjectsWithTag("EditorOnly");
		foreach(GameObject obj in objects)
			obj.SetActive(false);
  }
	void OnDisable()
  {
    Debug.Log("OnDisable");
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }
	void PlayCry()
	{
		_state = STATE.CRY_END;
		_audioSource.clip = musicEndCry;
		_audioSource.loop = false;
		_audioSource.Play();
	}
	public void OnDeath()
  {
		_state = STATE.CRY_START;
     _audioSource.clip = musicStartCry;
		_audioSource.loop = false;
		_audioSource.Play();
  }
}
