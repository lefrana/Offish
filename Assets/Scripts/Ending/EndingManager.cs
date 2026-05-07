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


    public bool isGameOver = false;

    void OnEnable()
    {
        isGameOver = Timer.isTimeUp;
        Timer.isTimeUp = false;

        StartCoroutine(StartingSequence());
    }

    IEnumerator StartingSequence()
    {
        //hide everything at start
        npcDialogue.HideNPC();
        gameOverObj.SetActive(false);
        gameClearObj.SetActive(false);
        dialogue.gameObject.SetActive(false);
        npcDialogue.gameObject.SetActive(false);

        //fade in
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

        if (!isGameOver)
        {
            yield return StartCoroutine(DialogueSequence());
        }
        else
        {
            yield return new WaitForSeconds(1.0f);
        }

        yield return new WaitForSeconds(0.5f);

        //fade out
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

        //go to game over screen
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