using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class avoidanceArea : MonoBehaviour
{
    public bool inZone;
    private List<Collider> colliders = new List<Collider>();
    public List<Collider> GetColliders() { return colliders; }
    private void OnTriggerEnter(Collider other)
    {
        if (!colliders.Contains(other)) {
            if (other.gameObject.tag == "playerBullet") { 
            colliders.Add(other);
                inZone = true;
            }
            
        }

    }
    private void OnTriggerExit(Collider other)
    {
        
            colliders.Remove(other);
            inZone = false;
        
    }
}
