﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LudoPathObjectController : MonoBehaviour
{

    public List<GameObject> pawns = new List<GameObject>();
    public bool isProtectedPlace;
    public GameGUIController Guicontroller;
    // Use this for initialization
    void Start()
    {
        Guicontroller = FindObjectOfType<GameGUIController>();
        GetComponent<Image>().enabled = false;
    }

    public void AddPawn(GameObject pawn)
    {
        pawns.Add(pawn);
        if (isProtectedPlace)
            Guicontroller.StarSound();
       
    }


    public void RemovePawn(GameObject pawn)
    {
        pawns.Remove(pawn);
    }

    public void CheckIfActuallyHasPawn()
    {
        for (int i = 0; i < pawns.Count; i++)
        {
            LudoPawnController pawn = pawns[i].GetComponent<LudoPawnController>();
            if (this != pawn.path[pawn.currentPosition].GetComponent<LudoPathObjectController>())
            {
                Debug.LogWarning("PATH BUG FIXED YAY!");
                RemovePawn(pawn.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int SiblingIndex
    {
        get
        {
            return transform.GetSiblingIndex();
        }
    }
}
