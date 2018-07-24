using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


// Script from Unity Multiplayer Networking tutorial to control player movement and firing of projectiles
// This script is attached to "Player" prefab

namespace kmb826_assignment07
{
    public class PlayerController : NetworkBehaviour
    {
        [HideInInspector]
        public Transform cam; //to cache the Camera.main.transform

        public Transform camParent;

        [HideInInspector]
        public Transform visor; // to cache the visor transform

        private const float MAXSPEED = 0.1f; // used for acceleration handling of movement
        public GameObject bulletPrefab; 
        public Transform bulletSpawn;
        private int time = 0; // time variable for movement
        private float speed = 0.0f; // speed variable for accelleration

        public override void OnStartLocalPlayer()
        {
            GetComponent<MeshRenderer>().material.color = Color.blue;
            visor = transform.Find("Visor"); // cache visor
            cam = Camera.main.transform;
            camParent = cam.parent; //set the parent to cam.parent
            camParent.position = visor.position; // set camera position to visor position

        }

        void Update()
        {
            if (!isLocalPlayer)
                return;

            if (Input.GetMouseButton(0))
            {
                ButtonHandler(); //Calls function that makes decisions of whether to shoot or move based on length of button press
            }

            camParent.position = visor.position;
            
            // Rotate player with camera rotation
            Vector3 turn = cam.eulerAngles;
            turn.x = 0f;
            turn.z = 0f;
            print(Vector3.Distance(transform.eulerAngles, turn));
            
            transform.eulerAngles = turn;
            
        }

        // Controls firing projectiles and movement
        
        void ButtonHandler()
        {

            if (Input.GetMouseButton(0) && time > 20) // if mouse button is being held down for certain amount of time
            {
                if (Input.GetMouseButtonDown(0)) { time = 0; speed = 0.0f; return; } // if button is released, reset variables and exit function
                if (speed < MAXSPEED) // accellerate
                {
                    camParent.position = visor.position;
                    Vector3 moveDirection = cam.forward.normalized; // direction to move player towards
                    moveDirection.y = 0f; // do not change y-position
                    transform.Translate(transform.InverseTransformDirection(moveDirection) * speed);
                    speed += 0.005f; //increase speed

                }
                else
                { //if MAXSPEED reached, continue at that rate
                    camParent.position = visor.transform.position;
                    Vector3 moveDirection = cam.forward.normalized;
                    moveDirection.y = 0f;
                    transform.Translate(transform.InverseTransformDirection(moveDirection) * MAXSPEED);

                }
            }
            else
            {
                //if button not held down long enough, shoot projectile
                if (Input.GetMouseButtonDown(0))
                {
                    CmdFire();
                    time = 0; // reset time
                }
                else
                    time++; //otherwise add time if button is being held down
            }
        }

        [Command]
        void CmdFire()
        {
          
            //create the bullet
            var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

            // Shoot the bullet by adding velocity
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

            //Spawn the bullet on the network
            NetworkServer.Spawn(bullet);

            //Destroy bullet after 2 seconds
            Destroy(bullet, 2.0f);
        }
    }
}
