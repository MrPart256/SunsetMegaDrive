using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float timeToDestroy;
    private void OnEnable()
    {
        Invoke("Destroy", timeToDestroy); 
    }
    private void Destroy() {
        Destroy(this.gameObject);
    }
}
