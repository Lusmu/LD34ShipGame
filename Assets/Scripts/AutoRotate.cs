using UnityEngine;
using System.Collections;

public class AutoRotate : MonoBehaviour 
{
	public Vector3 speed = new Vector3(0, 90, 0);

	void Update ()
	{
		transform.eulerAngles = speed * Time.time;
	}
}
