﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LudoPawnController : MonoBehaviour
{

    public AudioSource killedPawnSound;
    public AudioSource inHomeSound;
    public GameObject inHomeAnimationObj;
    public GameObject pawnTop;
    public GameObject pawnTopMultiple;

    public GameObject dice;
    private GameDiceController diceController;
    public GameObject pawnInJoint = null;
    public bool mainInJoint = false;
    public GameObject highlight;

    public bool isOnBoard = false;

    private LudoGameController ludoController;

    public RectTransform[] path;
    public int currentPosition = -1;

    private float singlePathSpeed = 0.25f;
    private float MoveToStartPositionSpeed = 0.25f;
    private RectTransform rect;
    private Vector3 initScale;

    public bool isMinePawn = false;

    public int index;
    public bool myTurn;

    //[HideInInspector]
    public int playerIndex;
    public AudioSource[] sound;
    public RectTransform initPosition;
    private bool canMakeJoint = false;

    GameObject BoardParentobj;

    private int currentAudioSource = 0;

    GameGUIController guiCntrlr;

    void Start()
    {

        BoardParentobj = GameObject.Find("ForgroundCanvas");
        //Debug.Log("Game mode: " + GameManager.Instance.mode.ToString());
        diceController = dice.GetComponent<GameDiceController>();
        ludoController = GameObject.Find("GameSpecific").GetComponent<LudoGameController>();
        guiCntrlr = FindObjectOfType<GameGUIController>();
        rect = GetComponent<RectTransform>();
        initScale = rect.localScale;
        initPosition = rect;

        GetComponent<Button>().interactable = false;

        if (GameManager.Instance.mode == MyGameMode.Master)
        {
            canMakeJoint = true;
        }
    }

    public void setPlayerIndex(int index)
    {
        this.playerIndex = index;
    }

    public void Highlight(bool active)
    {
        //
        if (GameManager.Instance.currentPlayer.isBot && GameManager.Instance.isPlayingWithComputer)
        {
            if (active)
            {
                GetComponent<Button>().interactable = true;
                GetComponent<Button>().enabled = true;
                //gameObject.transform.SetAsLastSibling();
                highlight.SetActive(true);
                rect.localScale = new Vector3(initScale.x, initScale.y, initScale.z);
            }
            else
            {
                GetComponent<Button>().interactable = false;
                GetComponent<Button>().enabled = false;
                highlight.SetActive(false);
                if (currentPosition >= 0)
                {
                    CheckIfPawnInJoint();
                }
            }

            // gameObject.transform.SetAsFirstSibling();
        }
        else
        {
            if (active)
            {

                GetComponent<Button>().interactable = true;
                GetComponent<Button>().enabled = true;
                //gameObject.transform.SetAsLastSibling();
                highlight.SetActive(true);
                rect.localScale = new Vector3(initScale.x, initScale.y, initScale.z);

            }
            else
            {
                GetComponent<Button>().interactable = false;
                GetComponent<Button>().enabled = false;
                highlight.SetActive(false);
                if (currentPosition >= 0)
                {
                    CheckIfPawnInJoint();
                }

                //gameObject.transform.SetAsFirstSibling();

            }
        }

    }

    public int GetMoveScore(int steps)
    {
        LudoPathObjectController pathControl = path[currentPosition + steps].GetComponent<LudoPathObjectController>();
        if (isOnBoard)
        {
            // finish
            if (currentPosition + steps == path.Length - 1)
            {
                return 900;
            }

            //KILL
            pathControl.CheckIfActuallyHasPawn();
            if (pathControl.pawns.Count > 0)
            {
                for (int i = 0; i < pathControl.pawns.Count; i++)
                {
                    if (pathControl.pawns[i].GetComponent<LudoPawnController>().playerIndex != playerIndex)
                    {
                        return 1000;
                    }
                }
            }
        }
        if (steps == 6 && !isOnBoard)
        {
            return 800;
        }
        else
        {
            if (isOnBoard)
            {
                if (!path[currentPosition].GetComponent<LudoPathObjectController>().isProtectedPlace)
                {
                    for (int i = 1; i < 7; i++)
                    {
                        if (currentPosition - i < 0) Debug.Log("Do nothing");
                        else if (path[currentPosition - i].GetComponent<LudoPathObjectController>().pawns.Count > 0)
                        {
                            for (int j = 0; j < path[currentPosition - i].GetComponent<LudoPathObjectController>().pawns.Count; j++)
                            {
                                if (path[currentPosition - i].GetComponent<LudoPathObjectController>().pawns[j].GetComponent<LudoPawnController>().playerIndex != playerIndex)
                                {
                                    return 700;
                                }
                            }
                        }
                    }
                }

                // safe place
                if (!path[currentPosition].GetComponent<LudoPathObjectController>().isProtectedPlace && path[currentPosition + steps].GetComponent<LudoPathObjectController>().isProtectedPlace)
                {
                    Debug.Log("SoundPlay1");

                    return 600;
                }

                //!OPP & (SWBB!! SWBNB)
                if (!path[currentPosition].GetComponent<LudoPathObjectController>().isProtectedPlace)
                {
                    bool isSomeoneBehind = false;
                    for (int i = 1; i < 7; i++)
                    {
                        if (currentPosition + steps - i < 0) Debug.Log("Do nothing");
                        else if (path[currentPosition + steps - i].GetComponent<LudoPathObjectController>().pawns.Count > 0)
                        {
                            for (int j = 0; j < path[currentPosition + steps - i].GetComponent<LudoPathObjectController>().pawns.Count; j++)
                            {
                                if (path[currentPosition + steps - i].GetComponent<LudoPathObjectController>().pawns[j].GetComponent<LudoPawnController>().playerIndex != playerIndex)
                                {
                                    isSomeoneBehind = true;
                                }
                            }
                        }
                    }
                    if (!isSomeoneBehind) return 500;
                    else return -100;
                }


                //OPP & (SWBB!! SWBNB)
                if (path[currentPosition].GetComponent<LudoPathObjectController>().isProtectedPlace)
                {
                    bool isSomeoneBehind = false;
                    for (int i = 1; i < 7; i++)
                    {
                        if (currentPosition + steps - i < 0) Debug.LogError("Do nothing");
                        else  if (path[currentPosition + steps - i].GetComponent<LudoPathObjectController>().pawns.Count > 0)
                        {
                            for (int j = 0; j < path[currentPosition + steps - i].GetComponent<LudoPathObjectController>().pawns.Count; j++)
                            {
                                if (path[currentPosition + steps - i].GetComponent<LudoPathObjectController>().pawns[j].GetComponent<LudoPawnController>().playerIndex != playerIndex)
                                {
                                    isSomeoneBehind = true;
                                }
                            }
                        }
                    }
                    if (!isSomeoneBehind) return 400;
                    else return -200;
                }


                if (GameManager.Instance.mode == MyGameMode.Quick && GameManager.Instance.currentPlayer.canEnterHome)
                {
                    return 300;
                }

                if (pawnInJoint != null)
                {
                    steps = steps / 2;
                }
                //LudoPathObjectController pathControl = path[currentPosition + steps].GetComponent<LudoPathObjectController>();


                // joint
                pathControl.CheckIfActuallyHasPawn();
                if (pathControl.pawns.Count > 0)
                {
                    for (int i = 0; i < pathControl.pawns.Count; i++)
                    {
                        if (pathControl.pawns[i].GetComponent<LudoPawnController>().playerIndex == playerIndex)
                        {
                            return -300;
                        }
                    }
                }


                

                //if (path[currentPosition].GetComponent<LudoPathObjectController>().isProtectedPlace)
                //{
                //    Debug.Log("SoundPlay2");
                //    return 100;
                //}

            }
        }
        return 0;
    }

    public bool CheckIfCanMove(int steps)
    {
        if (steps == 6 && !isOnBoard)
        {
            Highlight(true);
            return true;
        }
        else
        {
            if (isOnBoard)
            {

                if (pawnInJoint != null)
                {
                    if (steps % 2 != 0)
                        return false;
                    else
                    {
                        steps = steps / 2;
                    }
                }

                if (currentPosition + steps < path.Length)
                {
                    LudoPathObjectController pathControl = path[currentPosition + steps].GetComponent<LudoPathObjectController>();

                    Debug.Log("pawns count on destination: " + pathControl.pawns.Count);
                    if (pathControl.pawns.Count == 2 && pathControl.pawns[0].GetComponent<LudoPawnController>().pawnInJoint != null)
                    {
                        Debug.Log("im inside");
                        if (pawnInJoint != null)
                        {
                            Debug.Log("return true");
                            if (pathControl.pawns[0].GetComponent<LudoPawnController>().playerIndex != playerIndex)
                            {
                                Highlight(true);
                                return true;
                            }
                            else return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                for (int i = 1; i < steps + 1; i++)
                {
                    if (currentPosition + i < path.Length)
                    {
                        Debug.Log("check count: " + path[currentPosition + i].GetComponent<LudoPathObjectController>().pawns.Count);
                        if (path[currentPosition + i].GetComponent<LudoPathObjectController>().pawns.Count > 1)
                        {
                            Debug.Log("more than 1");
                            if (path[currentPosition + i].GetComponent<LudoPathObjectController>().pawns[0].GetComponent<LudoPawnController>().pawnInJoint != null)
                            {
                                Debug.Log("blockade");
                                return false;
                            }
                        }
                    }
                }

                if (currentPosition == path.Length - 1 || currentPosition + steps > path.Length - 1)
                {
                    return false;
                }

                if ((currentPosition + steps > path.Length - 1 - 6) &&
                    GameManager.Instance.needToKillOpponentToEnterHome &&
                    !GameManager.Instance.playerObjects[playerIndex].canEnterHome)
                {
                    return false;
                }

                Highlight(true);
                return true;
            }
            else
            {
                Highlight(true);
                return true;
            }
        }
        return false;
    }

    public void GoToStartPosition()
    {
        canCallFinish = true;
        rect.SetAsLastSibling();
        currentPosition = 0;
        if (GameManager.Instance.type == MyGameType.TwoPlayer && !GameManager.Instance.isLocalMultiplayer)
        {
            StartCoroutine(MoveDelayed1(0, initPosition, path[currentPosition], MoveToStartPositionSpeed, true, true));
        }
        else
        {
            StartCoroutine(MoveDelayed1(0, initPosition, path[currentPosition], MoveToStartPositionSpeed, true, true));
        }

        if (pawnInJoint != null)
        {
            pawnInJoint.GetComponent<LudoPawnController>().pawnInJoint = null;
            pawnInJoint.GetComponent<LudoPawnController>().GoToStartPosition();
            pawnInJoint = null;
        }
    }

    public void GoToInitPosition(bool callEnd)
    {
        canCallFinish = true;
       // killedPawnSound.Play();
        rect.SetAsLastSibling();
        if (true)
        {
            isOnBoard = false;
            currentPosition = 0;
        }
        else
        {
            isOnBoard = false;
            currentPosition = -1;
        }
        //pawnTop.SetActive(true);
        pawnTopMultiple.SetActive(false);
        StartCoroutine(MoveDelayed(0, rect, initPosition, MoveToStartPositionSpeed, true, false));
        if (pawnInJoint != null)
        {
            pawnInJoint.GetComponent<LudoPawnController>().pawnInJoint = null;
            pawnInJoint.GetComponent<LudoPawnController>().GoToInitPosition(true);
            pawnInJoint = null;
        }
        //path[currentPosition].GetComponent<LudoPathObjectController>().RemovePawn(this.gameObject);
    }

    public void MoveBySteps(int steps)
    {
        Debug.Log("currentPosition   " + currentPosition);
        if (currentPosition == -1) return;
        LudoPathObjectController controller = path[currentPosition].GetComponent<LudoPathObjectController>();
        controller.CheckIfActuallyHasPawn();

        controller.RemovePawn(this.gameObject);
        RepositionPawns(controller.pawns.Count, currentPosition);

        if (controller.pawns.Count == 1)
        {
            controller.pawns[0].GetComponent<LudoPawnController>().pawnInJoint = null;
            controller.pawns[0].GetComponent<LudoPawnController>().rect.localScale = new Vector3(initScale.x, initScale.y, initScale.z);
        }

        rect.SetAsLastSibling();
        //Debug.LogError("Moving steps slowlyyy::::");
        StartCoroutine(MoveStepsSlowly(steps));

    }

    void CheckIfPawnInJoint()
    {
        LudoPathObjectController pathController = path[currentPosition].GetComponent<LudoPathObjectController>();
        Debug.Log("currentPosition   " + currentPosition + "   count  " + pathController.pawns.Count);
        pathController.CheckIfActuallyHasPawn();
        if (pathController.pawns.Count > 1)
        {
            RepositionPawns(pathController.pawns.Count, currentPosition);
        }
    }

    void BackToHome()
    {
        Debug.Log("currentPosition   " + currentPosition);
        StartCoroutine(MoveStepsInBackSlowly(currentPosition));
    }

    IEnumerator MoveStepsInBackSlowly(int steps)
    {
        if (steps == 1 && currentPosition == 1)
        {
            StartCoroutine(MoveDelayedBackForth(1, path[currentPosition].anchoredPosition, path[currentPosition - 1].anchoredPosition, 0.0001f, false, true, path[currentPosition - 1]));
        }
        for (int i = 0; i < steps; i++)
        {
            bool last = false;
            if (i == steps - 1) last = true;

            currentPosition--;

            if (currentPosition == 0)
            {
                AddInJail();
            }
            else
            {
                StartCoroutine(MoveDelayedBackForth(i, path[currentPosition].anchoredPosition, path[currentPosition - 1].anchoredPosition, 0.0001f, last, true, path[currentPosition-1]));
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    public int CurrentPosition
    {
        get
        {
            return currentPosition;
        }
    }

    bool canCallFinish = true;

    IEnumerator MoveStepsSlowly(int steps)
    {
        canCallFinish = true;
        for (int i = 0; i < steps; i++)
        {
            bool last = false;
            if (i == steps - 1) last = true;

            currentPosition++;
            StartCoroutine("Particle");
            //Debug.LogError("BRUHHH::: " + last);
            yield return StartCoroutine(MoveDelayed(i, path[currentPosition - 1], path[currentPosition], singlePathSpeed, last, true));


            // yield return new WaitForSeconds(0.04f);

            // rect.localScale = new Vector3 (initScale.x, initScale.y, initScale.z);
            // yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator MoveDelayed1(int delay, RectTransform from, RectTransform to, float time, bool last, bool playSound)
    {

        //  rect.localScale = new Vector3(initScale.x * 1.25f, initScale.y * 1.25f, initScale.z);
        StartCoroutine("ScalingEffect");
        yield return new WaitForSeconds(0);

        if (playSound)
        {
            sound[currentAudioSource % sound.Length].Play();
            currentAudioSource++;
        }
        StopCoroutine("UpScalingAnimation");
        StopCoroutine("DeScalingAnimation");
        rect.localScale = new Vector3(1f, 1f, 1);
        DavMaster.CopyRectTransform(to, rect);
        //rect = to;
        Debug.Log(rect + "rect");
        Debug.Log(to  + "to");

        //Vector2 diff = to - from;
        //float x = diff.x / .17f;
        //float y = diff.y / .17f;
        //float epsilon = 5f;

        //Debug.Log("diff    " + diff + "  x    " + x + "   y  " + y);
        //float lastValue = Vector2.Distance(to, rect.anchoredPosition);
        //while (Vector2.Distance(to, rect.anchoredPosition) > (epsilon))
        //{
        //    rect.anchoredPosition += new Vector2(x * Time.deltaTime, y * Time.deltaTime);
        //    float gap = Vector2.Distance(to, rect.anchoredPosition);
        //    if (gap < lastValue)
        //    {
        //        lastValue = gap;
        //    }
        //    else
        //        break;
        //    yield return null;
        //}
        //StopCoroutine("UpScalingAnimation");
        //StopCoroutine("DeScalingAnimation");
        //rect.localScale = new Vector3(1f, 1f, 1);
        //rect.anchoredPosition = to;
        if (last)
        {
            //MoveFinished();
        }


        // if (last)
        // {
        //     iTween.ValueTo(gameObject, iTween.Hash("from", from, "to", to, "time", time, "easetype", iTween.EaseType.linear, "onupdate", "UpdatePosition", "oncomplete", "MoveFinished"));
        // }
        // else
        // {
        //     iTween.ValueTo(gameObject, iTween.Hash("from", from, "to", to, "time", time, "easetype", iTween.EaseType.linear, "onupdate", "UpdatePosition"));
        // }

    }

    IEnumerator Particle()
    {
        yield return new WaitForSeconds(0.07f);
        GameObject obj = Instantiate(Getprefab());
        obj.transform.SetParent(BoardParentobj.transform, false);
        obj.transform.localScale = Vector3.one;
        Vector2 pos = path[currentPosition - 1].anchoredPosition;
        obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(pos.x, pos.y, 1);
    }

    GameObject Getprefab()
    {
        switch (pawnTop.GetComponent<Image>().sprite.name)
        {
            case "01":
                return ludoController.BluePrefab;
            case "02":
                return ludoController.RedPrefab;
            case "03":
                return ludoController.GreenPrefab;
            case "04":
                return ludoController.YellowPrefab;
            default:
                return ludoController.BluePrefab;
        }
    }

    public void MakeMove()
    {
        if (guiCntrlr.canPlayGame)
        {
            Debug.Log("Make move button");
            string data = index + ";" + ludoController.gUIController.GetCurrentPlayerIndex() + ";" + ludoController.steps;
            Debug.Log("Ludo Steps" + ludoController.steps);
            Debug.Log("current player index   " + ludoController.gUIController.GetCurrentPlayerIndex());
            Debug.Log(playerIndex);
            //Debug.Log(GameManager.Instance.);
            if (playerIndex == GameManager.Instance.myPlayerIndex || (GameManager.Instance.isLocalMultiplayer && ludoController.gUIController.GetCurrentPlayerIndex() == playerIndex))
            {
                //Debug.LogError("MAKE MOVE: " + ludoController.steps);
                if (false)
                {
                    dice.GetComponent<GameDiceController>().IncreaseScore(ludoController.steps);
                }
                // Debug.Log("Make move button");
                // string data = index + ";" + ludoController.gUIController.GetCurrentPlayerIndex() + ";" + ludoController.steps;
                // Debug.Log("data   " + data);

                PhotonNetwork.RaiseEvent((int)EnumGame.PawnMove, data, true, null);

                if (pawnInJoint != null) ludoController.steps /= 2;
                GameManager.Instance.diceShot = true;
                myTurn = true;
                ludoController.gUIController.PauseTimers();
                ludoController.Unhighlight();

                if (!isOnBoard)
                {
                    GoToStartPosition();
                }
                if (pawnInJoint != null)
                {
                    pawnInJoint.GetComponent<LudoPawnController>().MoveBySteps(ludoController.steps);
                }
                //Debug.LogError("111");
                MoveBySteps(ludoController.steps);
                isOnBoard = true;
            }
        }
    }

    public void MakeMoveManually(int steps)
    {
        string data = index + ";" + ludoController.gUIController.GetCurrentPlayerIndex() + ";" + steps;
        PhotonNetwork.RaiseEvent((int)EnumGame.PawnMove, data, true, null);

        if (pawnInJoint != null) steps /= 2;
        GameManager.Instance.diceShot = true;
        myTurn = true;
        ludoController.gUIController.PauseTimers();
        //ludoController.Unhighlight();

        if (!isOnBoard)
        {
            GoToStartPosition();
        }
        //else
        //{
            if (pawnInJoint != null)
            {
                pawnInJoint.GetComponent<LudoPawnController>().MoveBySteps(steps);
            }
            MoveBySteps(steps);
        //}
        isOnBoard = true;
    }

    public void MakeMovePC()
    {
        if (pawnInJoint != null) ludoController.steps /= 2;

        myTurn = false;
        ludoController.gUIController.PauseTimers();
        
        if (!isOnBoard)
        {
            GoToStartPosition();
        }

        if (pawnInJoint != null)
        {
            pawnInJoint.GetComponent<LudoPawnController>().MoveBySteps(ludoController.steps);
        }
        if (false)
        {
            dice.GetComponent<GameDiceController>().IncreaseScore(ludoController.steps);
        }
        MoveBySteps(ludoController.steps);

        isOnBoard = true;
    }

    private IEnumerator MoveDelayedBackForth(int delay, Vector2 from, Vector2 to, float time, bool last, bool playSound, RectTransform targettransform)
    {

        rect.localScale = new Vector3(initScale.x * 1.15f, initScale.y * 1.15f, initScale.z);
        DavMaster.CopyRectTransform(targettransform, rect);
        if (last) AddInJail();
        yield return new WaitForSeconds(0);

        if (false)
        {
            if (last)
            {
                iTween.ValueTo(gameObject, iTween.Hash("from", from, "to", to, "time", time, "easetype", iTween.EaseType.linear, "onupdate", "UpdatePosition", "oncomplete", "AddInJail"));
            }
            else
            {
                iTween.ValueTo(gameObject, iTween.Hash("from", from, "to", to, "time", time, "easetype", iTween.EaseType.linear, "onupdate", "UpdatePosition"));
            }
        }

    }

    private IEnumerator MoveDelayed(int delay, RectTransform from, RectTransform to, float time, bool last, bool playSound)
    {

        //  rect.localScale = new Vector3(initScale.x * 1.25f, initScale.y * 1.25f, initScale.z);
        StartCoroutine("ScalingEffect");
        yield return new WaitForSeconds(0.3f);

        if (playSound)
        {
            sound[currentAudioSource % sound.Length].Play();
            currentAudioSource++;
        }
        StopCoroutine("UpScalingAnimation");
        StopCoroutine("DeScalingAnimation");
        rect.localScale = new Vector3(1f, 1f, 1);
        DavMaster.CopyRectTransform(to, rect);
        //rect = to;
        //Debug.LogError(rect + "rect");
        //Debug.LogError(to + "to");
        //Vector2 diff = to - from;
        //float x = diff.x / .17f;
        //float y = diff.y / .17f;
        //float epsilon = 5f;

        //Debug.Log("diff    " + diff + "  x    " + x + "   y  " + y);
        //float lastValue = Vector2.Distance(to, rect.anchoredPosition);
        //while (Vector2.Distance(to, rect.anchoredPosition) > (epsilon))
        //{
        //    rect.anchoredPosition += new Vector2(x * Time.deltaTime, y * Time.deltaTime);
        //    float gap = Vector2.Distance(to, rect.anchoredPosition);
        //    if (gap < lastValue)
        //    {
        //        lastValue = gap;
        //    }
        //    else
        //        break;
        //    yield return null;
        //}
        //StopCoroutine("UpScalingAnimation");
        //StopCoroutine("DeScalingAnimation");
        //rect.localScale = new Vector3(1f, 1f, 1);
        //rect.anchoredPosition = to;
        if (last)
        {
            last = false;
            if (canCallFinish)
            {
                canCallFinish = false;
                Debug.LogWarning("CUURRENT POSITIIOn: " + currentPosition);
                MoveFinished();
            }
        }


        // if (last)
        // {
        //     iTween.ValueTo(gameObject, iTween.Hash("from", from, "to", to, "time", time, "easetype", iTween.EaseType.linear, "onupdate", "UpdatePosition", "oncomplete", "MoveFinished"));
        // }
        // else
        // {
        //     iTween.ValueTo(gameObject, iTween.Hash("from", from, "to", to, "time", time, "easetype", iTween.EaseType.linear, "onupdate", "UpdatePosition"));
        // }

    }

    IEnumerator ScalingEffect()
    {
        yield return new WaitForSeconds(0.02f);
        float value = 1.5f;
        yield return (StartCoroutine(UpScalingAnimation(transform.gameObject, new Vector2(1.2f, 1.2f), value)));

        StartCoroutine(DeScalingAnimation(transform.gameObject, new Vector2(1f, 1f), value));

    }


    IEnumerator UpScalingAnimation(GameObject obj, Vector2 targetScale, float value)
    {
        Debug.Log("Time.deltaTime             " + Time.deltaTime);
        while (rect.localScale.x < targetScale.x)
        {
            rect.localScale = new Vector3(rect.localScale.x + (value * Time.deltaTime), rect.localScale.y + (value * Time.deltaTime), 1);
            yield return null;
        }

    }

    IEnumerator DeScalingAnimation(GameObject obj, Vector2 targetScale, float value)
    {
        //while (rect.localScale.x > targetScale.x)
        //{
        //    rect.localScale = new Vector3(rect.localScale.x - (value * Time.deltaTime), rect.localScale.y - (value * Time.deltaTime), 1);
        //    yield return null;
        //}
        yield return new WaitForSeconds(0);
        rect.localScale = new Vector3(1f, 1f, 1);
        Debug.Log("scaling Done   " + rect.localScale);
    }


    private void resetScale()
    {
        rect.localScale = initScale;
    }

    void AddInJail()
    {
        GameManager.Instance.currentPlayer.dice.GetComponent<GameDiceController>().EnableShot();
        GoToInitPosition(false);
    }

    private void MoveFinished()
    {
        //Debug.LogError("Move finished:::");
        if (currentPosition != 0) 
        {
            if (!FindObjectOfType<GameGUIController>().stopIncreasingScore && FindObjectOfType<GameGUIController>().hasSynchronized)
            {
                dice.GetComponent<GameDiceController>().IncreaseScore(ludoController.steps);
            }
            else FindObjectOfType<GameGUIController>().stopIncreasingScore = false;
        }
        resetScale();
        bool isKilled = false;

        if (currentPosition >= 0 || true)
        {
            bool canSendFinishTurn = true;

            LudoPathObjectController pathController = path[currentPosition].GetComponent<LudoPathObjectController>();

            pathController.AddPawn(this.gameObject);

            if (pawnInJoint == null || (pawnInJoint != null && mainInJoint))
            {
                Debug.Log("Main in joint");
                pathController.CheckIfActuallyHasPawn();
                int otherCount = pathController.pawns.Count;

                Debug.Log("Pawns count: " + otherCount);

                if (!pathController.isProtectedPlace)
                {
                    if (otherCount > 1) // Check and remove opponent pawns to home
                    {
                        for (int i = otherCount - 2; i >= 0; i--)
                        {
                            if (pathController.pawns[i].GetComponent<LudoPawnController>().playerIndex != playerIndex)
                            {
                                int color = pathController.pawns[i].GetComponent<LudoPawnController>().playerIndex;
                                // Coutn pawns in this color
                                int pawnsInColor = 0;
                                for (int k = 0; k < otherCount; k++)
                                {
                                    if (pathController.pawns[k].GetComponent<LudoPawnController>().playerIndex == color)
                                    {
                                        pawnsInColor++;
                                    }
                                }

                                if (pawnsInColor == 1 || canMakeJoint)
                                {
                                    isKilled = true;
                                    Debug.LogWarning("KILLED");
                                    // Killed opponent pawn, Additional turn
                                    // for new 
                                    ludoController.nextShotPossible = true;
                                    GameManager.Instance.playerObjects[playerIndex].canEnterHome = true;
                                    GameManager.Instance.playerObjects[playerIndex].homeLockObjects.SetActive(false);
                                    Debug.Log("canMakeJoint  Pawn Movement");
                                    // Move killed pawn to start position and remove from list
                                    killedPawnSound.Play();
                                    pathController.pawns[i].GetComponent<LudoPawnController>().dice.GetComponent<GameDiceController>().DecreaseScore(pathController.pawns[i].GetComponent<LudoPawnController>().currentPosition);
                                    pathController.pawns[i].GetComponent<LudoPawnController>().BackToHome();
                                   // pathController.pawns[i].GetComponent<LudoPawnController>().GoToInitPosition(t);

                                    pathController.RemovePawn(pathController.pawns[i]);
                                }
                            }
                            else
                            {
                                if (canMakeJoint && pawnInJoint == null)
                                {
                                    Debug.Log("Joint");
                                    pawnInJoint = pathController.pawns[i];
                                    mainInJoint = true;
                                    pathController.pawns[i].GetComponent<LudoPawnController>().mainInJoint = false;
                                    pathController.pawns[i].GetComponent<LudoPawnController>().pawnInJoint = this.gameObject;
                                    // pawnTop.SetActive(false);
                                    pawnTopMultiple.SetActive(true);
                                    //pathController.pawns[i].GetComponent<LudoPawnController>().pawnTop.SetActive(false);
                                    pathController.pawns[i].GetComponent<LudoPawnController>().pawnTopMultiple.SetActive(true);
                                }
                            }
                        }

                    }
                }
                else
                {
                    if (pawnInJoint != null)
                    {
                        canSendFinishTurn = false;
                        //pawnTop.SetActive(true);
                        pawnTopMultiple.SetActive(false);
                        // pawnInJoint.GetComponent<LudoPawnController>().pawnTop.SetActive(true);
                        pawnInJoint.GetComponent<LudoPawnController>().pawnTopMultiple.SetActive(false);

                        pawnInJoint.GetComponent<LudoPawnController>().pawnInJoint = null;
                        pawnInJoint = null;
                    }
                }
                pathController.CheckIfActuallyHasPawn();
                otherCount = pathController.pawns.Count;

                if (pawnInJoint == null && otherCount > 1)
                {
                    Debug.LogWarning("Repositioning pawns");
                    RepositionPawns(otherCount, currentPosition);
                }
                if (currentPosition == path.Length - 1)
                {
                    StartCoroutine(PawnHomeAnimation());
                    inHomeSound.Play();
                }

                if(currentPosition == path.Length - 1)
                {
                    GameManager.Instance.currentPlayer.dice.GetComponent<GameDiceController>().score += 56;
                    GameManager.Instance.currentPlayer.dice.GetComponent<GameDiceController>().myScore.text =
                    GameManager.Instance.currentPlayer.dice.GetComponent<GameDiceController>().score.ToString();
                    guiCntrlr.addedScorePopup.SetActive(true);
                    StartCoroutine(DavMaster.EnableDisableWithDelay(guiCntrlr.addedScorePopup, 2, false));
                }

                if ((myTurn || GameManager.Instance.currentPlayer.isBot) && currentPosition == path.Length - 1)
                {
                    Debug.Log("FINISHSSSS");

                    GameManager.Instance.currentPlayer.finishedPawns++;
                        //ludoController.finishedPawns++;
                    if (GameManager.Instance.mode == MyGameMode.Quick)
                    {
                        if (GameManager.Instance.currentPlayer.finishedPawns == 1)
                        {
                            ludoController.gUIController.FinishedGame();
                            return;
                        }
                    }
                    else
                    {
                        if (GameManager.Instance.currentPlayer.finishedPawns == 4)
                        {
                            ludoController.gUIController.FinishedPawnSoFinishGame();
                            return;
                        }
                    }
                    ludoController.nextShotPossible = true;
                }

                if (((myTurn && GameManager.Instance.diceShot) || GameManager.Instance.currentPlayer.isBot) && canSendFinishTurn)
                {
                    if (ludoController.nextShotPossible)
                    {
                        if (!isKilled)
                        {
                            Debug.Log("dice Shot");
                            GameManager.Instance.currentPlayer.dice.GetComponent<GameDiceController>().EnableShot();
                            ludoController.gUIController.restartTimer();
                        }

                    }
                    else
                    {
                        Debug.Log("move finished call finish turn");

                        StartCoroutine(CheckTurnDelay());
                        // else
                        //     ludoController.gUIController.SendFinishTurn();
                    }
                }
                else
                {
                    ludoController.gUIController.restartTimer();
                }
            }
        }

    }

    IEnumerator PawnHomeAnimation()
    {
        inHomeAnimationObj.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        inHomeAnimationObj.SetActive(false);
    }

    private IEnumerator CheckTurnDelay()
    {
        Debug.Log("   isBot               " + GameManager.Instance.currentPlayer.isBot);
        if (GameManager.Instance.currentPlayer.isBot)
        {
            ludoController.Unhighlight();
        }
        yield return new WaitForSeconds(0.3f);
        ludoController.gUIController.SendFinishTurn();


    }

    private void RepositionPawns(int otherCount, int currentPosition)
    {
        LudoPathObjectController pathController = path[currentPosition].GetComponent<LudoPathObjectController>();

        float scale = 0.8f;
        float offset = 20f / otherCount;
        float startPos = 0;

        startPos = (-offset / 2) * otherCount + offset / 2;
        scale -= 0.05f * otherCount + 0.05f;
        Debug.Log("other Count  " + otherCount);
        /*if (otherCount == 1)
        {
            startPos = 0;
            scale = 1;
        }
        else if (otherCount == 2)
        {
            startPos = -offset / 2;
            scale = 0.95f;
        }
        else if (otherCount == 3)
        {
            startPos = -offset;
            scale = 0.85f;
        }
        else if (otherCount == 4)
        {
            startPos = -offset * 1.5f;
            scale = 0.75f;
        }*/

        // Get my pawns, push on top of stack
        List<int> orderPawns = new List<int>();
        if (otherCount > 1)
        {
            for (int i = 0; i < otherCount; i++)
            {
                if (pathController.pawns[i].GetComponent<LudoPawnController>().playerIndex == GameManager.Instance.myPlayerIndex)
                {
                    orderPawns.Add(i);
                }
                else
                {
                    orderPawns.Insert(0, i);
                }
            }
            // Reposition pawns if more than 1 on spot
            for (int i = 0; i < otherCount; i++)
            {
                Debug.Log(" test    other Count  " + otherCount);
                RectTransform rT = pathController.pawns[orderPawns[i]].GetComponent<RectTransform>();
                pathController.pawns[orderPawns[i]].GetComponent<RectTransform>().anchoredPosition = new Vector2(
                    path[currentPosition].GetComponent<RectTransform>().anchoredPosition.x + startPos + i * offset,
                    path[currentPosition].GetComponent<RectTransform>().anchoredPosition.y);
                pathController.pawns[orderPawns[i]].GetComponent<RectTransform>().localScale = new Vector2(initScale.x * 0.9f, initScale.y * 0.9f);

                pathController.pawns[orderPawns[i]].GetComponent<RectTransform>().SetAsLastSibling();

            }
        }

        // }
    }

    private void UpdatePosition(Vector2 pos)
    {
        rect.anchoredPosition = pos;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.LogError("Current Position: " + currentPosition);
    }

    public int SiblingIndex
    {
        get
        {
            return path[CurrentPosition].GetComponent<LudoPathObjectController>().SiblingIndex;
        }
    }

    public void AddInstantly(int currentPos)
    {
        currentPosition = currentPos;
        if (currentPosition != -1)
        {
            isOnBoard = true;
            rect.anchoredPosition = path[CurrentPosition].anchoredPosition;
        }

    }
}