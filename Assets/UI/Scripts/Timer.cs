using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
   public float startTime = 10f; // Time in seconds
    private float remainingTime;
    public TextMeshProUGUI timerText; // Assign a TMPro Text component here

    void Start()
    {
        remainingTime = startTime;
    }

    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(remainingTime).ToString();
        }
        else
        {
            timerText.text = "Time's up!";
        }
    }
}
