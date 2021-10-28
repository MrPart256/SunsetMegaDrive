using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HpCount : MonoBehaviour
{
    public Text text;
    private PlayerControl Player;
    void OnEnable()
    {
        Player = gameObject.GetComponentInParent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
       text.text = Player.health.ToString();
    }
}
