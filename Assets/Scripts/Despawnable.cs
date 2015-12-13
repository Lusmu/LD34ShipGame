using UnityEngine;
using System.Collections;

public class Despawnable : MonoBehaviour 
{
	void OnTriggerExit(Collider other)
	{
		if (enabled && other.tag == "Despawner") 
		{
			//gameObject.SetActive (false);
		}
	}
}
