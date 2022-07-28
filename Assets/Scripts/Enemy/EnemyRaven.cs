using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRaven : Enemy
{
	[Range(0, 1)]
	public float amplitude = 0.0f;
	public float waveLength = 5.0f;
	private float _startY = 0.0f;
	private float _amplitude = 0.0f;
	private bool _active = false;
  // Start is called before the first frame update
  new void Start()
  {
		base.Start();
		_amplitude = amplitude;
		_startY = transform.position.y - Mathf.Sin(_amplitude*Mathf.PI*2.0f)*waveLength;
  }

  // Update is called once per frame
  new void Update()
  {
		base.Update();
		if(_sprite.isVisible)
			_active = true;
		if(hp > 0 && _active)
		{
			Vector3 pos = transform.position;
			pos.x -= Time.deltaTime*speed;
			_amplitude -= Time.deltaTime*0.5f;
			pos.y = _startY + Mathf.Sin(_amplitude*Mathf.PI*2.0f)*waveLength;
			transform.position = pos;
		}
		if(hp <= 0)
		{
			GetComponent<Animator>().Rebind();
			GetComponent<Animator>().enabled = false;
		}
  }
	void OnDrawGizmosSelected()
  {
		float am = amplitude;
		Vector3 prev = Vector3.zero;
		Vector3 pos1 = Vector3.zero;
		bool isFirst = true;
		Vector3 pos = transform.position;
		float startY = pos.y - Mathf.Sin(am*Mathf.PI*2.0f)*waveLength;
		float maxLength = Mathf.Min(speed*10.0f, 32.0f);
		for(float x = speed*2.0f; x >= -maxLength; x -= speed*0.1f)
		{
			Gizmos.color = Color.blue;
			am = (x*0.5f)/speed;
			float y = startY + Mathf.Sin(am*Mathf.PI*2.0f)*waveLength;
			prev = pos1;
			pos1 = new Vector3(pos.x + x - amplitude*speed*2.0f, y, pos.z);
			if(!isFirst)
				Gizmos.DrawLine(prev, pos1);
			isFirst = false;
		}
  }
}
