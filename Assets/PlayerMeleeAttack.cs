using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    public float cooldownTime = 0.2f;
    float currentCooldownTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.collider.GetComponent<EnemyController>())
        {
            if (currentCooldownTimer < cooldownTime)
            {
                return;
            }
            currentCooldownTimer = 0;
            collision.collider.GetComponent<EnemyController>().getDamage();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
    }
    // Update is called once per frame
    void Update()
    {

        currentCooldownTimer += Time.deltaTime;
    }
}
