using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LevelManager2 : MonoBehaviour
{
    public Dialogue dialogue;
    public NPCDialogue npcDialogue;
    public GameObject[] bubblePrefabs;
    public ShotGenerator shotGenerator;

    private List<GameObject> activeBubbles = new List<GameObject>();
    private List<GameObject> activeKeys = new List<GameObject>();

    void OnEnable()
    {
        activeBubbles.Clear();
        activeKeys.Clear();

        dialogue.SetDialogue(new string[] { "" });
        npcDialogue.HideNPC();

        // Instead of Invoke, let the dialogue sequence trigger the level start
        StartCoroutine(StartingSequence());
    }

    IEnumerator StartingSequence()
    {
        // Ensure NPC is hidden at the absolute start
        npcDialogue.HideNPC();

        // Wait for the full conversation to finish
        yield return StartCoroutine(DialogueSequence());

        // Give a tiny breather before bubbles appear
        yield return new WaitForSeconds(0.5f);

        StartLevel();
    }

    IEnumerator DialogueSequence()
    {
        // 1. Start Fish Dialogue
        dialogue.SetDialogue(new string[]
        {
        "testing level2222 start",
        "testing second dialogue line",
        "テストにさんハッピーはっぴあああああああああああああああ?!"
        });

        // Wait for Fish to finish
        while (dialogue.gameObject.activeSelf)
        {
            yield return null;
        }

        // 2. Small pause between speakers
        yield return new WaitForSeconds(0.6f);

        // 3. Start NPC Dialogue
        npcDialogue.SetDialogue(new string[]
        {
        "testing NPC dialogue",
        });

        // Wait for NPC to finish
        while (npcDialogue.gameObject.activeSelf)
        {
            yield return null;
        }
    }

    void StartLevel()
    {
        foreach (GameObject prefab in bubblePrefabs)
        {
            GameObject b = Instantiate(prefab);
            activeBubbles.Add(b);
        }

        StartCoroutine(SpawnObjects());

        //shotGenerator.SpawnKeys();
    }

    IEnumerator SpawnObjects()
    {
        //input can start
        shotGenerator.shotSpawned = true;

        List<GameObject> allObjects = new List<GameObject>(activeBubbles);

        //spawn keys
        GameObject[] keys = shotGenerator.SpawnKeys();
        if (keys != null)
        {
            foreach (GameObject k in keys)
            {
                if (k != null)
                {
                    allObjects.Add(k);
                    activeKeys.Add(k);
                }
            }
        }

        //initial transparency for bubbles and keys
        foreach (GameObject obj in allObjects)
        {
            if (obj != null)
            {
                SetAlpha(obj, 0f);
            }
        }

        //fading in
        float currentTime = 0.0f;
        float duration = 1.5f;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float alpha = (currentTime / duration) * (245f / 255f);

            foreach (GameObject obj in allObjects)
            {
                if (obj != null)
                {
                    SetAlpha(obj, alpha);
                }
            }
            yield return null;
        }

        shotGenerator.shotSpawned = false;
        //Debug.Log("Respawn Complete - Game Unlocked");
    }

    public void CheckAnswer(BubbleType type, GameObject chosenBubble)
    {
        switch (type)
        {
            case BubbleType.Correct:
                StartCoroutine(AnswerSequence(
                    new string[] { "Correct!" },
                    new string[] { "Great job, let's move on!" },
                    true, chosenBubble));
                break;

            case BubbleType.False1:
                StartCoroutine(AnswerSequence(
                    new string[] { "False 1..." },
                    new string[] { "That wasn't the right choice." },
                    false, chosenBubble));
                break;

            case BubbleType.False2:
                StartCoroutine(AnswerSequence(
                    new string[] { "False 2..." },
                    new string[] { "Maybe try a different bubble?" },
                    false, chosenBubble));
                break;

            default:
                StartCoroutine(AnswerSequence(
                    new string[] { "Not quite." },
                    new string[] { "Don't give up!" },
                    false, chosenBubble));
                break;
        }
    }

    IEnumerator AnswerSequence(string[] mainLines, string[] npcLines, bool isCorrect, GameObject chosenBubble)
    {
        shotGenerator.shotSpawned = true;

        if (!isCorrect)
        {
            // 1. Hide bubbles and destroy arrows immediately
            foreach (GameObject b in activeBubbles)
            {
                if (b != null && b != chosenBubble) SetAlpha(b, 0f);
            }

            // Clean up the current arrows so they disappear
            GameObject[] keys = GameObject.FindGameObjectsWithTag("Arrows");
            foreach (GameObject k in keys) { if (k != null) Destroy(k); }
            activeKeys.Clear();
        }

        // 2. Dialogues play...
        dialogue.SetDialogue(mainLines);
        while (dialogue.gameObject.activeSelf) yield return null;

        npcDialogue.SetDialogue(npcLines);
        while (npcDialogue.gameObject.activeSelf) yield return null;

        // 3. DIALOGUE IS DONE - PREPARE FOR NEXT ROUND
        shotGenerator.isWaitingForDialogue = false; // Allow keys to spawn again

        if (isCorrect)
        {
            NextLevel();
        }
        else
        {
            if (chosenBubble != null)
            {
                activeBubbles.Remove(chosenBubble);
                Destroy(chosenBubble);
            }
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SpawnObjects());
        }
    }


    void SetAlpha(GameObject obj, float alpha)
    {
        if (obj == null) return;
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }

        //for the text inside the bubble
        TMPro.TextMeshPro text = obj.GetComponentInChildren<TMPro.TextMeshPro>();
        if (text != null)
        {
            Color tc = text.color;
            tc.a = alpha;
            text.color = tc;
        }
    }

    void NextLevel()
    {
        // 1. Tell ShotGenerator to clear the air
        if (shotGenerator != null)
        {
            shotGenerator.ResetShotGenerator();
        }

        // 2. Clear bubbles and anything else
        foreach (GameObject b in activeBubbles) { if (b != null) Destroy(b); }
        activeBubbles.Clear();

        // 3. Move to next level
        GameManager gm = GetComponentInParent<GameManager>();
        if (gm != null)
        {
            gm.MoveToNextLevel();
        }
    }
}
