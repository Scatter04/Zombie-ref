using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootingEnemy : MonoBehaviour
{
    private StateMach stateMach;
    private NavMeshAgent agent;
    private Animator animator;
    private GameObject player;

    public NavMeshAgent Agent { get { return agent; } }
    public GameObject Player { get { return player; } }

    [SerializeField]
    private string currentState;

    public int enemyHealth = 100;
    public Path path;
    public float sightDistance = 20f;
    public float fieldOfView = 65f;
    public Transform gunBarrel;
    public float fireRate = 2f;

    //Audio vars
    public AudioClip hurtSound;
    public AudioSource src;

    void Start()
    {
        stateMach = GetComponent<StateMach>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on ShootingEnemy.");
        }

        stateMach.Initialise(this);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        canSeePlayer();
        currentState = stateMach.activeState.ToString();
    }

    public bool canSeePlayer()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
            {
                Vector3 target = player.transform.position - transform.position;
                float angle = Vector3.Angle(transform.forward, target);

                if (angle >= -fieldOfView && angle <= fieldOfView)
                {
                    Ray ray = new Ray(transform.position, target);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, sightDistance))
                    {
                        if (hit.transform.gameObject == player)
                        {
                            return true;
                        }
                    }
                    Debug.DrawRay(ray.origin, ray.direction * sightDistance);
                }
            }
        }
        return false;
    }

    public void takeDamage(int damage)
    {
        src.clip = hurtSound;
        src.Play();
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            StopAllCoroutines();

            animator.SetTrigger("death");
            StartCoroutine(DestroyAfterDelay(3f));
        }
        else
        {
            animator.SetTrigger("hit");
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public void SetPatrolling(bool isPatrolling)
    {
        if (animator == null)
        {
            Debug.LogError("Patrolling animator component not found on ShootingEnemy.");
            return;
        }
        Debug.Log("SetPatrolling: " + isPatrolling);
        animator.SetBool("isPatrolling", isPatrolling);
    }

    public void SetAttacking(bool isAttacking)
    {
        if (animator == null)
        {
            Debug.LogError("Shooting animator component not found on ShootingEnemy.");
            return;
        }
        Debug.Log("SetAttacking: " + isAttacking);
        animator.SetBool("isShooting", isAttacking);
    }

    public void SetMovingLeft(bool isMovingLeft)
    {
        if (animator == null)
        {
            Debug.LogError("Animator component not found on ShootingEnemy.");
            return;
        }
        Debug.Log("SetMovingLeft: " + isMovingLeft);
        animator.SetBool("isMovingLeft", isMovingLeft);
    }

    public void SetMovingRight(bool isMovingRight)
    {
        if (animator == null)
        {
            Debug.LogError("Animator component not found on ShootingEnemy.");
            return;
        }
        Debug.Log("SetMovingRight: " + isMovingRight);
        animator.SetBool("isMovingRight", isMovingRight);
    }
}
