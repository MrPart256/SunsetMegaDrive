using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AmmoCount : MonoBehaviour
{
    private PlayerControl Player;
    public Text text;
    void Start()
    {
        Player = gameObject.GetComponentInParent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.GetComponentInChildren<weapon>() != null) { 
        text.text = Player.GetComponentInChildren<weapon>().magazineCurrent + "/" + Player.GetComponentInChildren<weapon>().ammoAmount;
        }
        else
        {
            text.text = "";
        }
    }
}
