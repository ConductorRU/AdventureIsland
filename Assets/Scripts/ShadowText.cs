using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowText : MonoBehaviour
{
	public string text;
  // Start is called before the first frame update
  void Start()
  {
        
  }

  // Update is called once per frame
  void Update()
  {
		GetComponent<TextMesh>().text = text;
		GetComponent<TextMesh>().font.material.mainTexture.filterMode = FilterMode.Point;
		TextMesh[] meshes = GetComponentsInChildren<TextMesh>();
		foreach(TextMesh m in meshes)
		{
			m.font.material.mainTexture.filterMode = FilterMode.Point;
			m.text = text;
		}
  }
}
