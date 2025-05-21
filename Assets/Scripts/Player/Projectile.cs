using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private string[] ignoreTags = { "Trap", "Untagged" }; // Теги, которые нужно игнорировать
    private bool hit;
    private float direction;
    private float lifetime;

    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 5)
            Deactivate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, нужно ли игнорировать этот объект
        if (ShouldIgnoreCollision(collision))
            return;

        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("explode");

        // Проверяем, что это враг (а не ловушка)
        if (collision.CompareTag("Enemy") && collision.TryGetComponent<Health>(out var health))
        {
            health.TakeDamage(1);
        }
    }

    private bool ShouldIgnoreCollision(Collider2D collision)
    {
        // Проверяем все теги, которые нужно игнорировать
        foreach (var tag in ignoreTags)
        {
            if (collision.CompareTag(tag))
                return true;
        }
        return false;
    }

    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}