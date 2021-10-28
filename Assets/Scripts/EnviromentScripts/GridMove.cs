using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMove : MonoBehaviour
{
    public float moveSpeed;
    private float xSpeed;
    private MeshRenderer meshRenderer;
    private void OnEnable()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        Time.timeScale = 1f;

    }
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        xSpeed = Time.time * moveSpeed;
        Vector2 offset = new Vector2(0,xSpeed);
        meshRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }
}
