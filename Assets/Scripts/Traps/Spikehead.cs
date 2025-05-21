using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Spikehead : EnemyDamage
{
    [Header("Movement Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask obstacleLayers;
    [SerializeField] private LayerMask playerLayer;

    [Header("SFX")]
    [SerializeField] private AudioClip impactSound;

    private readonly Vector3[] directions = new Vector3[4];
    private Vector3 destination;
    private float checkTimer;
    private bool attacking;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ConfigureRigidbody();
        CalculateDirections();
    }

    private void ConfigureRigidbody()
    {
        rb.isKinematic = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Update()
    {
        if (attacking)
        {
            MoveWithCollisionCheck();
        }
        else
        {
            UpdateCheckTimer();
        }
    }

    private void UpdateCheckTimer()
    {
        checkTimer += Time.deltaTime;
        if (checkTimer >= checkDelay)
        {
            CheckForPlayer();
            checkTimer = 0f;
        }
    }

    private void CalculateDirections()
    {
        directions[0] = Vector3.right;
        directions[1] = Vector3.left;
        directions[2] = Vector3.up;
        directions[3] = Vector3.down;
    }

    private void CheckForPlayer()
    {
        for (int i = 0; i < directions.Length; i++)
        {
            if (CheckDirection(directions[i]))
            {
                attacking = true;
                destination = directions[i];
                return;
            }
        }
    }

    private bool CheckDirection(Vector3 direction)
    {
        var hit = Physics2D.Raycast(transform.position, direction, range, playerLayer);
        return hit.collider != null && !attacking;
    }

    private void MoveWithCollisionCheck()
    {
        float moveDistance = speed * Time.deltaTime;
        float checkDistance = moveDistance + 0.1f;

        if (!HasObstacleAhead(checkDistance))
        {
            transform.Translate(destination * moveDistance);
        }
    }

    private bool HasObstacleAhead(float distance)
    {
        var hit = Physics2D.Raycast(transform.position, destination, distance, obstacleLayers);
        if (hit.collider != null)
        {
            transform.position = hit.point - (Vector2)(destination * 0.1f);
            Stop();
            return true;
        }
        return false;
    }

    private void Stop()
    {
        attacking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsPlayer(collision.gameObject))
        {
            PlayImpactSound();
            base.OnTriggerEnter2D(collision.collider);
        }
        Stop();
    }

    private bool IsPlayer(GameObject obj)
    {
        return ((1 << obj.layer) & playerLayer) != 0;
    }

    private void PlayImpactSound()
    {
        if (SoundManager.instance != null && impactSound != null)
        {
            SoundManager.instance.PlaySound(impactSound);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 dir in directions)
        {
            if (dir != Vector3.zero)
            {
                Gizmos.DrawRay(transform.position, dir * range);
            }
        }
    }
}