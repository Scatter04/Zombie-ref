using System.Collections;
using NUnit.Framework;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerEnemyInteractionTests
{
    private GameObject playerObject;
    private GameObject enemyObject;
    private Player player;
    private Enemy enemy;

    [SetUp]
    public void SetUp()
    {
        // Create a simple plane and bake a NavMesh on it
        var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        NavMeshSurface surface = plane.AddComponent<NavMeshSurface>();
        surface.BuildNavMesh();

        // Create player and enemy game objects
        playerObject = new GameObject("Player");
        enemyObject = new GameObject("Enemy");

        // Add components to player and enemy game objects
        player = playerObject.AddComponent<Player>();
        enemy = enemyObject.AddComponent<Enemy>();

        // Set up player health bar images (mocked with empty GameObjects)
        player.frontHealthBar = new GameObject("FrontHealthBar").AddComponent<UnityEngine.UI.Image>();
        player.backHealthBar = new GameObject("BackHealthBar").AddComponent<UnityEngine.UI.Image>();

        // Mock setting up the enemy with necessary components
        var agent = enemyObject.AddComponent<NavMeshAgent>();
        var animator = enemyObject.AddComponent<Animator>();

        player.bloodScreen = new GameObject("BloodScreen");
        player.bloodScreen.AddComponent<UnityEngine.UI.Image>(); // Add an Image component

        // Ensure necessary initialization for scripts
        player.Start(); // Call the Start method to initialize
        enemy.Start();  // Call the Start method to initialize
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(playerObject);
        Object.Destroy(enemyObject);
        // Clean up NavMesh
        var surfaces = GameObject.FindObjectsOfType<NavMeshSurface>();
        foreach (var surface in surfaces)
        {
            Object.Destroy(surface.gameObject);
        }
    }

    [Test]
    public void PlayerTakesDamage()
    {
        // Set initial states
        player.HP = player.getMAXHP();

        player.canTakeDamage = true;
        // Simulate taking damage
        player.takeDamage(10f);

        // Assert player took damage
        Assert.AreEqual(player.getMAXHP() - 10f, player.getHP());
    }

    [UnityTest]
    public IEnumerator PlayerTakesDamageWithCooldown()
    {
        // Set player HP and enable damage
        player.HP = player.getMAXHP();
        player.canTakeDamage = true;

        // Simulate taking damage
        player.takeDamage(10);

        // Check if damage was taken
        Assert.AreEqual(player.getMAXHP() - 10, player.getHP());

        // Simulate taking damage during cooldown
        player.takeDamage(10);

        // Check if HP did not decrease due to cooldown
        Assert.AreEqual(player.getMAXHP() - 10, player.getHP());

        // Wait for cooldown
        yield return new WaitForSeconds(player.damageCooldown);

        // Simulate taking damage after cooldown
        player.takeDamage(10);

        // Check if damage was taken after cooldown
        Assert.AreEqual(player.getMAXHP() - 20, player.getHP());
        yield return null;
    }

    [Test]
    public void PlayerTakesDamageFromEnemy()
    {
        // Arrange
        float initialPlayerHP = player.getHP();
        enemy.damage = 10f;

        player.takeDamage(enemy.damage);
        Debug.Log(initialPlayerHP - enemy.damage + ": player health after damage");
        // Assert
        Assert.Less(initialPlayerHP - enemy.damage, player.getHP());
    }

    [Test]
    public void EnemyDiesAfterTakingDamage()
    {
        // Arrange
        int damageAmount = enemy.enemyHealth + 10;

        enemy.takeDamage(damageAmount);

        // Assert
        Assert.IsTrue(enemy.isDead);
    }
}
