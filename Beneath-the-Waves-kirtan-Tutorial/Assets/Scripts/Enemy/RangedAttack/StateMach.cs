using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMach : MonoBehaviour
{
    public BaseState activeState;
    public NewPatrol newPatrol;

    private ShootingEnemy shootingEnemy;

    public void Initialise(ShootingEnemy enemy)
    {
        shootingEnemy = enemy;
        newPatrol = new NewPatrol();
        newPatrol.shootingEnemy = enemy;
        ChangeState(newPatrol);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activeState != null) {
            activeState.Perform();
        }
    }

    public void ChangeState(BaseState newState)
    {
        if (activeState != null)
        {
            activeState.Exit();
        }
        activeState = newState;

        if (activeState != null)
        {
            //get new state
            activeState.stateMach = this;
            activeState.shootingEnemy = shootingEnemy;
            activeState.Enter();
        }
    }
}
