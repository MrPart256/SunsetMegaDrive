using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class weaponPickUp : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "weapon")
        {
            if (gameObject.GetComponentInParent<PlayerControl>().noWeapon == true)
            {
                if (Input.GetKey(KeyCode.E))
                {

                    other.GetComponent<weapon>().pickUp(GetComponentInParent<PhotonView>().ViewID);
                    gameObject.GetComponentInParent<PlayerControl>().noWeapon = false;
                }
            }
        }

    }
}

