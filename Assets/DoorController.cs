using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject openedDoor;
    public GameObject closedDoor;
    bool isOpened = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueEventHelper.Instance.dialogueFinished && !isOpened)
        {
            if (EnemyManager.instance.isLevelCleared)
            {
                openedDoor.SetActive(true);
                closedDoor.SetActive(false);
                isOpened = true;
                if(GameManager.Instance.currentLevel == 0)
                {

                }
                else
                {
                    //AudioManager.Instance.playVicotry();
                    FModSoundManager.Instance.SetParam("Victory", 1);
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isOpened)
        {

            if (collision.collider.GetComponent<PlayerMeleeAttack>())
            {
                GameManager.Instance.GoToNextLevel();
            }
        }
    }
}
