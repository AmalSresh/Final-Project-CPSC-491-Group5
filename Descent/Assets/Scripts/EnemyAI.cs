using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f;
    public int baseDamage = 1;
    public float damageCooldown = 1f;

    private int currentDamage; 
    private Transform player;
    private float lastDamageTime;
    private Rigidbody2D rb;

    private SpriteRenderer SpriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();

        int currentLevel = 1; 
        if (GameManager.Instance != null)
        {
            currentLevel = GameManager.Instance.level;
        }

        currentDamage = baseDamage + (currentLevel - 1);

        currentDamage = Mathf.Max(baseDamage, currentDamage);

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 direction = ((Vector2)player.position - rb.position).normalized;
        Vector2 newPosition = rb.position + direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);

        if (direction.x > 0.1f)
        {
            SpriteRenderer.flipX = false;
        }
        else if (direction.x < -0.1f)
        {
            SpriteRenderer.flipX = true;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    // 3. Deal the exact integer damage
                    playerHealth.TakeDamage(currentDamage);
                    lastDamageTime = Time.time;
                }
            }
        }
    }
}