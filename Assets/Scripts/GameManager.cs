using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerDescription
{
    [HideInInspector] public int id;
    [HideInInspector] public ChariotController chariot;
    [HideInInspector] public SlaveFlowManager spawner;
    public Color color;
    public InputController input;
}

public class GameManager : MonoBehaviour {

    public BoxCollider gameZone;
    public PlayerDescription[] players;

    public PlayerController playerControllerPrefab;
    public GameObject chariotPrefab;
    public SlaveFlowManager slaveFlowManagerPrefab;
    public GameObject finishLinePrefab;


    void Start()
    {
        for (int i = 0; i < players.Length; ++i)
        {
            PlayerDescription desc = players[i];
            desc.id = i;

            float horizontalStep = gameZone.bounds.size.x / (players.Length + 1);

            PlayerController controller = (PlayerController)GameObject.Instantiate(playerControllerPrefab);

            Vector3 chariotPosition = new Vector3(gameZone.bounds.min.x + (i + 1) * horizontalStep, 0.0f, gameZone.bounds.min.z);
            GameObject chariot = (GameObject)GameObject.Instantiate(chariotPrefab, chariotPosition, Quaternion.identity);
            desc.chariot = chariot.GetComponent<ChariotController>();

            SlaveFlowManager spawner = (SlaveFlowManager)GameObject.Instantiate(slaveFlowManagerPrefab, chariotPosition - new Vector3(0.0f, 0.0f, 2.0f), Quaternion.identity);
            desc.spawner = spawner;

            desc.chariot.playerDescription = desc;
            controller.playerDescription = desc;
            spawner.playerDescription = desc;
        }

        Instantiate(finishLinePrefab, new Vector3(0.0f, 0.01f, gameZone.bounds.max.z), Quaternion.identity);
	}
	

	void Update()
    {
	    foreach (PlayerDescription player in players)
        {
            if (player.chariot.gameObject.transform.position.z >= gameZone.bounds.max.z)
            {
                Debug.Log("Player " + player.id + " Wins !");
            }
        }
	}
}
