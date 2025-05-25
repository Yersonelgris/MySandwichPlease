using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    public AudioSource musicSource;
    private bool isMusicOn = true;
    public Sprite soundOnIcon;
    public Sprite soundOffIcon;
    private Image buttonImage;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        UpdateIcon();
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        musicSource.mute = !isMusicOn;
        UpdateIcon();
    }

    void UpdateIcon()
    {
        if (buttonImage != null && soundOnIcon != null && soundOffIcon != null)
        {
            buttonImage.sprite = isMusicOn ? soundOnIcon : soundOffIcon;
        }
    }
}
