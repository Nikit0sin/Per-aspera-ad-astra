using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DeathPit : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private AudioClip fallSound; // Звук падения в яму
    [SerializeField] private bool instantKill = true; // Мгновенная смерть или нанесение урона
    [SerializeField] private float damageAmount = 100f; // Если не instantKill

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Health playerHealth = collision.GetComponent<Health>();
        if (playerHealth == null) return;

        // Воспроизводим звук падения
        if (fallSound != null)
        {
            SoundManager.instance?.PlaySound(fallSound);
        }

        // Наносим урон или мгновенно убиваем
        if (instantKill)
        {
            playerHealth.TakeDamage(playerHealth.currentHealth);
        }
        else
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }
}