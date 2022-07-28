using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
  // Start is called before the first frame update
	private Vector3 _dir = Vector3.right;
	private float _gravity = 0.0f;
	private float _gravityTime = 0.0f;
	public float speed = 32.0f;
	public float weight = 1.0f;
	public AudioClip sound;
	public Player player;
	public void SetDirection(Vector3 dir)
  {
		_dir = dir;
  }
  void Start()
  {
        
  }
	void OnDestroy()
  {
		if(player)
			player.hammersCount--;
  }
  // Update is called once per frame
  void Update()
  {
		Vector3 pos = transform.position;
		pos += _dir*speed*Time.deltaTime;
		_gravityTime += Time.deltaTime*1.5f;
		pos.y += -9.8f*weight*_gravityTime*Time.deltaTime*2.0f;

		transform.position = pos;
		Collider2D col = GetComponent<Collider2D>();
		if(!GetComponent<SpriteRenderer>().isVisible)
			Destroy(gameObject);
  }
	void OnCollisionEnter2D(Collision2D col)
	{
		Destroy(gameObject);
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		Destroy(gameObject);
	}
}
