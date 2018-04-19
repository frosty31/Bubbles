using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedPlayerState : NetworkBehaviour {

    public void UpdateIsBubbledBool(GameObject boardPlayer, bool newIsBubbledBool)
    {
        SpriteController spriteController = boardPlayer.GetComponent<SpriteController>();
        spriteController.isBubbled = newIsBubbledBool;

        if (isServer)
        {
            RpcUpdateIsBubbledBool(boardPlayer, newIsBubbledBool);
        }
        else
        {
            CmdUpdateIsBubbledBool(boardPlayer, newIsBubbledBool);
        }
    }

    [ClientRpc]
    public void RpcUpdateIsBubbledBool(GameObject boardPlayer, bool newIsBubbledBool)
    {
        SpriteController spriteController = boardPlayer.GetComponent<SpriteController>();
        spriteController.isBubbled = newIsBubbledBool;
    }

    [Command]
    public void CmdUpdateIsBubbledBool(GameObject boardPlayer, bool newIsBubbledBool)
    {
        SpriteController spriteController = boardPlayer.GetComponent<SpriteController>();
        spriteController.isBubbled = newIsBubbledBool;
    }

    public void ChangeBreakFreeNumber(GameObject boardPlayer, int newBreakFreeNumber)
    {
        SpriteController spriteController = boardPlayer.GetComponent<SpriteController>();
        spriteController.breakFree = newBreakFreeNumber;

        if (isServer)
        {
            RpcChangeBreakFreeNumber(boardPlayer, newBreakFreeNumber);
        }
        else
        {
            CmdChangeBreakFreeNumber(boardPlayer, newBreakFreeNumber);
        }
    }

    [ClientRpc]
    private void RpcChangeBreakFreeNumber(GameObject boardPlayer, int newBreakFreeNumber)
    {
        SpriteController spriteController = boardPlayer.GetComponent<SpriteController>();
        spriteController.breakFree = newBreakFreeNumber;
    }

    [Command]
    private void CmdChangeBreakFreeNumber(GameObject boardPlayer, int newBreakFreeNumber)
    {
        SpriteController spriteController = boardPlayer.GetComponent<SpriteController>();
        spriteController.breakFree = newBreakFreeNumber;
    }

    public void UpdateBoardPlayerAnimator(GameObject boardPlayer, bool isRunning, bool isIdle, bool isFalling)
    {
        // Do this locally first.
        Animator boardPlayerAnimator = boardPlayer.GetComponent<Animator>();

        if (isRunning && isIdle && isFalling)
        {
            isRunning = false;
            isIdle = true;
            isFalling = boardPlayerAnimator.GetBool("isFalling");
        }

        if (isServer)
        {
            boardPlayerAnimator.SetBool("isRunning", isRunning);
            boardPlayerAnimator.SetBool("isIdle", isIdle);
            boardPlayerAnimator.SetBool("isFalling", isFalling);

            RpcUpdateBoardPlayerAnimator(boardPlayer, isRunning, isIdle, isFalling);
        }
        else
        {
            CmdUpdateBoardPlayerAnimator(boardPlayer, isRunning, isIdle, isFalling);
        }
    }

    [ClientRpc]
    public void RpcUpdateBoardPlayerAnimator(GameObject boardPlayer, bool isRunning, bool isIdle, bool isFalling)
    {
        SpriteController spriteController = boardPlayer.GetComponent<SpriteController>();
        Animator boardPlayerAnimator = boardPlayer.GetComponent<Animator>();

        if (!spriteController.iAmLocalPlayer)
        {
            boardPlayerAnimator.SetBool("isRunning", isRunning);
            boardPlayerAnimator.SetBool("isIdle", isIdle);
            boardPlayerAnimator.SetBool("isFalling", isFalling);
        }
    }

    [Command]
    public void CmdUpdateBoardPlayerAnimator(GameObject boardPlayer, bool isRunning, bool isIdle, bool isFalling)
    {
        SpriteController spriteController = boardPlayer.GetComponent<SpriteController>();
        Animator boardPlayerAnimator = boardPlayer.GetComponent<Animator>();

        if (!spriteController.iAmLocalPlayer)
        {
            boardPlayerAnimator.SetBool("isRunning", isRunning);
            boardPlayerAnimator.SetBool("isIdle", isIdle);
            boardPlayerAnimator.SetBool("isFalling", isFalling);
        }
    }

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
