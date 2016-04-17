using UnityEngine;
using System.Collections.Generic;

public struct SpotStruct
{
    public GameObject spotGO;
    public bool isFreeSpot;
    public void ToggleSpot()
    {
        isFreeSpot = !isFreeSpot;
    }
}

public class Zone : MonoBehaviour
{
    public Collider zoneCollider;
    public int slaveCount;
    public ChariotController chariot;
    public int side; // -1 = left, 0 = buttzone, 1 = right
    public List<SpotStruct> spots = new List<SpotStruct>();

    void Start()
    {
        int nlines = 4; // because of the chariot sprite ^^
        int ncols = ChariotController.MAX_SPOTS/nlines;
        Vector3 position;
        string name;

        // Instantiate a lot of spots
        for (int i = 0; i < nlines; i++)
        {
            for (int j = 0; j < ncols; j++)
            {
                // Compute position
                position = this.transform.position;
                position.z += 0.15f*(nlines - i);
                position.x += 0.15f * (ncols - j);
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

    void OnTriggerEnter(Collider slaveCollider)
    {
        SlaveController sc = slaveCollider.gameObject.GetComponent<SlaveController>();
        if (sc != null)
        {
            slaveCount += sc.strength;
            //Debug.Log("Slave added to " + this);

            // Select a free spot for the slave or send him back to buttzone
            //bool foundFreeSpot = false;
            foreach (SpotStruct ss in spots)
            {
                if (ss.isFreeSpot)
                {
                    sc.targetZone = ss.spotGO;
                    sc.isWorking = side;
                    ss.ToggleSpot();
                    break;
                }
            }
        }
    }

    void OnTriggerExit(Collider slaveCollider)
    {
        SlaveController sc = slaveCollider.gameObject.GetComponent<SlaveController>();
        if (sc != null)
        {
            slaveCount -= sc.strength;
            sc.isWorking = 0;
            //Debug.Log("Slave removed from " + this);
        }
    }




}
