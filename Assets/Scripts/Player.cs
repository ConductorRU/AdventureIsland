using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerBase
{
	public Bullet weaponHammer;
	public float speed = 10.0f;
	public float run = 2.0f;
	public float jumpForce = 50.0f;
	public bool immortal = false;
	public AudioClip _jumpSound;
	[Range(0, 1)] public float sliding = 0.9f;
	[HideInInspector] public int hammersCount = 0;
	protected AudioSource _audio;
	private bool _isLive = true;
	private float _deathHeight = 0.0f;
	private float _deathUp = 0.0f;
	private float _deathFloat = 1.0f;
	private bool _isDeathUp = true;
	private bool _isAttack = false;
	private float _attackDelta = 0.0f;

	private Vector2 _move = new Vector2(0.0f, 0.0f);
	private bool _flip = false;
	private bool _isJump = false;
	private float _pressJump = 0.0f;
	private bool _back = false;
	private float _moveHor = 0.0f;
	private float _deltaRun = 0.0f;
	public AudioSource GetAudio()
  {
		return _audio;
	}
  // Start is called before the first frame update
  void Start()
  {
		_audio = gameObject.AddComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update()
  {
		_moveHor = Input.GetAxisRaw("Horizontal");
		if(_moveHor != 0.0f)
			_flip = (_moveHor < 0.0f);
		if(_moveHor > 0.0f && _back || _moveHor < 0.0f && !_back)
			_deltaRun = 0.0f;
		_back = _moveHor < 0.0f;
		Vector3 vel = GetComponent<Rigidbody2D>().velocity;
		GetComponent<SpriteRenderer>().flipX = _flip;
		bool isGrounded = IsGrounded();
		GetComponent<Animator>().SetBool("IsGround", isGrounded);
		if(Input.GetKeyDown(KeyCode.L) && !isGrounded)
			_pressJump = 0.25f;
		if(_pressJump > 0.0f)
			_pressJump -= Time.deltaTime;
		if((Input.GetKeyDown(KeyCode.L) || (Input.GetKey(KeyCode.L) && _pressJump > 0.0f)) && isGrounded)
		{
			_isJump = true;
			if(_jumpSound && (_audio.clip != _jumpSound || !_audio.isPlaying))
			{
				_audio.clip = _jumpSound;
				_audio.Play();
			}
		}
		bool isRun = Mathf.Abs(vel.x) > 10.0f;

		GetComponent<Animator>().SetBool("IsRun", isRun);
		if(Input.GetKey(KeyCode.K) && Mathf.Abs(_moveHor) > 0.1f)
			_deltaRun += Time.deltaTime*2.0f;
		else
			_deltaRun -= Time.deltaTime*2.0f;
		_deltaRun = Mathf.Clamp(_deltaRun, 0.0f, 1.0f);
		if(_isJump)
		{
			_isJump = false;
			vel.y = jumpForce;
			GetComponent<Rigidbody2D>().velocity = vel;
		}
		if(!Input.GetKey(KeyCode.L) && vel.y > jumpForce*0.5f && !isGrounded)
		{
			vel.y = jumpForce*0.25f;
			Debug.Log(vel.y);
			GetComponent<Rigidbody2D>().velocity = vel;
		}

		if(_isLive)
		{ 
			if(Input.GetKeyDown(KeyCode.K) && hammersCount < 2)
			{
				_isAttack = true;
				_attackDelta = 0.1f;
				Vector2 pos = transform.position;
				Bullet hammer = Instantiate<Bullet>(weaponHammer);
				if(GetComponent<SpriteRenderer>().flipX)
					hammer.SetDirection(Vector3.left);
				pos.y += 3.0f;
				hammer.transform.position = pos;
				hammer.player = this;
				GetComponent<AudioSource>().clip = hammer.sound;
				GetComponent<AudioSource>().Play();
				++hammersCount;
			}
			if(_isAttack)
			{
				_attackDelta -= Time.deltaTime;
				if(_attackDelta <= 0.0f)
				{
					_isAttack = false;
				}
			}
			GetComponent<Animator>().SetBool("IsAttack", _isAttack);
		}
		else
		{
			float deathSpeed = 24.0f;
			float deathHeight = 4.0f;
			if(_isDeathUp)
				_deathUp += Time.deltaTime*deathSpeed;
			else if(_deathFloat < 0.0f)
				_deathUp -= Time.deltaTime*deathSpeed;
			if(_isDeathUp && _deathUp >= deathHeight)
			{
				_deathUp = deathHeight;
				_isDeathUp = false;
			}
			else if(!_isDeathUp && _deathFloat >= 0.0f)
			{
				_deathFloat -= Time.deltaTime;
			}
			else if(_deathUp <= -20.0f)
			{
				//Destroy(gameObject);
			}
			Vector3 pos = transform.position;
			pos.y = _deathHeight + _deathUp;
			transform.position = pos;
		}
  }

	void FixedUpdate()
  {
		Vector2 v = GetComponent<Rigidbody2D>().velocity;
		float s = Mathf.Lerp(speed, speed*run, _deltaRun);
		Vector2 vel;
		if(Mathf.Abs(_moveHor) > 0.01f)
		{
			vel = new Vector2(_moveHor*s, v.y);
		}
		else
		{
			vel = new Vector2(v.x*sliding, v.y);
		}
		vel.x = Camera.main.GetComponent<CameraController>().ConstraintTransformX(transform, 2.0f, vel.x);
		GetComponent<Rigidbody2D>().velocity = vel;
  }

	public void Damage()
  {
		if(GetComponent<Rigidbody2D>().simulated && !immortal)
		{
			GetComponent<Animator>().SetBool("IsDeath", true);
			GetComponent<Rigidbody2D>().simulated = false;
			_deathHeight = transform.position.y;
			_isLive = false;
			Game.game.OnDeath();
		}
		//Destroy(gameObject);
  }

	public void Reset()
	{
		GetComponent<Rigidbody2D>().simulated = true;
		GetComponent<Animator>().SetBool("IsDeath", false);
		_isLive = true;
		_deathHeight = 0.0f;
		_deathUp = 0.0f;
		_deathFloat = 1.0f;
		_isDeathUp = true;
		_isAttack = false;
		_attackDelta = 0.0f;
	}
}
