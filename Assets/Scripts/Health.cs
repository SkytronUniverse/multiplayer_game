using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

// Script from Unity Multiplayer Networking tutorial to keep track of health across all players and enemies
// Attached to "Player" and "Enemy" GameObjects

namespace kmb826_assignment07
{
    public class Health : NetworkBehaviour
    {

        private NetworkStartPosition[] spawnPoints;

        public bool destroyOnDeath;

        public RectTransform healthBar;

        public const int maxHealth = 100;

        [SyncVar(hook = "OnChangeHealth")]
        public int currentHealth = maxHealth;

        private void Start()
        {
            if (isLocalPlayer)
            {
                spawnPoints = FindObjectsOfType<NetworkStartPosition>();
            }
        }
        public void TakeDamage(int amt)
        {
            if (!isServer)
                return;

            currentHealth -= amt;
            if (currentHealth <= 0)
            {

                if (destroyOnDeath)
                {
                    Destroy(gameObject);
                }
                else
                {
                    currentHealth = maxHealth;
                    RpcRespawn(); //called on the server, but replaced on the client
                }
            }

        }

        void OnChangeHealth(int currentHealth)
        {
            healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
        }
        [ClientRpc]
        void RpcRespawn()
        {
            if (isLocalPlayer)
            {

                Vector3 spawnPoint = Vector3.zero;

                if (spawnPoints != null && spawnPoints.Length > 0)
                {
                    spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
                }

                transform.position = spawnPoint;
            }
        }
    }
}
