using UnityEngine;
using System.Collections;

namespace IfelseMedia.GuideShip
{
	public class ShipSpawner : MonoBehaviour 
	{
		public ObjectPooler<TransportAIController> shipPool;

		[SerializeField]
		private TransportAIController prefab;

		[SerializeField]
		private float spawnCheckRadius = 5;

		[SerializeField]
		private float spawnRadius = 10;

		[SerializeField]
		private float spawnInterval = 10;

		private float timeTillNextSpawn;

        [SerializeField]
        private int maxShips = 2;

		void Update () 
		{
			if (timeTillNextSpawn <= 0 && IsInDespawnerRange()) 
			{
				var randomPosition = transform.position +
					Vector3.forward * Random.Range(-spawnRadius, spawnRadius) +
					Vector3.right * Random.Range(-spawnRadius, spawnRadius);

				Collider[] res = new Collider[0];
				if (Physics.OverlapSphereNonAlloc (randomPosition, spawnCheckRadius, res) == 0) 
				{
					if (shipPool == null) 
					{
                        shipPool = new ObjectPooler<TransportAIController>(prefab.gameObject, 1, maxShips);
					}

					var ship = shipPool.Get ();

					if (ship != null) 
					{
						Debug.Log ("Got ship");
						timeTillNextSpawn = spawnInterval;

						ship.transform.position = randomPosition;
						ship.transform.rotation = transform.rotation;
					}
				}
				else
				{
					Debug.Log ("Checksphere fail " + res.Length);
				}
			}
			else
			{
				timeTillNextSpawn -= Time.deltaTime;
			}
		}

		bool IsInDespawnerRange()
		{
			var sphere = GameManager.Instace.despawner.GetComponent<SphereCollider> ();

			float distSqr = (GameManager.Instace.despawner.position - transform.position).sqrMagnitude;
			return distSqr < (sphere.radius - spawnRadius) * (sphere.radius - spawnRadius);
		}
	}
}