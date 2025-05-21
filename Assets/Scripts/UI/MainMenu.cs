using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject mainMenuScreen; // Основной экран меню
    [SerializeField] private AudioClip buttonClickSound; // Звук клика кнопок

    private void Start()
    {
        // Активируем экран меню при старте
        if (mainMenuScreen != null)
        {
            mainMenuScreen.SetActive(true);
        }

        // Сбрасываем timescale на случай возврата из паузы
        Time.timeScale = 1;
    }

    public void StartGame()
    {
        PlayButtonSound();
        // Загружаем следующую сцену по порядку в Build Settings
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Проверяем существует ли следующая сцена
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Если следующей сцены нет, загружаем первую сцену (по кругу)
            SceneManager.LoadScene(0);
            Debug.LogWarning("No next scene available, loading first scene");
        }
    }

    public void QuitGame()
    {
        PlayButtonSound();
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void SoundVolume()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.ChangeSoundVolume(0.2f);
            PlayButtonSound();
        }
    }

    public void MusicVolume()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.ChangeMusicVolume(0.2f);
            PlayButtonSound();
        }
    }

    private void PlayButtonSound()
    {
        if (SoundManager.instance != null && buttonClickSound != null)
        {
            SoundManager.instance.PlaySound(buttonClickSound);
        }
    }
}