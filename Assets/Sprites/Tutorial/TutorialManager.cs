using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public SpriteRenderer yellowFish;
    public SpriteRenderer fish;

    void Start()
    {
        StartCoroutine(CrossfadeImages(5.0f));
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    IEnumerator CrossfadeImages(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / duration;

            // Fade the old one out
            if (yellowFish != null)
            {
                Color c = yellowFish.color;
                c.a = 1f - normalizedTime;
                yellowFish.color = c;
            }

            // Fade the new one in
            if (fish != null)
            {
                Color c = fish.color;
                c.a = normalizedTime;
                fish.color = c;
            }

            yield return null;
        }

        // Ensure final values are set
        if (yellowFish != null) SetSpriteAlpha(yellowFish, 0f);
        if (fish != null) SetSpriteAlpha(fish, 1f);
    }

    // Helper to keep code clean
    void SetSpriteAlpha(SpriteRenderer sr, float alpha)
    {
        Color c = sr.color;
        c.a = alpha;
        sr.color = c;
    }
}