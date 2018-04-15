using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



[NetworkSettings(sendInterval = 0.033f)]
public class PlayerController : NetworkBehaviour {



    public GameObject bubble;

    public GameObject sprite;

	

	/// <summary>
	/// Our position.
	/// </summary>
	[SyncVar]
	private Vector3 localPosition;

	/// <summary>
	/// Our rotation.
	/// </summary>
	[SyncVar]
	private Quaternion localRotation;

	/// <summary>
	/// Sets the localPosition and localRotation on clients.
	/// </summary>
	/// <param name="postion">the localPosition to set</param>
	/// <param name="rotation">the localRotation to set</param>
	[Command]
	public void CmdTransform(Vector3 postion, Quaternion rotation)
	{
		if (!isLocalPlayer)
		{
			localPosition = postion;
			localRotation = rotation;
		}
	}

	public override void OnStartLocalPlayer()
	{
       
        CmdSpawnSprite();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// If we aren't the local player, we just need to make sure that the position of this object is set properly
		// so that we properly render their avatar in our world.

		if (Input.GetKeyDown(KeyCode.Space) && isLocalPlayer)
		{
			CmdShootBubble();
		}

		if (!isLocalPlayer)
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition, localPosition, 0.3f);
			transform.localRotation = localRotation;
			return;
		}

		
		// if we are the remote player then we need to update our worldPosition and then set our 
		// local position for other clients to update our position in their world.
		transform.position = Camera.main.transform.position;
		transform.rotation = Camera.main.transform.rotation;

		// Depending on if you are host or client, either setting the SyncVar (client) 
		// or calling the Cmd (host) will update the other users in the session.
		// So we have to do both.
		localPosition = transform.localPosition;
		localRotation = transform.localRotation;
		CmdTransform(localPosition, localRotation);
		
	}

	[Command]
	void CmdShootBubble()
	{
		Vector3 bubbleDirection = this.transform.forward;
		Vector3 bubblePosition = this.transform.position + bubbleDirection * 1.5f;
		GameObject nextBubble = (GameObject)Instantiate(bubble, bubblePosition, Quaternion.Euler(bubbleDirection));
		nextBubble.GetComponentInChildren<Rigidbody>().velocity = bubbleDirection * 3.0f;
		NetworkServer.Spawn(nextBubble);
		Destroy(nextBubble, 8.0f);

	}

    [Command]
    void CmdSpawnSprite() {
        
		GameObject playerSprite = (GameObject)Instantiate(sprite);
        NetworkServer.Spawn(playerSprite);
    }
}
