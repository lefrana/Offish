using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Dialogue dialogue;
    public NPCDialogue npcDialogue;
    public GameObject[] bubblePrefabs;
    public ShotGenerator shotGenerator;

    void Start()
    {
        StartingDialogue();
        Invoke("StartLevel", 5.0f);
    }

    void StartingDialogue()
    {
        dialogue.SetDialogue(new string[]
        {
            "testing level start",
            "testing second dialogue line",
            "テストにさんハッピーはっぴあああああああああああああああ?!",
        });
    }

    void StartLevel()
    {

        npcDialogue.SetDialogue(new string[]
        {
            "testing NPC dialogue",
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

    public void CheckAnswer(BubbleType type)
    {
        switch(type)
        {
            case BubbleType.Correct:
                dialogue.SetDialogue(new string[] { "Correct" });

                //load next level after 2 seconds
                //Invoke("LoadNextLevel", 2.0f);
                break;

            case BubbleType.False1:
                dialogue.SetDialogue(new string[] { "False1" });
                break;

            case BubbleType.False2:
                dialogue.SetDialogue(new string[] { "False2" });
                break;

            default:
                dialogue.SetDialogue(new string[] { "False3" });
                break;
        }
    }

    void LoadNextLevel()
    {
        //// This calculates the next level index in your Build Settings
        //int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        //// Check if there actually IS a next scene to avoid errors
        //if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        //{
        //    SceneManager.LoadScene(nextSceneIndex);
        //}
        //else
        //{
        //    dialogue.SetDialogue(new string[] { "You've finished all the stages!" });
        //}
    }
}
