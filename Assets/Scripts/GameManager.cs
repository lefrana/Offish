using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Drag the CHILD GameObjects (Level1, Level2) into this list
    public GameObject[] levels;
    private int currentLevelIndex = 0;

    void Awake()
    {
        // Force every level child to be OFF at the very start
        foreach (GameObject lvl in levels)
        {
            if (lvl != null) lvl.SetActive(false);
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
            Debug.Log("All Levels Cleared!");
        }
    }
}