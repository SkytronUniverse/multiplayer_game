using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
/*
 * Script designed to create pseudo-random movement of the enemies, and to fire a shot every few seconds
 * Attached to Enemy prefab
 */ 
namespace kmb826_assignment07 {
    public class EnemyMovement : NetworkBehaviour
    {

        float timeElapsed = 0.0f; //time to determine when to change direction and when to shoot projectile

        private readonly float rSpeed = 2.5f; // rotation speed
        private readonly float speed = 1.15f; // movement speed
        int shootCount = 0;

        public GameObject bulletPrefab;
        public Transform bulletSpawn;

        Quaternion dest;

        void Start()
        {
            dest = Quaternion.Euler(new Vector3(0.0f, Random.Range(-270.0f, -270.0f), 0.0f)); //get random rotation for enemy direction to move towards
        }

        void Update()
        {

            timeElapsed += Time.deltaTime; // keep track of time passed

            // Fire a projectile when time is an even number
            if (shootCount == 100)
            {
                CmdFire();
                shootCount = 0;
            }

            if (timeElapsed > 5) // every count of 5 change direction
            {
                dest = Quaternion.Euler(new Vector3(0.0f, Random.Range(-270.0f, 270.0f), 0.0f)); // New Quaternion destination
                timeElapsed = 0.0f;
            }
            
            transform.rotation = Quaternion.Slerp(transform.rotation, dest, Time.deltaTime * rSpeed);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            shootCount++;


        }

        [Command]
        void CmdFire()
        {
            //create the bullet
            var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

            // Shoot the bullet by adding velocity
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10;

            //Spawn the bullet on the network
            NetworkServer.Spawn(bullet);

            //Destroy bullet after 2 seconds
            Destroy(bullet, 2.0f);
        }

    }
}
