using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleManager : MonoBehaviour
{
    public GameObject titleText;
    public GameObject titleTextOutline;

    public AudioSource audioSource;

    // This variable controls when the player is allowed to click
    private bool canClick = false;

    void Start()
    {
        SetAlpha(titleText, 0f);
        SetAlpha(titleTextOutline, 0f);

        Invoke("PlayTitleSFX", 0.2f);

        StartCoroutine(FadeInSequence());
    }

    void PlayTitleSFX()
    {
        if (audioSource != null) audioSource.Play();
    }

    void Update()
    {
        // Now we check BOTH: is a key pressed AND is canClick true?
        if (canClick && Input.anyKeyDown)
        {
            SceneManager.LoadScene("TutorialScene");
        }
    }

    IEnumerator FadeInSequence()
    {
        yield return new WaitForSeconds(1.0f);


        float currentTime = 0.0f;
        float duration = 2.0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float alpha = currentTime / duration;

            SetAlpha(titleText, alpha);
            SetAlpha(titleTextOutline, alpha);

            yield return null;
        }

        SetAlpha(titleText, 3f);
        SetAlpha(titleTextOutline, 3f);

        yield return new WaitForSeconds (2.0f);
        // --- THE MAGIC LINE ---
        // The fade is finished, so now we allow input!
        canClick = true;
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