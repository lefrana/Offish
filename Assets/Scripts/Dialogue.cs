using UnityEngine;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    public Animator charaAnim;
    public TextMeshProUGUI textComponent;
    private string[] lines;
    public float textSpeed;
    private int index;

    public AudioSource audioSource;

    void Start()
    {
        textComponent.text = string.Empty;
    }

    public void SetDialogue(string[] newLines)
    {
        StopAllCoroutines();
        gameObject.SetActive(true);

        lines = newLines;
        textComponent.text = string.Empty;
        index = 0;

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        textComponent.text = string.Empty;
        charaAnim.SetBool("isTalking", true);

        // --- START AUDIO ---
        if (audioSource != null) audioSource.Play();

        string currentLine = lines[index];
        int i = 0;

        while (i < currentLine.Length)
        {
            char c = currentLine[i];

            if (c == '<')
            {
                int closeTagIndex = currentLine.IndexOf('>', i);
                if (closeTagIndex != -1)
                {
                    textComponent.text += currentLine.Substring(i, closeTagIndex - i + 1);
                    i = closeTagIndex + 1;
                    continue;
                }
            }

            textComponent.text += c;
            i++;
            yield return new WaitForSeconds(textSpeed);
        }

        // --- STOP AUDIO ---
        if (audioSource != null) audioSource.Stop();

        charaAnim.SetBool("isTalking", false);

        yield return new WaitForSeconds(2.0f);
        NextLine();
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            charaAnim.SetBool("isTalking", false);
        }
    }
}
