using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PlayerBase
{
  // Start is called before the first frame update
	public int hp = 1;
	public float speed = 0.125f;
	public AudioClip _deathSound;

	private float _deathUp = -2.0f;
	private Vector3 _deathStart;
	protected AudioSource _audio;
	protected Rigidbody2D _rigid;
	protected SpriteRenderer _sprite;
  protected void Start()
  {
		_audio = gameObject.AddComponent<AudioSource>();
		_rigid = GetComponent<Rigidbody2D>();
		_sprite = GetComponent<SpriteRenderer>();
		_audio.loop = false;
  }

  // Update is called once per frame
  protected void Update()
  {
		if(hp == 0)
		{
			_deathStart.x += Time.deltaTime*8.0f;
			Vector3 pos = _deathStart;
			_deathUp += Time.deltaTime*8.0f;
			pos.y = _deathStart.y - (_deathUp*_deathUp);
			transform.position = pos;
		}
  }
	public bool IsLive()
  {
		return hp > 0;
	}
	void OnCollisionEnter2D(Collision2D collision)
	{
		
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Player")
		{
			if(hp > 0)
				col.gameObject.GetComponent<PlayerTrigger>().player.Damage();
		}
		if(col.gameObject.tag == "Weapon" && hp > 0)
		{
			--hp;
			if(_deathSound)
			{
				_audio.clip = _deathSound;
				_audio.Play();
			}
			if(hp <= 0)
			{
				if(_rigid)
					_rigid.simulated = false;
				_sprite.flipX = _sprite.flipY = true;
				_deathStart = transform.position;
				_deathUp = -2.0f;
				_deathStart.y = _deathStart.y - (_deathUp*_deathUp) + 10.0f;
				transform.position = _deathStart;
			}
		}

	}
}
