using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour {

    private void OnDestroy()
    {
        LogicManager.instance.localPlayerController.maxNumberOfBubbles += 1;
    }
}
