using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ButtonPlay : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Pause ();
    }
    public void Play ()
    {
        Time.timeScale = 1;
        Debug.Log("Juego reanudado: " + (Time.timeScale == 1));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Pause()
    {
    Time.timeScale = 0;
    Debug.Log("Juego pausado: " + (Time.timeScale == 0));
    }

}
