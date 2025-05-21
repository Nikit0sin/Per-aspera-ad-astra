using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DeathPit : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private AudioClip fallSound; // ���� ������� � ���
    [SerializeField] private bool instantKill = true; // ���������� ������ ��� ��������� �����
    [SerializeField] private float damageAmount = 100f; // ���� �� instantKill

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Health playerHealth = collision.GetComponent<Health>();
        if (playerHealth == null) return;

        // ������������� ���� �������
        if (fallSound != null)
        {
            SoundManager.instance?.PlaySound(fallSound);
        }

        // ������� ���� ��� ��������� �������
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