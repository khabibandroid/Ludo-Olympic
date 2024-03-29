﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using LitJson;
using System;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

//Pun ID: 67c241bd-3a98-4352-b078-cd48e60868c7
//Chat ID: aee9d632-9aff-4839-854d-dfa26a47e87b

[System.Serializable]
public class Bets
{
    public float winningAmount;
    public float betAmount;
    public string tableValue;
}

public class ReferenceManager : MonoBehaviour
{
    public Sprite[] backgroundSprites;
    public string BGThemePP = "BGTheme";
    public static ReferenceManager refMngr;
    public bool isBidSelected = false;
    public bool isOnlineBidSelected = false;
    public List<Bets> bets = new List<Bets>();
    public GameObject loadingPanel;
    public GameObject loadingPanelFullBlack;
    public GameObject onlineGameWaitingPanel;
    [SerializeField]ReedemCoinsScript redeemCoinScript;
    int invokeCount = 0;
    public GameObject errordialogBox;
    public Text errorTitle;
    public Text errorMsg;

    public TextMeshProUGUI onlineInvestmentText, onlineEarningText, privateInvestText, privateEarningText;
    public int onlineNoOfPlayer;
    public string onlineRoomId;

    public string firstPlacePrize;
    public string secondPlacePrize;
    public string thirdPlacePrize;

    public string[] onlinePlayersNames = new string[4];
    public List<string> botsAdded;

    public string tableStartTime = "";
    public string gameDuration = "";

    void Start()
    {
        if (refMngr == null) refMngr = this;
        else if (refMngr != this)
        {
            refMngr.onlineInvestmentText = onlineInvestmentText;
            refMngr.onlineEarningText = onlineEarningText;
            refMngr.privateInvestText = privateInvestText;
            refMngr.privateEarningText = privateEarningText;
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Debug.Log("My Version: " + Application.version);
        Debug.Log("API Version: " + GameManager.appVersionFromApi);
        //if (Application.version != GameManager.appVersionFromApi)
        //{
        //    UIFlowHandler.uihandler.UpdateApp();
        //}
        GetDate();
        Invoke(nameof(HideLoadingPanel), 2);
        Invoke(nameof(HideLoadingPanel), 5);
        //APIManager apimngr = FindObjectOfType<APIManager>();
        //if (apimngr.LoadingPage.activeInHierarchy || apimngr.splashCanvas.activeInHierarchy)
        //{
        //    Invoke(nameof(apimngr.Delayforloading), 2);
        //}
    }


    void HideLoadingPanel()
    {
        APIManager apimngr = FindObjectOfType<APIManager>();
        apimngr.Delayforloading();
    }

    public void ShowError(string error, string errorTitlee)
    {
        errorMsg.text = error;
        errorTitle.text = errorTitlee;
        errordialogBox.SetActive(true);
    }

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GetRedeemScript();
        isBidSelected = false;
        isOnlineBidSelected = false;
    }

    public void ShowOnlineInvestment(float betAmount, float winAmount)
    {
        onlineInvestmentText.text = "Invest: " + betAmount.ToString();
        onlineEarningText.text = "Earn: " + winAmount.ToString();
    }

    public void ShowOfflineInvestment(float betAmount,float winAmount)
    {
        privateInvestText.text = "Invest: " + betAmount.ToString();
        privateEarningText.text = "Earn: " + winAmount.ToString();
    }

    void GetRedeemScript()
    {
        invokeCount++;
        try
        {
            redeemCoinScript = FindObjectOfType<ReedemCoinsScript>();
            if(redeemCoinScript!=null)
            Invoke(nameof(SetRedeemPanel), 1);
        }
        catch
        {
            if (invokeCount < 10)
            {
                try
                {
                    Invoke(nameof(GetRedeemScript), 0.1f);
                }
                catch { }
            }
        }
    }

