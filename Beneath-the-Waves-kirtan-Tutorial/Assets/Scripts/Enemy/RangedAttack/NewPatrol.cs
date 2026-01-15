using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPatrol : BaseState
{
    public int wayPoint;
    public float waitTime;
    public override void Enter()
    {
        shootingEnemy.SetPatrolling(true);
    }

    public override void Exit()
    {
        shootingEnemy.SetPatrolling(false);
    }

    public override void Perform()
    {
        Patrol();

        if (shootingEnemy.canSeePlayer())
        {
            shootingEnemy.SetAttacking(true);
            stateMach.ChangeState(new NewAttack());
        }
    }

    public void Patrol()
    {
        if (shootingEnemy.Agent.remainingDistance < 0.2f)
        {
            waitTime += Time.deltaTime;
            if (waitTime > 3)
            {
                if (wayPoint < shootingEnemy.path.wayPoints.Count - 1)
                {
                    wayPoint++;
                }
                else { wayPoint = 0; }

                shootingEnemy.Agent.SetDestination(shootingEnemy.path.wayPoints[wayPoint].position);
                waitTime = 0;
            }       
        }
    }
}
