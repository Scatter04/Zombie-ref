using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerMovementTest
{
    private float walkSpeed;
    private float runSpeed;
    // A Test behaves as an ordinary method
    [Test]
    public void MoveLeft()
    {
        walkSpeed = 1f;
        runSpeed = 1f;
        Assert.AreEqual(walkSpeed, new HumbleMovementCalc().CalcMovement(false, 1f, 0, walkSpeed, runSpeed).x, 1);
    }

    [Test]
    public void MoveRight()
    {
        walkSpeed = 1f;
        runSpeed = 1f;
        Assert.AreEqual(-walkSpeed, new HumbleMovementCalc().CalcMovement(false, -1f, 0, walkSpeed, runSpeed).x, 1);
    }

    [Test]
    public void MoveUp()
    {
        walkSpeed = 1f;
        runSpeed = 1f;
        Assert.AreEqual(walkSpeed, new HumbleMovementCalc().CalcMovement(false, 0, 1f, walkSpeed, runSpeed).y, 1);
    }

    [Test]
    public void MoveDown()
    {
        walkSpeed = 1f;
        runSpeed = 1f;
        Assert.AreEqual(walkSpeed, new HumbleMovementCalc().CalcMovement(false, 0, -1f, walkSpeed, runSpeed).y, 1);
    }

    [Test]
    public void Run()
    {
        walkSpeed = 1f;
        runSpeed = 5f;
        Assert.AreNotEqual(walkSpeed, runSpeed, "Run speed and walk speed are identical");
        Assert.AreEqual(runSpeed, new HumbleMovementCalc().CalcMovement(true, 1f, 0f, walkSpeed, runSpeed).x, 1);
    }
    [Test]
    public void Walk()
    {
        walkSpeed = 1f;
        runSpeed = 5f;
        Assert.AreNotEqual(walkSpeed, runSpeed, "Run speed and walk speed are identical");
        Assert.AreEqual(walkSpeed, new HumbleMovementCalc().CalcMovement(false, 1f, 0f, walkSpeed, runSpeed).x, 1);
    }
    [Test]
    public void Normalise()
    {
        walkSpeed = 3f;
        runSpeed = 1f;
        float expectedMagnitude = new Vector3(walkSpeed, 0f, walkSpeed).magnitude;
        Assert.AreNotEqual(expectedMagnitude, new HumbleMovementCalc().CalcMovement(false, 1f, 1f, walkSpeed, runSpeed).magnitude, "Vector is not normalised properly");
    }
}
