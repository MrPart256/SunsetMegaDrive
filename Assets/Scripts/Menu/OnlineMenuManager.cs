using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class OnlineMenuManager : MonoBehaviour
{
    public void openMenu(GameObject Menu)
    {
        Menu.SetActive(true);
    }
    public void closeMenu(GameObject Menu)
    {
        Menu.SetActive(false);
    }
    public void Back()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
}
