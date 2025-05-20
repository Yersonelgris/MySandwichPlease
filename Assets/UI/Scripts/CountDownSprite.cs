using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountDownSprite : MonoBehaviour
{
    public Image countdownImage;
    public Sprite[] countdownSprites;
    public float scaleDuration = 0.3f;

    public Action OnCountdownFinished; // <- NUEVO

    public void StartCountdown()
    {
        if (countdownSprites == null || countdownSprites.Length == 0)
        {
            Debug.LogError("No hay sprites asignados al countdown.");
            return;
        }

        if (countdownImage == null)
        {
            Debug.LogError("No hay imagen asignada para mostrar el countdown.");
            return;
        }

        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        Time.timeScale = 0;
        countdownImage.gameObject.SetActive(true);

        foreach (Sprite sprite in countdownSprites)
        {
            if (sprite == null) continue;

            countdownImage.sprite = sprite;
            countdownImage.SetNativeSize();
            countdownImage.transform.localScale = Vector3.zero;

            float t = 0f;
            while (t < scaleDuration)
            {
                float scale = Mathf.Lerp(0f, 0.5f, t / scaleDuration);
                countdownImage.transform.localScale = new Vector3(scale, scale, scale);
                t += Time.unscaledDeltaTime;
                yield return null;
            }

            t = 0f;
            while (t < scaleDuration)
            {
                float scale = Mathf.Lerp(0.5f, 0f, t / scaleDuration);
                countdownImage.transform.localScale = new Vector3(scale, scale, scale);
                t += Time.unscaledDeltaTime;
                yield return null;
            }

            yield return new WaitForSecondsRealtime(0.1f);
        }

        countdownImage.gameObject.SetActive(false);

        OnCountdownFinished?.Invoke(); // <- Aquí llamamos el evento
    }
}
