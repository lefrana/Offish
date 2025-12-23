using UnityEngine;

public class ShotGenerator : MonoBehaviour
{
    public GameObject keyUpPrefab, keyDownPrefab, keyLeftPrefab, keyRightPrefab;
    public GameObject shotPrefab;
    public int keyMax = 4;

    private ArrowKey[] arrowKey;
    private int keyCount = 0; //
    private bool shotSpawned = false;

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

        if (keyCount >= keyMax)
        {
            SpawnShot();
            shotSpawned = true;
            return;
        }

        ArrowType? input = GetInput();
        if (input == null)
        {
            return;
        }

        if (arrowKey[keyCount].Check(input.Value))
        {
            keyCount++;
        }
        else
        {
            //if wrong input, redo generating keys
            ResetKeys();
        }
    }

    public void SpawnKeys()
    {
        arrowKey = new ArrowKey[keyMax];

        Vector2 basePos = new Vector2(3.0f, 4.3f);
        float spacing = 1.0f;

        for (int i = 0; i < keyMax; i++)
        {
            ArrowType type = (ArrowType)Random.Range(0, 4);
            GameObject prefab = GetPrefab(type);

            Vector2 pos = basePos + new Vector2(i * spacing, 0.0f);
            GameObject obj = Instantiate(prefab, pos, Quaternion.identity);

            ArrowKey arrow = obj.GetComponent<ArrowKey>();
            arrow.type = type;
            arrowKey[i] = arrow;
            Debug.Log(type);
        }
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
        Instantiate(shotPrefab, shotPos, Quaternion.identity);
    }

    void ResetKeys()
    {
        //destroy existing arrowKey and generate new ones
        for (int i = 0; i < arrowKey.Length; i++)
        {
            if (arrowKey[i] != null)
            {
                Destroy(arrowKey[i].gameObject);
            }
        }
        keyCount = 0;
        SpawnKeys();
    }
}
