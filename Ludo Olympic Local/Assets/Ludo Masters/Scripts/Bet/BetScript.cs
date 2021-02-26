﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LitJson;

public class BetScript : MonoBehaviour
{
    public Text gamePriceText;
    public Text winPriceText;
    public TextMeshProUGUI totalJoinedPlayersText;
    public TextMeshProUGUI timeLeftText;
    public Button myJoiningButton;


    public string gameId;
    public string noOfPlayer;
    public string startDate;
    public string startTime;
    public string endDate;
    public string endTime;
    public string gamePrice;
    public string winPrice;
    public string gameDuration;
    public string firstPrize;
    public string secondPrize;
    public string thirdPrize;
    public string fourthPrize;
    public string totalPlayerJoined;
    public TextMeshProUGUI pubTitle;

    public string myRoomId;
    public string myFirstPlayerName;
    public string mySecondPlayerName;
    public string myThirdPlayerName;
    public string myFourthPlayerName;

    public bool isTablePlaying = false;
    public bool isJoined = false;

    public string timeLeft;

    public int Callingfunction;
    public int hr, mns, secs;

    public void SetTexts()
    {
        string rupeee = FindObjectOfType<InitMenuScript>().rupeeText.text;
        gamePriceText.text = rupeee + gamePrice;
        winPriceText.text = rupeee + winPrice;
        totalJoinedPlayersText.text = "Players Joined: " + totalPlayerJoined;
        //timeLeftText.text = gameDuration;
        if (noOfPlayer == "2")
        {
            pubTitle.text = "1v1 Battle";
        }
        else pubTitle.text = "3 Winners";
        if (isTablePlaying) SetTableAsPlaying();
        CheckIfJoined();
    }

