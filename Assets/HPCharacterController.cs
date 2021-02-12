using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPCharacterController : MonoBehaviour
{
    public int maxHp = 10;
    int hp = 0;
    HPBarHandler hpBar;
    protected bool isDead;
    protected bool isStuned;

    public float stunTime = 0.3f;
    float currentStunTimer = 0;

    public bool hasInvinsibleTime;
    public float invinsibleTime = 0.3f;
    float currentInvinsibleTimer;
    // Start is called before the first frame update
    virtual protected void Start()
    {

        hp = maxHp;
        hpBar = GetComponentInChildren<HPBarHandler>();
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

    void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }
}
