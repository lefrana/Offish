using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialText : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    private string[] lines;
    public float textSpeed;
    private int index;

    public GameObject textBox;
    private bool isTalking = false;

    public AudioSource audioSource;

    void Start()
    {
        if (!isTalking)
        {
            HideBox();
        }

        textComponent.text = string.Empty;
    }

    public void SetDialogue(string[] newLines)
    {
        StopAllCoroutines();

        if (newLines == null || newLines.Length == 0 || string.IsNullOrEmpty(newLines[0]))
        {
            HideBox();
            return;
        }

        isTalking = true;
        gameObject.SetActive(true);

        lines = newLines;
        textComponent.text = string.Empty;
        index = 0;

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        //textComponent.text = string.Empty;
        isTalking = true;

        //start text sfx
        if (audioSource != null)
        {
            audioSource.Play();
        }

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

        //stop audio
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        isTalking = false;

        yield return new WaitForSeconds(1.2f);
        NextLine();
        //StartCoroutine(NextLine());
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text += "\n";
            StartCoroutine(TypeLine());
        }
        else
        {
            //yield return new WaitForSeconds(3.0f);
            //gameObject.SetActive(false);
        }
    }

    public void HideBox()
    {
        if (textBox != null)
        {
            textBox.SetActive(false);
        }

        textComponent.text = string.Empty;
    }
}
