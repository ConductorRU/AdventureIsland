using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnake : Enemy
{
	public float jumpDelay = 1.0f;
	public float jumpForce = 30.0f;
	public float jumpLength = 8.0f;
	public int jumpCount = 3;
	private float _jumpDelay = 0.0f;
	private int _jumpCount = 0;
	private float _direction = 0.0f;
  // Start is called before the first frame update
  new void Start()
  {
		base.Start();
  }

  // Update is called once per frame
  new void Update()
  {
		base.Update();
		if(!IsLive())
			return;
		Vector3 playerPos = Game.game.GetPlayer().transform.position;
		_sprite.flipX = (playerPos.x > transform.position.x);
    bool isJump = !IsGrounded();
		if(!isJump && (_jumpCount > 0 || Game.game.GetPlayer().GetDistanceX(transform.position) <= 20.0f))
		{
			_jumpDelay += Time.deltaTime;
			if(_jumpDelay >= jumpDelay)
			{
				if(playerPos.x > transform.position.x)
					_direction = 1.0f;
				else
					_direction = -1.0f;
				_jumpDelay = 0.0f;
				GetComponent<Rigidbody2D>().velocity = Vector2.up*jumpForce;
				++_jumpCount;
				if(_jumpCount >= jumpCount)
					_jumpCount = 0;
			}
		}
		GetComponent<Animator>().SetBool("IsJump", isJump);
  }

	void FixedUpdate()
  {
		bool isJump = !IsGrounded();
		float speed = isJump ? jumpLength : 0.0f;
		Vector2 v = _rigid.velocity;
		Vector2 vel = new Vector2(_direction*speed, v.y);
		_rigid.velocity = vel;
	}
}
