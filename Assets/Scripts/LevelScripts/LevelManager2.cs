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

    public CanvasGroup levelFade; //for level transition

    void OnEnable()
    {
        activeBubbles.Clear();
        activeKeys.Clear();

        dialogue.SetDialogue(new string[] { "" });
        npcDialogue.HideNPC();

        //start starting dialogue sequence
        StartCoroutine(StartingSequence());
    }

    IEnumerator StartingSequence()
    {
        //hide npc at start
        npcDialogue.HideNPC();

        if (levelFade != null)
        {
            float elapsed = 0f;
            float duration = 2.0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                levelFade.alpha = 1f - (elapsed / duration);
                yield return null;
            }
            levelFade.alpha = 0f;
        }

        //start dialogue sequence
        yield return StartCoroutine(DialogueSequence());

        //pause before starting level
        yield return new WaitForSeconds(0.5f);

        StartLevel();
    }

    IEnumerator DialogueSequence()
    {
        dialogue.SetDialogue(new string[]
        {
        "<i>（仕事の経験を話せばいいんだな。。）</i>"
        });

        while (dialogue.gameObject.activeSelf)
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

        //start timer
        if (Timer.instance != null)
        {
            Timer.instance.StartTimer();
        }
    }

    public void CheckAnswer(BubbleType type, GameObject chosenBubble)
    {
        switch (type)
        {
            case BubbleType.Correct:
                StartCoroutine(AnswerSequence(
                    new string[] { "この分野で5年ほど働いています。" },
                    new string[] { "それは結構長いですね！" },
                    true, chosenBubble));
                break;

            case BubbleType.False1:
                StartCoroutine(AnswerSequence(
                    new string[] { "20歳です。" },
                    new string[] { "。。それで？" },
                    false, chosenBubble));
                break;

            case BubbleType.False2:
                StartCoroutine(AnswerSequence(
                    new string[] { "まあ、けっこう長く働いてます。" },
                    new string[] { "長くって、どれくらいですか？" },
                    false, chosenBubble));
                break;

            default:
                StartCoroutine(AnswerSequence(
                    new string[] { "5回くらい働きました。" },
                    new string[] { "そんな回数でいいなら、その仕事教えてほしいです。" },
                    false, chosenBubble));
                break;
        }
    }

    IEnumerator AnswerSequence(string[] mainLines, string[] npcLines, bool isCorrect, GameObject chosenBubble)
    {
        shotGenerator.shotSpawned = true;

        if (Timer.instance != null)
        {
            Timer.instance.StopTimer(); //pause timer
        }

        //hide other bubbles and arrows
        foreach (GameObject b in activeBubbles)
        {
            if (b != null && b != chosenBubble)
            {
                SetAlpha(b, 0f);
            }
        }

        //destroy keys to reset
        GameObject[] keys = GameObject.FindGameObjectsWithTag("Arrows");
        foreach (GameObject k in keys)
        {
            if (k != null)
            {
                Destroy(k);
            }
        }
        activeKeys.Clear();

        //answer dialogue starts
        dialogue.SetDialogue(mainLines);
        while (dialogue.gameObject.activeSelf)
        {
            yield return null;
        }

        npcDialogue.SetDialogue(npcLines);
        while (npcDialogue.gameObject.activeSelf)
        {
            yield return null;
        }

        //prepare for next round when dialogue is done
        shotGenerator.isWaitingForDialogue = false; //reset keys

        //check answer
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
    IEnumerator FadeToNextLevel()
    {
        // --- WHITE FADE OUT (Clear to White) ---
        if (levelFade != null)
        {
            float elapsed = 0f;
            float duration = 1.0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                levelFade.alpha = elapsed / duration;
                yield return null;
            }
            levelFade.alpha = 1f; // Screen is now fully white
        }

        // Standard cleanup
        if (shotGenerator != null) shotGenerator.ResetShotGenerator();
        foreach (GameObject b in activeBubbles) { if (b != null) Destroy(b); }
        activeBubbles.Clear();

        // Move to next level
        GameManager gm = GetComponentInParent<GameManager>();
        if (gm != null) gm.MoveToNextLevel();
    }
    void NextLevel()
    {
        StartCoroutine(FadeToNextLevel());
    }
}
