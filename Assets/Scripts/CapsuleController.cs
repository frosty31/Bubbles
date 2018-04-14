using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(sendInterval = 0.033f)]
public class CapsuleController : NetworkBehaviour
{

    public float speed;

    public GameObject bubble;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isLocalPlayer)
        {
            CmdShootBubble();
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Bubble(Clone)")
        {
            Destroy(this.gameObject);
        }
    }
	
	[Command]  
    void CmdShootBubble()
    {
		Vector3 bubbleDirection = Camera.main.transform.forward;
		Vector3 bubblePosition = Camera.main.transform.position + bubbleDirection * 1.5f;
		GameObject nextBubble = (GameObject)Instantiate(bubble, bubblePosition, Quaternion.Euler(bubbleDirection));
		nextBubble.GetComponentInChildren<Rigidbody>().velocity = bubbleDirection * 1.0f;
		NetworkServer.Spawn(nextBubble);
		Destroy(nextBubble, 8.0f);

    }
}
