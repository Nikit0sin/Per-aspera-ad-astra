using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string playerTag = "Player"; // Тег объекта игрока
    [SerializeField] private AudioClip levelCompleteSound; // Звук завершения уровня
    [SerializeField] private float loadDelay = 1f; // Задержка перед загрузкой
    
    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем что коснулся игрок и триггер еще не активирован
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

        // Отключаем коллайдер
        GetComponent<Collider2D>().enabled = false;

        // Загружаем следующий уровень с задержкой
        Invoke(nameof(LoadNextLevel), loadDelay);
    }

    private void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Проверяем существует ли следующая сцена
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Если сцен больше нет - возвращаем в главное меню
            SceneManager.LoadScene(0);
            Debug.Log("No more levels! Returning to main menu.");
        }
    }

    // Для отладки - визуализация триггера в редакторе
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, GetComponent<Collider2D>().bounds.size);
    }
}