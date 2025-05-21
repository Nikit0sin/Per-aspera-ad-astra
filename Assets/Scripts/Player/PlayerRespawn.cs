using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uiManager;
    private Rigidbody2D rb;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void CheckRespawn()
    {
        if (currentCheckpoint == null)
        {
            uiManager.GameOver();
            return;
        }

        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        // ���������� ������ ���������
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // ������������� � ���������
        transform.position = currentCheckpoint.position;

        // ��������������� ��������
        playerHealth.Respawn();

        // ���������� ������
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);

        // ��������� �������� ��� ������������
        yield return new WaitForEndOfFrame();

        // ������������ ���������� (���� ������������)
        GetComponent<PlayerMovement>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            currentCheckpoint = collision.transform;
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("Appear");
        }
    }
}