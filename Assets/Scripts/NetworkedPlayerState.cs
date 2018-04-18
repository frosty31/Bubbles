using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedPlayerState : NetworkBehaviour {

    public void UpdateBoardPlayerTransform(GameObject boardPlayer, Vector3 newPosition, Quaternion newRotation)
    {
        CmdUpdateBoardPlayerTransform(boardPlayer, newPosition, newRotation);
    }

    [Command]
    public void CmdUpdateBoardPlayerTransform(GameObject boardPlayer, Vector3 newPosition, Quaternion newRotation)
    {
        SpriteController spriteController = boardPlayer.GetComponent<SpriteController>();

        if (!spriteController.iAmLocalPlayer)
        {
            spriteController.localPosition = newPosition;
            spriteController.localRotation = newRotation;
        }
    }
}
