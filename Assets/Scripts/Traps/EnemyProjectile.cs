using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    private BoxCollider2D coll;
    private bool hit;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        coll.enabled = true;
    }

    private void Update()
    {
        if (hit) return;

        transform.Translate(speed * Time.deltaTime, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            Deactivate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        base.OnTriggerEnter2D(collision);
        coll.enabled = false;
        Deactivate();
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}