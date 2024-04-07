using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Collider2D collider;
    public GameObject projectile;
    [HideInInspector]
    public Vector2 direction = new Vector2(1, 0);
    public float force = 100f;
    public float duration = 10f;
    public Transform shootPosition;

    // Start is called before the first frame update
    public void WeaponStart()
    {
        collider.enabled = true;
    }
    
    public void WeaponFinish()
    {
        collider.enabled = false;
    }
}
