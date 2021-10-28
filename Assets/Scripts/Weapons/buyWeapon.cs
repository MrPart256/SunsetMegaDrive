using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
public class buyWeapon : MonoBehaviour
{
    public Button butItem;
    public GameObject item;
    public GameObject weaponPlace;
    public PlayerControl control;
    public void buyItem()
    {
        if (control.noWeapon == true) {
            Debug.Log("Купил");
        var weapon=PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", item.name.ToString()),weaponPlace.transform.position,Quaternion.identity);
            weapon.GetComponent<weapon>().pickUp(GetComponentInParent<PhotonView>().ViewID); 
            control.noWeapon = false;
        }
        else
        {
            return;
        }
    }
}