    void SetRedeemPanel()
    {
        if (GameManager.paytmNumber != "")
        {
            redeemCoinScript.pytmsubmitBtn.SetActive(false);
            redeemCoinScript.pytmwithdrawlBtn.SetActive(true);
        }
        else
        {
            redeemCoinScript.pytmsubmitBtn.SetActive(true);
            redeemCoinScript.pytmwithdrawlBtn.SetActive(false);
        }
        if (GameManager.bankIfscCode != "")
        {
            redeemCoinScript.BankSubmitbtn.SetActive(false);
            redeemCoinScript.bankwithdrawBtn.SetActive(true);
        }
        else
        {
            redeemCoinScript.BankSubmitbtn.SetActive(true);
            redeemCoinScript.bankwithdrawBtn.SetActive(false);
        }
        if (GameManager.upiID != "")
        {
            redeemCoinScript.UpiSubmitBtn.SetActive(false);
            redeemCoinScript.UpiwithdrawlBtn.SetActive(true);
        }
        else
        {
            redeemCoinScript.UpiSubmitBtn.SetActive(true);
            redeemCoinScript.UpiwithdrawlBtn.SetActive(false);
        }
    }

    public void CloseLoadingPanel()
    {
        Invoke(nameof(CloseLoadingPanelAfterDelay), 1);
    }

    void CloseLoadingPanelAfterDelay()
    {
        loadingPanel.SetActive(false);
    }

    /// <summary>
    /// Background Sprite of Main Game
    /// </summary>
    /// <returns>Returns Sprite</returns>
    public Sprite GetBackgroundSprite()
    {
        if (!PlayerPrefs.HasKey(BGThemePP)) PlayerPrefs.SetInt(BGThemePP, 7);
        return backgroundSprites[PlayerPrefs.GetInt(BGThemePP) - 1];
    }

