using UnityEngine;
using TMPro;
using System.Collections;

public class NPCDialogue : MonoBehaviour
{
    public GameObject npc;         // The Portrait
    public GameObject visualBox;   // The Text Bubble/Background Image

    public TextMeshProUGUI textComponent;
    public float textSpeed = 0.05f;

    private string[] lines;
    private int index;
    private bool isTyping = false; // Add this to track state

    void Start()
    {
        // Only hide if we aren't currently typing a message
        if (!isTyping)
        {
            HideNPC();
        }
    }

    public void SetDialogue(string[] newLines)
    {
        StopAllCoroutines();

        if (newLines == null || newLines.Length == 0 || string.IsNullOrEmpty(newLines[0]))
        {
            HideNPC();
            return;
        }

        isTyping = true; // We are now active

        if (npc != null) npc.SetActive(true);
        if (visualBox != null) visualBox.SetActive(true);

        lines = newLines;
        textComponent.text = string.Empty;
        index = 0;

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        textComponent.text = string.Empty;
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(2.0f);
        NextLine();
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            HideNPC();
        }
    }

    public void HideNPC()
    {
        isTyping = false;
        if (npc != null) npc.SetActive(false);
        if (visualBox != null) visualBox.SetActive(false);
        textComponent.text = string.Empty;
    }
}