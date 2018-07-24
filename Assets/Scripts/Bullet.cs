using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script from Unity Multiplayer Networking tutorial to handle collisions of bullets
// Attached to Bullet prefab
namespace kmb826_assignment07
{
    public class Bullet : MonoBehaviour
    {

        //Detects collision and deducts health from object hit
        private void OnCollisionEnter(Collision collision)
        {
            var hit = collision.gameObject;
            var health = hit.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(10);
            }

            Destroy(gameObject);
        }
    }
}
