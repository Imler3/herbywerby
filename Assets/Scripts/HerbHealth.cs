using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerbHealth : MonoBehaviour
{
    private int currentHealth;
    private HerbMovement herbMovement;

    // Start is called before the first frame update
    void Start()
    {
        herbMovement = GetComponent<HerbMovement>();
    }

    public void DecreaseHealth()
    {
        currentHealth -= 1;
        if (currentHealth <=  0) 
        { 
            currentHealth = 0;
        }
    }

    public void ResetHealth()
    {
        currentHealth = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        Collider2D thisCollision = GetComponent<Collider2D>();
        if(collision.otherCollider == thisCollision)
        {
            if(collision.gameObject.TryGetComponent(out Weapon weapon))
            {
                DecreaseHealth();
                if(currentHealth == 0)
                {
                    herbMovement.TimeToDie();
                }
            }
        }
    }   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D thisCollider = GetComponent<Collider2D>();
        if (collision.IsTouching(thisCollider))
        {
            if(collision.gameObject.TryGetComponent(out Weapon weapon))
            {
                DecreaseHealth();
                if(currentHealth == 0)
                {
                    herbMovement.TimeToDie();
                }
            }
        }
    }
}
