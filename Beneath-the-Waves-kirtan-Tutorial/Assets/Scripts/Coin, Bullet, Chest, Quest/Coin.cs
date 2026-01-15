using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerAccount.Instance.AddCoins(value);  // Access via Singleton Instance
            Destroy(gameObject);
        }
    }
}