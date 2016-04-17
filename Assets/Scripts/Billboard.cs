using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Billboard : MonoBehaviour {

	void Update ()
    {
        Vector3 directionToCamera = Camera.main.transform.position - transform.position;
        directionToCamera.Normalize();
        Quaternion rotation = transform.rotation;
        rotation.SetLookRotation(directionToCamera);
        transform.rotation = rotation;
    }
}
