using UnityEngine;

public class SlaveFlowManager : MonoBehaviour
{
    public const int WAIT_TIME = 150;
    public const int MAX_SLAVES = 30;
    private int remainingSlaves = MAX_SLAVES;
    public GameObject slavePrefab;
    private int waitCounter;
    public ChariotController chariot;

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
        GameObject slaveGO = (GameObject)Instantiate(slavePrefab, transform.position, Quaternion.identity);
        SlaveController slave = slaveGO.GetComponent<SlaveController>();
        if (slave != null)
        {
            slave.masterChariot = chariot;
        }
        else
        {
            Debug.Log("slave is null !");
        }
    }

}
