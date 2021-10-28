using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    public static List<GameObject> enemyList= new List<GameObject>();
    public int enemyAmount;
    private GameObject enemy;
    void Awake()
    {
            enemyList.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
    }
}
