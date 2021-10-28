using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBulletFly : MonoBehaviour
{
 
        public int damage;
        public float flySpeed;
        public float lifeTime;
        private Rigidbody rb;
        void Start()
        {
            rb = this.GetComponent<Rigidbody>();
            Invoke("die", lifeTime);
        }

        // Update is called once per frame
        void Update()
        {
            rb.velocity = transform.forward * flySpeed;
        }
        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Enviroment")
            {
                die();
            }
            if (col.gameObject.tag == "Player")
            {
                die();
                col.gameObject.GetComponent<PlayerControl>().getDamage(damage);
            }
        }
        private void die()
        {
            Destroy(this.gameObject);
        }
    
}
