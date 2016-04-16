using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public GameObject cursorGO;
    public GameObject chariotGO;
    public Vector2 chariotPosition;
    public Vector2 cursorPosition;
    public InputController inputController;

	// Use this for initialization
	void Start ()
    {
        chariotPosition = chariotGO.transform.position;
        cursorPosition = chariotPosition;
        MoveCursor(cursorPosition);
        DisplayCursor(true);
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Get new input direction 
        Vector2 newPosition = inputController.GetDirection();

        // Update cursor position
        MoveCursor(newPosition);
	}
 
    void DisplayCursor(bool onOff)
    {
        cursorGO.SetActive(onOff);
    }

    void MoveCursor(Vector2 newPosition)
    {
        cursorPosition = newPosition;
        cursorGO.transform.position = cursorPosition;
    }
}
