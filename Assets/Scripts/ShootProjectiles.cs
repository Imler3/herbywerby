using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectiles : MonoBehaviour
{
    public GameObject projectilePrefab;
    private float waitTime = 1.0f;
    private float timer = 0.0f;
    private int width, height;
    private float scrollBar = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        width = Screen.width;
        height = Screen.height;
        Time.timeScale = scrollBar;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > waitTime)
        { 
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            timer = timer - waitTime;
            Time.timeScale = scrollBar;
        }
    }

}
