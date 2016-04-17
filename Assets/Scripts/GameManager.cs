using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public BoxCollider gameZone;
    public InputController[] players;

    public PlayerController playerControllerPrefab;
    public GameObject chariotPrefab;
    public SlaveFlowManager slaveFlowManagerPrefab;


    void Start()
    {
        int i = 0;
	    foreach (InputController input in players)
        {
            float horizontalStep = gameZone.bounds.size.x / (players.Length + 1);

            PlayerController controller = (PlayerController)GameObject.Instantiate(playerControllerPrefab);
            controller.id = i;
            controller.inputController = input;

            Vector3 chariotPosition = new Vector3(gameZone.bounds.min.x + (i + 1) * horizontalStep, 0.0f, gameZone.bounds.min.z);
            GameObject chariot = (GameObject)GameObject.Instantiate(chariotPrefab, chariotPosition, Quaternion.identity);
            controller.chariotGO = chariot;

            SlaveFlowManager spawner = (SlaveFlowManager)GameObject.Instantiate(slaveFlowManagerPrefab, chariotPosition - new Vector3(0.0f, 0.0f, 2.0f), Quaternion.identity);
            spawner.chariot = chariot.GetComponent<ChariotController>();

            ++i;
        }
	}
	

	void Update()
    {
	
	}
}
