using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPCharacterController : MonoBehaviour
{
    protected Animator animator;
    public int maxHp = 10;
    int hp = 0;
    HPBarHandler hpBar;
    public  bool isDead;
    protected bool isStuned;

    public float stunTime = 0.3f;
    float currentStunTimer = 0;

    public bool hasInvinsibleTime;
    public float invinsibleTime = 0.3f;
    float currentInvinsibleTimer;
    protected EmotesController emotesController;
    protected GameObject spriteObject;
    // Start is called before the first frame update
    virtual protected void Awake()
    {

        emotesController = GetComponentInChildren<EmotesController>();
        hpBar = GetComponentInChildren<HPBarHandler>();
        animator = transform.Find("Sprites").GetComponent<Animator>();
        spriteObject = animator.gameObject;
    }
    virtual protected void Start()
    {
        hp = maxHp;
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        if (isStuned)
        {
            currentStunTimer += Time.deltaTime;
            if (currentStunTimer >= stunTime)
            {
                isStuned = false;
            }
        }
        currentInvinsibleTimer += Time.deltaTime;
    }


    public void getDamage(int damage = 1)
    {
        if (isDead)
        {
            return;
        }
        if(hasInvinsibleTime && currentInvinsibleTimer < invinsibleTime)
        {
            return;
        }
        currentInvinsibleTimer = 0;
        hp -= damage;
        hp = Mathf.Clamp(hp, 0, maxHp);
        hpBar.SetHealthBarValue(hp / (float)(maxHp));

        if (hp == 0)
        {
            Die();
        }
        else
        {
            isStuned = true;
            currentStunTimer = 0;
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }


    bool facingRight = true;
    void flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = spriteObject.transform.localScale;
        scaler.x = -scaler.x;
        spriteObject.transform.localScale = scaler;
    }
    public void testFlip(Vector3 movement)
    {

        if (facingRight == false && movement.x > 0)
        {
            flip();
        }
        if (facingRight == true && movement.x < 0)
        {
            flip();
        }
    }
}
