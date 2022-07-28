using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

[CustomEditor(typeof(CameraController))]
public class CameraBounds2DEditor : Editor
{
  CameraController _bounds;
  readonly BoxBoundsHandle m_BoundsHandle = new BoxBoundsHandle();

  public void OnSceneGUI()
  {
    _bounds = (CameraController)target;
    Matrix4x4 handleMatrix = Matrix4x4.identity;
    handleMatrix.SetRow(0, Vector4.Scale(handleMatrix.GetRow(0), new Vector4(1f, 1f, 0f, 1f)));
    handleMatrix.SetRow(1, Vector4.Scale(handleMatrix.GetRow(1), new Vector4(1f, 1f, 0f, 1f)));
    handleMatrix.SetRow(2, new Vector4(0f, 0f, 1f, _bounds.transform.position.z));

    using (new Handles.DrawingScope(handleMatrix))
    {
			Vector2 size = (_bounds.maxConstrant - _bounds.minConstrant);
			Vector2 center = _bounds.minConstrant + (_bounds.maxConstrant - _bounds.minConstrant)*0.5f;
			m_BoundsHandle.center = center;
			m_BoundsHandle.size = size;
			m_BoundsHandle.SetColor(Color.white);
			EditorGUI.BeginChangeCheck();
			m_BoundsHandle.DrawHandle();
			Rect rect = new Rect(m_BoundsHandle.center.x-(m_BoundsHandle.size.x/2), m_BoundsHandle.center.y - (m_BoundsHandle.size.y / 2), m_BoundsHandle.size.x, m_BoundsHandle.size.y);
			Handles.DrawSolidRectangleWithOutline(rect, new Color(1, 1, 1, 0.1f), Color.yellow);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(_bounds, string.Format("Modify {0}", ObjectNames.NicifyVariableName(_bounds.GetType().Name)));
				_bounds.minConstrant = m_BoundsHandle.center - m_BoundsHandle.size*0.5f;
				_bounds.maxConstrant = m_BoundsHandle.center + m_BoundsHandle.size*0.5f;
			}
    }


  }
}
