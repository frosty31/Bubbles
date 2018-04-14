using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleController : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 bubbleDirection = Camera.main.transform.forward;
            Vector3 bubblePosition = Camera.main.transform.position + bubbleDirection * 1.5f;
            GameObject nextBubble = (GameObject)Instantiate(bubble, bubblePosition, Quaternion.Euler(bubbleDirection));
            nextBubble.GetComponentInChildren<Rigidbody>().velocity = bubbleDirection*1.0f;

            Destroy(nextBubble, 8.0f);
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
        if (collision.collider.name=="Bubble(Clone)") {
            Destroy(this.gameObject);
        }
            
            
    }
}
