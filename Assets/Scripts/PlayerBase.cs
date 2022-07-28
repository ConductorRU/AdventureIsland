using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
	protected bool IsGrounded()
	{
		Bounds bounds = GetComponent<BoxCollider2D>().bounds;
		float range = bounds.size.y*0.1f;
		Vector2 vC =  new Vector2(bounds.center.x, bounds.min.y - range);
		Vector2 vL =  new Vector2(bounds.min.x, bounds.min.y - range);
		Vector2 vR =  new Vector2(bounds.max.x, bounds.min.y - range);
		LayerMask mask = LayerMask.GetMask("Default");
		RaycastHit2D hitC = Physics2D.Linecast(vC, bounds.center, mask);
		RaycastHit2D hitL = Physics2D.Linecast(vL, new Vector3(bounds.min.x, bounds.center.y, bounds.center.z), mask);
		RaycastHit2D hitR = Physics2D.Linecast(vR, new Vector3(bounds.max.x, bounds.center.y, bounds.center.z), mask);
		return ((hitC.collider && hitC.collider.gameObject != gameObject) || (hitL.collider && hitL.collider.gameObject != gameObject) || (hitR.collider && hitR.collider.gameObject != gameObject));
	}
	public float GetDistanceX(Vector3 target)
  {
		Vector3 pos = transform.position;
		return Mathf.Abs(target.x - pos.x);
	}
}
