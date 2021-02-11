using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : HPCharacterController
{
    Animator animator;
    NavMeshAgent agent;

    public EnemyType enemyType;

    public int mergeLevel;


    //Rigidbody2D rb;
    // Start is called before the first frame update
    protected override void Start()
    {

        //rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        EnemyManager.instance.enemiesDictionary[enemyType].Add(this);
        base.Start();
    }
    public bool canBePaired()
    {
        return !isDead;
    }

    float getDistanceToTarget(Transform target)
    {
        //todo use navmesh distance instead of position distance
        return ((Vector2)transform.position - (Vector2)target.position).magnitude;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (isStuned)
        {
            agent.isStopped = true;
            return;
        }
        //find the cloest target, either player or enemy with same type and merge level
        float shortestDistance = getDistanceToTarget(EnemyManager.instance.player.transform);
        Transform shortestTarget = EnemyManager.instance.player.transform;
        foreach (EnemyController enemy in EnemyManager.instance.enemiesDictionary[enemyType])
        {
            if (enemy == this)
            {
                continue;
            }
            if (!enemy.canBePaired())
            {
                continue;
            }
            float distance = getDistanceToTarget(enemy.transform);
            if (distance < shortestDistance)
            {
                shortestTarget = enemy.transform;
                shortestDistance = distance;
            }
        }

        agent.isStopped = false;
        agent.SetDestination(shortestTarget.position);
    }

}
