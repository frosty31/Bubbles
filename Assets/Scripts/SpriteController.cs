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

    GameObject createdBubbled;

    bool isBubbled;

    Rigidbody rb;
    Animator animator;
    Collider myCollider;
    private Vector3 vectorAtOrigin;

    public GameObject gotBubbledEffect;
    public GameObject destroyedEffect;

    int breakFree = 100;

    public bool iAmLocalPlayer { get; set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        myCollider = GetComponent<CapsuleCollider>();

        vectorAtOrigin = new Vector3(0, 0, 0);

        if (transform.parent)
        {
            PlayerController myPlayerController = transform.parent.GetComponent<PlayerController>();
            iAmLocalPlayer = myPlayerController.isLocalPlayer;
        }
    }

    private void Update()
    {
        if (iAmLocalPlayer)
        {
            var x = Input.GetAxisRaw("Horizontal");
            var z = Input.GetAxisRaw("Vertical");

            // Vector3 vectorMovement = new Vector3(x, 0.0f, z);
            Vector3 vectorMovement = Camera.main.transform.forward * z + Camera.main.transform.right * x;
            if (vectorMovement != vectorAtOrigin)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vectorMovement), rotationSpeed);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            transform.Translate(vectorMovement * movementSpeed * Time.deltaTime, Space.World);

            if (Input.GetKey(KeyCode.UpArrow))
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isIdle", false);

                if (isBubbled)
                {
                    breakFree -= 1;
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isIdle", false);

                if (isBubbled)
                {
                    breakFree -= 1;
                }
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isIdle", false);

                if (isBubbled)
                {
                    breakFree -= 1;
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isIdle", false);

                if (isBubbled)
                {
                    breakFree -= 1;
                }
            }
            else
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isIdle", true);
            }
        }
        else
        {
            return;
        }
    }

    void FixedUpdate()
    {
        if (isBubbled)
        {
            this.transform.localPosition = new Vector3(0, 0, 0);
        }

        if (breakFree <= 0)
        {
            Debug.Log("BROKE FREE");

            transform.parent = null;
            isBubbled = false;

            rb.useGravity = true;

            Destroy(createdBubbled);

            Physics.gravity = new Vector3(0, -400.0f, 0);

            animator.SetBool("isFalling", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isIdle", false);

            StartCoroutine(ResetGravity(1));

            breakFree = 100;
        }
    }

    IEnumerator ResetGravity(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Physics.gravity = new Vector3(0, -9.81f, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BubbledTag")
        {
            Physics.IgnoreCollision(collision.collider, myCollider);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Bubble(Clone)" && !createdBubbled)
        {
            Debug.Log("Bubbled");

            rb.useGravity = false;
            createdBubbled = (GameObject)Instantiate(bubbled, new Vector3(this.transform.position.x, this.transform.position.y + 1f, this.transform.position.z), Quaternion.Euler(this.transform.position));
            createdBubbled.GetComponent<Rigidbody>().AddForce(other.transform.InverseTransformDirection(other.gameObject.GetComponent<Rigidbody>().velocity) * 500);
            createdBubbled.transform.Find("BubbleShot").GetComponent<BubbleShotController>().attachedPlayer = this.gameObject;
            this.transform.SetParent(createdBubbled.transform);

            GameObject gotBubbled = Instantiate(gotBubbledEffect, new Vector3(this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z), Quaternion.Euler(this.transform.position));
            Destroy(gotBubbled, 1);

            animator.SetBool("isFalling", true);
            animator.SetBool("isRunning", false);
            animator.SetBool("isIdle", false);

            isBubbled = true;
        }

        if (other.tag == "DeathZone")
        {
            GameObject destroyed = Instantiate(destroyedEffect, new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z), Quaternion.Euler(this.transform.position));
            Destroy(destroyed, 1);
            Destroy(gameObject);
        }
    }
}
