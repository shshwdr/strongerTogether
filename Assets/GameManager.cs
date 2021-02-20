﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool isCheatOn = true;
    public int currentLevel;
    public bool isGameOver;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void GoToNextLevel()
    {
        currentLevel += 1;
        GotoLevel(currentLevel);
    }
    public void GotoLevel(int level)
    {
        currentLevel = level;
        //hideRestartButton();
        Time.timeScale = 1;
        isGameOver = false;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(level+1);
    }


    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            Time.timeScale = 0;
        }
        for (int i = 0; i < 9; i++)
        {
            if (isCheatOn && Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                GotoLevel(i);

                //SceneManager.LoadScene(i);
            }
        }
        if ((!EnemyManager.instance.player || EnemyManager.instance.player.isDead) && Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);

        }
    }
}
