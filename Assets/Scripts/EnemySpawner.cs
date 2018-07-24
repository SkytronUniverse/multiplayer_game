using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//Script from Unity Networking Tutorial to spawn enemies
//Attached to Enemy Spawner GameObject in "Main" scene
namespace kmb826_assignment07
{
    public class EnemySpawner : NetworkBehaviour
    {

        public GameObject enemyPrefab;
        public int numberOfEnemies;

        // When server starts, spawn enemies
        public override void OnStartServer()
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                var spawnPosition = new Vector3(Random.Range(-8.0f, 8.0f), 0.0f, Random.Range(-8.0f, 8.0f));

                var spawnRotation = Quaternion.Euler(0.0f, Random.Range(0, 180), 0.0f);

                var enemy = (GameObject)Instantiate(enemyPrefab, spawnPosition, spawnRotation);
                NetworkServer.Spawn(enemy);
            }
        }

    }
}
