using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    public Dialogue dialogue;
    public NPCDialogue npcDialogue;

    public GameObject gameOverObj;
    public GameObject gameClearObj;

    public CanvasGroup levelFade;

    // ADD THIS: Check this in the Inspector to test the Game Over version
    public bool isGameOver = false;

    void OnEnable()
    {
        // Look at the Timer script to see if the time ran out
        isGameOver = Timer.isTimeUp;
        Timer.isTimeUp = false;

        StartCoroutine(StartingSequence());
    }

    IEnumerator StartingSequence()
    {
        // 1. SETUP: Hide everything
        npcDialogue.HideNPC();
        gameOverObj.SetActive(false);
        gameClearObj.SetActive(false);
        dialogue.gameObject.SetActive(false);
        npcDialogue.gameObject.SetActive(false);

        // 2. FADE IN (White to Clear)
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

        // --- 3. THE FIX: ONLY PLAY DIALOGUE IF NOT GAME OVER ---
        if (!isGameOver)
        {
            yield return StartCoroutine(DialogueSequence());
        }
        else
        {
            // If it IS Game Over, just wait a moment in silence
            yield return new WaitForSeconds(1.0f);
        }

        yield return new WaitForSeconds(0.5f);

        // 4. FADE OUT (Clear back to White)
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
            levelFade.alpha = 1f;
        }

        yield return new WaitForSeconds(1.0f);

        // 5. THE CHOICE
        if (isGameOver)
        {
            gameOverObj.SetActive(true);
            SetAlpha(gameOverObj, 1f);
            gameClearObj.SetActive(false);

            yield return new WaitForSeconds(2.0f);

            SceneManager.LoadScene("TitleScene");
        }
        else
        {
            gameClearObj.SetActive(true);
            gameOverObj.SetActive(false);
            yield return StartCoroutine(FadeInSequence());
        }
    }

    IEnumerator DialogueSequence()
    {
        // Your original dialogue code stays the same
        npcDialogue.SetDialogue(new string[]
        {
            "では、本日の面接は以上です。",
            "とても良いお話が聞けました。良い結果につながると思います。",
            "ご連絡をお待ちください。",
        });

        while (npcDialogue.gameObject.activeSelf) yield return null;

        dialogue.SetDialogue(new string[]
        {
            "A承知しました。", //bug
            "本日はありがとうございました！",
        });

        while (dialogue.gameObject.activeSelf) yield return null;
    }

    IEnumerator FadeInSequence()
    {
        yield return new WaitForSeconds(1.0f);
        float currentTime = 0.0f;
        float duration = 2.0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            SetAlpha(gameClearObj, currentTime / duration);
            yield return null;
        }
        SetAlpha(gameClearObj, 1f);

        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene("TitleScene");
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
    }
}