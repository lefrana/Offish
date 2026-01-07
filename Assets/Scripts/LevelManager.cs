using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject[] bubblePrefabs;
    public ShotGenerator shotGenerator;

    void Start()
    {
        StartLevel();
    }

    void StartLevel()
    {
        dialogue.SetDialogue(new string[]
        {
            "testing level start",
            "testing second dialogue line",
            "テストにさんハッピーはっぴあああああああああああああああ?!",
        });

        SpawnBubbles();

        shotGenerator.SpawnKeys();
    }

    void SpawnBubbles()
    {
        for (int i = 0; i < bubblePrefabs.Length; ++i)
        {
            Instantiate(bubblePrefabs[i]);
        }
    }
}
