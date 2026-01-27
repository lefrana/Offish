using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject yellowFish;
    public GameObject fish;
    public GameObject tutorialScreen;

    public GameObject titleText;
    public GameObject titleTextOutline;

    public TutorialText tutorialText;
    private bool canClick = false;

    void Start()
    {
        SetAlpha(tutorialScreen, 0f);
        SetAlpha(yellowFish, 1f);
        SetAlpha(fish, 0f);
        SetAlpha(titleText, 1f);
        SetAlpha(titleTextOutline, 1f);

        StartCoroutine(CrossfadeImages(3.0f));
    }

    void Update()
    {
        if (canClick && Input.anyKeyDown)
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    IEnumerator CrossfadeImages(float duration)
    {
        float elapsed = 0f;
        float fadeOutDuration = 0.8f; //title fade-out

        // --- STEP 1: Fade OUT Title Text ONLY ---
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = 1f - (elapsed / fadeOutDuration);

            SetAlpha(titleText, alpha);
            SetAlpha(titleTextOutline, alpha);
            // Note: We don't touch the fish yet!
            yield return null;
        }

        SetAlpha(titleText, 0f);
        SetAlpha(titleTextOutline, 0f);

        titleText.SetActive(false);
        titleTextOutline.SetActive(false);

        // --- STEP 2: Tiny Pause (Optional, for better feel) ---
        yield return new WaitForSeconds(0.5f);

        // --- STEP 3: Crossfade Yellow Fish to Normal Fish ---
        elapsed = 0f; // Reset timer for the next part
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = elapsed / duration;

            // Fade Yellow OUT
            SetAlpha(yellowFish, 1f - alpha);
            // Fade Normal Fish IN
            SetAlpha(fish, alpha);

            yield return null;
        }

        SetAlpha(yellowFish, 0f);
        SetAlpha(fish, 1f);

        yield return new WaitForSeconds(2.0f);
        StartCoroutine(TextSequence());
    }

    IEnumerator TextSequence()
    {
        tutorialText.SetDialogue(new string[]
        {
            "名前：アジ・ヒカル",
            "年齢：20歳（魚年齢）",
            "陸で働くため、就職活動中ー"

            //"name: Aji Hikaru",
            //"age : 20 years old (fish years)",
            //"currently looking for a job on land."
        });

        // Wait for the text to finish/be read before showing the tutorial overlay
        yield return new WaitForSeconds(8.0f);

        StartCoroutine(FadeInSequence());
    }

    IEnumerator FadeInSequence()
    {
        if (tutorialText != null)
        {
            tutorialText.gameObject.SetActive(false);
        }

        if (yellowFish != null)
        {
            yellowFish.gameObject.SetActive(false);
        }

        if (fish != null)
        {
            fish.gameObject.SetActive(false);
        }

        float currentTime = 0.0f;
        float duration = 2.0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            SetAlpha(tutorialScreen, currentTime / duration);
            yield return null;
        }

        SetAlpha(tutorialScreen, 1f);

        yield return new WaitForSeconds(2.0f);

        canClick = true;
    }

    // Your single, universal alpha controller
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