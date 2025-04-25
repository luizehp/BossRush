using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class ManagerMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void playGame()
    {
        SceneManager.LoadSceneAsync(2);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
