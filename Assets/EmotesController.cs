using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EmoteType { heart, angry};

public class EmotesController : MonoBehaviour
{
    Dictionary<EmoteType, string> emoteDictionary;
    Animator animator;
    private void Awake()
    {

        animator = GetComponent<Animator>();
        emoteDictionary = new Dictionary<EmoteType, string>()
        {
            { EmoteType.heart , "heart" },
            { EmoteType.angry , "angry" }
        };
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    public void showEmote(EmoteType emote)
    {
        var animationState = animator.GetCurrentAnimatorStateInfo(0);
        animator.SetTrigger(emoteDictionary[emote]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
