using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(sendInterval = 0.033f)]
public class SpriteController : NetworkBehaviour
{

    public float speed;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            rb.AddForce(movement * speed);

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Bubble(Clone)")
        {
            Destroy(this.gameObject);
        }
    }
	
}
