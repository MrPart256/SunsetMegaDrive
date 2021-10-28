using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletFly : MonoBehaviour
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
    void Update()
    {
        rb.velocity = transform.forward * flySpeed;
   
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag=="Enviroment")
        {
            die();
        }
        if (col.gameObject.tag == "Player")
        {
            die();
            col.gameObject.GetComponent<PlayerControl>().getDamage(damage);
        }
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<enemyAI>().getDamage(damage);
            die();
        }
    }
    private void die()
    {
        Destroy(this.gameObject);
    }
}
