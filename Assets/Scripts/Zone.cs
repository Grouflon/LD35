using UnityEngine;

public class Zone : MonoBehaviour
{
    public Collider zoneCollider;
    public int slaveCount;
    public ChariotController chariot;

    void OnTriggerEnter(Collider slaveCollider)
    {
        SlaveController sc = slaveCollider.gameObject.GetComponent<SlaveController>();
        if (sc != null)
        {
            slaveCount += sc.strength;
        }
    }

    void OnTriggerExit(Collider slaveCollider)
    {
        SlaveController sc = slaveCollider.gameObject.GetComponent<SlaveController>();
        if (sc != null)
        {
            slaveCount -= sc.strength;
        }
    }




}
