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
        // Clear lists immediately
        activeBubbles.Clear();
        activeKeys.Clear();

        npcDialogue.HideNPC();

        // Delay the dialogue slightly so the UI can reset from Level 1
        Invoke("StartingDialogue", 0.1f);
        Invoke("StartLevel", 1.0f);
    }

    void StartingDialogue()
    {
        dialogue.SetDialogue(new string[]
        {
            "testing level 22222",
        });
    }

    void StartLevel()
    {

        npcDialogue.SetDialogue(new string[]
        {
            "testing NPC dialogue",
        });

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
                dialogue.SetDialogue(new string[] { "Correct" });
                Invoke("NextLevel", 2.0f);
                break;

            case BubbleType.False1:
                dialogue.SetDialogue(new string[] { "False1" });

                StartCoroutine(WrongAnswerReset(chosenBubble));
                break;

            case BubbleType.False2:
                dialogue.SetDialogue(new string[] { "False2" });

                StartCoroutine(WrongAnswerReset(chosenBubble));
                break;

            default:
                dialogue.SetDialogue(new string[] { "False3" });

                StartCoroutine(WrongAnswerReset(chosenBubble));
                break;
        }
    }

    IEnumerator WrongAnswerReset(GameObject chosenBubble)
    {

        shotGenerator.shotSpawned = true;

        yield return new WaitForSeconds(0.1f);

        foreach (GameObject b in activeBubbles)
        {
            // If it's NOT the bubble we just shot, make it invisible
            if (b != null && b != chosenBubble)
            {
                SetAlpha(b, 0f);
            }
        }

        GameObject[] keys = GameObject.FindGameObjectsWithTag("Arrows");
        foreach (GameObject k in keys)
        {
            Destroy(k);
        }

        //freeze the bubble for a bit before destroying it
        yield return new WaitForSeconds(2.0f);

        if (chosenBubble != null)
        {
            activeBubbles.Remove(chosenBubble);

            // Physically delete it from the scene
            Destroy(chosenBubble);
        }

        StartCoroutine(SpawnObjects());
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
