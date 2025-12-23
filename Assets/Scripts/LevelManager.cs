using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Dialogue dialogue;

    void Start()
    {
        dialogue.SetDialogue(new string[]
        {
            "testing level start",
            "testing second dialogue line",
            "testkjshfkwrgiulrhgbiqrivyeatv jgeirbic",
        });
    }
}
