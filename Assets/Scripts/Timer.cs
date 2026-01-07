using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float remainingTime;

    void Start()
    {
        remainingTime = 180.0f;
    }

    void Update()
    {
        if (remainingTime > 0.0f)
        {
            remainingTime -= Time.deltaTime;
        }
        else
        {
            remainingTime = 0.0f;
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
