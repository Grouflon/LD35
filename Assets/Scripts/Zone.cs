using UnityEngine;
using System.Collections.Generic;

public class SpotStruct
{
    public GameObject spotGO;
    public bool isFreeSpot;
    public void ToggleSpot(SlaveController slave)
    {
        if (slave != null)
        {
            isFreeSpot = false;
            assignedSlave = slave;
        }
        else
        {
            isFreeSpot = true;
            assignedSlave = null;
        }
    }
    public SlaveController assignedSlave;
}

public class Zone : MonoBehaviour
{
    public Collider zoneCollider;
    public int slaveCount;
    public ChariotController chariot;
    public int side; // -1 = left, 0 = buttzone, 1 = right
    public List<SpotStruct> spots = new List<SpotStruct>();
    public List<SlaveController> registeredSlaves = new List<SlaveController>();

    void Start()
    {
        if (side != 0)
        {
            int nlines = 4; // because of the chariot sprite ^^
            int ncols = ChariotController.MAX_SPOTS / nlines;
            Vector3 position;
            string name;

            // Instantiate a lot of spots
            for (int i = 0; i < nlines; i++)
            {
                for (int j = 0; j < ncols; j++)
                {
                    // Compute position
                    position = this.transform.position;
                    position.z += 0.60f * (nlines / 2 - i);
                    position.x += 0.18f * (ncols / 2 - j);
                    position.y = 0f;

                    // Create spot
                    name = "spot_" + i.ToString() + "_" + j.ToString();
                    GameObject spot = new GameObject(name);
                    spot.transform.SetParent(this.transform);
                    spot.transform.position = position;

                    // Add it to the list
                    SpotStruct spotStruct = new SpotStruct();
                    spotStruct.spotGO = spot;
                    spotStruct.isFreeSpot = true;
                    spots.Add(spotStruct);
                }
            }
        }
    }

    void OnTriggerEnter(Collider slaveCollider)
    {
        SlaveController sc = slaveCollider.gameObject.GetComponent<SlaveController>();
        if (sc != null)
        {
            if (sc.playerID != chariot.playerID) // KILL ENEMIES
            {
                sc.DestroySlave(new Vector3(0f, 0f, 0f));
            }
            else
            {
                if (this.side != 0 && !registeredSlaves.Contains(sc))
                {
                    // Update workforce
                    registeredSlaves.Add(sc);
                    slaveCount += sc.strength;

                    //Debug.Log("Slave enters zone " + side);

                    // Select a free spot for the slave or send him back to buttzone
                    for (int s = 0; s < spots.Count; s++)
                    {
                        if (spots[s].isFreeSpot)
                        {
                            sc.targetZone = spots[s].spotGO;
                            sc.currentZone = this;
                            sc.isWorking = side;
                            spots[s].ToggleSpot(sc);
                            break;
                        }
                    }
                }
                else if (this.side == 0 && sc.isWorking == 0)
                {
                    chariot.SendSlaveToWork(sc);
                    //Debug.Log("Slave sent to work");
                }
            }
        }
    }

    void OnTriggerExit(Collider slaveCollider)
    {
        SlaveController sc = slaveCollider.gameObject.GetComponent<SlaveController>();
        if ( sc !=null && sc.currentZone == this)
        {
            sc.currentZone = null;
            ExitSlave(sc);
        }
    }

    public void ExitSlave(SlaveController sc)
    {
        if (sc != null && this.side != 0 && registeredSlaves.Contains(sc))
        {
            // Administrative stuff
            registeredSlaves.Remove(sc);
            slaveCount -= sc.strength;

            // Tell slave to go towards buttzone
            sc.isWorking = 0;

            // Free spot
            for (int s = 0; s < spots.Count; s++)
            {
                if (spots[s].assignedSlave == sc)
                {
                    spots[s].ToggleSpot(null);
                    break;
                }
            }
        }
    }




}
