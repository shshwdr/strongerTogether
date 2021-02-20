using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Gameover()
    {
        gameOverPanel.SetActive(true);
        GameManager.Instance.isGameOver = true;
        Time.timeScale = 0;
    }

    public void Restart()
    {
        DialogueLua.SetVariable("merged", false);
        DialogueLua.SetVariable("hasHelp", false);
        GameManager.Instance.GotoLevel(0);
    }

    public void RestartLevel()
    {
        DialogueLua.SetVariable("merged", false);
        DialogueLua.SetVariable("hasHelp", false);
        GameManager.Instance.GotoLevel(6);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
