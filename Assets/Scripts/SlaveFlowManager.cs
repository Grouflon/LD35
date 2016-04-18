using UnityEngine;

public class SlaveFlowManager : MonoBehaviour
{
    public const int WAIT_TIME = 60;
    public const int MAX_SLAVES = 150;
    private int remainingSlaves = MAX_SLAVES;
    public GameObject slavePrefab;
    private int waitCounter;
    public PlayerDescription playerDescription;

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
        Vector3 spawnPosition = transform.position;
        spawnPosition.x += Random.Range(-3f, 3f);
        GameObject slaveGO = (GameObject)Instantiate(slavePrefab, spawnPosition, Quaternion.identity);
        SlaveController slave = slaveGO.GetComponent<SlaveController>();
        slave.playerID = playerDescription.id;
        if (slave != null)
        {
            slave.masterChariot = playerDescription.chariot;
        }
        else
        {
            Debug.Log("slave is null !");
        }
    }

}
