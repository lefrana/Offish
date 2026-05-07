using UnityEngine;

public class ShotGenerator : MonoBehaviour
{
    public GameObject keyUpPrefab, keyDownPrefab, keyLeftPrefab, keyRightPrefab;
    public GameObject shotPrefab;
    public int keyMax = 4;

    private ArrowKey[] arrowKey;
    private int keyCount = 0; //
    public bool shotSpawned = false;

    public bool isWaitingForDialogue = false;

    public AudioSource audioSource;


    void Start()
    {
        //SpawnKeys();
    }

    void Update()
    {
        if (shotSpawned)
        {
            return;
        }

        if (arrowKey == null)
        {
            return;
        }

        if (isWaitingForDialogue)
        {
            return;
        }

        if (keyCount >= keyMax)
        {
            SpawnShot();
            shotSpawned = true;
            return;
        }

        ArrowType? input = GetInput();
        if (input == null) return;

        //play sound
        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (arrowKey[keyCount].Check(input.Value))
        {
            keyCount++;
        }
        else
        {
            ResetKeys();
        }
    }

    public GameObject[] SpawnKeys()
    {
        keyCount = 0;
        arrowKey = new ArrowKey[keyMax];
        GameObject[] keyObjects = new GameObject[keyMax];

        Vector2 basePos = new Vector2(3.0f, 4.3f);
        float spacing = 1.0f;

        for (int i = 0; i < keyMax; i++)
        {
            ArrowType type = (ArrowType)Random.Range(0, 4);
            GameObject prefab = GetPrefab(type);

            Vector2 pos = basePos + new Vector2(i * spacing, 0.0f);
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity);

            //force tag
            obj.tag = "Arrows";

            keyObjects[i] = obj;

            ArrowKey arrow = obj.GetComponent<ArrowKey>();
            arrow.type = type;
            arrowKey[i] = arrow;
        }

        return keyObjects;
    }

    GameObject GetPrefab(ArrowType type)
    {
        switch (type)
        {
            case ArrowType.Up:      return keyUpPrefab;
            case ArrowType.Down:    return keyDownPrefab;
            case ArrowType.Left:    return keyLeftPrefab;
            default:                return keyRightPrefab;
        }
    }

    ArrowType? GetInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))      return ArrowType.Up;
        if (Input.GetKeyDown(KeyCode.DownArrow))    return ArrowType.Down;
        if (Input.GetKeyDown(KeyCode.LeftArrow))    return ArrowType.Left;
        if (Input.GetKeyDown(KeyCode.RightArrow))   return ArrowType.Right;

        return null;
    }

    void SpawnShot()
    {
        Vector2 shotPos = new Vector2(4.0f, 4.3f);
        GameObject newShot = Instantiate(shotPrefab, shotPos, Quaternion.identity);

        ShotController controller = newShot.GetComponent<ShotController>();
        if (controller != null)
        {
            controller.shotGenerator = this;
            controller.isShot = true;
        }

        shotSpawned = true;
    }
    public void OnShotFinished()
    {
        if (isWaitingForDialogue)
        {
            return;
        }

        shotSpawned = false;
        keyCount = 0;
        ResetKeys();
    }

    void ResetKeys()
    {
        if (arrowKey != null)
        {
            for (int i = 0; i < arrowKey.Length; i++)
            {
                if (arrowKey[i] != null && arrowKey[i].gameObject != null)
                {
                    Destroy(arrowKey[i].gameObject);
                }
            }
        }

        shotSpawned = false;
        keyCount = 0;
        SpawnKeys();
    }

    public void ResetShotGenerator()
    {
        shotSpawned = true;

        GameObject[] oldShots = GameObject.FindGameObjectsWithTag("Shot");
        foreach (GameObject s in oldShots) Destroy(s);

        GameObject[] oldKeys = GameObject.FindGameObjectsWithTag("Arrows");
        foreach (GameObject k in oldKeys) Destroy(k);
    }
}
