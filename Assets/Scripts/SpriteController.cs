using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[NetworkSettings(sendInterval = 0.033f)]
public class SpriteController : NetworkBehaviour
{
    public GameObject bubbled;

    public float movementSpeed;
    public float rotationSpeed;

    Rigidbody rb;
    Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        var x = Input.GetAxisRaw("Horizontal");
        var z = Input.GetAxisRaw("Vertical");

        // Vector3 vectorMovement = new Vector3(x, 0.0f, z);
        Vector3 vectorMovement = Camera.main.transform.forward * z + Camera.main.transform.right * x;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vectorMovement), rotationSpeed);

        transform.Translate(vectorMovement * movementSpeed * Time.deltaTime, Space.World);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isIdle", false);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isIdle", false);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isIdle", false);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isIdle", false);
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isIdle", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Bubble(Clone)")
        {
            Debug.Log("Bubbled");
            // Destroy(this.gameObject);

            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1.2f, this.transform.position.z);
            rb.useGravity = false;
            GameObject bubbledGameObject = (GameObject)Instantiate(bubbled, new Vector3(this.transform.position.x, this.transform.position.y + 1.75f, this.transform.position.z), Quaternion.Euler(this.transform.position));
            bubbledGameObject.GetComponent<Rigidbody>().AddForce(other.transform.InverseTransformDirection(other.gameObject.GetComponent<Rigidbody>().velocity) * 100);
            this.transform.SetParent(bubbledGameObject.transform);
        }
    }
}
