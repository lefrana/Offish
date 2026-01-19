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

        string currentLine = lines[index];
        int i = 0;

        // Use a while loop instead of foreach to control the index manually
        while (i < currentLine.Length)
        {
            char c = currentLine[i];

            // TAG DETECTION: If we hit a '<', find the matching '>'
            if (c == '<')
            {
                int closeTagIndex = currentLine.IndexOf('>', i);
                if (closeTagIndex != -1)
                {
                    // Extract the full tag: e.g., "<i>" or "</i>"
                    string fullTag = currentLine.Substring(i, closeTagIndex - i + 1);
                    textComponent.text += fullTag;

                    // Move the index to right after the '>'
                    i = closeTagIndex + 1;

                    // We don't yield return here because tags should be instant
                    continue;
                }
            }

            // Normal character typing
            textComponent.text += c;
            i++;

            // Wait for the next character
            yield return new WaitForSeconds(textSpeed);
        }

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
