﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isCheatOn = true;
    int currentLevel;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void GotoLevel(int level)
    {
        currentLevel = level;
        //hideRestartButton();
        Time.timeScale = 1;

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(level);
    }
   

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 9; i++)
        {
            if (isCheatOn && Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                GotoLevel(i);

                //SceneManager.LoadScene(i);
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);

        }
    }
}