    void CheckIfJoined()
    {
        List<ActiveGamInfo> temp;
        temp = GameManager.activeGameInfo;
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i].game_id == gameId)
            {
                myRoomId = temp[i].game_room_id;
                if (temp[i].first_Player_Name != null)
                {
                    myFirstPlayerName = temp[i].first_Player_Name;
                }
                if (temp[i].second_Player_Name!= null)
                {
                    mySecondPlayerName = temp[i].second_Player_Name;
                }
                if (temp[i].third_Player_Name != null)
                {
                    myThirdPlayerName = temp[i].third_Player_Name;
                }
                if (temp[i].fourth_Player_Name != null)
                {
                    myFourthPlayerName = temp[i].fourth_Player_Name;
                }
                myJoiningButton.transform.GetChild(0).GetComponent<Text>().text = "Joined";
                isJoined = true;
                break;
            }
        }

    }

    void SetTableAsPlaying()
    {
        isTablePlaying = true;
        timeLeftText.text = "Table Playing";
    }

    public void ToggleButtonPower(Toggle theToggle)
    {
        if (theToggle.IsInteractable()) ButtonPOwer();
    }

    public void ButtonPOwer()
    {
        ReferenceManager.refMngr.isOnlineBidSelected = true;
        FindObjectOfType<GameConfigrationController>().ChangeBettingAmount(Callingfunction);
        ReferenceManager.refMngr.ShowOnlineInvestment(GameManager.Instance.currentBetAmount, GameManager.Instance.currentWinningAmount);
    }

    public void EnterTable()
    {
        if (!isJoined)
        {
            float tempfloat = float.Parse(gamePrice);
            Debug.LogError("Amount: " + tempfloat);
            if (GameManager.Instance.coinsCount >= tempfloat)
            {
                GameManager.Instance.playfabManager.apiManager.isClickedPubButton = true;
                GameManager.Instance.playfabManager.apiManager.clickedBet = this;
                FindObjectOfType<APIManager>().tablevalue = gameId;
                GameManager.Instance.playfabManager.apiManager.DeductCoins(tempfloat);
            }
        }
        else
        {
            StartTable();
            //ReferenceManager.refMngr.ShowError("Already Joined Game", "Error");
        }
    }

    //[ContextMenu("Start Table Manually")]
    public void StartTable()
    {
        StartCoroutine(GetPlayerData());
    }

    void ActuallyStartTable()
    {
        ReferenceManager.refMngr.botsAdded.Clear();
        ReferenceManager.refMngr.botsAdded.Add(GameManager.Instance.nameMy);
        FindObjectOfType<GameConfigrationController>().SetTwoPlayerGameDav();
        ReferenceManager.refMngr.onlineNoOfPlayer = int.Parse(noOfPlayer);
        ReferenceManager.refMngr.onlineRoomId = myRoomId;
        ReferenceManager.refMngr.firstPlacePrize = firstPrize;
        ReferenceManager.refMngr.secondPlacePrize = secondPrize;
        ReferenceManager.refMngr.thirdPlacePrize = thirdPrize;
        GameManager.gameDuration = gameDuration;
        FindObjectOfType<InitMenuScript>().onlineGamePlayButton.onClick.Invoke();
    }

    IEnumerator GetPlayerData()
    {
        string url = GameManager.apiBase1 + "client_details/my_referral_code=" + GameManager.Instance.userID;
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null)
        {
            JsonData jsonvale = JsonMapper.ToObject(www.text);
            string activePlayer = jsonvale["result_push"][0]["active_game_info"].ToJson();
            activePlayer = "{\"result_push\":" + activePlayer + "}";
            JsonData jsonvale1 = JsonMapper.ToObject(activePlayer);
            for (int i = 0; i < jsonvale1["result_push"].Count; i++)
            {
                ActiveGamInfo temp = new ActiveGamInfo();
                temp.game_id = jsonvale1["result_push"][i]["game_id"].ToString();
                if (temp.game_id == gameId)
                {
                    myRoomId = jsonvale1["result_push"][i]["game_room_id"].ToString();
                    if (jsonvale1["result_push"][i]["first_player"].ToString() != null)
                    {
                        ReferenceManager.refMngr.onlinePlayersNames[0] = jsonvale1["result_push"][i]["first_player"].ToString();
                    }
                    if (jsonvale1["result_push"][i]["second_player"].ToString() != null)
                    {
                        ReferenceManager.refMngr.onlinePlayersNames[1] = jsonvale1["result_push"][i]["second_player"].ToString();
                    }
                    if (jsonvale1["result_push"][i]["third_player"].ToString() != null)
                    {
                        ReferenceManager.refMngr.onlinePlayersNames[2] = jsonvale1["result_push"][i]["third_player"].ToString();
                    }
                    if (jsonvale1["result_push"][i]["fourth_player"].ToString() != null)
                    {
                        ReferenceManager.refMngr.onlinePlayersNames[3] = jsonvale1["result_push"][i]["fourth_player"].ToString();
                    }
                    ActuallyStartTable();
                    break;
                }
            }
        }
        else
        {
            ReferenceManager.refMngr.ShowError(www.error, "Error");
        }
    }

    public void ShowPrizeDIstribution()
    {
        if (noOfPlayer == "2")
        {
            FindObjectOfType<InitMenuScript>().OpenPrizeDistributionPopup(false, gamePrice, noOfPlayer, firstPrize, secondPrize, thirdPrize);
        }
        else
        {
            FindObjectOfType<InitMenuScript>().OpenPrizeDistributionPopup(true, gamePrice, noOfPlayer, firstPrize, secondPrize, thirdPrize);
        }
    }

    private void OnEnable()
    {
        UpdateClock();
    }

    public void UpdateClock()
    {
        if (!isTablePlaying)
        {
            if (hr <= 0)
            {
                timeLeftText.text = mns.ToString() + "m:" + secs.ToString() + "s";
            }
            else
            {
                timeLeftText.text = hr.ToString() + "h:" + mns.ToString() + "m:" + secs.ToString() + "s";
            }
            secs--;
            if (secs < 0)
            {
                mns--;
                secs = 59;
                if (mns < 0)
                {
                    hr--;
                    mns = 59;
                }
            }
            if (hr != 0 || mns != 0 || secs != 0)
            {
                Invoke(nameof(UpdateClock), 1);
            }
            else
            {
                SetTableAsPlaying();
                //StartTable();
                Debug.LogError("Table Opened");
            }
        }
    }

}
