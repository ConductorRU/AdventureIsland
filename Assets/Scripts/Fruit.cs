using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
  // Start is called before the first frame update
	public AudioClip sound;
	public int score = 100;
	private ShadowText _text;
	private SpriteRenderer _sprite;
	private bool _picked = false;
	private bool _hided = true;
	private float _pickedTime = 0.5f;
  void Start()
  {
		_text  = gameObject.GetComponentInChildren<ShadowText>(true);
		_text.text = score.ToString();
		_sprite = GetComponent<SpriteRenderer>();
		_sprite.enabled = false;
  }

  // Update is called once per frame
  void Update()
  {
		if(_hided && Game.game.GetPlayer())
		{
			if(Game.game.GetPlayer().GetDistanceX(transform.position) <= 10.0f)
			{
				_sprite.enabled = true;
				_hided = false;
			}
		}
		if(_picked)
			_pickedTime -= Time.deltaTime;
		if(_picked && _pickedTime <= 0.0f)
			Destroy(gameObject);
		if(_picked && _text)
		{
			Vector3 pos = _text.gameObject.transform.position;
			pos.y += Time.deltaTime*12.0f;
			_text.gameObject.transform.position = pos;
		}
  }
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Player" && !_picked)
		{
			Player pl = col.gameObject.GetComponent<PlayerTrigger>().player;
			_sprite.enabled = false;
			if(sound)
			{
				pl.GetAudio().clip = sound;
				pl.GetAudio().Play();
			}
			_picked = true;
			if(_text)
				_text.gameObject.SetActive(true);
		}

	}
}
