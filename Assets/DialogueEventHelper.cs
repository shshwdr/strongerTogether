﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEventHelper : Singleton<DialogueEventHelper>
{
    public bool dialogueFinished = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void startDialogue()
    {
        dialogueFinished = false;
        Time.timeScale = 0;
    }
    public void finishDialogue()
    {
        Time.timeScale = 1;
        dialogueFinished = true;
        EnemyManager.instance.updateLevel();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
