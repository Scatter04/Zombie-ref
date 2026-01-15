using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAccount : MonoBehaviour
{
    public static PlayerAccount Instance;
    private int coins = 0;
    private int bullets = 20;

    // Declare the events
    public event System.Action<int> OnCoinsChanged;
    public event System.Action<int> OnBulletsChanged;

    //Audio vars
    public AudioClip coinSound;
    public AudioClip ammoSound;
    public AudioSource src;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
    }

    public void AddCoins(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Attempt to add a negative amount of coins.");
            return;
        }
        src.clip = coinSound;
        src.Play();
        coins += amount;
        Debug.Log($"Coins added. Total coins: {coins}");
        UIManager.SafeUpdateCount(coins, UIManager.Instance.coinText, "Coins: ");
        OnCoinsChanged?.Invoke(coins);
    }

    public void AddBullets(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Attempt to add a negative amount of bullets.");
            return;
        }
        src.clip = ammoSound;
        src.Play();
        bullets += amount;
        Debug.Log($"Bullets added. Total bullets: {bullets}");
        UIManager.SafeUpdateCount(bullets, UIManager.Instance.ammoText, "Ammo: ");
        OnBulletsChanged?.Invoke(bullets);
    }

    public bool UseBullet()
    {
        if (bullets > 0)
        {
            bullets--;
            UIManager.SafeUpdateCount(bullets, UIManager.Instance.ammoText, "Ammo: "); // Update UI here
            Debug.Log($"Shot fired! Bullets left: {bullets}");
            OnBulletsChanged?.Invoke(bullets);
            return true;
        }
        Debug.LogError("Attempted to shoot but no bullets left!");
        return false;
    }

    public void CompleteQuest()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.CompleteQuest();
        }
    }


    // Getters for coins and bullets
    public int GetCoins() => coins;
    public int GetBullets() => bullets;
}