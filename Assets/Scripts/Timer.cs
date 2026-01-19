using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // This allows us to change scenes

public class Timer : MonoBehaviour
{
    public static Timer instance;

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

    System.Collections.IEnumerator GoToEndScene()
    {
        yield return new WaitForSeconds(1.0f); // Wait for 1 second
        SceneManager.LoadScene("EndScene");   // Change "EndScene" to your actual scene name
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