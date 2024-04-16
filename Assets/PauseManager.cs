using UnityEngine;

public class PauseManager 
    : MonoBehaviour {

    public Canvas canvas;
    public Canvas canvas2;
    private bool funFactor = false;
    private bool light = false;
    private bool isPaused = false;
    void Start () {
        canvas.enabled = isPaused;
        canvas2.enabled = isPaused;
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
        if (isPaused && funFactor)
        {
            canvas2.enabled = light;
            if (light)            
                light = false;            
            else 
                light = true;
        }
    }

    void PauseGame() {
        Time.timeScale = 0;
        isPaused = true;
        canvas.enabled = true;
        if (Random.Range(0, 20) == 1) 
            funFactor = true;
    }
    void ResumeGame() {
        Time.timeScale = 1;
        isPaused = false;
        canvas.enabled = false;
        canvas2.enabled = false;
        funFactor = false;
    }
}
