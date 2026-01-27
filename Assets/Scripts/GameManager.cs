using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] levels;
    private int currentLevelIndex = 0;

    void Awake()
    {
        //all level is off at the start
        foreach (GameObject lvl in levels)
        {
            if (lvl != null)
            {
                lvl.SetActive(false);
            }
        }
    }

    void Start()
    {
        // Turn on ONLY Level 1
        if (levels.Length > 0 && levels[0] != null)
        {
            levels[0].SetActive(true);
            currentLevelIndex = 0;
        }
    }

    public void MoveToNextLevel()
    {
        // 1. Turn off the current level child completely
        levels[currentLevelIndex].SetActive(false);

        // 2. Move to next index
        currentLevelIndex++;

        // 3. Turn on the next level child
        if (currentLevelIndex < levels.Length)
        {
            levels[currentLevelIndex].SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("EndScene");
            Debug.Log("All Levels Cleared!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            MoveToNextLevel();
        }

        // If you press 'E', it jumps directly to the Ending Scene
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("EndScene");
        }
    }
}