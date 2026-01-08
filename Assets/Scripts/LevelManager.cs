using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using NUnit.Framework;

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

        StartCoroutine(SpawnObjects());

        //shotGenerator.SpawnKeys();
    }

    IEnumerator SpawnObjects()
    {
        List<GameObject> allObjects = new List<GameObject>();

        //GameObject[] spawnedBubbles = new GameObject[bubblePrefabs.Length];

        for (int i = 0; i < bubblePrefabs.Length; ++i)
        {
            GameObject bubble = Instantiate(bubblePrefabs[i]);
            allObjects.Add(bubble);
        }

        GameObject[] keys = shotGenerator.SpawnKeys();
        if (keys != null)
        {
            allObjects.AddRange(keys);
        }

        float currentTime = 0.0f;
        float duration = 2.0f;
        float targetAlpha = 245.0f / 255.0f; //end result not fully solid

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            float progress = currentTime / duration; //calculate percentage of fade
            float finalAlpha = progress * targetAlpha;

            foreach (GameObject obj in allObjects)
            {
                if (obj != null)
                {
                    SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
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
