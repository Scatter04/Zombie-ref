using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    //reference to player and agent
    Transform player;
    NavMeshAgent agent;

    public float chasingSpeed = 5f;
    public float stopChasing = 20f;
    public float attackDistance = 2f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = chasingSpeed;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        //move enemy to player
        agent.SetDestination(player.position);
        animator.transform.LookAt(player.position);

        // stop chasing
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer > stopChasing)
        {
            animator.SetBool("isChasing", false);
        }

        //start attacking
        if (distanceFromPlayer < attackDistance)
        {
            animator.SetBool("isAttacking", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        agent.SetDestination(animator.transform.position);
    }
}
