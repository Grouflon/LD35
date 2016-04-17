using UnityEngine;

public class Zone : MonoBehaviour
{
    public Collider zoneCollider;
    public int slaveCount;
    public ChariotController chariot;
    public string side;

    void OnTriggerEnter(Collider slaveCollider)
    {
        SlaveController sc = slaveCollider.gameObject.GetComponent<SlaveController>();
        if (sc != null)
        {
            slaveCount += sc.strength;
            //Debug.Log("Slave added to " + this);
        }
    }

    void OnTriggerExit(Collider slaveCollider)
    {
        SlaveController sc = slaveCollider.gameObject.GetComponent<SlaveController>();
        if (sc != null)
        {
            slaveCount -= sc.strength;
            //Debug.Log("Slave removed from " + this);
        }
    }




}
