using UnityEngine;

public class ButtZone : Zone
{
    void Start()
    {
        side = 0;
    }

    void OnTriggerEnter(Collider slaveCollider)
    {
        SlaveController sc = slaveCollider.gameObject.GetComponent<SlaveController>();
        if (sc != null)
        {
            chariot.SendSlaveToWork(sc);
        }
    }

}
