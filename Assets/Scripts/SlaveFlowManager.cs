using UnityEngine;

public class SlaveFlowManager : MonoBehaviour
{
    public const int WAIT_TIME = 60;
    public const int MAX_SLAVES = 150;
    private int remainingSlaves = MAX_SLAVES;
    public Vector2 spawnPosition;
    public GameObject slavePrefab;
    private int waitCounter;

    void Start()
    {
        waitCounter = 0;
    }

    void Update()
    {
        waitCounter += 1;

        if (waitCounter > WAIT_TIME)
        {
            waitCounter = 0;
            SpawnSlave();
        }
    }

    public void SpawnSlave()
    {
        remainingSlaves -= 1;
        Instantiate(slavePrefab, spawnPosition, Quaternion.identity);
    }

}
