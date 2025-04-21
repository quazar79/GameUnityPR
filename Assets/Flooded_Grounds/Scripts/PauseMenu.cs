using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public static bool isPaused = false;
    public static PauseMenu instance;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Awake()
    {
        instance = this;
    }
    void Update()
    {
        Debug.Log($"Cursor.lockState = {Cursor.lockState}, isPaused = {isPaused}");

        
    }

    


    public void LoadMainMenu()
    {
        
        
    }

    public void QuitGame()
    {
        
    }
}
