using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public Dialogue dialogue;
    public NPCDialogue npcDialogue;
    public GameObject[] bubblePrefabs;
    public ShotGenerator shotGenerator;

    void Start()
    {
        StartingDialogue();
        Invoke("StartLevel", 1.0f);
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

        StartCoroutine(SpawnBubbles());

        shotGenerator.SpawnKeys();
    }

    IEnumerator SpawnBubbles()
    {
        GameObject[] spawnedBubbles = new GameObject[bubblePrefabs.Length];

        for (int i = 0; i < bubblePrefabs.Length; ++i)
        {
            spawnedBubbles[i] = Instantiate(bubblePrefabs[i]);
        }

        float currentTime = 0.0f;
        float duration = 2.0f;
        float targetAlpha = 245.0f / 255.0f; //end result not fully solid

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            float progress = currentTime / duration; //calculate percentage of fade
            float finalAlpha = progress * targetAlpha;

            foreach (GameObject bubble in spawnedBubbles)
            {
                if (bubble != null)
                {
                    SpriteRenderer sr = bubble.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        Color color = sr.color;
                        color.a = finalAlpha;
                        sr.color = color;
                    }
                }
            }
            yield return null;
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
