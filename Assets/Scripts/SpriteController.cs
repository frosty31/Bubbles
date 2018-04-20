using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

[NetworkSettings(sendInterval = 0.033f)]
public class SpriteController : NetworkBehaviour
{
    public GameObject MainCamera;

    public GameObject bubbled;

    public float movementSpeed;
    public float rotationSpeed;

    GameObject createdBubbled;

    public bool isBubbled;

    Rigidbody rb;
    Animator animator;
    Collider myCollider;
    private Vector3 vectorAtOrigin;

    public GameObject gotBubbledEffect;
    public GameObject destroyedEffect;

    public int breakFree = 100;

    public bool iAmLocalPlayer { get; set; }

    public NetworkedPlayerState myNetworkedPlayerState;

    /// <summary>
    /// Our position.
    /// </summary>
    [SyncVar]
    public Vector3 localPosition;

    /// <summary>
    /// Our rotation.
    /// </summary>
    [SyncVar]
    public Quaternion localRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        myCollider = GetComponent<CapsuleCollider>();

        myNetworkedPlayerState = LogicManager.instance.localPlayerObject.GetComponent<NetworkedPlayerState>();

        vectorAtOrigin = new Vector3(0, 0, 0);

        if (transform.parent)
        {
            PlayerController myPlayerController = transform.parent.GetComponent<PlayerController>();
            iAmLocalPlayer = myPlayerController.isLocalPlayer;
        }

#if UNITY_STANDALONE
        MainCamera = Camera.main.gameObject;
#else
        MainCamera = GameObject.Find("ARCore Device").transform.Find("Main Camera").gameObject;
#endif
    }

    private void Update()
    {
        if (!iAmLocalPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, localPosition, 0.3f);
            transform.rotation = localRotation;
            return;
        }

        // Depending on if you are host or client, either setting the SyncVar (client) 
        // or calling the Cmd (host) will update the other users in the session.
        // So we have to do both.
        localPosition = transform.position;
        localRotation = transform.rotation;

        // CmdTransform(localPosition, localRotation);
        // This conditional is the equivalent.
        myNetworkedPlayerState.UpdateBoardPlayerTransform(this.gameObject, localPosition, localRotation);
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

            // isBubbled = false;
            myNetworkedPlayerState.UpdateIsBubbledBool(this.gameObject, false);

            rb.useGravity = true;

            Destroy(createdBubbled);

            Physics.gravity = new Vector3(0, -400.0f, 0);

            /*
            animator.SetBool("isFalling", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isIdle", true);
            */
            myNetworkedPlayerState.UpdateBoardPlayerAnimator(this.gameObject,
                false, true, false);

            StartCoroutine(ResetGravity(1));

            myNetworkedPlayerState.ChangeBreakFreeNumber(this.gameObject, 100);
        }

        if (iAmLocalPlayer)
        {
#if UNITY_STANDALONE
            var x = Input.GetAxisRaw("Horizontal");
            var z = Input.GetAxisRaw("Vertical");
#else
            var x = CrossPlatformInputManager.GetAxis("Horizontal");
            var z = CrossPlatformInputManager.GetAxis("Vertical");
#endif

            // Vector3 vectorMovement = new Vector3(x, 0.0f, z);
            Vector3 vectorMovement = MainCamera.transform.forward * z + MainCamera.transform.right * x;
            if (vectorMovement != vectorAtOrigin)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vectorMovement), rotationSpeed);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            transform.Translate(vectorMovement * movementSpeed * Time.deltaTime, Space.World);

#if UNITY_STANDALONE
            if (Input.GetKey(KeyCode.UpArrow))
#else
            if (z > 0)
#endif
            {
                if (isBubbled)
                {
                    myNetworkedPlayerState.ChangeBreakFreeNumber(this.gameObject, breakFree - 1);
                } else
                {
                    myNetworkedPlayerState.UpdateBoardPlayerAnimator(this.gameObject,
                        true, false, false);
                }
            }
#if UNITY_STANDALONE
            else if (Input.GetKey(KeyCode.DownArrow))
#else
            else if (z < 0)
#endif
            {
                if (isBubbled)
                {
                    myNetworkedPlayerState.ChangeBreakFreeNumber(this.gameObject, breakFree - 1);
                }
                else
                {
                    myNetworkedPlayerState.UpdateBoardPlayerAnimator(this.gameObject,
                        true, false, false);
                }
            }
#if UNITY_STANDALONE
            else if (Input.GetKey(KeyCode.LeftArrow))
#else
            else if (x < 0)
#endif
            {
                if (isBubbled)
                {
                    myNetworkedPlayerState.ChangeBreakFreeNumber(this.gameObject, breakFree - 1);
                }
                else
                {
                    myNetworkedPlayerState.UpdateBoardPlayerAnimator(this.gameObject,
                        true, false, false);
                }
            }
#if UNITY_STANDALONE
            else if (Input.GetKey(KeyCode.RightArrow))
#else
            else if (x > 0)
#endif
            {
                if (isBubbled)
                {
                    myNetworkedPlayerState.ChangeBreakFreeNumber(this.gameObject, breakFree - 1);
                }
                else
                {
                    myNetworkedPlayerState.UpdateBoardPlayerAnimator(this.gameObject,
                        true, false, false);
                }
            }
            else
            {
                myNetworkedPlayerState.UpdateBoardPlayerAnimator(this.gameObject,
                    true, true, true);
            }
        }
        else
        {
            return;
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
            createdBubbled.transform.Find("VisualBubble").GetComponent<VisualBubbleController>().attachedPlayer = this.gameObject;
            this.transform.SetParent(createdBubbled.transform);

            GameObject gotBubbled = Instantiate(gotBubbledEffect, new Vector3(this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z), Quaternion.Euler(this.transform.position));
            Destroy(gotBubbled, 1);

            animator.SetBool("isFalling", true);
            animator.SetBool("isRunning", false);
            animator.SetBool("isIdle", false);

            /*
            myNetworkedPlayerState.UpdateBoardPlayerAnimator(this.gameObject,
                false, false, true);
            */

            // isBubbled = true;
            myNetworkedPlayerState.UpdateIsBubbledBool(this.gameObject, true);
        }

        if (other.tag == "DeathZone")
        {
            GameObject destroyed = Instantiate(destroyedEffect, new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z), Quaternion.Euler(this.transform.position));
            Destroy(destroyed, 1);
            Destroy(gameObject);
        }
    }
}
