using UnityEngine;

public class PauseManager : MonoBehaviour {
    private bool isPaused = false;

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            if (isPaused)
                ResumeGame();            
            else            
                PauseGame();            
        }
    }

    void PauseGame() {
        Time.timeScale = 0;
        isPaused = true;        
    }
    void ResumeGame() {
        Time.timeScale = 1;
        isPaused = false;        
    }
}
