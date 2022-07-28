using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
	public Sprite brokenEgg;
	public Pickup item;
  private bool _isBroken = false;
	private SpriteRenderer _sprite;
  void Start()
  {
		_sprite = GetComponent<SpriteRenderer>();
  }

  // Update is called once per frame
  void Update()
  {
        
  }
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Weapon")
		{
			if(!_isBroken)
			{
				_isBroken = true;
				if(brokenEgg)
					_sprite.sprite = brokenEgg;
			}
			else
			{
				DestroyEgg();
			}
		}
		if(col.gameObject.tag == "Player")
		{
			DestroyEgg();
		}
	}
	void DestroyEgg()
	{
		if(item)
			{
				Pickup pickup = Instantiate(item);
				Vector3 pos = gameObject.transform.position;
				pos.y += 1.0f;
				pickup.transform.position = pos;
			}
			Destroy(gameObject);
	}
}
