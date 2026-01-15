using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // Variable Declaration
    public static QuestManager Instance;
    private Quest currentQuest;

    // This method enforces a singleton pattern ensuring there is one instance of QuestManager
    void Awake()
    {
        // Ensure there is only one instance of QuestManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // Do not destroy the QuestManager when loading a new scene
            DontDestroyOnLoad(gameObject);
        }
    }

    // Enemy killed event which ensures QuestManager doesn't listen to 'onEnemyKilled'
    void OnEnable()
    {
        EnemyDeath.OnEnemyKilled += OnEnemyKilledHandler;
    }
    
    // Used when game object is destroyed and inactive in the scene
    void OnDisable()
    {
        EnemyDeath.OnEnemyKilled -= OnEnemyKilledHandler;
    }

    // Start the Quest
    public void StartQuest(Quest quest)
    {
        currentQuest = quest;
        UIManager.Instance.ShowQuestWindow(quest);
    }

    // This method checks for active quests and their completion
    private void OnEnemyKilledHandler()
    {
        if (currentQuest != null && !currentQuest.isCompleted)
        {
            //currentQuest.isCompleted = true;
            CompleteQuest();
        }
    }

    // Manages the quest completion process
    public void CompleteQuest()
    {
        if (currentQuest != null && !currentQuest.isCompleted)
        {
            // Marks the quest as complete
            currentQuest.isCompleted = true;
            // Adds coins to player coin balance
            PlayerAccount.Instance.AddCoins(currentQuest.reward);
            UIManager.Instance.HideQuestWindow();
            // Hides Quest Window UI
            Debug.Log($"Quest completed: {currentQuest.description}. Reward: {currentQuest.reward} coins.");
        }
    }

    // Returns the status regarding the quest completion
    public bool IsQuestCompleted()
    {
        return currentQuest != null && currentQuest.isCompleted;
    }
}