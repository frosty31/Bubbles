using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbledController : MonoBehaviour
{
    Collider myCollider;

    private void Start()
    {
        myCollider = GetComponent<SphereCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BubbledTag")
        {
            Physics.IgnoreCollision(collision.collider, myCollider);
        }
    }
}
