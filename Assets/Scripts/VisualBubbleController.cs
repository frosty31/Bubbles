using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualBubbleController : MonoBehaviour {

    public GameObject attachedPlayer;
	
	// Update is called once per frame
	void Update () {
        if (!attachedPlayer)
        {
            Destroy(this.gameObject);
        }
        this.transform.localPosition = attachedPlayer.transform.localPosition;
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1.25f, this.transform.position.z);
	}
}
