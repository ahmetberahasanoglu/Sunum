using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject winPanel;
    public bool gameEnded = false;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void EndGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("OYUN BİTTİ - KAZANDIN");
        
        // Paneli aç
        if(winPanel != null) winPanel.SetActive(true);

        // Mouse'u serbest bırak (Tıklayabilmek için)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Oyunu süper yavaşlat (Sinematik bitiş)
        Time.timeScale = 0.1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Zamanı düzelt
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}