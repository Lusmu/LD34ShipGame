using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour
{
    public Transform target;

	void LateUpdate ()
    {
	    if (target)
        {
            transform.LookAt(target);
        }
	}
}
