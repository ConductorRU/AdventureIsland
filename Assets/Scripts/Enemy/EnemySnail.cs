using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnail : Enemy
{
  // Start is called before the first frame update
  new void Start()
  {
		base.Start();
  }

  // Update is called once per frame
  new void Update()
  {
		base.Update(); 
  }
	void OnMoveFrame()
	{
		_rigid.MovePosition(GetComponent<Rigidbody2D>().position - new Vector2(speed, 0.0f));

		//GetComponent<Rigidbody2D>().transform.position = vel;
	}
}
