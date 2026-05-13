using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Stats")]
    public int damage = 1;
    public float attackRange = 2f;

    [Header("Attack Keys")]
    public KeyCode primaryAttackKey = KeyCode.E;
    public KeyCode secondaryAttackKey = KeyCode.Space;

    // play hit sound
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(primaryAttackKey) || Input.GetKeyDown(secondaryAttackKey))
        {
            Attack();
        }
    }

    void Attack()
    {
        EnemyHealth[] enemies = Object.FindObjectsByType<EnemyHealth>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );

        // check to see if enemy was hit
        bool hitAnything = false;

        foreach (EnemyHealth enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance <= attackRange)
            {
                Debug.Log("Hit enemy: " + enemy.name + " for " + damage + " damage.");
                enemy.TakeDamage(damage);
                hitAnything = true;
            }
        }
        //play sound if condition is true
        if (hitAnything && audioSource != null)
        {
            audioSource.Play();
        }
    }

    public void IncreaseDamage(int amount)
    {
        damage += amount;
        Debug.Log("Damage increased! New damage: " + damage);
    }

    public void IncreaseRange(float amount)
    {
        attackRange += amount;
        Debug.Log("Attack range increased! New range: " + attackRange);
    }
}