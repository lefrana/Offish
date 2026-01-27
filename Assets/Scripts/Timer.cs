using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class Timer : MonoBehaviour
{
    public static Timer instance;
    public static bool isTimeUp = false;

    [SerializeField] TextMeshProUGUI timerText;
    public float remainingTime;
    private bool isTimerRunning = false;
    private bool isEnding = false; // Prevents the scene from loading multiple times

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (isTimerRunning && remainingTime > 0.0f)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime <= 0.0f && !isEnding)
        {
            // Time is up!
            remainingTime = 0.0f;
            isEnding = true;
            StartCoroutine(GoToEndScene());
        }

        // Update the text display
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator GoToEndScene()
    {
        // Set this to true because the timer ran out!
        isTimeUp = true;

        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("EndScene");
    }

    // Inside Timer.cs

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }
}