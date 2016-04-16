using UnityEngine;

public class ButtZone : Zone
{
    public Collider zoneCollider;

    override void OnTriggerEnter(Collider slaveCollider)
    {
        SlaveController sc = slaveCollider.gameObject.GetComponent<SlaveController>();
        if (sc != null)
        {
            chariot.SendSlaveToWork(sc);
        }
    }

}