    /// <summary>
    /// Sets Bets in system loaded from the URL for in game use.
    /// </summary>
    /// <returns>Returns Sprite</returns>
    public void SetBets(string tableValues, float betAmounttt, float winningAmounttt)
    {
        Bets tempBet = new Bets();
        tempBet.tableValue = tableValues;
        tempBet.betAmount = betAmounttt;
        tempBet.winningAmount = winningAmounttt;
        bets.Add(tempBet);
    }
    /// <summary>
    /// Returns Table value of game according to bet and winning amount
    /// </summary>
    public string GetTableValue(float betAmountt, float winningAmountt)
    {
        for (int i = 0; i < bets.Count; i++)
        {
            if (bets[i].betAmount == betAmountt && bets[i].winningAmount == winningAmountt)
            {
                return bets[i].tableValue;
            }
        }
        return "26";
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance != null)
        {
            //Debug.LogError(GameManager.Instance.nameMy);
            //Debug.LogError("Is Master Client: " + PhotonNetwork.isMasterClient);
            //Debug.LogError("Is Inside Room: " + PhotonNetwork.inRoom);
            //Debug.LogError("Is Inside Lobby: " + PhotonNetwork.insideLobby);
            //for (int i = 0; i < GameManager.Instance.opponentsIDs.Count; i++)
            //{
            //    Debug.LogError("IDS: " + i + " " + GameManager.Instance.opponentsIDs[i]);
            //}
            //for (int i = 0; i < GameManager.Instance.opponentsNames.Count; i++)
            //{
            //    Debug.LogError("Names: " + i + " " + GameManager.Instance.opponentsNames[i]);
            //}
            //Debug.LogError("Name: " + GameManager.Instance.nameMy);
            //Debug.LogError("Name: " + GameManager.playerName);
            //Debug.LogError("Name: " + GameManager.emailId);
            //Debug.LogWarning("Current Bet: " + GameManager.Instance.currentBetAmount);
            //Debug.LogWarning("Table: " + GetTableValue(GameManager.Instance.currentBetAmount, GameManager.Instance.currentWinningAmount));
            //Debug.LogError(GameManager.paytmNumber);
            //Debug.LogError(GameManager.bankIfscCode);
            //Debug.LogError(GameManager.upiID);
            //Debug.LogError("Coins Count" + GameManager.Instance.coinsCount);
            //Debug.LogError("Ref Manager Type: " + GameManager.Instance.type);
            //Debug.LogError("ISSSSSS : " + GameManager.Instance.isLocalMultiplayer);
            //if (PhotonNetwork.inRoom)
            //{
            //    Debug.LogError("Players in room: " + PhotonNetwork.room.playerCount);
            //    Debug.LogError("Players in room: " + GameManager.Instance.currentPlayersCount);
            //}
            //else
            //{
            //    Debug.LogError("Not In Room");
            //}
            //Debug.LogError("IS Connected: " + PhotonNetwork.connected);
            //Debug.LogError("Connection State: " + PhotonNetwordsafsdsdafask.connectionState);
            //Debug.LogError("Inside Lobby: " + PhotonNetwork.insideLobby);
            //PhotonNetwork.con
            //Debug.LogError("Bot moves" + GameManager.Instance.doesContainBotMoves);
            //Debug.LogError("Current Bet Amount" + GameManager.Instance.currentBetAmount);
            //Debug.LogError("Current Winning Amount" + GameManager.Instance.currentWinningAmount);
            //for (int i = 0; i < GameManager.Instance.opponentsIDs.Count; i++)
            //{
            //    Debug.LogError(GameManager.Instance.opponentsIDs[i] + " IDs" + i);
            //}
            //for (int i = 0; i < GameManager.Instance.opponentsNames.Count; i++)
            //{
            //    Debug.LogError(GameManager.Instance.opponentsNames[i] + " Names" + i);
            //}
            //Debug.LogError("Disconnect Timeout: " + PhotonNetwork.BackgroundTimeout);
            //Debug.LogError("local  multiplayer: " + GameManager.Instance.isLocalMultiplayer);
            //Debug.LogError("Playing with computer: " + GameManager.Instance.isPlayingWithComputer);
            //Debug.LogError("Current Payout Coins" + GameManager.Instance.payoutCoins);
        }
        else
        {
            Debug.LogError("Null");
        }
    }

    

    public void ChangeWinningAmountManually(float currentBettingAmount, int totalPlayers)
    {
        GameManager.Instance.currentBettingIndex = 0;
        float tempWinAmount = currentBettingAmount * totalPlayers - float.Parse((0.1 * currentBettingAmount * totalPlayers).ToString());
        GameManager.Instance.currentBetting = new Betting(BettingType.FourPlayer, currentBettingAmount, tempWinAmount);
        GameManager.Instance.currentWinningAmount = tempWinAmount;
        GameManager.Instance.payoutCoins = GameManager.Instance.currentWinningAmount;
    }

    public string GetDate()
    {
        string month, day, year;
        DateTime datetime = DateTime.Now;
        month = datetime.Month.ToString();
        day = datetime.Day.ToString();
        year = datetime.Year.ToString();
        if (month.Length == 1)
        {
            month = "0" + month;
        }
        if (day.Length == 1)
        {
            day = "0" + day;
        }
        string date = year + "-" + month + "-" + day;
        return date;
    }

    public string GetTime()
    {
        string hour, minutes, seconds;
        //TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        //DateTime datetime = TimeZoneInfo.ConvertTime(DateTime.Now, istZone);
        DateTime datetime = DateTime.Now;
        //Debug.LogError("FFFFFF" + datetime);
        hour = datetime.Hour.ToString();
        minutes = datetime.Minute.ToString();
        seconds = datetime.Second.ToString();
        if (hour.Length == 1) hour = "0" + hour;
        if (minutes.Length == 1) minutes = "0" + minutes;
        if (seconds.Length == 1) seconds = "0" + seconds;
        string time = hour + ":" + minutes + ":" + seconds;
        return time;
    }

    public bool CheckDate(string requiredDatee, string enddDatee, string currentDatee)
    {
        try
        {
            string[] start = requiredDatee.Split('-');
            string[] end = enddDatee.Split('-');
            string[] cur = currentDatee.Split('-');
            if (CheckDayOfYear(int.Parse(cur[2]), int.Parse(cur[1]), int.Parse(cur[0])) == CheckDayOfYear(int.Parse(start[2]), int.Parse(start[1]), int.Parse(start[0])))
            {
                if (CheckDayOfYear(int.Parse(cur[2]), int.Parse(cur[1]), int.Parse(cur[0])) == CheckDayOfYear(int.Parse(end[2]), int.Parse(end[1]), int.Parse(end[0])))
                {
                    matchingDateType = 3;
                    return true;
                }
                else
                { 
                    matchingDateType = 1;
                    return true;
                }
            }
            else if (CheckDayOfYear(int.Parse(cur[2]), int.Parse(cur[1]), int.Parse(cur[0])) == CheckDayOfYear(int.Parse(end[2]), int.Parse(end[1]), int.Parse(end[0])))
            {
                if (CheckDayOfYear(int.Parse(cur[2]), int.Parse(cur[1]), int.Parse(cur[0])) == CheckDayOfYear(int.Parse(start[2]), int.Parse(start[1]), int.Parse(start[0])))
                {
                    matchingDateType = 3;
                    return true;
                }
                else
                {
                    matchingDateType = 2;
                    return true;
                }
            }
            else if (CheckDayOfYear(int.Parse(cur[2]), int.Parse(cur[1]), int.Parse(cur[0])) > CheckDayOfYear(int.Parse(start[2]), int.Parse(start[1]), int.Parse(start[0])))
            {
                if (CheckDayOfYear(int.Parse(cur[2]), int.Parse(cur[1]), int.Parse(cur[0])) < CheckDayOfYear(int.Parse(end[2]), int.Parse(end[1]), int.Parse(end[0])))
                {
                    matchingDateType = 4;
                    return true;
                }
                else
                {
                    matchingDateType = 0;
                    return false;
                }
            }
            else
            {
                matchingDateType = 0;
                return false;
            }
        }
        catch
        {
            Debug.LogError("Catch false");
            matchingDateType = 0;
            return false;
        }
    }

    int matchingDateType;

    public string TwentyFourToTweleveFormat(string twentyFourFormat)
    {
        string[] time = twentyFourFormat.Split(':');
        string ampm;
        int hour = int.Parse(time[0]);
        if (hour > 11) ampm = "pm";
        else ampm = "am";
        hour = hour % 12;
        if (hour == 0) hour = 12;
        return hour.ToString() + ":" + time[1] + ampm;
    }
    public bool CheckTime(string startTimee, string endTimee, string currentTimee)
    {
        try
        {
            string[] start = startTimee.Split(':');
            string[] end = endTimee.Split(':');
            string[] cur = currentTimee.Split(':');
            int startOne, endOne, curOne;
            if (start.Length == 2) startOne = (int.Parse(start[0]) * 3600) + (int.Parse(start[1]) * 60);
            else startOne = (int.Parse(start[0]) * 3600) + (int.Parse(start[1]) * 60) + int.Parse(start[2]);

            if (end.Length == 2) endOne = (int.Parse(end[0]) * 3600) + (int.Parse(end[1]) * 60);
            else endOne = (int.Parse(end[0]) * 3600) + (int.Parse(end[1]) * 60) + int.Parse(end[2]);

            if (cur.Length == 2) curOne = (int.Parse(cur[0]) * 3600) + (int.Parse(cur[1]) * 60);
            else curOne = (int.Parse(cur[0]) * 3600) + (int.Parse(cur[1]) * 60) + int.Parse(cur[2]);

            if (startOne == endOne) return true;

            if (curOne > startOne && curOne < endOne)
            {
                return true;
            }
            else return false;
        }
        catch
        {
            return false;
        }
    }

    public int hour, minutes, seconds;

    public void SecondsToTime(int secondss)
    {
        int hr, mnts, scnds;
        hr = secondss / 3600;
        mnts = (secondss % 3600) / 60;
        scnds = (secondss % 3600) % 60;
        hour = hr;
        minutes = mnts;
        seconds = scnds;
    }

    public int timeToSecondsMnsScs(string time, char charToDiffer)
    {
        string[] theTime = time.Split(charToDiffer);
        int mns, secs;
        int totalSeconds;
        if (theTime.Length == 2)
        {
            mns = int.Parse(theTime[0]) * 60;
            secs = int.Parse(theTime[1]);
            totalSeconds = mns + secs;
            return totalSeconds;
        }
        else
        {
            return -1;
        }
    }

    public int timeToSecondsHrMns(string time, char charToDiffer)
    {
        string[] theTime = time.Split(charToDiffer);
        int hr, mns;
        int totalSeconds;
        if (theTime.Length == 2)
        {
            hr = int.Parse(theTime[0]) * 3600;
            mns = int.Parse(theTime[1]) * 60;
            totalSeconds = hr + mns;
            return totalSeconds;
        }
        else
        {
            return -1;
        }
    }

    public bool isTablePlaying = false;

    public bool IsLessThanADay(string requiredDate, string currentDate, string currentTimee, string startTimee, string durationToAdd)
    {
        string[] reqDate = new string[3];
        string[] curDate = new string[3];
        string[] startTime = startTimee.Split(':');
        string[] curTime = currentTimee.Split(':');
        int toAddDuration = int.Parse(durationToAdd);
        reqDate = requiredDate.Split('-');
        curDate = currentDate.Split('-');
        if(int.Parse(reqDate[0]) < int.Parse(curDate[0]))
        {
            return false;
        }
        else if(int.Parse(reqDate[0]) == int.Parse(curDate[0]))
        {
            if(int.Parse(reqDate[1]) < int.Parse(curDate[1]))
            {
                return false;
            }
            else
            {
                if(int.Parse(reqDate[2]) < int.Parse(curDate[2]))
                {
                    return false;
                }
            }
        }

        if (int.Parse(reqDate[1]) - int.Parse(curDate[1]) <= 1 && int.Parse(reqDate[1]) - int.Parse(curDate[1]) >= 0)
        {
            if (int.Parse(reqDate[1]) == int.Parse(curDate[1]))
            {
                int startOne, curOne;
                if (startTime.Length == 2) startOne = (int.Parse(startTime[0]) * 3600) + (int.Parse(startTime[1]) * 60);
                else startOne = (int.Parse(startTime[0]) * 3600) + (int.Parse(startTime[1]) * 60) + int.Parse(startTime[2]);

                if (curTime.Length == 2) curOne = (int.Parse(curTime[0]) * 3600) + (int.Parse(curTime[1]) * 60);
                else curOne = (int.Parse(curTime[0]) * 3600) + (int.Parse(curTime[1]) * 60) + int.Parse(curTime[2]);
                
                int tempStartOne = startOne + toAddDuration;

                if (tempStartOne < curOne)
                {
                    return false;
                }

                int a = startOne - curOne;
                if (a < 1)
                {
                    isTablePlaying = true;
                }
                else isTablePlaying = false;
                SecondsToTime(a);
                return true;
            }
            else
            {
                int startOne, curOne;
                if (startTime.Length == 2) startOne = (int.Parse(startTime[0]) * 3600) + (int.Parse(startTime[1]) * 60);
                else startOne = (int.Parse(startTime[0]) * 3600) + (int.Parse(startTime[1]) * 60) + int.Parse(startTime[2]);

                if (curTime.Length == 2) curOne = (int.Parse(curTime[0]) * 3600) + (int.Parse(curTime[1]) * 60);
                else curOne = (int.Parse(curTime[0]) * 3600) + (int.Parse(curTime[1]) * 60) + int.Parse(curTime[2]);

                int tempStartOne = startOne + toAddDuration;

                if ((86400 - curOne) + tempStartOne <= 86400)
                {
                    int a = (86400 - curOne) + startOne;
                    Debug.LogError("AAAAA: " + a);
                    if (a < 0)
                    {
                        isTablePlaying = true;
                    }
                    else isTablePlaying = false;
                    SecondsToTime(a);
                    return true;
                }
                else
                {
                    int a = (86400 - curOne) + startOne;
                    return false;
                }
            }
        }
        else
        {
            return false;
        }
    }


    Hashtable daysInMonth = new Hashtable()
    {
         {"1","31"},  {"2","28"}, {"3","31"}, {"4","30"}, {"5","31"}, {"6","30"},
          {"7","31"}, {"8","31"}, {"9","30"}, {"10","31"}, {"11","30"}, {"12","31"},
    };
    public int CheckDayOfYear(int datee, int monthh, int yearr)
    {
        bool isLeapYear = false;
        int day = 0;
        if (yearr % 4 == 0)
        {
            isLeapYear = true;
        }
        for (int i = 1; i < monthh; i++)
        {
            day += int.Parse(daysInMonth[i.ToString()].ToString());
            if(isLeapYear && i == 2)
            {
                day += 1;
            }
        }
        day += datee;
        return day;
    }
}