using UnityEngine;
using System.Collections;

public class GroundTargetController : MonoBehaviour {

    public float angularSpeed = 0.0f;
    public Color color;
    public GameObject inCylinder;
    public GameObject outCylinder;
    public GameObject plane;

    public void FadeOut(float _duration)
    {
        GetComponent<FadeObjectInOut>().FadeOut(_duration);
        Destroy(gameObject, _duration);
    }

	void Start()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y = Random.Range(0.0f, 360.0f);
        transform.rotation = Quaternion.Euler(rotation);

        inCylinder.GetComponent<Renderer>().material.color = color;
        outCylinder.GetComponent<Renderer>().material.color = color;
        plane.GetComponent<Renderer>().material.color = color;
    }
	
	void Update()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y += angularSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rotation);
    }
}
