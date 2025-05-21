using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HealthCollectable : MonoBehaviour
{
    [SerializeField] private float healthValue;
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        var health = collision.GetComponent<Health>();
        if (health == null) return;

        SoundManager.instance?.PlaySound(pickupSound);
        health.AddHealth(healthValue);
        gameObject.SetActive(false);
    }
}