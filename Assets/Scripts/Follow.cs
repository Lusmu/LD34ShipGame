using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour 
{
	public Transform target;

	public bool lockYAxis = true;

	void LateUpdate () 
	{
		var pos = target.position;
		if (lockYAxis) pos.y = transform.position.y;
		transform.position = pos;
	}
}
