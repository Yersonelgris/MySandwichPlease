using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseController : MonoBehaviour
{
    public TMP_Text buttonText;
    public Button pauseButton;
    public Button[] slotButtons;  // The buttons that get disabled when paused

    private bool isPaused = false;

    void Start()
    {
        // Make sure the game starts in the correct state
        // UpdateState();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        UpdateState();
    }

    private void UpdateState()
    {
        // Set game time
        Time.timeScale = isPaused ? 0 : 1;

        // Update button text
        if (buttonText != null)
        {
            buttonText.text = isPaused ? "Resume" : "Pause";
        }

        // Enable/disable slot buttons
        foreach (var button in slotButtons)
        {
            button.interactable = !isPaused;
        }
    }
}
