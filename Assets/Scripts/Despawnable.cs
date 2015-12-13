using UnityEngine;
using System.Collections;

public class Despawnable : MonoBehaviour 
{
	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Despawner") 
		{
			gameObject.SetActive (false);
		}
	}
}
