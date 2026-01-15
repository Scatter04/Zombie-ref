using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    public StateMach stateMach;
    public ShootingEnemy shootingEnemy;

    public abstract void Enter();
    public abstract void Perform();
    public abstract void Exit();
}
