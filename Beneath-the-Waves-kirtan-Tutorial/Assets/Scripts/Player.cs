using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float MAXHP = 100f;
    public float HP;
    public float chipSpeed = 2f;

    private float lerpTimer;
    public UnityEngine.UI.Image frontHealthBar;
    public UnityEngine.UI.Image backHealthBar;

    public GameObject DeathMsg;

    public bool canTakeDamage = false;
    public float damageCooldown = 1.5f;

    public bool isPDead;
    public GameObject bloodScreen;

    public AudioClip hurtSound;
    public AudioSource src;

    private GameController gameController;

    private Transform spawnPoint;

    public void Start()
    {
        HP = MAXHP;
        gameController = FindObjectOfType<GameController>();
        if (gameController == null)
        {
            Debug.LogError("GameController not found in the scene.");
        }

        spawnPoint = GameObject.Find("SpawnPoint").transform;
        if (spawnPoint == null)
        {
            Debug.LogError("SpawnPoint not found in the scene.");
        }
        else
        {
            transform.position = spawnPoint.position;
            Debug.Log($"Player spawned at: {transform.position}");
        }

        StartCoroutine(CheckInitialization());

        //testing
        GetComponent<MouseLook>().enabled = false;
        GetComponent<PlayerMove>().enabled = false;

        // Re-enable MouseLook script and test
        StartCoroutine(EnableComponentAfterDelay<MouseLook>(2f));

        // Then re-enable PlayerMove script and test
        StartCoroutine(EnableComponentAfterDelay<PlayerMove>(4f));
    }

    private IEnumerator EnableComponentAfterDelay<T>(float delay) where T : MonoBehaviour
    {
        yield return new WaitForSeconds(delay);
        GetComponent<T>().enabled = true;
        Debug.Log($"{typeof(T).Name} enabled.");
    }
    void Update()
    {
        HP = Mathf.Clamp(HP, 0, MAXHP);
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = HP / MAXHP;
        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.grey;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);
        }
    }

    public void takeDamage(float damage)
    {
        src.clip = hurtSound;
        src.Play();

        if (!canTakeDamage) return;
        HP -= damage;

        if (HP <= 0f)
        {
            print("YOU ARE DEAD!");
            PlayerDead();
            isPDead = true;
        }
        else
        {
            print("HIT!");
            StartCoroutine(BloodyScreen());
        }

        StartCoroutine(DamageCooldown());
        lerpTimer = 0f;
    }

    private IEnumerator BloodyScreen()
    {
        if (bloodScreen.activeInHierarchy == false)
        {
            bloodScreen.SetActive(true);
        }

        var image = bloodScreen.GetComponentInChildren<UnityEngine.UI.Image>();
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        if (bloodScreen.activeInHierarchy)
        {
            bloodScreen.SetActive(false);
        }
    }

    public void healDamage(float heal)
    {
        HP += heal;
        if (HP > MAXHP)
        {
            HP = MAXHP;
        }
        lerpTimer = 0f;
    }

    public float getHP()
    {
        return HP;
    }

    public float getMAXHP()
    {
        return MAXHP;
    }

    private IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    private void PlayerDead()
    {
        GetComponent<MouseLook>().enabled = false;
        GetComponent<PlayerMove>().enabled = false;

        GetComponentInChildren<Animator>().enabled = true;

        GetComponent<DeathScreen>().StartFade();

        StartCoroutine(ShowDeathMsg());
    }

    private IEnumerator ShowDeathMsg()
    {
        yield return new WaitForSeconds(1f);
        DeathMsg.gameObject.SetActive(true);
    }

    private IEnumerator CheckInitialization()
    {
        yield return new WaitForSeconds(2f);

        if (transform.position == Vector3.zero)
        {
            Debug.Log("Player is stuck at (0,0,0), setting to spawn position");
            transform.position = spawnPoint.position;
        }
        Debug.Log($"Player position after CheckInitialization: {transform.position}");
    }
}
