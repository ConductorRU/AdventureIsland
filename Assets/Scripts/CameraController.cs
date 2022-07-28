using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Vector2 minConstrant;
	public Vector2 maxConstrant;

  [HideInInspector] public Vector2 maxXlimit;
  [HideInInspector] public Vector2 maxYlimit;

	public Transform target = null;
	private Camera _camera;

  // Start is called before the first frame update
  void Start()
  {
		_camera = GetComponent<Camera>();
  }

  // Update is called once per frame
  void Update()
  {
		CalculateBounds();
		if(target)
		{
			Vector3 pos = transform.position;
			pos.x = target.position.x;
			transform.position = pos;
		}
		Vector3 currentPosition = transform.position;
	  Vector3 targetPosition = new Vector3(Mathf.Clamp(currentPosition.x, maxXlimit.x, maxXlimit.y), Mathf.Clamp(currentPosition.y, maxYlimit.x, maxYlimit.y), currentPosition.z);

		transform.position = targetPosition;
  }

  public void CalculateBounds()
  {
    float halfWidth = _camera.aspect * _camera.orthographicSize;
		float halfHeight = _camera.orthographicSize;
    maxXlimit = new Vector2(minConstrant.x + halfWidth, maxConstrant.x - halfWidth);
    maxYlimit = new Vector2(minConstrant.y + halfHeight, maxConstrant.x - halfHeight);
  }
	public float ConstraintTransformX(Transform transform, float offset = 0.0f, float velocity = 0.0f)
  {
		Vector3 pos = transform.position;
		if(pos.x - offset <= minConstrant.x)
		{
			pos.x = minConstrant.x + offset;
			if(velocity < 0.0f)
				velocity = 0.0f;
		}
		if(pos.x + offset >= maxConstrant.x)
		{
			pos.x = maxConstrant.x - offset;
			if(velocity > 0.0f)
				velocity = 0.0f;
		}
		transform.position = pos;
		return velocity;
	}
}
