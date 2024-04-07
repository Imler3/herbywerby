using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMove : MonoBehaviour
{
    public int sceneBuildIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<HerbHealth>())
        {
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        }
    }
}
