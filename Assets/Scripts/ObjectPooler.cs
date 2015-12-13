using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler<T> where T : Component
{
	private List<T> objects;
	private GameObject prefab;
	private float maxActive;

	public ObjectPooler(GameObject prefab, int preSpawn = 0, int maxActive = -1)
	{
		objects = new List<T>(preSpawn);
		this.prefab = prefab;
		this.maxActive = maxActive;

		for (int i = 0; i < preSpawn; i++) 
		{
			var go = GameObject.Instantiate(prefab) as GameObject;
			var instance = go.GetComponent<T> ();
			objects.Add (instance);
			instance.gameObject.SetActive (false);
		}
	}

	public T Get()
	{
		for (int i = 0; i < objects.Count; i++) 
		{
			if (objects [i] == null) 
			{
				objects.RemoveAt (i);
				i--;
				continue;
			}

			if (!objects [i].gameObject.activeSelf)
			{
				objects [i].gameObject.SetActive (true);
				return objects [i];
			}
		}

		if (maxActive < 0 || objects.Count < maxActive)
		{
			var go = GameObject.Instantiate(prefab) as GameObject;
			var instance = go.GetComponent<T> ();
			objects.Add (instance);
			return instance;
		}

		return null;
	}

	public int CountActive()
	{
		int count = 0;

		for (int i = 0; i < objects.Count; i++) 
		{
			if (objects [i] == null) 
			{
				objects.RemoveAt (i);
				i--;
				continue;
			}

			if (objects [i].gameObject.activeSelf)
			{
				count++;
			}
		}

		return count;
	}
}
