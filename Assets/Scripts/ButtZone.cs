using UnityEngine;

public class ButtZone : Zone
{

    void OnTriggerEnter(Collider slaveCollider)
    {
        SlaveController sc = slaveCollider.gameObject.GetComponent<SlaveController>();
        if (sc != null)
        {
            chariot.SendSlaveToWork(sc);
        }
    }

}
