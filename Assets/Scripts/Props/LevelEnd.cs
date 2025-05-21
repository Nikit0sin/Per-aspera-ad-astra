using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string playerTag = "Player"; // ��� ������� ������
    [SerializeField] private AudioClip levelCompleteSound; // ���� ���������� ������
    [SerializeField] private float loadDelay = 1f; // �������� ����� ���������
    
    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ��������� ��� �������� ����� � ������� ��� �� �����������
        if (!isActivated && collision.CompareTag(playerTag))
        {
            ActivateLevelEnd();
        }
    }

    private void ActivateLevelEnd()
    {
        isActivated = true;

        if (levelCompleteSound != null)
            AudioSource.PlayClipAtPoint(levelCompleteSound, transform.position);

        // ��������� ���������
        GetComponent<Collider2D>().enabled = false;

        // ��������� ��������� ������� � ���������
        Invoke(nameof(LoadNextLevel), loadDelay);
    }

    private void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // ��������� ���������� �� ��������� �����
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // ���� ���� ������ ��� - ���������� � ������� ����
            SceneManager.LoadScene(0);
            Debug.Log("No more levels! Returning to main menu.");
        }
    }

    // ��� ������� - ������������ �������� � ���������
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, GetComponent<Collider2D>().bounds.size);
    }
}