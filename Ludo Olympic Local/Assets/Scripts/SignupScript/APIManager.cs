﻿using System;
using LitJson;
using System.IO;
using System.Text;
using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;
using System.Collections;
using Random = System.Random;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using TMPro;

using System.Security.Authentication.ExtendedProtection;

public class APIManager : MonoBehaviour
{
    public bool isTestingLogin = false;

    [Header("SignUp Object")]

    public InputField _nameWithdraw;
    public InputField _amountWithdraw;
    public InputField _mobileNumberWithdraw;

    [Header("Login Object")]
    public TMP_InputField _password;
    public TMP_InputField _emailId;

    [Header("Forgot Password Object")]
    public InputField _forgotPasswordEmail;
    public Text ferrorText, withdrwal;
    public Text cerrorText;
    public InputField changeForgetPasswordEmail;
    public InputField otp;
    public InputField psswrd;
    public InputField cpasswrd;
    public GameObject forgetPasswordPanel;
    public GameObject changePasswordPanel;
    //public InputField _emailIdOtp;

    [Header("Player Stats")]

    public Text twoWonTxt;
    public Text fourWonTxt;
    public Text gamesWonTxt;
    public Text playerNameTxt;

    [Header("SignUp Object")]

    public TMP_InputField _name;
    public TMP_InputField _SignUppassword;
    public TMP_InputField _confirmPassword;
    public TMP_InputField _date;
    public TMP_InputField _month;
    public TMP_InputField _year;
    public TMP_InputField _mobileNumber;
    public TMP_InputField _emailIdR;

    [Header("Coins Request ")]

    public InputField _requestCoins;

    [Header("Payment Data")]

    public Transform contentPanel;
    public GameObject paymentData, scrollPanel;

    [Header("Common")]
    public Text errorMsg, coinsInfo, loginerror;
    public GameObject SignupPanel;
    public GameObject LoginPanel;
    public Image loginBtn;
    public Image signupBtn;
    public GameObject withdrawPanel;
    public GameObject storePanel, notificationPanel, choosePanel, successPanel, addMoneyView, addCoinsNofitications;
    public string fullname;
    public GameObject playerStatsPanel;
    public GameObject paymentHistoryPanel;
    public GameObject loadinPanel;
    public GameObject kycPanel;
    public GameObject error;
    public GameObject walletPanel;

    public GameObject MinimumObject;
    public Button[] WithdrawBtn;
    public Text MinimumText;

    [Header("KYC Attibute")]

    public RawImage adharFront;
    public RawImage adharBack;
    public RawImage pancard;

    public string image1;
    public string image2;
    public string image3;

    [Header("Profile Attribute")]

    public Text mainAccountBalance;
    public Text totalDeposit;
    public Text totalWithDrawal;
    public Text totalgamePlay;
    public Text totalCashWin;
    public Text nameP;
    public TextMeshProUGUI emailID;
    public TextMeshProUGUI phoneNumber;
    public TextMeshProUGUI playerName;

    public InputField playername;

    [Header("Player Profile")]

    public GameObject userDashboard;
    public GameObject updateProfile;

    public string playerImageUrl;
    public RawImage playerImage;
    public RawImage playerImage3;


    [Header("My Wallet Attributes")]
    public Text newTotalBalance;
    public Text newAddedAmount;
    public Text newWithdrawableAmount;

    int age;
    public string dob;
    string userName;
    public Text disText;
    [Obsolete]
    WWW w;
    string status;
    string url;

    NewGameManager newGameManager;

    private string getBettingApi = GameManager.apiBase1 + "betting";

    private string getBotDifficulty = "https://onlystore.in/ludomoney/api/gamedifficulty.php";

    private string getBotAvailiblity = "https://onlystore.in/ludomoney/api/botswinning.php";

    public const string MatchEmailPattern =
      @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
      + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
      + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
      + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    public GameObject newOptionsPanel;
    public GameObject newAboutUs;
    public GameObject newAddMoney;
    public GameObject newBankDetails;
    public GameObject newCompleteYourKYCPanel;
    public GameObject newMyWallet;
    public GameObject newNotificationsPanel;
    public GameObject newPaymentHistory;
    public GameObject newPrivacyPolicy;
    public GameObject newPrizeDistributionPopUp;
    public GameObject newProfilePanel;
    public GameObject newRedeemCoins;
    public GameObject newReferAFriends;
    public GameObject newRules;
    public GameObject newTermsAndConditions;
    public GameObject newUpdateProfilePage;

    public bool isFirstTimeLogin = false;
    public Sprite defaultPP;

    public bool joinedOnlineOnTime = false;

    public bool isEmployee;

    void Awake()
    {
        GetStartedTable();
        isFirstTimeLogin = false;
        StartCoroutine(OpenLogin());
        StartCoroutine(LoginMenu());
        newGameManager = FindObjectOfType<NewGameManager>();
        if (PlayerPrefs.GetInt("Muted", 0) == 0)
        {
            SetSoundButton(true);
        }
        else SetSoundButton(false);
        SetStates();

        if (PlayerPrefs.GetInt("IsSoundEffect", 1) == 1)
        {
            SetEffectButton(true);
            SetEffectToMute(false);
        }
        else
        {
            SetEffectButton(false);
            SetEffectToMute(true);
        }
    }



    void Update()
    {
        // Make sure user is on Android platform

        if (Application.platform == RuntimePlatform.Android)
        {
            //AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            //activity.Call<bool>("moveTaskToBack", true);
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                // Quit the application
                //Application.Quit();

            }
        }
        else
        {
            //Application.Quit();
        }

    }

    private string results;
    public string Results
    {
        get
        {
            return results;
        }
    }
    public GameObject LoadingPage;
    public GameObject splashCanvas;
    public IEnumerator LoginMenu()
    {
        yield return new WaitForSeconds(3.1f);
        if (PlayerPrefs.HasKey("Logintoken"))
        {
            FindObjectOfType<NewGameManager>().newLoginScreen.SetActive(false);
            choosePanel.SetActive(false);
            GameManager.Instance.userID = PlayerPrefs.GetString("Logintoken");
            Debug.Log("has key");


            LoginPanel.SetActive(false);
            choosePanel.SetActive(false);
            // yield return new WaitForSeconds(5f);
            //OnPlayerProfileData();
            //CloseSplash();
            Invoke("Delayforloading", 0.01f);
        }
        else
        {
            //PlayerPrefs.SetString("Logintoken", "CL614221");
            FindObjectOfType<NewGameManager>().newLoginScreen.SetActive(true);
            //LoginPanel.SetActive(true);
            // choosePanel.SetActive(true);
        }
    }
    public void Delayforloading()
    {
        LoadingPage.SetActive(false);
        splashCanvas.SetActive(false);
    }
    public IEnumerator OpenLogin()
    {
        yield return new WaitForSeconds(3.1f);
        splashCanvas.SetActive(true);
        choosePanel.SetActive(false);
    }
    public void OnLoginBtnClick()
    {

        // url = "http://onlystore.in/ludomoney/api/login_user.php?mobile=" + _emailId.text + "&password=" + _password.text;
        url = GameManager.apiBase1 + "login";

        if (string.IsNullOrEmpty(_emailId.text))
        {
            loginerror.text = "Username Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }

        if (string.IsNullOrEmpty(_password.text))
        {
            loginerror.text = "Password Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        Debug.Log(url);
        WWWForm form = new WWWForm();
        form.AddField("username", _emailId.text);
        form.AddField("password", _password.text);
        CloseSplash();
        Debug.Log("userName" + _emailId.text);
        Debug.Log("Paas" + _password.text);
        WWW w = new WWW(url, form);
        loadinPanel.SetActive(true);
        StartCoroutine(LoginAPI(w));
        Invoke("Delayforloading", 3f);
    }


    IEnumerator LoginAPI(WWW _w)
    {
        yield return _w;
        print("responce=" + _w.text);
        print("responce=" + _w.error);
        if (_w.error == null)
        {

            string msg = _w.text;
            msg = msg.Replace("{", "");
            msg = msg.Replace("}", "");
            msg = msg.Replace("[", "");
            msg = msg.Replace("]", "");
            msg = msg.Replace(@"""", string.Empty);

            results = GetDataValue(msg, "message:");
            status = GetDataValue(msg, "status:");
            GameManager.Instance.nameMy = GetDataValue(msg, "username:");

            print("Results occur" + Results);
            if (results == "Login Successfull" || status == "True")
            {
                //loginPanel.SetActive(false);
                GameManager.Instance.avatarMyIndex = UnityEngine.Random.Range(0, 22);
                GameManager.Instance.avatarMy = GameManager.Instance.playfabManager.staticGameVariables.avatars[GameManager.Instance.avatarMyIndex];
                Debug.Log("playername " + PhotonNetwork.player.CustomProperties["name"]);
                GameManager.Instance.coinsCount = float.Parse(GetDataValue(msg, "coins:"));
                GameManager.onlineAmount = GetDataValue(msg, "online_amount:");
                GameManager.offlineAmount = GetDataValue(msg, "offline_amount:");
                Debug.Log(GameManager.Instance.nameMy);
                Debug.Log(GameManager.Instance.coinsCount);
                Debug.Log(results);
                Debug.Log(status);
                LoginPanel.SetActive(false);
                choosePanel.SetActive(false);
                splashCanvas.SetActive(false);
                loadinPanel.SetActive(false);
                _emailId.text = "";
                _password.text = "";
                GameManager.Instance.userID = GetDataValue(msg, "my_referral_code:");
                GameManager.Uid = GetDataValue(msg, "uid:");
                GameManager.friendrefferalCode = GetDataValue(msg, "reference_code:");

                Debug.Log("UID" + GameManager.Uid);
                Debug.Log("codeRR" + GameManager.friendrefferalCode);
                Debug.Log("my_referral_code " + GameManager.Instance.userID);
                PlayerPrefs.SetString("Logintoken", GameManager.Instance.userID);


                // Reedem Amount Data...!!!
                JsonData jsonvale = JsonMapper.ToObject(_w.text);

                try
                {
                    GameManager.appVersionFromApi = jsonvale["result_push"][0]["version"].ToString();
                }
                catch
                {
                    Debug.LogError("Catch for version called");
                }
                GameManager.playerName = jsonvale["result_push"][0]["fullname"].ToString();
                ExitGames.Client.Photon.Hashtable someCustomPropertiesToSet = new ExitGames.Client.Photon.Hashtable() { { "name", GameManager.Instance.nameMy }, { "avatarId", GameManager.Instance.avatarMyIndex }, { "fullName", GameManager.playerName } };
                PhotonNetwork.player.SetCustomProperties(someCustomPropertiesToSet);
                if (jsonvale["result_push"][0]["usertype"].ToString() == "1")
                {
                    isEmployee = true;
                }
                else isEmployee = false;
                GameManager.mobileNumber = jsonvale["result_push"][0]["mobile"].ToString();
                GameManager.emailId = jsonvale["result_push"][0]["email"].ToString();
                PlayerPrefs.SetString("FriendRefCodePP", jsonvale["result_push"][0]["referal_refer_code"].ToString());
                emailID.text = GameManager.emailId;
                phoneNumber.text = GameManager.mobileNumber;
                playerName.text = GameManager.playerName;

                Debug.Log("Name" + GameManager.playerName);
                Debug.Log("Mobile" + GameManager.mobileNumber);
                Debug.Log("Emaild" + GameManager.emailId);
                pytmNumber = (jsonvale["result_push"][0]["paytm"].ToString());
                GameManager.paytmNumber = pytmNumber;
                Debug.Log("Pytm" + GameManager.paytmNumber);


                upi = (jsonvale["result_push"][0]["upi"].ToString());
                GameManager.upiID = upi;
                Debug.Log("UPI" + GameManager.upiID);


                bank = (jsonvale["result_push"][0]["bank_name"].ToString());
                GameManager.bankName = bank;
                Debug.Log("bank" + GameManager.bankName);



                ifscCode = (jsonvale["result_push"][0]["ifsc_code"].ToString());
                GameManager.bankIfscCode = ifscCode;
                Debug.Log("IFSC" + GameManager.bankIfscCode);


                accoutnumber = (jsonvale["result_push"][0]["account"].ToString());
                GameManager.accountNumber = accoutnumber;
                Debug.Log("IFSC" + GameManager.accountNumber);


                // Player Profile Data..!!!

                mainAccountBalance.text = GameManager.Instance.coinsCount.ToString();

                GameManager.depositAmount = jsonvale["result_push"][0]["deposite_amount"].ToString();
                totalDeposit.text = GameManager.depositAmount;

                GameManager.withdraw = jsonvale["result_push"][0]["withdraw_amount"].ToString();
                totalWithDrawal.text = GameManager.withdraw;

                GameManager.gamesWon = jsonvale["result_push"][0]["games_won"].ToString();
                totalgamePlay.text = GameManager.gamesWon;

                GameManager.cashWon = jsonvale["result_push"][0]["cash_won"].ToString();
                totalCashWin.text = GameManager.cashWon;

                GameManager.playerName = jsonvale["result_push"][0]["fullname"].ToString();
                nameP.text = GameManager.playerName;

                playername.text = GameManager.playerName;

                newTotalBalance.text = GameManager.Instance.coinsCount.ToString();
                newAddedAmount.text = GameManager.depositAmount;
                newWithdrawableAmount.text = GameManager.withdraw;
                LoadingPage.SetActive(true);
                SceneManager.LoadScene("MenuScene");

                GameManager.adharcardFront = jsonvale["result_push"][0]["aadhar_first"].ToString();
                if (jsonvale["result_push"][0]["aadhar_first"].ToString() != "0")
                {
                    adharFront.gameObject.SetActive(true);
                    UnityWebRequest unityWebRequest = UnityWebRequest.Get(GameManager.adharcardFront);
                    yield return unityWebRequest.SendWebRequest();
                    byte[] bytes = unityWebRequest.downloadHandler.data;
                    Texture2D tex = new Texture2D(2, 2);
                    tex.LoadImage(bytes);
                    Sprite adharF = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                    adharFront.texture = adharF.texture;
                }
                else
                {
                    adharFront.gameObject.SetActive(false);
                }
                GameManager.adharcardBack = jsonvale["result_push"][0]["aadhar_second"].ToString();
                if (jsonvale["result_push"][0]["aadhar_second"].ToString() != "0")
                {
                    adharBack.gameObject.SetActive(true);
                    UnityWebRequest unityWebRequest1 = UnityWebRequest.Get(GameManager.adharcardBack);
                    yield return unityWebRequest1.SendWebRequest();
                    byte[] bytes1 = unityWebRequest1.downloadHandler.data;
                    Texture2D tex1 = new Texture2D(2, 2);
                    tex1.LoadImage(bytes1);
                    Sprite adharB = Sprite.Create(tex1, new Rect(0.0f, 0.0f, tex1.width, tex1.height), new Vector2(0.5f, 0.5f), 100.0f);
                    pancard.texture = adharB.texture;
                }
                else
                {
                    adharBack.gameObject.SetActive(false);
                }

                GameManager.pancard = jsonvale["result_push"][0]["pan_pic"].ToString();
                if (jsonvale["result_push"][0]["pan_pic"].ToString() != "0")
                {
                    pancard.gameObject.SetActive(true);
                    UnityWebRequest unityWebRequest2 = UnityWebRequest.Get(GameManager.pancard);
                    yield return unityWebRequest2.SendWebRequest();
                    byte[] bytes2 = unityWebRequest2.downloadHandler.data;
                    Texture2D tex2 = new Texture2D(2, 2);
                    tex2.LoadImage(bytes2);
                    Sprite panC = Sprite.Create(tex2, new Rect(0.0f, 0.0f, tex2.width, tex2.height), new Vector2(0.5f, 0.5f), 100.0f);
                    adharBack.texture = panC.texture;
                }
                else
                {
                    pancard.gameObject.SetActive(false);
                }
                playerImageUrl = jsonvale["result_push"][0]["profile_pic"].ToString();
                if (jsonvale["result_push"][0]["profile_pic"].ToString() != "0")
                {
                    playerImage.gameObject.SetActive(true);
                    playerImageUrl = jsonvale["result_push"][0]["profile_pic"].ToString();
                    Debug.LogError("pLAYER IMAGE URL: " + playerImageUrl);
                    UnityWebRequest unityWebRequest3 = UnityWebRequest.Get(playerImageUrl);
                    yield return unityWebRequest3.SendWebRequest();
                    byte[] bytes3 = unityWebRequest3.downloadHandler.data;
                    Texture2D tex3 = new Texture2D(2, 2);
                    tex3.LoadImage(bytes3);
                    Sprite playerimage = Sprite.Create(tex3, new Rect(0.0f, 0.0f, tex3.width, tex3.height), new Vector2(0.5f, 0.5f), 100.0f);
                    playerImage.texture = playerimage.texture;
                    playerImage3.texture = playerimage.texture;
                    GameManager.profileImge = playerimage;
                }
                else
                {
                    playerImage.gameObject.SetActive(false);
                }

                GameManager.playerName = jsonvale["result_push"][0]["fullname"].ToString();
                nameP.text = GameManager.playerName;
            }
            if (results == "wrong username and password" || status == "False")
            {
                loginerror.text = results;
                loadinPanel.SetActive(false);

                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
            }
            if (results == "User does not exist" || status == "False")
            {
                loginerror.text = results;
                loadinPanel.SetActive(false);

                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
            }
            if (status == "false" || results == "Login Fail")
            {
                loginerror.text = results;
                loadinPanel.SetActive(false);
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());

            }
            else
            {
                loginerror.text = results;
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
            }
        }
        else
        {

            loginerror.text = "Invalid Login";
            Debug.Log("error");
            loadinPanel.SetActive(false);
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
        }

    }

    #region Social Media Login
    public void SetPlayerImagee(Texture tex)
    {
        UIFlowHandler.uihandler.THETEXTURE = tex;
        Debug.LogError("Setting texture in api manager");
        playerImage.texture = tex;
        playerImage3.texture = tex;
        Texture2D tex1 = (Texture2D)tex;
        //Sprite playerimage = Sprite.Create(tex1, new Rect(0.0f, 0.0f, tex1.width, tex1.height), new Vector2(0.5f, 0.5f), 100.0f);
        Sprite playerimage = Sprite.Create(tex1, new Rect(0.0f, 0.0f, tex1.width, tex1.height), new Vector2(0.5f, 0.5f));
        GameManager.profileImge = playerimage;
        Debug.LogError("Setting texture in api manager finished");
        Invoke(nameof(SetPlayerImageInMenuScene), 4);
    }

    void SetPlayerImageInMenuScene()
    {
        UIFlowHandler.uihandler.SetPlayerImage();
    }

    IEnumerator SocialLoginAPI(WWW _w)
    {
        Debug.LogError("Social Login API");

        yield return _w;
        Debug.LogError("responce=" + _w.text);
        Debug.LogError("responce error=" + _w.error);
        if (_w.error == null)
        {

            string msg = _w.text;
            msg = msg.Replace("{", "");
            msg = msg.Replace("}", "");
            msg = msg.Replace("[", "");
            msg = msg.Replace("]", "");
            msg = msg.Replace(@"""", string.Empty);

            results = GetDataValue(msg, "message:");
            status = GetDataValue(msg, "status:");
            GameManager.Instance.nameMy = GetDataValue(msg, "username:");

            print("Results occur" + Results);
            if (results == "Login Successfull" || status == "True")
            {
                canRegister = true;
                //loginPanel.SetActive(false);
                GameManager.Instance.avatarMyIndex = UnityEngine.Random.Range(0, 22);
                GameManager.Instance.avatarMy = GameManager.Instance.playfabManager.staticGameVariables.avatars[GameManager.Instance.avatarMyIndex];
                Debug.Log("playername " + PhotonNetwork.player.CustomProperties["name"]);
                GameManager.Instance.coinsCount = float.Parse(GetDataValue(msg, "coins:"));
                GameManager.onlineAmount = GetDataValue(msg, "online_amount:");
                GameManager.offlineAmount = GetDataValue(msg, "offline_amount:");
                Debug.Log(GameManager.Instance.nameMy);
                Debug.Log(GameManager.Instance.coinsCount);
                Debug.Log(results);
                Debug.Log(status);
                LoginPanel.SetActive(false);
                choosePanel.SetActive(false);
                splashCanvas.SetActive(false);
                loadinPanel.SetActive(false);
                _emailId.text = "";
                _password.text = "";
                GameManager.Instance.userID = GetDataValue(msg, "my_referral_code:");
                GameManager.Uid = GetDataValue(msg, "uid:");
                GameManager.friendrefferalCode = GetDataValue(msg, "reference_code:");
                FindObjectOfType<PickerController>().ChangePlayerTextureFromOutside(UIFlowHandler.uihandler.THETEXTURE);
                Debug.Log("UID" + GameManager.Uid);
                Debug.Log("codeRR" + GameManager.friendrefferalCode);
                Debug.Log("my_referral_code " + GameManager.Instance.userID);
                PlayerPrefs.SetString("Logintoken", GameManager.Instance.userID);


                // Reedem Amount Data...!!!
                JsonData jsonvale = JsonMapper.ToObject(_w.text);

                try
                {
                    GameManager.appVersionFromApi = jsonvale["result_push"][0]["version"].ToString();
                }
                catch
                {
                    Debug.LogError("Catch for version called");
                }
                GameManager.playerName = jsonvale["result_push"][0]["fullname"].ToString();
                ExitGames.Client.Photon.Hashtable someCustomPropertiesToSet = new ExitGames.Client.Photon.Hashtable() { { "name", GameManager.Instance.nameMy }, { "avatarId", GameManager.Instance.avatarMyIndex }, { "fullName", GameManager.playerName } };
                PhotonNetwork.player.SetCustomProperties(someCustomPropertiesToSet);
                if (jsonvale["result_push"][0]["usertype"].ToString() == "1")
                {
                    isEmployee = true;
                }
                else isEmployee = false;
                GameManager.mobileNumber = jsonvale["result_push"][0]["mobile"].ToString();
                GameManager.emailId = jsonvale["result_push"][0]["email"].ToString();
                PlayerPrefs.SetString("FriendRefCodePP", jsonvale["result_push"][0]["referal_refer_code"].ToString());
                emailID.text = GameManager.emailId;
                phoneNumber.text = GameManager.mobileNumber;
                playerName.text = GameManager.playerName;

                Debug.Log("Name" + GameManager.playerName);
                Debug.Log("Mobile" + GameManager.mobileNumber);
                Debug.Log("Emaild" + GameManager.emailId);
                pytmNumber = (jsonvale["result_push"][0]["paytm"].ToString());
                GameManager.paytmNumber = pytmNumber;
                Debug.Log("Pytm" + GameManager.paytmNumber);

                string activePlayer = jsonvale["result_push"][0]["active_game_info"].ToJson();
                Debug.LogError(activePlayer);
                activePlayer = "{\"result_push\":" + activePlayer + "}";
                ///*LiveGameInfo myobj = */
                // JsonUtility.FromJson<LiveGameInfo>(activePlayer);
                JsonData jsonvale1 = JsonMapper.ToObject(activePlayer);
                for (int i = 0; i < jsonvale1["result_push"].Count; i++)
                {
                    ActiveGamInfo temp = new ActiveGamInfo();
                    temp.game_id = jsonvale1["result_push"][i]["game_id"].ToString();
                    temp.game_room_id = jsonvale1["result_push"][i]["game_room_id"].ToString();
                    if (jsonvale1["result_push"][i]["first_player"].ToString() != null)
                    {
                        temp.first_Player_Name = jsonvale1["result_push"][i]["first_player"].ToString();
                    }
                    if (jsonvale1["result_push"][i]["second_player"].ToString() != null)
                    {
                        temp.second_Player_Name = jsonvale1["result_push"][i]["second_player"].ToString();
                    }
                    if (jsonvale1["result_push"][i]["third_player"].ToString() != null)
                    {
                        temp.third_Player_Name = jsonvale1["result_push"][i]["third_player"].ToString();
                    }
                    if (jsonvale1["result_push"][i]["fourth_player"].ToString() != null)
                    {
                        temp.fourth_Player_Name = jsonvale1["result_push"][i]["fourth_player"].ToString();
                    }
                    GameManager.activeGameInfo.Add(temp);
                }

                KYCDob = jsonvale["result_push"][0]["dob"].ToString();
                KYCadharNumber = jsonvale["result_push"][0]["aadhar"].ToString();
                KYCpanNumber = jsonvale["result_push"][0]["pan"].ToString();
                KYCState = jsonvale["result_push"][0]["state"].ToString();
                if (KYCadharNumber == "0" || KYCadharNumber == "" || KYCadharNumber == null)
                {
                    KYCadhaarStatus = "Not Verified";
                }
                else KYCadhaarStatus = "Verified";
                if (KYCpanNumber == "0" || KYCpanNumber == "" || KYCpanNumber == null)
                {
                    KYCpanStatus = "Not Verified";
                }
                else KYCpanStatus = "Verified";

                string tempStatus = jsonvale["result_push"][0]["kyc_status"].ToString();
                if (tempStatus == "0") KYCStatus = "Pending";
                else if (tempStatus == "1") KYCStatus = "Verified";
                else if (tempStatus == "2") KYCStatus = "Rejected";
                else KYCStatus = "Not Verified";

                upi = (jsonvale["result_push"][0]["upi"].ToString());
                GameManager.upiID = upi;
                Debug.Log("UPI" + GameManager.upiID);


                bank = (jsonvale["result_push"][0]["bank_name"].ToString());
                GameManager.bankName = bank;
                if (bank != "" && bank != null)
                {
                    bankeDetailIP.text = "Verified";
                    bankDetailButtonText.text = "Edit";
                }
                Debug.Log("bank" + GameManager.bankName);



                ifscCode = (jsonvale["result_push"][0]["ifsc_code"].ToString());
                GameManager.bankIfscCode = ifscCode;
                Debug.Log("IFSC" + GameManager.bankIfscCode);


                accoutnumber = (jsonvale["result_push"][0]["account"].ToString());
                GameManager.accountNumber = accoutnumber;
                Debug.Log("IFSC" + GameManager.accountNumber);


                // Player Profile Data..!!!

                mainAccountBalance.text = GameManager.Instance.coinsCount.ToString();

                GameManager.depositAmount = jsonvale["result_push"][0]["deposite_amount"].ToString();
                totalDeposit.text = GameManager.depositAmount;

                GameManager.withdraw = jsonvale["result_push"][0]["withdraw_amount"].ToString();
                totalWithDrawal.text = GameManager.withdraw;

                GameManager.gamesWon = jsonvale["result_push"][0]["games_won"].ToString();
                totalgamePlay.text = GameManager.gamesWon;

                GameManager.cashWon = jsonvale["result_push"][0]["cash_won"].ToString();
                totalCashWin.text = GameManager.cashWon;

                GameManager.playerName = jsonvale["result_push"][0]["fullname"].ToString();
                nameP.text = GameManager.playerName;

                playername.text = GameManager.playerName;

                newTotalBalance.text = GameManager.Instance.coinsCount.ToString();
                newAddedAmount.text = GameManager.depositAmount;
                newWithdrawableAmount.text = GameManager.withdraw;
                LoadingPage.SetActive(true);
                UIFlowHandler.uihandler.loadingPanel.SetActive(false);
                SceneManager.LoadScene("MenuScene");

                GameManager.adharcardFront = jsonvale["result_push"][0]["aadhar_first"].ToString();
                if (jsonvale["result_push"][0]["aadhar_first"].ToString() != "0")
                {
                    adharFront.gameObject.SetActive(true);
                    UnityWebRequest unityWebRequest = UnityWebRequest.Get(GameManager.adharcardFront);
                    yield return unityWebRequest.SendWebRequest();
                    byte[] bytes = unityWebRequest.downloadHandler.data;
                    Texture2D tex = new Texture2D(2, 2);
                    tex.LoadImage(bytes);
                    Sprite adharF = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                    adharFront.texture = adharF.texture;
                }
                else
                {
                    adharFront.gameObject.SetActive(false);
                }
                GameManager.adharcardBack = jsonvale["result_push"][0]["aadhar_second"].ToString();
                if (jsonvale["result_push"][0]["aadhar_second"].ToString() != "0")
                {
                    adharBack.gameObject.SetActive(true);
                    UnityWebRequest unityWebRequest1 = UnityWebRequest.Get(GameManager.adharcardBack);
                    yield return unityWebRequest1.SendWebRequest();
                    byte[] bytes1 = unityWebRequest1.downloadHandler.data;
                    Texture2D tex1 = new Texture2D(2, 2);
                    tex1.LoadImage(bytes1);
                    Sprite adharB = Sprite.Create(tex1, new Rect(0.0f, 0.0f, tex1.width, tex1.height), new Vector2(0.5f, 0.5f), 100.0f);
                    pancard.texture = adharB.texture;
                }
                else
                {
                    adharBack.gameObject.SetActive(false);
                }

                GameManager.pancard = jsonvale["result_push"][0]["pan_pic"].ToString();
                if (jsonvale["result_push"][0]["pan_pic"].ToString() != "0")
                {
                    pancard.gameObject.SetActive(true);
                    UnityWebRequest unityWebRequest2 = UnityWebRequest.Get(GameManager.pancard);
                    yield return unityWebRequest2.SendWebRequest();
                    byte[] bytes2 = unityWebRequest2.downloadHandler.data;
                    Texture2D tex2 = new Texture2D(2, 2);
                    tex2.LoadImage(bytes2);
                    Sprite panC = Sprite.Create(tex2, new Rect(0.0f, 0.0f, tex2.width, tex2.height), new Vector2(0.5f, 0.5f), 100.0f);
                    adharBack.texture = panC.texture;
                }
                else
                {
                    pancard.gameObject.SetActive(false);
                }
                playerImageUrl = jsonvale["result_push"][0]["profile_pic"].ToString();
                if (jsonvale["result_push"][0]["profile_pic"].ToString() != "0")
                {
                    playerImage.gameObject.SetActive(true);
                    playerImageUrl = jsonvale["result_push"][0]["profile_pic"].ToString();
                    UnityWebRequest unityWebRequest3 = UnityWebRequest.Get(playerImageUrl);
                    yield return unityWebRequest3.SendWebRequest();
                    byte[] bytes3 = unityWebRequest3.downloadHandler.data;
                    Texture2D tex3 = new Texture2D(2, 2);
                    tex3.LoadImage(bytes3);
                    Sprite playerimage = Sprite.Create(tex3, new Rect(0.0f, 0.0f, tex3.width, tex3.height), new Vector2(0.5f, 0.5f), 100.0f);
                    playerImage.texture = playerimage.texture;
                    playerImage3.texture = playerimage.texture;
                    GameManager.profileImge = playerimage;
                }
                else
                {
                    GameManager.profileImge = defaultPP;
                    playerImage.texture = defaultPP.texture;
                    playerImage3.texture = defaultPP.texture;
                    //playerImage.gameObject.SetActive(false);
                }

                GameManager.playerName = jsonvale["result_push"][0]["fullname"].ToString();
                nameP.text = GameManager.playerName;
            }
            else
            {
                if (canRegister)
                {
                    if (results == "wrong username and password" || status == "False")
                    {
                        Invoke(nameof(AddSocialMediaSinupForm), 1);
                    }
                    else if (results == "User does not exist" || status == "False")
                    {
                        Invoke(nameof(AddSocialMediaSinupForm), 1);
                    }
                    else if (status == "false" || results == "Login Fail")
                    {
                        Invoke(nameof(AddSocialMediaSinupForm), 1);
                    }
                    else
                    {
                        Invoke(nameof(AddSocialMediaSinupForm), 1);
                    }
                }
                else
                {
                    canRegister = true;
                    loginerror.text = "Invalid Login";
                    Debug.Log("error");
                    FindObjectOfType<GoogleSignInDemo>().SignOutFromGooglePublic1();
                    FindObjectOfType<FacebookScriptDav>().SignOutFromFacebookPublic1();
                    loadinPanel.SetActive(false);
                    UIFlowHandler.uihandler.loadingPanel.SetActive(false);
                    UIFlowHandler.uihandler.ShowError(results, "Error!");
                    StopCoroutine(ErrorClose());
                    StartCoroutine(ErrorClose());
                }
            }
        }
        else
        {
            if (canRegister)
                Invoke(nameof(AddSocialMediaSinupForm), 1);
            else
            {
                canRegister = true;
                loginerror.text = "Invalid Login";
                Debug.LogError("error Text: " + _w.text);
                Debug.LogError("error: " + _w.error);
                FindObjectOfType<GoogleSignInDemo>().SignOutFromGooglePublic1();
                FindObjectOfType<FacebookScriptDav>().SignOutFromFacebookPublic1();
                UIFlowHandler.uihandler.ShowError(_w.error, "Error!");
                loadinPanel.SetActive(false);
                UIFlowHandler.uihandler.loadingPanel.SetActive(false);
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
            }
        }

    }

    public void AddSocialMediaSinupForm()
    {
        Debug.LogError("Social Media Signup Form");
        string uurl = GameManager.apiBase1 + "signup";
        //Debug.Log(url);

        WWWForm form = new WWWForm();
        string username;
        if (socialEmail.Contains("@"))
            username = socialEmail.Remove(socialEmail.IndexOf('@'), socialEmail.Length - socialEmail.IndexOf('@'));
        else username = socialEmail;
        form.AddField("username", username);
        form.AddField("password", "QAZWSX");
        form.AddField("fullname", socialName);
        form.AddField("dob", "01" + "-" + "01" + "-" + "1990");
        double phoneNumberrr = UnityEngine.Random.Range(1000000000, 9999999999);
        form.AddField("mobile", phoneNumberrr.ToString());
        form.AddField("email", socialEmail);
        form.AddField("vcode", Application.version);
        //form.AddField("username", "wahoplay");
        //form.AddField("password", "QAZWSX");
        //form.AddField("fullname", "Waho Play");
        //form.AddField("dob", "01" + "-" + "01" + "-" + "1990");
        //double phoneNumberrr = UnityEngine.Random.Range(1000000000, 9999999999);
        //form.AddField("mobile", phoneNumberrr.ToString());
        //form.AddField("email", "wahoplay@gmail.com");
        //form.AddField("vcode", Application.version);

        //Debug.LogError("USername: " + username);
        Debug.LogError("Password: " + "QAZWSX");
        Debug.LogError("fullname: " + socialName);
        Debug.LogError("DOB: " + "01" + "-" + "01" + "-" + "1990");

        WWW w = new WWW(uurl, form);
        loadinPanel.SetActive(true);
        StartCoroutine(SocialSignup_time(w));
    }

    public string socialEmail, socialName;
    bool canRegister = true;

    public void SocialMediaSignInStart()
    {
        //AddSocialMediaSinupForm();
        UIFlowHandler.uihandler.loadingPanel.SetActive(true);
        string loginurl = GameManager.apiBase1 + "login";
        WWWForm form = new WWWForm();
        string username;
        if (socialEmail.Contains("@"))
            username = socialEmail.Remove(socialEmail.IndexOf('@'), socialEmail.Length - socialEmail.IndexOf('@'));
        else username = socialEmail;
        form.AddField("username", username);
        form.AddField("password", "QAZWSX");
        CloseSplash();
        Debug.Log("userName" + socialEmail);
        Debug.Log("Paas" + socialName);
        WWW w = new WWW(loginurl, form);
        loadinPanel.SetActive(true);
        StartCoroutine(SocialLoginAPI(w));
        Invoke(nameof(Delayforloading), 3f);
    }

    IEnumerator SocialSignup_time(WWW _w)
    {
        Debug.LogError("Social Media Signup time");
        canRegister = false;
        CloseSplash();
        yield return _w;

        if (true)
        {
            Debug.LogError("responce=" + _w.text);
            Debug.LogError("Error = " + _w.error);
            if (_w.error == null)
            {
                JsonData jsonvale = JsonMapper.ToObject(_w.text);
                //  string userID = jsonvale["result_push"][0]["my_referral_id"].ToString();
                string results = jsonvale["result_push"][0]["message"].ToString();
                status = jsonvale["result_push"][0]["status"].ToString();
                string user = jsonvale["result_push"][0]["username"].ToString();
                userStatus = jsonvale["result_push"][0]["status"].ToString();
                //Debug.LogError("VErsion: " + jsonvale["result_push"][0]["version"].ToString());
                GameManager.friendrefferalCode = jsonvale["result_push"][0]["reference_code"].ToString();
                Debug.Log("Code" + GameManager.friendrefferalCode);
                Debug.Log(results);
                // GameManager.Instance.nameMy = GetDataValue(msg, "fullname:");
                // GameManager.Instance.avatarMyIndex = UnityEngine.Random.Range(0, 22);
                // GameManager.Instance.avatarMy = GameManager.Instance.playfabManager.staticGameVariables.avatars[ GameManager.Instance.avatarMyIndex];
                // GameManager.Instance.nameMy = _name.text;

                if (results == "Registration successfull" || status == "True")
                {
                    canRegister = true;
                    // SceneManager.LoadScene ("MenuScene");
                    //  LoginPanel.SetActive(true);
                    Debug.LogError("Registeration succesful");
                    SignupPanel.SetActive(false);
                    loadinPanel.SetActive(false);
                    isFirstTimeLogin = true;
                    SocialMediaSignInStart();

                    //successPanel.SetActive(true);
                    //Invoke(nameof(RegisterDone), 2);
                    //  CloseSplash();
                    //  ExitGames.Client.Photon.Hashtable someCustomPropertiesToSet = new   ExitGames.Client.Photon.Hashtable() {{"name", GameManager.Instance.nameMy},{"avatarId",  GameManager.Instance.avatarMyIndex}};
                    //  PhotonNetwork.player.SetCustomProperties(someCustomPropertiesToSet);
                    //  Debug.Log("playername "+   PhotonNetwork.player.CustomProperties["name"]);
                    //  GameManager.Instance.userID = userID;
                    //  PlayerPrefs.SetString("Logintoken",GameManager.Instance.userID);
                    //  GameManager.Instance.coinsCount = 0;//int.Parse(GetDataValue(msg, "coins:"));
                    // PlayerPrefs.SetString("Logintoken",GetDataValue(msg, "userid:"));

                    _name.text = "";
                    _SignUppassword.text = "";
                    _confirmPassword.text = "";
                    _date.text = "";
                    _mobileNumber.text = "";
                    _month.text = "";
                    _year.text = "";
                    _emailId.text = "";
                    _password.text = "";
                }
                else if (results == "The username has already been taken" || status == "false")
                {
                    canRegister = true;
                    errorMsg.text = "The username has already been taken";
                    FindObjectOfType<GoogleSignInDemo>().SignOutFromGooglePublic1();
                    FindObjectOfType<FacebookScriptDav>().SignOutFromFacebookPublic1();
                    UIFlowHandler.uihandler.ShowError(results, "Error!");
                    loadinPanel.SetActive(false);
                    StopCoroutine(ErrorClose());
                    StartCoroutine(ErrorClose());
                }
                else
                {
                    canRegister = true;
                    Debug.Log("A" + results);
                    errorMsg.text = results;
                    FindObjectOfType<GoogleSignInDemo>().SignOutFromGooglePublic1();
                    FindObjectOfType<FacebookScriptDav>().SignOutFromFacebookPublic1();
                    UIFlowHandler.uihandler.ShowError("Unable to Sinup!", "Error!");
                    StopCoroutine(ErrorClose());
                    StartCoroutine(ErrorClose());
                }
                UIFlowHandler.uihandler.loadingPanel.SetActive(false);
            }
            else
            {
                if (results == "The mobile has already been taken." || status == "false")
                {
                    canRegister = true;
                    errorMsg.text = "The mobile has already been taken.";
                    loadinPanel.SetActive(false);
                    UIFlowHandler.uihandler.loadingPanel.SetActive(false);
                    UIFlowHandler.uihandler.ShowError(results, "Error!");
                    StopCoroutine(ErrorClose());
                    StartCoroutine(ErrorClose());
                    //FindObjectOfType<GoogleSignInDemo>().SignOutFromGooglePublic1();
                    //FindObjectOfType<FacebookScriptDav>().SignOutFromFacebookPublic1();
                }
                else
                    UIFlowHandler.uihandler.ShowError(_w.error, "Error!");
                Debug.Log("error");
                Debug.Log(results);
                if (_w.text != "" && _w.text != null)
                {
                    Debug.LogError("Should owrk");
                    //GetErrors abc = JsonUtility.FromJson<GetErrors>(_w.text);
                    //Debug.LogError("Should owrk");
                    //errorMsg.text = abc.errors[0];
                    Debug.LogError("Should owrk");
                }
                else errorMsg.text = "Sinup Failed";
                Debug.LogError("Should owrk");
                canRegister = true;
                loadinPanel.SetActive(false);
                UIFlowHandler.uihandler.loadingPanel.SetActive(false);
                FindObjectOfType<GoogleSignInDemo>().SignOutFromGooglePublic1();
                FindObjectOfType<FacebookScriptDav>().SignOutFromFacebookPublic1();
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
            }
        }
    }

    #endregion

    #region Phone Number Login

    IEnumerator PhoneLoginAPI(WWW _w)
    {
        Debug.LogError("Social Login API");

        yield return _w;
        Debug.LogError("responce=" + _w.text);
        Debug.LogError("responce error=" + _w.error);
        if (_w.error == null)
        {

            string msg = _w.text;
            msg = msg.Replace("{", "");
            msg = msg.Replace("}", "");
            msg = msg.Replace("[", "");
            msg = msg.Replace("]", "");
            msg = msg.Replace(@"""", string.Empty);

            results = GetDataValue(msg, "message:");
            status = GetDataValue(msg, "status:");
            GameManager.Instance.nameMy = GetDataValue(msg, "username:");

            print("Results occur" + Results);
            if (results == "Login Successfull" || status == "True")
            {
                //newGameManager.newLoginScreen.SetActive(false);
                //newGameManager.mobileVerificationScreen.SetActive(false);
                //newGameManager.EnterYourPinScreen.SetActive(false);
                canRegisterPhone = true;
                //loginPanel.SetActive(false);
                GameManager.Instance.avatarMyIndex = UnityEngine.Random.Range(0, 22);
                GameManager.Instance.avatarMy = GameManager.Instance.playfabManager.staticGameVariables.avatars[GameManager.Instance.avatarMyIndex];
                Debug.Log("playername " + PhotonNetwork.player.CustomProperties["name"]);
                GameManager.Instance.coinsCount = float.Parse(GetDataValue(msg, "coins:"));
                GameManager.onlineAmount = GetDataValue(msg, "online_amount:");
                GameManager.offlineAmount = GetDataValue(msg, "offline_amount:");
                Debug.Log(GameManager.Instance.nameMy);
                Debug.Log(GameManager.Instance.coinsCount);
                Debug.Log(results);
                Debug.Log(status);
                LoginPanel.SetActive(false);
                choosePanel.SetActive(false);
                splashCanvas.SetActive(false);
                loadinPanel.SetActive(false);
                _emailId.text = "";
                _password.text = "";
                GameManager.Instance.userID = GetDataValue(msg, "my_referral_code:");
                GameManager.Uid = GetDataValue(msg, "uid:");
                GameManager.friendrefferalCode = GetDataValue(msg, "reference_code:");
                //FindObjectOfType<PickerController>().ChangePlayerTextureFromOutside(UIFlowHandler.uihandler.THETEXTURE);
                Debug.Log("UID" + GameManager.Uid);
                Debug.Log("codeRR" + GameManager.friendrefferalCode);
                Debug.Log("my_referral_code " + GameManager.Instance.userID);
                PlayerPrefs.SetString("Logintoken", GameManager.Instance.userID);


                // Reedem Amount Data...!!!
                JsonData jsonvale = JsonMapper.ToObject(_w.text);

                try
                {
                    GameManager.appVersionFromApi = jsonvale["result_push"][0]["version"].ToString();
                }
                catch
                {
                    Debug.LogError("Catch for version called");
                }
                GameManager.playerName = jsonvale["result_push"][0]["fullname"].ToString();
                ExitGames.Client.Photon.Hashtable someCustomPropertiesToSet = new ExitGames.Client.Photon.Hashtable() { { "name", GameManager.Instance.nameMy }, { "avatarId", GameManager.Instance.avatarMyIndex }, { "fullName", GameManager.playerName } };
                PhotonNetwork.player.SetCustomProperties(someCustomPropertiesToSet);
                if (jsonvale["result_push"][0]["usertype"].ToString() == "1")
                {
                    isEmployee = true;
                }
                else isEmployee = false;
                GameManager.mobileNumber = jsonvale["result_push"][0]["mobile"].ToString();
                GameManager.emailId = jsonvale["result_push"][0]["email"].ToString();
                PlayerPrefs.SetString("FriendRefCodePP", jsonvale["result_push"][0]["referal_refer_code"].ToString());
                emailID.text = GameManager.emailId;
                phoneNumber.text = GameManager.mobileNumber;
                playerName.text = GameManager.playerName;

                Debug.Log("Name" + GameManager.playerName);
                Debug.Log("Mobile" + GameManager.mobileNumber);
                Debug.Log("Emaild" + GameManager.emailId);
                pytmNumber = (jsonvale["result_push"][0]["paytm"].ToString());
                GameManager.paytmNumber = pytmNumber;
                Debug.Log("Pytm" + GameManager.paytmNumber);

                string activePlayer = jsonvale["result_push"][0]["active_game_info"].ToJson();
                Debug.LogError(activePlayer);
                activePlayer = "{\"result_push\":" + activePlayer + "}";
                ///*LiveGameInfo myobj = */
                // JsonUtility.FromJson<LiveGameInfo>(activePlayer);
                JsonData jsonvale1 = JsonMapper.ToObject(activePlayer);
                for (int i = 0; i < jsonvale1["result_push"].Count; i++)
                {
                    ActiveGamInfo temp = new ActiveGamInfo();
                    temp.game_id = jsonvale1["result_push"][i]["game_id"].ToString();
                    temp.game_room_id = jsonvale1["result_push"][i]["game_room_id"].ToString();
                    if (jsonvale1["result_push"][i]["first_player"].ToString() != null)
                    {
                        temp.first_Player_Name = jsonvale1["result_push"][i]["first_player"].ToString();
                    }
                    if (jsonvale1["result_push"][i]["second_player"].ToString() != null)
                    {
                        temp.second_Player_Name = jsonvale1["result_push"][i]["second_player"].ToString();
                    }
                    if (jsonvale1["result_push"][i]["third_player"].ToString() != null)
                    {
                        temp.third_Player_Name = jsonvale1["result_push"][i]["third_player"].ToString();
                    }
                    if (jsonvale1["result_push"][i]["fourth_player"].ToString() != null)
                    {
                        temp.fourth_Player_Name = jsonvale1["result_push"][i]["fourth_player"].ToString();
                    }
                    GameManager.activeGameInfo.Add(temp);
                }

                KYCDob = jsonvale["result_push"][0]["dob"].ToString();
                KYCadharNumber = jsonvale["result_push"][0]["aadhar"].ToString();
                KYCpanNumber = jsonvale["result_push"][0]["pan"].ToString();
                KYCState = jsonvale["result_push"][0]["state"].ToString();
                if (KYCadharNumber == "0" || KYCadharNumber == "" || KYCadharNumber == null)
                {
                    KYCadhaarStatus = "Not Verified";
                }
                else KYCadhaarStatus = "Verified";
                if (KYCpanNumber == "0" || KYCpanNumber == "" || KYCpanNumber == null)
                {
                    KYCpanStatus = "Not Verified";
                }
                else KYCpanStatus = "Verified";

                string tempStatus = jsonvale["result_push"][0]["kyc_status"].ToString();
                if (tempStatus == "0") KYCStatus = "Pending";
                else if (tempStatus == "1") KYCStatus = "Verified";
                else if (tempStatus == "2") KYCStatus = "Rejected";
                else KYCStatus = "Not Verified";
                upi = (jsonvale["result_push"][0]["upi"].ToString());
                GameManager.upiID = upi;
                Debug.Log("UPI" + GameManager.upiID);


                bank = (jsonvale["result_push"][0]["bank_name"].ToString());
                GameManager.bankName = bank;
                Debug.Log("bank" + GameManager.bankName);
                if (bank != "" && bank != null)
                {
                    bankeDetailIP.text = "Verified";
                    bankDetailButtonText.text = "Edit";
                }

                ifscCode = (jsonvale["result_push"][0]["ifsc_code"].ToString());
                GameManager.bankIfscCode = ifscCode;
                Debug.Log("IFSC" + GameManager.bankIfscCode);


                accoutnumber = (jsonvale["result_push"][0]["account"].ToString());
                GameManager.accountNumber = accoutnumber;
                Debug.Log("IFSC" + GameManager.accountNumber);


                // Player Profile Data..!!!

                mainAccountBalance.text = GameManager.Instance.coinsCount.ToString();

                GameManager.depositAmount = jsonvale["result_push"][0]["deposite_amount"].ToString();
                totalDeposit.text = GameManager.depositAmount;

                GameManager.withdraw = jsonvale["result_push"][0]["withdraw_amount"].ToString();
                totalWithDrawal.text = GameManager.withdraw;

                GameManager.gamesWon = jsonvale["result_push"][0]["games_won"].ToString();
                totalgamePlay.text = GameManager.gamesWon;

                GameManager.cashWon = jsonvale["result_push"][0]["cash_won"].ToString();
                totalCashWin.text = GameManager.cashWon;

                GameManager.playerName = jsonvale["result_push"][0]["fullname"].ToString();
                nameP.text = GameManager.playerName;

                playername.text = GameManager.playerName;

                newTotalBalance.text = GameManager.Instance.coinsCount.ToString();
                newAddedAmount.text = GameManager.depositAmount;
                newWithdrawableAmount.text = GameManager.withdraw;
                LoadingPage.SetActive(true);
                UIFlowHandler.uihandler.loadingPanel.SetActive(false);
                SceneManager.LoadScene("MenuScene");

                GameManager.adharcardFront = jsonvale["result_push"][0]["aadhar_first"].ToString();
                if (jsonvale["result_push"][0]["aadhar_first"].ToString() != "0")
                {
                    adharFront.gameObject.SetActive(true);
                    UnityWebRequest unityWebRequest = UnityWebRequest.Get(GameManager.adharcardFront);
                    yield return unityWebRequest.SendWebRequest();
                    byte[] bytes = unityWebRequest.downloadHandler.data;
                    Texture2D tex = new Texture2D(2, 2);
                    tex.LoadImage(bytes);
                    Sprite adharF = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                    adharFront.texture = adharF.texture;
                }
                else
                {
                    adharFront.gameObject.SetActive(false);
                }
                GameManager.adharcardBack = jsonvale["result_push"][0]["aadhar_second"].ToString();
                if (jsonvale["result_push"][0]["aadhar_second"].ToString() != "0")
                {
                    adharBack.gameObject.SetActive(true);
                    UnityWebRequest unityWebRequest1 = UnityWebRequest.Get(GameManager.adharcardBack);
                    yield return unityWebRequest1.SendWebRequest();
                    byte[] bytes1 = unityWebRequest1.downloadHandler.data;
                    Texture2D tex1 = new Texture2D(2, 2);
                    tex1.LoadImage(bytes1);
                    Sprite adharB = Sprite.Create(tex1, new Rect(0.0f, 0.0f, tex1.width, tex1.height), new Vector2(0.5f, 0.5f), 100.0f);
                    pancard.texture = adharB.texture;
                }
                else
                {
                    adharBack.gameObject.SetActive(false);
                }

                GameManager.pancard = jsonvale["result_push"][0]["pan_pic"].ToString();
                if (jsonvale["result_push"][0]["pan_pic"].ToString() != "0")
                {
                    pancard.gameObject.SetActive(true);
                    UnityWebRequest unityWebRequest2 = UnityWebRequest.Get(GameManager.pancard);
                    yield return unityWebRequest2.SendWebRequest();
                    byte[] bytes2 = unityWebRequest2.downloadHandler.data;
                    Texture2D tex2 = new Texture2D(2, 2);
                    tex2.LoadImage(bytes2);
                    Sprite panC = Sprite.Create(tex2, new Rect(0.0f, 0.0f, tex2.width, tex2.height), new Vector2(0.5f, 0.5f), 100.0f);
                    adharBack.texture = panC.texture;
                }
                else
                {
                    pancard.gameObject.SetActive(false);
                }
                playerImageUrl = jsonvale["result_push"][0]["profile_pic"].ToString();
                Debug.LogWarning("Player img urllll: " + playerImageUrl);
                if (jsonvale["result_push"][0]["profile_pic"].ToString() != "0" && jsonvale["result_push"][0]["profile_pic"].ToString() != "")
                {
                    playerImage.gameObject.SetActive(true);
                    playerImageUrl = jsonvale["result_push"][0]["profile_pic"].ToString();
                    UnityWebRequest unityWebRequest3 = UnityWebRequest.Get(playerImageUrl);
                    yield return unityWebRequest3.SendWebRequest();
                    byte[] bytes3 = unityWebRequest3.downloadHandler.data;
                    Texture2D tex3 = new Texture2D(2, 2);
                    tex3.LoadImage(bytes3);
                    Sprite playerimage = Sprite.Create(tex3, new Rect(0.0f, 0.0f, tex3.width, tex3.height), new Vector2(0.5f, 0.5f), 100.0f);
                    playerImage.texture = playerimage.texture;
                    playerImage3.texture = playerimage.texture;
                    GameManager.profileImge = playerimage;
                }
                else
                {
                    GameManager.profileImge = defaultPP;
                    playerImage.texture = defaultPP.texture;
                    playerImage3.texture = defaultPP.texture;
                    //playerImage.gameObject.SetActive(false);
                }

                GameManager.playerName = jsonvale["result_push"][0]["fullname"].ToString();
                nameP.text = GameManager.playerName;
            }
            else
            {
                if (canRegisterPhone)
                {
                    if (results == "wrong username and password" || status == "False")
                    {
                        Invoke(nameof(AddPhoneMediaSinupForm), 1);
                    }
                    else if (results == "User does not exist" || status == "False")
                    {
                        Invoke(nameof(AddPhoneMediaSinupForm), 1);
                    }
                    else if (status == "false" || results == "Login Fail")
                    {
                        Invoke(nameof(AddPhoneMediaSinupForm), 1);
                    }
                    else
                    {
                        Invoke(nameof(AddPhoneMediaSinupForm), 1);
                    }
                }
                else
                {
                    canRegisterPhone = true;
                    loginerror.text = "Invalid Login";
                    Debug.Log("error");
                    UIFlowHandler.uihandler.loadingPanel.SetActive(false);
                    UIFlowHandler.uihandler.ShowError(results, "Error!");
                    FindObjectOfType<GoogleSignInDemo>().SignOutFromGooglePublic1();
                    FindObjectOfType<FacebookScriptDav>().SignOutFromFacebookPublic1();
                    loadinPanel.SetActive(false);
                    StopCoroutine(ErrorClose());
                    StartCoroutine(ErrorClose());
                }
            }
        }
        else
        {
            if (canRegisterPhone)
                Invoke(nameof(AddPhoneMediaSinupForm), 1);
            else
            {
                canRegisterPhone = true;
                loginerror.text = "Invalid Login";
                Debug.LogError("error Text: " + _w.text);
                Debug.LogError("error: " + _w.error);
                UIFlowHandler.uihandler.loadingPanel.SetActive(false);
                UIFlowHandler.uihandler.ShowError(_w.error, "Error!");
                FindObjectOfType<GoogleSignInDemo>().SignOutFromGooglePublic1();
                FindObjectOfType<FacebookScriptDav>().SignOutFromFacebookPublic1();
                loadinPanel.SetActive(false);
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
            }
        }

    }

    Hashtable emailNames = new Hashtable()
    {
        {"1","Ran"}, {"2","Ema"},{"3","Lud"},{"4","Gam"},{"5","Dam"}
    };

    public void AddPhoneMediaSinupForm()
    {
        Debug.LogError("Phone Media Signup Form");
        string uurl = GameManager.apiBase1 + "signup";
        //Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("username", phoneEmail);
        form.AddField("password", "QAZWSX");
        form.AddField("fullname", phoneEmail.Remove(0, 2));
        form.AddField("dob", "01" + "-" + "01" + "-" + "1990");
        form.AddField("mobile", phoneEmail.Remove(0, 2));
        string prefix = emailNames[UnityEngine.Random.Range(1, 6).ToString()].ToString();
        string email = prefix + UnityEngine.Random.Range(10000000, 99999999) + "@gmail.com";
        form.AddField("email", email.ToString());
        form.AddField("vcode", Application.version);

        Debug.LogError("USername: " + phoneEmail);
        Debug.LogError("Password: " + "QAZWSX");
        Debug.LogError("email: " + email);
        Debug.LogError("DOB: " + "01" + "-" + "01" + "-" + "1990");

        Debug.LogError(uurl);
        WWW w = new WWW(uurl, form);
        loadinPanel.SetActive(true);
        GameManager.mobileNumber = "0123456789";
        Debug.Log("MobileNum" + GameManager.mobileNumber);
        StartCoroutine(PhoneSignup_time(w));
    }

    public string phoneEmail;
    bool canRegisterPhone = true;

    public void PhoneMediaSignInStart()
    {
        UIFlowHandler.uihandler.loadingPanel.SetActive(true);
        //AddSocialMediaSinupForm();
        string loginurl = GameManager.apiBase1 + "login";
        WWWForm form = new WWWForm();
        form.AddField("username", phoneEmail);
        if(!isTestingLogin) form.AddField("password", "QAZWSX");
        else form.AddField("password", "222222");
        CloseSplash();
        Debug.Log("userName" + phoneEmail);
        WWW w = new WWW(loginurl, form);
        loadinPanel.SetActive(true);
        StartCoroutine(PhoneLoginAPI(w));
        Invoke(nameof(Delayforloading), 3f);
    }

    IEnumerator PhoneSignup_time(WWW _w)
    {
        Debug.LogError("Social Media Signup time");
        canRegisterPhone = false;
        CloseSplash();
        yield return _w;

        if (true)
        {
            Debug.LogError("responce=" + _w.text);
            Debug.LogError("Error = " + _w.error);
            if (_w.error == null)
            {
                JsonData jsonvale = JsonMapper.ToObject(_w.text);
                //  string userID = jsonvale["result_push"][0]["my_referral_id"].ToString();
                string results = jsonvale["result_push"][0]["message"].ToString();
                status = jsonvale["result_push"][0]["status"].ToString();
                string user = jsonvale["result_push"][0]["username"].ToString();
                userStatus = jsonvale["result_push"][0]["status"].ToString();
                //Debug.LogError("VErsion: " + jsonvale["result_push"][0]["version"].ToString());
                GameManager.friendrefferalCode = jsonvale["result_push"][0]["reference_code"].ToString();
                Debug.Log("Code" + GameManager.friendrefferalCode);
                Debug.Log(results);
                // GameManager.Instance.nameMy = GetDataValue(msg, "fullname:");
                // GameManager.Instance.avatarMyIndex = UnityEngine.Random.Range(0, 22);
                // GameManager.Instance.avatarMy = GameManager.Instance.playfabManager.staticGameVariables.avatars[ GameManager.Instance.avatarMyIndex];
                // GameManager.Instance.nameMy = _name.text;

                if (results == "Registration successfull" || status == "True")
                {
                    canRegisterPhone = true;
                    // SceneManager.LoadScene ("MenuScene");
                    //  LoginPanel.SetActive(true);
                    Debug.LogError("Registeration succesful");
                    SignupPanel.SetActive(false);
                    loadinPanel.SetActive(false);
                    isFirstTimeLogin = true;
                    PhoneMediaSignInStart();

                    //successPanel.SetActive(true);
                    //Invoke(nameof(RegisterDone), 2);
                    //  CloseSplash();
                    //  ExitGames.Client.Photon.Hashtable someCustomPropertiesToSet = new   ExitGames.Client.Photon.Hashtable() {{"name", GameManager.Instance.nameMy},{"avatarId",  GameManager.Instance.avatarMyIndex}};
                    //  PhotonNetwork.player.SetCustomProperties(someCustomPropertiesToSet);
                    //  Debug.Log("playername "+   PhotonNetwork.player.CustomProperties["name"]);
                    //  GameManager.Instance.userID = userID;
                    //  PlayerPrefs.SetString("Logintoken",GameManager.Instance.userID);
                    //  GameManager.Instance.coinsCount = 0;//int.Parse(GetDataValue(msg, "coins:"));
                    // PlayerPrefs.SetString("Logintoken",GetDataValue(msg, "userid:"));

                    _name.text = "";
                    _SignUppassword.text = "";
                    _confirmPassword.text = "";
                    _date.text = "";
                    _mobileNumber.text = "";
                    _month.text = "";
                    _year.text = "";
                    _emailId.text = "";
                    _password.text = "";
                }
                else if (results == "The username has already been taken" || status == "false")
                {
                    UIFlowHandler.uihandler.loadingPanel.SetActive(false);
                    canRegisterPhone = true;
                    errorMsg.text = "The username has already been taken";
                    FindObjectOfType<GoogleSignInDemo>().SignOutFromGooglePublic1();
                    FindObjectOfType<FacebookScriptDav>().SignOutFromFacebookPublic1();
                    UIFlowHandler.uihandler.ShowError(results, "Error!");
                    loadinPanel.SetActive(false);
                    StopCoroutine(ErrorClose());
                    StartCoroutine(ErrorClose());
                }
                else
                {
                    UIFlowHandler.uihandler.loadingPanel.SetActive(false);
                    canRegisterPhone = true;
                    Debug.Log("A" + results);
                    errorMsg.text = results;
                    FindObjectOfType<GoogleSignInDemo>().SignOutFromGooglePublic1();
                    FindObjectOfType<FacebookScriptDav>().SignOutFromFacebookPublic1();
                    UIFlowHandler.uihandler.ShowError(results, "Error!");
                    StopCoroutine(ErrorClose());
                    StartCoroutine(ErrorClose());
                }
            }
            else
            {
                if (results == "The mobile has already been taken." || status == "false")
                {
                    canRegisterPhone = true;
                    errorMsg.text = "The mobile has already been taken.";
                    loadinPanel.SetActive(false);
                    UIFlowHandler.uihandler.ShowError(results, "Error!");
                    StopCoroutine(ErrorClose());
                    StartCoroutine(ErrorClose());
                    //FindObjectOfType<GoogleSignInDemo>().SignOutFromGooglePublic1();
                    //FindObjectOfType<FacebookScriptDav>().SignOutFromFacebookPublic1();
                }
                Debug.LogError("error");
                Debug.LogError(results);
                if (_w.text != "" && _w.text != null)
                {
                    Debug.LogError("Should owrk");
                    //GetErrors abc = JsonUtility.FromJson<GetErrors>(_w.text);
                    //Debug.LogError("Should owrk");
                    //errorMsg.text = abc.errors[0];
                    Debug.LogError("Should owrk");
                }
                else errorMsg.text = "Sinup Failed";
                Debug.LogError("Should owrk");
                canRegisterPhone = true;
                loadinPanel.SetActive(false);
                FindObjectOfType<GoogleSignInDemo>().SignOutFromGooglePublic1();
                FindObjectOfType<FacebookScriptDav>().SignOutFromFacebookPublic1();
                UIFlowHandler.uihandler.loadingPanel.SetActive(false);
                UIFlowHandler.uihandler.ShowError("Unable to Sign you up.", "Error!");
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
            }
        }
    }

    #endregion

    string userStatus;
    public void OnRegisterBtnClick()
    {
        if (string.IsNullOrEmpty(_name.text))
        {
            Debug.LogError("Name");
            errorMsg.text = "Name Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }


        if (string.IsNullOrEmpty(_SignUppassword.text))
        {
            Debug.LogError("Password");
            errorMsg.text = "Password Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }

        if (string.IsNullOrEmpty(_confirmPassword.text))
        {
            errorMsg.text = "Full Name Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }

        if (string.IsNullOrEmpty(_date.text))
        {
            errorMsg.text = "DOB Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        if (string.IsNullOrEmpty(_month.text))
        {
            errorMsg.text = "month Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        if (string.IsNullOrEmpty(_year.text))
        {
            errorMsg.text = "Year Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }

        if (string.IsNullOrEmpty(_mobileNumber.text))
        {
            errorMsg.text = "Mobile Number Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        if (string.IsNullOrEmpty(_emailIdR.text))
        {
            errorMsg.text = "Email Id Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }

        dob = _date.text + "-" + _month.text + "-" + _year.text;
        Debug.Log("Invalid Birth Day");
        int Todaydate = int.Parse(DateTime.Now.ToString("dd"));
        int TodayMOnth = int.Parse(DateTime.Now.ToString("MM"));
        int TodayYear = int.Parse(DateTime.Now.ToString("yyyy"));

        print("Today Date time line" + Todaydate + TodayMOnth + TodayYear);
        if (Todaydate >= int.Parse(_date.text))
        {
            if (TodayMOnth >= int.Parse(_month.text))
            {
                if (TodayYear >= int.Parse(_year.text))
                {
                    print("Simple calculation");
                    print("age " + (TodayYear - int.Parse(_year.text)));
                    age = (TodayYear - int.Parse(_year.text));
                }
            }
            else
            {
                print("Year deducation");
                TodayYear--;
                TodayMOnth += 12;
                if (TodayYear >= int.Parse(_year.text))
                {
                    print("age " + (TodayYear - int.Parse(_year.text)));
                    age = (TodayYear - int.Parse(_year.text));
                }

            }
        }
        else
        {

            TodayMOnth--;
            Todaydate += 30;
            print("Month deducation" + TodayMOnth + "compare with " + int.Parse(_month.text));
            if (TodayMOnth >= int.Parse(_month.text))
            {
                if (TodayYear >= int.Parse(_year.text))
                {
                    print("age " + (TodayYear - int.Parse(_year.text)));
                    age = (TodayYear - int.Parse(_year.text));
                }
            }
            else
            {
                print("Year deducation");
                TodayYear--;
                TodayMOnth += 12;
                if (TodayYear >= int.Parse(_year.text))
                {
                    print("age " + (TodayYear - int.Parse(_year.text)));
                    age = (TodayYear - int.Parse(_year.text));
                }

            }

        }

        if (age <= 17)
        {
            error.SetActive(true);
            disText.text = "You are not +18.";
            return;
        }

        url = GameManager.apiBase1 + "signup";
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("username", _name.text);
        form.AddField("password", _SignUppassword.text);
        form.AddField("fullname", _confirmPassword.text);
        form.AddField("dob", _date.text + "-" + _month.text + "-" + _year.text);
        form.AddField("mobile", _mobileNumber.text);
        form.AddField("email", _emailIdR.text);
        form.AddField("vcode", Application.version);
        form.AddField("token", "sjdsak121");
        Debug.Log("Password" + _SignUppassword.text);
        Debug.Log("confirm_password" + _confirmPassword.text);
        WWW w = new WWW(url, form);
        loadinPanel.SetActive(true);
        GameManager.mobileNumber = _mobileNumber.text;
        Debug.Log("MobileNum" + GameManager.mobileNumber);
        StartCoroutine(Signup_time(w));
    }

    IEnumerator Signup_time(WWW _w)
    {
        CloseSplash();
        yield return _w;

        print("responce=" + _w.text);
        if (_w.error == null)
        {
            JsonData jsonvale = JsonMapper.ToObject(_w.text);
            //  string userID = jsonvale["result_push"][0]["my_referral_id"].ToString();
            string results = jsonvale["result_push"][0]["message"].ToString();
            status = jsonvale["result_push"][0]["status"].ToString();
            string user = jsonvale["result_push"][0]["username"].ToString();
            userStatus = jsonvale["result_push"][0]["status"].ToString();
            //Debug.LogError("VErsion: " + jsonvale["result_push"][0]["version"].ToString());
            GameManager.friendrefferalCode = jsonvale["result_push"][0]["reference_code"].ToString();
            Debug.Log("Code" + GameManager.friendrefferalCode);
            Debug.Log(results);
            // GameManager.Instance.nameMy = GetDataValue(msg, "fullname:");
            // GameManager.Instance.avatarMyIndex = UnityEngine.Random.Range(0, 22);
            // GameManager.Instance.avatarMy = GameManager.Instance.playfabManager.staticGameVariables.avatars[ GameManager.Instance.avatarMyIndex];
            // GameManager.Instance.nameMy = _name.text;

            if (results == "Registration successfull" || status == "True")
            {

                // SceneManager.LoadScene ("MenuScene");
                //  LoginPanel.SetActive(true);
                SignupPanel.SetActive(false);
                loadinPanel.SetActive(false);
                successPanel.SetActive(true);
                Invoke(nameof(RegisterDone), 2);
                //  CloseSplash();
                //  ExitGames.Client.Photon.Hashtable someCustomPropertiesToSet = new   ExitGames.Client.Photon.Hashtable() {{"name", GameManager.Instance.nameMy},{"avatarId",  GameManager.Instance.avatarMyIndex}};
                //  PhotonNetwork.player.SetCustomProperties(someCustomPropertiesToSet);
                //  Debug.Log("playername "+   PhotonNetwork.player.CustomProperties["name"]);
                //  GameManager.Instance.userID = userID;
                //  PlayerPrefs.SetString("Logintoken",GameManager.Instance.userID);
                //  GameManager.Instance.coinsCount = 0;//int.Parse(GetDataValue(msg, "coins:"));
                // PlayerPrefs.SetString("Logintoken",GetDataValue(msg, "userid:"));

                _name.text = "";
                _SignUppassword.text = "";
                _confirmPassword.text = "";
                _date.text = "";
                _mobileNumber.text = "";
                _month.text = "";
                _year.text = "";
                _emailId.text = "";
                _password.text = "";
            }
            if (results == "The username has already been taken" || status == "false")
            {
                errorMsg.text = "The username has already been taken";
                loadinPanel.SetActive(false);
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
            }
            else
            {
                Debug.Log("A" + results);
                errorMsg.text = results;
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
            }
        }
        else
        {
            if (results == "The mobile has already been taken." || status == "false")
            {
                errorMsg.text = "The mobile has already been taken.";
                loadinPanel.SetActive(false);
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
            }
            Debug.Log("error");
            Debug.Log(results);
            GetErrors abc = JsonUtility.FromJson<GetErrors>(_w.text);
            errorMsg.text = abc.errors[0];
            loadinPanel.SetActive(false);
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
        }
    }
    string pytmNumber;
    string upi;
    string bank;
    string ifscCode;
    string accoutnumber;

    public void OnPlayerProfileData()
    {
        FindObjectOfType<UIFlowHandler>().loadingPanel.SetActive(true);
        StartCoroutine(GetPlayerData());
    }

    IEnumerator GetPlayerData()
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        string url = GameManager.apiBase1 + "client_details/my_referral_code=" + GameManager.Instance.userID;
        Debug.Log(url);
        WWW www = new WWW(url);
        yield return www;
        Debug.Log(www.text);
        JsonData jsonvale = JsonMapper.ToObject(www.text);
        try
        {
            GameManager.appVersionFromApi = jsonvale["result_push"][0]["version"].ToString();
        }
        catch
        {
            Debug.LogError("Catch for version called");
        }
        userName = jsonvale["result_push"][0]["username"].ToString();
        if (jsonvale["result_push"][0]["usertype"].ToString() == "1")
        {
            isEmployee = true;
        }
        else isEmployee = false;
        float coin = float.Parse(jsonvale["result_push"][0]["coins"].ToString());
        GameManager.Uid = jsonvale["result_push"][0]["uid"].ToString();
        GameManager.friendrefferalCode = jsonvale["result_push"][0]["reference_code"].ToString();
        GameManager.onlineAmount = jsonvale["result_push"][0]["online_amount"].ToString();
        GameManager.offlineAmount = jsonvale["result_push"][0]["offline_amount"].ToString();
        string activePlayer = jsonvale["result_push"][0]["active_game_info"].ToJson();
        activePlayer = "{\"result_push\":" + activePlayer + "}";
        ///*LiveGameInfo myobj = */
        // JsonUtility.FromJson<LiveGameInfo>(activePlayer);
        JsonData jsonvale1 = JsonMapper.ToObject(activePlayer);
        for (int i = 0; i < jsonvale1["result_push"].Count; i++)
        {
            ActiveGamInfo temp = new ActiveGamInfo();
            temp.game_id = jsonvale1["result_push"][i]["game_id"].ToString();
            temp.game_room_id = jsonvale1["result_push"][i]["game_room_id"].ToString();
            if (jsonvale1["result_push"][i]["first_player"].ToString() != null)
            {
                temp.first_Player_Name = jsonvale1["result_push"][i]["first_player"].ToString();
            }
            if (jsonvale1["result_push"][i]["second_player"].ToString() != null)
            {
                temp.second_Player_Name = jsonvale1["result_push"][i]["second_player"].ToString();
            }
            if (jsonvale1["result_push"][i]["third_player"].ToString() != null)
            {
                temp.third_Player_Name = jsonvale1["result_push"][i]["third_player"].ToString();
            }
            if (jsonvale1["result_push"][i]["fourth_player"].ToString() != null)
            {
                temp.fourth_Player_Name = jsonvale1["result_push"][i]["fourth_player"].ToString();
            }
            GameManager.activeGameInfo.Add(temp);
        }
        //Debug.LogError("YAYYY WORKINGGGG : :  : : " + myobj.gameId[0]);


        GameManager.playerName = jsonvale["result_push"][0]["fullname"].ToString();
        nameP.text = GameManager.playerName;

        GameManager.mobileNumber = jsonvale["result_push"][0]["mobile"].ToString();
        GameManager.emailId = jsonvale["result_push"][0]["email"].ToString();
        PlayerPrefs.SetString("FriendRefCodePP", jsonvale["result_push"][0]["referal_refer_code"].ToString());
        emailID.text = GameManager.emailId;
        phoneNumber.text = GameManager.mobileNumber;
        playerName.text = GameManager.playerName;

        Debug.Log("Name" + GameManager.playerName);
        Debug.Log("Mobile" + GameManager.mobileNumber);
        Debug.Log("Emaild" + GameManager.emailId);

        KYCDob = jsonvale["result_push"][0]["dob"].ToString();
        KYCadharNumber = jsonvale["result_push"][0]["aadhar"].ToString();
        KYCpanNumber = jsonvale["result_push"][0]["pan"].ToString();
        KYCState = jsonvale["result_push"][0]["state"].ToString();
        if (KYCadharNumber == "0" || KYCadharNumber == "" || KYCadharNumber == null)
        {
            KYCadhaarStatus = "Not Verified";
        }
        else KYCadhaarStatus = "Verified";
        if (KYCpanNumber == "0" || KYCpanNumber == "" || KYCpanNumber == null)
        {
            KYCpanStatus = "Not Verified";
        }
        else KYCpanStatus = "Verified";
        string tempStatus = jsonvale["result_push"][0]["kyc_status"].ToString();

        if (tempStatus == "0") KYCStatus = "Pending";
        else if (tempStatus == "1") KYCStatus = "Verified";
        else if (tempStatus == "2") KYCStatus = "Rejected";
        else KYCStatus = "Not Verified";
        // Reedem Amount Data...!!!

        pytmNumber = (jsonvale["result_push"][0]["paytm"].ToString());
        GameManager.paytmNumber = pytmNumber;
        // Debug.Log("Pytm" + GameManager.paytmNumber);

        upi = (jsonvale["result_push"][0]["upi"].ToString());
        GameManager.upiID = upi;
        // Debug.Log("UPI" + GameManager.upiID);

        bank = (jsonvale["result_push"][0]["bank_name"].ToString());
        GameManager.bankName = bank;
        // Debug.Log("bank" + GameManager.bankName);
        if (bank != "" && bank != null)
        {
            bankeDetailIP.text = "Verified";
            bankDetailButtonText.text = "Edit";
        }

        ifscCode = (jsonvale["result_push"][0]["ifsc_code"].ToString());
        GameManager.bankIfscCode = ifscCode;
        // Debug.Log("IFSC" + GameManager.bankIfscCode);

        accoutnumber = (jsonvale["result_push"][0]["account"].ToString());
        GameManager.accountNumber = accoutnumber;
        //Debug.Log("IFSC" + GameManager.accountNumber);



        // Player Profile Data..!!!

        mainAccountBalance.text = coin.ToString();

        GameManager.depositAmount = jsonvale["result_push"][0]["deposite_amount"].ToString();
        totalDeposit.text = GameManager.depositAmount;

        GameManager.withdraw = jsonvale["result_push"][0]["withdraw_amount"].ToString();
        totalWithDrawal.text = GameManager.withdraw;

        GameManager.gamesWon = jsonvale["result_push"][0]["games_won"].ToString();
        totalgamePlay.text = GameManager.gamesWon;

        GameManager.cashWon = jsonvale["result_push"][0]["cash_won"].ToString();
        totalCashWin.text = GameManager.cashWon;

        GameManager.playerName = jsonvale["result_push"][0]["fullname"].ToString();
        nameP.text = GameManager.playerName;

        playername.text = GameManager.playerName;
        // kyc Verification...!!!!

        newTotalBalance.text = GameManager.Instance.coinsCount.ToString();
        newAddedAmount.text = GameManager.depositAmount;
        newWithdrawableAmount.text = GameManager.withdraw;
        Debug.Log("Profile Plan" + GameManager.Uid);
        GameManager.Instance.nameMy = userName;
        GameManager.Instance.avatarMyIndex = UnityEngine.Random.Range(0, 22);
        GameManager.Instance.avatarMy = GameManager.Instance.playfabManager.staticGameVariables.avatars[GameManager.Instance.avatarMyIndex];
        ExitGames.Client.Photon.Hashtable someCustomPropertiesToSet = new ExitGames.Client.Photon.Hashtable() { { "name", GameManager.Instance.nameMy }, { "avatarId", GameManager.Instance.avatarMyIndex }, { "fullName", GameManager.playerName } };
        PhotonNetwork.player.SetCustomProperties(someCustomPropertiesToSet);
        Debug.Log("playername " + PhotonNetwork.player.CustomProperties["name"]);
        GameManager.Instance.coinsCount = coin;
        LoginPanel.SetActive(false);
        splashCanvas.SetActive(false);
        FindObjectOfType<UIFlowHandler>().loadingPanel.SetActive(false);
        LoadingPage.SetActive(true);
        GameManager.Instance.playfabManager.splashCanvas.SetActive(false);
        // GameManager.Instance.playfabManager.Loading.SetActive(false);
        if (SceneManager.GetActiveScene().name != "MenuScene")
            SceneManager.LoadScene("MenuScene");


        FindObjectOfType<UIFlowHandler>().loadingPanel.SetActive(false);
        GameManager.adharcardFront = jsonvale["result_push"][0]["aadhar_first"].ToString();
        print(jsonvale["result_push"][0]["aadhar_first"].ToString() + "Value1");
        if (jsonvale["result_push"][0]["aadhar_first"].ToString() != "0")
        {
            adharFront.gameObject.SetActive(true);
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(GameManager.adharcardFront);
            yield return unityWebRequest.SendWebRequest();
            byte[] bytes = unityWebRequest.downloadHandler.data;
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            Sprite adharF = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            adharFront.texture = adharF.texture;
        }
        else
        {
            adharFront.gameObject.SetActive(false);
        }
        GameManager.adharcardBack = jsonvale["result_push"][0]["aadhar_second"].ToString();
        if (jsonvale["result_push"][0]["aadhar_second"].ToString() != "0")
        {
            adharBack.gameObject.SetActive(true);
            UnityWebRequest unityWebRequest1 = UnityWebRequest.Get(GameManager.adharcardBack);
            yield return unityWebRequest1.SendWebRequest();
            byte[] bytes1 = unityWebRequest1.downloadHandler.data;
            Texture2D tex1 = new Texture2D(2, 2);
            tex1.LoadImage(bytes1);
            Sprite adharB = Sprite.Create(tex1, new Rect(0.0f, 0.0f, tex1.width, tex1.height), new Vector2(0.5f, 0.5f), 100.0f);
            pancard.texture = adharB.texture;
        }
        else
        {
            adharBack.gameObject.SetActive(false);
        }

        GameManager.pancard = jsonvale["result_push"][0]["pan_pic"].ToString();
        if (jsonvale["result_push"][0]["pan_pic"].ToString() != "0")
        {
            pancard.gameObject.SetActive(true);
            UnityWebRequest unityWebRequest2 = UnityWebRequest.Get(GameManager.pancard);
            yield return unityWebRequest2.SendWebRequest();
            byte[] bytes2 = unityWebRequest2.downloadHandler.data;
            Texture2D tex2 = new Texture2D(2, 2);
            tex2.LoadImage(bytes2);
            Sprite panC = Sprite.Create(tex2, new Rect(0.0f, 0.0f, tex2.width, tex2.height), new Vector2(0.5f, 0.5f), 100.0f);
            adharBack.texture = panC.texture;
        }
        else
        {
            pancard.gameObject.SetActive(false);
        }

        playerImageUrl = jsonvale["result_push"][0]["profile_pic"].ToString();
        Debug.LogWarning("Checking pic");
        Debug.LogWarning("Player img urllll: " + playerImageUrl);
        if (jsonvale["result_push"][0]["profile_pic"].ToString() != "0" && jsonvale["result_push"][0]["profile_pic"].ToString() != "")
        {
            playerImage.gameObject.SetActive(true);
            playerImageUrl = jsonvale["result_push"][0]["profile_pic"].ToString();
            UnityWebRequest unityWebRequest3 = UnityWebRequest.Get(playerImageUrl);
            yield return unityWebRequest3.SendWebRequest();
            byte[] bytes3 = unityWebRequest3.downloadHandler.data;
            Texture2D tex3 = new Texture2D(2, 2);
            tex3.LoadImage(bytes3);
            Sprite playerimage = Sprite.Create(tex3, new Rect(0.0f, 0.0f, tex3.width, tex3.height), new Vector2(0.5f, 0.5f), 100.0f);
            GameManager.profileImge = playerimage;
            playerImage.texture = playerimage.texture;
            playerImage3.texture = playerimage.texture;
        }
        else
        {
            GameManager.profileImge = defaultPP;
            playerImage.texture = defaultPP.texture;
            playerImage3.texture = defaultPP.texture;
            Debug.LogError("NO PROFILE PIC");
            //playerImage.gameObject.SetActive(false);
        }
        GameManager.playerName = jsonvale["result_push"][0]["fullname"].ToString();
        nameP.text = GameManager.playerName;
        //  Debug.Log("logged in");
        //UpdatePlayerStats(2, 0);
        //BotDifficulty();
        //BotMatch();
    }

    public void OpenWebview()
    {
        addMoneyView.SetActive(true);
    }
    public Text uu;
    public void CloseMoneyView()
    {
        addMoneyView.SetActive(false);
        var v = FindObjectsOfType<SampleWebView>();
        if (v != null)
        {
            foreach (var item in v)
            {
                item.gameObject.SetActive(false);
            }

        }
        addCoinsNofitications.SetActive(true);
        var g = GameObject.FindObjectOfType<WebViewObject>();
        if (g != null)
        {
            Destroy(g);
        }
        Debug.Log(GameManager.Instance.playfabManager.closeUrl);
        uu.text = GameManager.Instance.playfabManager.closeUrl;
        Debug.Log("Msg" + uu.text);
        if (GameManager.Instance.playfabManager.closeUrl == "https://api1.ludocashwin.com/cashfree/success_payment.php")
        {
            coinsInfo.text = "Coins Added Successfully";
            //OnPlayerProfileData();
            //LoadingPage.SetActive(false);
            //GameManager.Instance.playfabManager.splashCanvas.SetActive(false);
            FindObjectOfType<WalletApiScript>().OnWalletButtonClick();
        }
        else
        {
            coinsInfo.text = "Coins not Added";
        }
    }

    public void GuestLogin()
    {
        LoginPanel.SetActive(false);
        LoadingPage.SetActive(false);
        if (PlayerPrefs.HasKey("coins"))
        {
            GameManager.Instance.nameMy = PlayerPrefs.GetString("guest");
            GameManager.Instance.coinsCount = int.Parse(PlayerPrefs.GetString("coins"));
        }
        else
        {
            string guestName = "Guest" + UnityEngine.Random.Range(1000, 10000);
            PlayerPrefs.SetInt("coins", 500);
            PlayerPrefs.SetString("guest", guestName);
        }

        SceneManager.LoadScene(1);
        CloseSplash();
        GameManager.Instance.nameMy = PlayerPrefs.GetString("guest");
        GameManager.Instance.userID = GameManager.Instance.nameMy;
        GameManager.Instance.coinsCount = int.Parse(PlayerPrefs.GetString("coins"));

        GameManager.Instance.avatarMyIndex = UnityEngine.Random.Range(0, 22);
        GameManager.Instance.avatarMy = GameManager.Instance.playfabManager.staticGameVariables.avatars[GameManager.Instance.avatarMyIndex];
        ExitGames.Client.Photon.Hashtable someCustomPropertiesToSet = new ExitGames.Client.Photon.Hashtable() { { "name", GameManager.Instance.nameMy }, { "avatarId", GameManager.Instance.avatarMyIndex } };
        PhotonNetwork.player.SetCustomProperties(someCustomPropertiesToSet);

    }

    public void CloseSplash()
    {
        GameManager.Instance.playfabManager.splashCanvas.SetActive(true);
        // Invoke("CloseSplash", 0.5f);
        // GameManager.Instance.playfabManager.splashCanvas.SetActive(false);
    }
    public void Betting()
    {
        StartCoroutine(GetBetting());
    }
    float bettingValue;
    public string tablevalue;
    public string startedTableValue;
    public Tables tables;

    public void AddStartedTable(string tableId)
    {
        if (!tables.tables.Contains(tableId))
            tables.tables.Add(tableId);
        PlayerPrefs.SetString("ABC", JsonUtility.ToJson(tables));
    }

    public void GetStartedTable()
    {
        tables = JsonUtility.FromJson<Tables>(PlayerPrefs.GetString("ABC"));
        if (tables == null)
        {
            tables = new Tables();
            tables.tables = new List<string>();
        }
        else
        {
            for (int i = 0; i < tables.tables.Count; i++)
            {
                Debug.LogWarning(tables.tables[i]);
            }
        }
    }

    IEnumerator GetBetting()
    {
        using (WWW www = new WWW(getBettingApi))
        {
            yield return www;
            // Debug.Log(www.text);
            JsonData jsonvale = JsonMapper.ToObject(www.text);
            GameManager.Instance.initMenuScript.twoPlayerBetting.Clear();
            GameManager.Instance.initMenuScript.fourPlayerBetting.Clear();
            for (int i = 0; i < jsonvale["result_push"].Count; i++)
            {
                tablevalue = jsonvale["result_push"][i]["table_id"].ToString();

                string playerType = jsonvale["result_push"][i]["betting type"].ToString();
                if (playerType == "2")
                {
                    bettingValue = float.Parse(jsonvale["result_push"][i]["betting value "].ToString());
                    float winningAmount = float.Parse(jsonvale["result_push"][i]["winning amount"].ToString());
                    Betting b = new Betting(BettingType.TwoPlayer, bettingValue, winningAmount);
                    GameManager.Instance.initMenuScript.twoPlayerBetting.Add(b);
                    ReferenceManager.refMngr.SetBets(tablevalue, bettingValue, winningAmount);
                    if (i == 0)
                    {

                        GameManager.Instance.currentBettingIndex = 0;
                        GameManager.Instance.currentBetAmount = bettingValue;
                        GameManager.Instance.currentWinningAmount = winningAmount;
                        GameManager.Instance.currentBetting = b;
                        GameManager.Instance.payoutCoins = GameManager.Instance.currentBetAmount;
                    }
                    //  Debug.Log("bet amount "+GameManager.Instance.currentBetAmount);
                }
                if (playerType == "4")
                {
                    float bettingValue = float.Parse(jsonvale["result_push"][i]["betting value "].ToString());
                    float winningAmount = float.Parse(jsonvale["result_push"][i]["winning amount"].ToString());
                    //if (bettingValue == 50)
                    //{
                    //    bettingValue = 0;
                    //}
                    Betting b = new Betting(BettingType.FourPlayer, bettingValue, winningAmount);
                    GameManager.Instance.initMenuScript.fourPlayerBetting.Add(b);
                    tablevalue = jsonvale["result_push"][i]["table_id"].ToString();
                    ReferenceManager.refMngr.SetBets(tablevalue, bettingValue, winningAmount);
                    //  Debug.Log("TableNum4Player" + tablevalue);
                    if (i == 0)
                    {

                        GameManager.Instance.currentBettingIndex = 0;
                        GameManager.Instance.currentBetAmount = bettingValue;
                        GameManager.Instance.currentWinningAmount = winningAmount;
                        GameManager.Instance.currentBetting = b;
                        GameManager.Instance.payoutCoins = GameManager.Instance.currentBetAmount;
                    }
                    //   Debug.Log("bet amount " + GameManager.Instance.currentBetAmount);
                }
            }
        }
    }
    // Update is called once per frame

    public void BotDifficulty()
    {
        StartCoroutine(GetBotDifficulty());
    }

    public void BotMatch()
    {
        StartCoroutine(GetBotMatch());
    }
    IEnumerator GetBotMatch()
    {
        using (WWW www = new WWW(getBotAvailiblity))
        {
            yield return www;
            JsonData jsonvale = JsonMapper.ToObject(www.text);
            string isOn = jsonvale["result_push"][0]["botswinning"].ToString();
            Debug.Log("is on" + isOn);
            if (isOn == "On")
            {
                GameManager.Instance.isMultiplayerBot = true;
            }
            else
            {
                GameManager.Instance.isMultiplayerBot = false;
            }

            //GameManager.Instance.isMultiplayerBot = false;
            Debug.Log(www.text);
        }
    }

    IEnumerator GetBotDifficulty()
    {
        using (WWW www = new WWW(getBotDifficulty))
        {
            yield return www;
            Debug.Log(www.text);
            JsonData jsonvale = JsonMapper.ToObject(www.text);
            string isHard = jsonvale["result_push"][0]["difficulty"].ToString();
            Debug.Log("is hard" + isHard);
            if (isHard == "Easy")
            {
                GameManager.Instance.isHard = false;
            }
            else
            {
                GameManager.Instance.isHard = true;
            }
        }
    }
    public void DeductCoins(float amount)
    {
        Debug.Log("Deduct coins" + amount + " " + GameManager.Instance.coinsCount);
        if (!GameManager.Instance.userID.Contains("Guest"))
        {
            StartCoroutine(DecreaseCoins(amount));
        }
        else
        {
            GameManager.Instance.coinsCount -= amount;
            PlayerPrefs.SetString("coins", GameManager.Instance.coinsCount.ToString());
        }
    }

    public void DeductpCoins(int amount)
    {
        Debug.Log("Deduct Private Coins" + amount + " " + GameManager.Instance.coinsCount);
        if (!GameManager.Instance.userID.Contains("Guest"))
        {
            StartCoroutine(DecreasePrivateTableCoins(amount));
        }
        else
        {
            GameManager.Instance.coinsCount -= amount;
            PlayerPrefs.SetString("coins", GameManager.Instance.coinsCount.ToString());
        }
    }
    IEnumerator DecreaseCoins(float amount)
    {
        url = GameManager.apiBase1 + "deduction";
        Debug.Log(url);
        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Uid);
        form.AddField("amount", amount.ToString());
        form.AddField("table_id", tablevalue);
        Debug.Log("ValueTable" + tablevalue);
        WWW www = new WWW(url, form);
        yield return www;
        Debug.Log(www.text);
        if (www.error == null)
        {
            //Debug.Log(www.error);
            JsonData jsonvale = JsonMapper.ToObject(www.text);
            status = jsonvale["result_push"][0]["status"].ToString();
            string roomId = jsonvale["result_push"][0]["game_room_id"].ToString();
            string first_Player_Name = "", second_Player_Name = "", third_Player_Name = "", fourth_Player_Name = "";
            if (jsonvale["result_push"][0]["first_player"].ToString() != null)
            {
                first_Player_Name = jsonvale["result_push"][0]["first_player"].ToString();
            }
            if (jsonvale["result_push"][0]["second_player"].ToString() != null)
            {
                second_Player_Name = jsonvale["result_push"][0]["second_player"].ToString();
            }
            if (jsonvale["result_push"][0]["third_player"].ToString() != null)
            {
                third_Player_Name = jsonvale["result_push"][0]["third_player"].ToString();
            }
            if (jsonvale["result_push"][0]["fourth_player"].ToString() != null)
            {
                fourth_Player_Name = jsonvale["result_push"][0]["fourth_player"].ToString();
            }
            // Debug.Log(status);
            if (status == "True")
            {
                GameManager.Instance.coinsCount -= amount;
                if (isClickedPubButton)
                {
                    isClickedPubButton = false;
                    clickedBet.myJoiningButton.transform.GetChild(0).GetComponent<Text>().text = "Joined";
                    clickedBet.isJoined = true;
                    clickedBet.myRoomId = roomId;
                    clickedBet.SetThisTableNotification();
                    ActiveGamInfo temp = new ActiveGamInfo();
                    temp.game_id = tablevalue;
                    temp.game_room_id = roomId;
                    if (first_Player_Name != null)
                        temp.first_Player_Name = first_Player_Name;
                    if (second_Player_Name != null)
                        temp.second_Player_Name = second_Player_Name;
                    if (third_Player_Name != null)
                        temp.third_Player_Name = third_Player_Name;
                    if (fourth_Player_Name != null)
                        temp.first_Player_Name = fourth_Player_Name;
                    GameManager.activeGameInfo.Add(temp);
                    ReferenceManager.refMngr.loadingPanel.SetActive(false);
                    //ActiveGamInfo
                }
            }
            else
            {
                ReferenceManager.refMngr.loadingPanel.SetActive(false);
                ReferenceManager.refMngr.ShowError(status, "Error");
            }
        }
        else
        {
            ReferenceManager.refMngr.loadingPanel.SetActive(false);
            ReferenceManager.refMngr.ShowError(w.error, "Error");
        }
    }

    public BetScript clickedBet;
    public bool isClickedPubButton = false;

    IEnumerator DecreasePrivateTableCoins(int amount)
    {
        url = GameManager.apiBase1 + "private-deduction";
        Debug.Log(url);
        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Uid);
        form.AddField("amount", amount);
        form.AddField("table_id", tablevalue);
        Debug.Log("ValueTable" + tablevalue);
        WWW www = new WWW(url, form);
        yield return www;
        Debug.Log(www.text);
        //Debug.Log(www.error);
        JsonData jsonvale = JsonMapper.ToObject(www.text);
        status = jsonvale["result_push"][0]["status"].ToString();
        // Debug.Log(status);
        if (status == "True")
        {
            GameManager.Instance.coinsCount -= amount;

        }
    }
    public void AddCoins(float amount)
    {
        Debug.Log("Increase coins" + amount);
        if (!GameManager.Instance.userID.Contains("Guest"))
        {
            StartCoroutine(IncreaseCoins(amount));

        }
        else
        {
            GameManager.Instance.coinsCount += amount;
            PlayerPrefs.SetString("coins", GameManager.Instance.coinsCount.ToString());
        }
    }

    IEnumerator IncreaseCoins(float amount)
    {
        Debug.Log("Increase Coins");
        url = GameManager.apiBase1 + "winning";

        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Uid);
        form.AddField("amount", amount.ToString());
        form.AddField("table_id", tablevalue);
        form.AddField("key", GameTableConfiguration.Instance.Key);
        Debug.Log("ValueTable" + tablevalue);
        WWW www = new WWW(url, form);
        yield return www;
        //        Debug.Log(www.text);
        JsonData jsonvale = JsonMapper.ToObject(www.text);
        status = jsonvale["result_push"][0]["status"].ToString();
        if (status == "True")
        {
            GameManager.Instance.coinsCount += amount;
            FindObjectOfType<InitMenuScript>().coinsText.text = GameManager.Instance.coinsCount.ToString();
            Debug.Log("Add coins" + amount + " " + GameManager.Instance.coinsCount);
        }
    }
    public void OnAmountValueChanged()
    {
        Debug.Log(_amountWithdraw.text + string.IsNullOrWhiteSpace(_amountWithdraw.text));
        if (!string.IsNullOrEmpty(_amountWithdraw.text))
        {
            int amount = int.Parse(_amountWithdraw.text.ToString());
            Debug.Log(amount);
            Debug.Log(GameManager.Instance.coinsCount);

            if (GameManager.Instance.winAmount <= 0)
            {
                for (int i = 0; i <= WithdrawBtn.Length; i++)
                {
                    WithdrawBtn[i].interactable = false;
                }
                MinimumObject.SetActive(true);
                MinimumText.text = "#Can only redeem earned coins";
                // ServiceObject.SetActive(false);
            }
            else if (amount < GameManager.Instance.playfabManager.minimumWithdrawlAmount)
            {
                for (int i = 0; i <= WithdrawBtn.Length; i++)
                {
                    WithdrawBtn[i].interactable = false;
                }
                MinimumObject.SetActive(true);
                MinimumText.text = "#Minimum Withdrawl Amount is " + GameManager.Instance.playfabManager.minimumWithdrawlAmount;
                // ServiceObject.SetActive(false);
            }
            else if (amount > GameManager.Instance.winAmount && amount >= GameManager.Instance.playfabManager.minimumWithdrawlAmount)
            {
                for (int i = 0; i <= WithdrawBtn.Length; i++)
                {
                    WithdrawBtn[i].interactable = false;
                }
                MinimumObject.SetActive(true);
                MinimumText.text = "#Amount should be less or equal to earned coins";
                // ServiceObject.SetActive(false);
            }
            else
            {
                for (int i = 0; i <= WithdrawBtn.Length; i++)
                {
                    WithdrawBtn[i].interactable = true;
                }
                MinimumObject.SetActive(false);
                // ServiceObject.SetActive(true);
                MinimumText.text = "";
                /* serviceTax = ((int)(amount * 0.05f));
                 amountToWithdraw = (amount - (int)(amount * 0.05f));
                 serviceChargeTxt.text = "5%";
                 redeemAmountTxt.text = amountToWithdraw.ToString();*/
            }
        }
        else
        {
            for (int i = 0; i <= WithdrawBtn.Length; i++)
            {
                WithdrawBtn[i].interactable = false;
            }
            MinimumObject.SetActive(false);
            //  ServiceObject.SetActive(false);
        }
        /*
                    if (amount <= GameManager.Instance.winAmount && amount >= GameManager.Instance.playfabManager.minimumWithdrawlAmount)
                    {
                        WithdrawBtn.interactable = true;
                        MinimumObject.SetActive(false);
                        ServiceObject.SetActive(true);
                        MinimumText.text = "";
                        serviceTax = ((int)(amount * 0.05f));
                        amountToWithdraw = (amount - (int)(amount * 0.05f));
                        serviceChargeTxt.text = "5%" ;
                        redeemAmountTxt.text = amountToWithdraw.ToString();
                    }
                    else if (amount > GameManager.Instance.winAmount && amount >= GameManager.Instance.playfabManager.minimumWithdrawlAmount)
                    {
                        WithdrawBtn.interactable = false;
                        MinimumObject.SetActive(true);
                        MinimumText.text = "#Can only redeem earned coins";
                        ServiceObject.SetActive(false);
                    }
                    else if (amount < GameManager.Instance.playfabManager.minimumWithdrawlAmount && GameManager.Instance.winAmount > amount)
                    {
                        WithdrawBtn.interactable = false;
                        MinimumObject.SetActive(true);
                        MinimumText.text = "#Minimum Withdrawl Amount is "+ GameManager.Instance.playfabManager.minimumWithdrawlAmount;
                        ServiceObject.SetActive(false);
                        Debug.Log("less 500");

                    }
                    else
                    {
                        WithdrawBtn.interactable = false;
                        MinimumObject.SetActive(true);
                        MinimumText.text = "#Can only redeem earned coins";
                        ServiceObject.SetActive(false);
                    }
                }*/

    }
    public void OnWithDrawBtnClick()
    {
        if (!GameManager.Instance.userID.Contains("Guest"))
        {
            //url = "http://ludo-cash.knickglobal.co.in/public/api/withdrawal";
            url = "http://ludolympic.knickglobal.com/api/withdrawal";

            if (string.IsNullOrEmpty(_amountWithdraw.text))
            {
                withdrwal.text = "Amount Can't be Blank";
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
                return;
            }
            else
            {
                int amount = int.Parse(_amountWithdraw.text.ToString());
                Debug.Log(amount);
                Debug.Log(GameManager.Instance.coinsCount);
                if (amount > GameManager.Instance.coinsCount)
                {
                    withdrwal.text = "Amount Can't be greater than Coins";
                    StopCoroutine(ErrorClose());
                    StartCoroutine(ErrorClose());
                    return;
                }
            }

            Debug.Log(url);

            WWWForm form = new WWWForm();
            form.AddField("user_id", GameManager.Uid);
            form.AddField("amount", _amountWithdraw.text);
            WWW w = new WWW(url, form);
            StartCoroutine(Withdraw(w));
        }
    }
    IEnumerator Withdraw(WWW _w)
    {
        yield return _w;
        print("responce=" + _w.text);
        if (_w.error == null)
        {
            string msg = _w.text;
            msg = msg.Replace("{", "");
            msg = msg.Replace("}", "");
            msg = msg.Replace("[", "");
            msg = msg.Replace("]", "");
            msg = msg.Replace(@"""", string.Empty);
            results = GetDataValue(msg, "message:");
            status = GetDataValue(msg, "status:");
            // Debug.Log(results);
            // Debug.Log(status);
            if (results == "Transaction Successfully" || status == "True")
            {
                withdrawPanel.SetActive(false);
                notificationPanel.SetActive(true);
                // loginPanel.SetActive(false);
                _amountWithdraw.text = "";
            }
            else
            {
                errorMsg.text = results;
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
            }
        }
        else
        {
            Debug.Log("error");
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
        }
    }
    public void PaymentHistory()
    {
        paymentHistoryPanel.SetActive(true);
        StartCoroutine(PaymentList());
    }

    IEnumerator PaymentList()
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        string url = "https://onlystore.in/api/transaction_history.php?my_referral_code=" + GameManager.Instance.userID;
        // string url = "https://onlystore.in/ludomoney/api/show-transaction.php?userid=" + "USR101";
        // Debug.Log(auth);
        WWW www = new WWW(url);

        yield return www;
        Debug.Log(www.text);
        JsonData jsonvale = JsonMapper.ToObject(www.text);
        if (jsonvale["result_push"][0]["transaction_id"] != null)
        {
            for (int i = 0; i < jsonvale["result_push"].Count; i++)
            {
                PaymentScript payment = Instantiate(paymentData, contentPanel).GetComponent<PaymentScript>();
                string transactionId = jsonvale["result_push"][i]["transaction_id"].ToString();
                payment.transactionId.text = transactionId;
                string transactionby = jsonvale["result_push"][i]["transaction_by"].ToString();
                payment.transactionby.text = transactionby;
                string transactionAmount = jsonvale["result_push"][i]["transaction_amount"].ToString();
                payment.transactionAmount.text = transactionAmount;
                status = jsonvale["result_push"][i]["status"].ToString();
                payment.status.text = status;
                string date = jsonvale["result_push"][i]["date"].ToString();
                payment.date.text = date;
                string game = jsonvale["result_push"][i]["game"].ToString();
                // payment.game.text = game;

            }
        }
    }
    public void WithdrawPayment()
    {
        withdrawPanel.SetActive(true);
    }
    public void OnRequestPlans()
    {
        if (!GameManager.Instance.userID.Contains("Guest"))
        {

            url = "https://onlystore.in/api/client_plan.php";

            WWW w = new WWW(url);
            StartCoroutine(RequestPlan(w));
        }
    }
    IEnumerator RequestPlan(WWW www)
    {
        yield return www;

        Debug.Log(www.text);
        JsonData jsonvale = JsonMapper.ToObject(www.text);
        GameManager.Instance.shopCost.Clear();
        GameManager.Instance.shopCoins.Clear();
        for (int i = 0; i < jsonvale["result"].Count; i++)
        {
            string amountString = jsonvale["result"][i]["amount"].ToString();
            string coinsString = jsonvale["result"][i]["coins"].ToString();
            GameManager.Instance.shopCost.Add(int.Parse(amountString));
            GameManager.Instance.shopCoins.Add(int.Parse(coinsString));
        }
    }

    public void ResetPassword()
    {
        string url = GameManager.apiBase1 + "forgot-password";
        Debug.Log(url);
        if (string.IsNullOrEmpty(_forgotPasswordEmail.text))
        {
            ferrorText.text = "Email Id Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        WWWForm form = new WWWForm();
        form.AddField("email", _forgotPasswordEmail.text);
        WWW www = new WWW(url, form);
        StartCoroutine(ForgetPassword(www));
        loadinPanel.SetActive(true);
    }

    IEnumerator ForgetPassword(WWW _w)
    {

        yield return _w;
        print("response=" + _w.text);

        if (_w.error == null)
        {
            JsonData jsonvale = JsonMapper.ToObject(_w.text);
            results = jsonvale["result_push"].ToString();
            status = jsonvale["status"].ToString();

            Debug.Log("get player info" + results);
            if (results == "OTP containing mail sent." || status == "true")
            {
                ferrorText.text = "OTP sent to email id";
                loadinPanel.SetActive(false);
                forgetPasswordPanel.SetActive(false);
                changePasswordPanel.SetActive(true);
                StopCoroutine(ErrorClose());
                StartCoroutine(ErrorClose());
                _forgotPasswordEmail.text = "";
            }
            else
            {
                errorMsg.text = results;
            }
        }
        else
        {
            Debug.Log("error");
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());

        }
    }

    public void OnChangePasswordBtnClick()
    {

        if (string.IsNullOrEmpty(changeForgetPasswordEmail.text))
        {
            cerrorText.text = "Email Id Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        if (string.IsNullOrEmpty(otp.text))
        {
            cerrorText.text = "OTP Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        if (string.IsNullOrEmpty(psswrd.text))
        {
            cerrorText.text = "Password Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        if (string.IsNullOrEmpty(cpasswrd.text))
        {
            cerrorText.text = "Confirm Password Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        string curl = GameManager.apiBase1 + "reset-password";
        Debug.Log(curl);
        WWWForm form = new WWWForm();
        form.AddField("email", changeForgetPasswordEmail.text);
        form.AddField("otp", otp.text);
        form.AddField("password", psswrd.text);
        form.AddField("confirm_password", cpasswrd.text);
        WWW www = new WWW(curl, form);
        StartCoroutine(changePassword(www));
        loadinPanel.SetActive(true);
    }
    IEnumerator changePassword(WWW w)
    {
        yield return w;
        print("response=" + w.text);

        if (w.error == null)
        {
            string msg = w.text;
            msg = msg.Replace("{", "");
            msg = msg.Replace("}", "");
            msg = msg.Replace("[", "");
            msg = msg.Replace("]", "");
            msg = msg.Replace(@"""", string.Empty);

            results = GetDataValue(msg, "result_push:");
            results = GetDataValue(msg, "message:");
            status = GetDataValue(msg, "status:");
            Debug.Log("get player info" + results);
            if (status == "true")
            {
                cerrorText.text = "Password has been changed.";
                loadinPanel.SetActive(false);
                changePasswordPanel.SetActive(false);
                LoginPanel.SetActive(true);
                StartCoroutine(ErrorClose());
                StopCoroutine(ErrorClose());
                changeForgetPasswordEmail.text = "";
                otp.text = "";
                cpasswrd.text = "";
                psswrd.text = "";
            }
            if (status == "false")
            {
                loadinPanel.SetActive(false);
                changeForgetPasswordEmail.text = "";
                otp.text = "";
                cpasswrd.text = "";
                psswrd.text = "";
            }
            else
            {
                cerrorText.text = results;
            }
        }
        else
        {
            Debug.Log("error");
            loadinPanel.SetActive(false);
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
        }
    }
    public void RegisterDone()
    {
        LoginPanel.SetActive(true);
        successPanel.SetActive(false);
        SignupPanel.SetActive(false);
        GameManager.Instance.playfabManager.splashCanvas.SetActive(false);
    }
    public void UpdatePlayerStats(int mode, int gameStatus)
    {
        //  playerNameTxt.text = GameManager.Instance.nameMy;
        //  playerStatsPanel.SetActive(true);

        if (!GameManager.Instance.userID.Contains("Guest"))
        {
            string url = "https://onlystore.in/api/player_stats.php?my_referral_code=" + GameManager.Instance.userID + "&game_mode=" + mode + "&game_status=" + gameStatus;
            Debug.Log(url);
            WWW www = new WWW(url);
            StartCoroutine(UpdateStats(www));
        }
    }

    IEnumerator UpdateStats(WWW _w)
    {

        yield return _w;
        print("response=" + _w.text);
        if (_w.error == null)
        {
            JsonData jsonvale = JsonMapper.ToObject(_w.text);
            status = jsonvale["result_push"][0]["status"].ToString();

            Debug.Log("get player info" + status);
            if (status == "True")
            {
                Debug.Log("Stats Updated");
            }
            else
            {
                errorMsg.text = results;
            }
        }
        else
        {
            Debug.Log("error");
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());

        }
    }
    public void PlayerStats()
    {
        Debug.Log(GameManager.Instance.userID);
        Debug.Log(GameManager.Instance.userID.Contains("Guest"));
        if (!GameManager.Instance.userID.Contains("Guest"))
        {

            playerNameTxt.text = GameManager.Instance.nameMy;
            playerStatsPanel.SetActive(true);
            string url = "http://onlystore.in/api/client_details.php?my_referral_code=" + GameManager.Instance.userID;
            Debug.Log(url);
            WWW www = new WWW(url);
            StartCoroutine(GetPlayerStats(www));
        }
        else
        {
            GameManager.Instance.playfabManager.apiManager.LoginPanel.SetActive(true);
        }
        //  GetPlayerStats(www);

    }

    IEnumerator GetPlayerStats(WWW _w)
    {

        yield return _w;
        print("response=" + _w.text);
        if (_w.error == null)
        {

            string msg = _w.text;
            msg = msg.Replace("{", "");
            msg = msg.Replace("}", "");
            msg = msg.Replace("]", "");
            msg = msg.Replace(@"""", string.Empty);
            status = GetDataValue(msg, "status:");
            Debug.Log("get player info" + status);
            if (status == "True")
            {
                Debug.Log("get player info");
                string twoWin = GetDataValue(msg, "two_player_count:");
                string fourWin = GetDataValue(msg, "four_player_count:");
                string gamesWin = GetDataValue(msg, "game_played_count:");

                gamesWonTxt.text = gamesWin;
                twoWonTxt.text = twoWin;
                fourWonTxt.text = fourWin;

            }
            else
            {
                errorMsg.text = results;
            }
        }
        else
        {
            Debug.Log("error");
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());

        }
    }

    public void LogOut()
    {
        GoogleSignInDemo google = FindObjectOfType<GoogleSignInDemo>();
        FacebookScriptDav facebook = FindObjectOfType<FacebookScriptDav>();
        if (!google.IsCurrentUserNull())
        {
            Debug.LogError("APi manager google User not null");
            google.SignOutFromGooglePublic();
        }
        else if (facebook.IsFbLoggedIn())
        {
            Debug.LogError("APi manager facebook User not null");
            facebook.SignOutFromFacebookPublic();
        }
        else
        {
            Debug.LogError("APi manager user null");
            GameManager.Instance.userID = "";
            GameManager.Instance.nameMy = "";
            GameManager.Instance.avatarMy = null;
            GameManager.Instance.avatarIndex = null;
            newGameManager.newLoginScreen.SetActive(true);
            LoginPanel.SetActive(true);
            newGameManager.EnterYourPinScreen.SetActive(false);
            newGameManager.mobileVerificationScreen.SetActive(false);
            PlayerPrefs.DeleteAll();
            GameManager.activeGameInfo.Clear();
            playerStatsPanel.SetActive(false);
            _name.text = "";
            _password.text = "";
            isFirstTimeLogin = false;
            newOptionsPanel.SetActive(false);

            CloseSplash();
            // SceneManager.LoadScene(0);
            Debug.Log("Log out");
        }
    }
    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains(","))
            value = value.Remove(value.IndexOf(","));

        return value;
    }
    public static bool validateEmail(string email)
    {
        if (email != null)
            return Regex.IsMatch(email, MatchEmailPattern);
        else
            return false;
    }
    public static bool IsPhoneNumber(string number)
    {
        return Regex.Match(number, @"^(\+[0-9]{9})$").Success;
    }
    IEnumerator ErrorClose()
    {
        yield return new WaitForSeconds(2f);
        errorMsg.text = "";
        loginerror.text = "";
        ferrorText.text = "";
        cerrorText.text = "";
        withdrwal.text = "";
        newBankDetailsError.text = "";
        yield return null;
    }
    public void OnBtn()
    {
        LoginPanel.SetActive(false);
        notificationPanel.SetActive(false);
        SceneManager.LoadScene("MenuScene");
        StartCoroutine(waut());
    }

    IEnumerator waut()
    {
        string url = GameManager.apiBase1 + "client_details/my_referral_code=" + GameManager.Instance.userID;
        Debug.Log(url);
        WWW www = new WWW(url);

        yield return www;
        JsonData jsonvale = JsonMapper.ToObject(www.text);

        try
        {
            GameManager.appVersionFromApi = jsonvale["result_push"][0]["version"].ToString();
        }
        catch
        {
            Debug.LogError("Catch from version");
        }
        GameManager.adharcardFront = jsonvale["result_push"][0]["aadhar_first"].ToString();
        print(jsonvale["result_push"][0]["aadhar_first"].ToString() + "Value1");
        if (jsonvale["result_push"][0]["aadhar_first"].ToString() != "0")
        {
            adharFront.gameObject.SetActive(true);
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(GameManager.adharcardFront);
            yield return unityWebRequest.SendWebRequest();
            byte[] bytes = unityWebRequest.downloadHandler.data;
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            Sprite adharF = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            adharFront.texture = adharF.texture;
        }
        else
        {
            adharFront.gameObject.SetActive(false);
        }
        GameManager.adharcardBack = jsonvale["result_push"][0]["aadhar_second"].ToString();
        if (jsonvale["result_push"][0]["aadhar_second"].ToString() != "0")
        {
            adharBack.gameObject.SetActive(true);
            UnityWebRequest unityWebRequest1 = UnityWebRequest.Get(GameManager.adharcardBack);
            yield return unityWebRequest1.SendWebRequest();
            byte[] bytes1 = unityWebRequest1.downloadHandler.data;
            Texture2D tex1 = new Texture2D(2, 2);
            tex1.LoadImage(bytes1);
            Sprite adharB = Sprite.Create(tex1, new Rect(0.0f, 0.0f, tex1.width, tex1.height), new Vector2(0.5f, 0.5f), 100.0f);
            pancard.texture = adharB.texture;
        }
        else
        {
            adharBack.gameObject.SetActive(false);
        }

        GameManager.pancard = jsonvale["result_push"][0]["pan_pic"].ToString();
        if (jsonvale["result_push"][0]["pan_pic"].ToString() != "0")
        {
            pancard.gameObject.SetActive(true);
            UnityWebRequest unityWebRequest2 = UnityWebRequest.Get(GameManager.pancard);
            yield return unityWebRequest2.SendWebRequest();
            byte[] bytes2 = unityWebRequest2.downloadHandler.data;
            Texture2D tex2 = new Texture2D(2, 2);
            tex2.LoadImage(bytes2);
            Sprite panC = Sprite.Create(tex2, new Rect(0.0f, 0.0f, tex2.width, tex2.height), new Vector2(0.5f, 0.5f), 100.0f);
            adharBack.texture = panC.texture;
        }
        else
        {
            pancard.gameObject.SetActive(false);
        }
        playerImageUrl = jsonvale["result_push"][0]["profile_pic"].ToString();
        if (jsonvale["result_push"][0]["profile_pic"].ToString() != "0")
        {
            playerImage.gameObject.SetActive(true);
            playerImageUrl = jsonvale["result_push"][0]["profile_pic"].ToString();
            UnityWebRequest unityWebRequest3 = UnityWebRequest.Get(playerImageUrl);
            yield return unityWebRequest3.SendWebRequest();
            byte[] bytes3 = unityWebRequest3.downloadHandler.data;
            Texture2D tex3 = new Texture2D(2, 2);
            tex3.LoadImage(bytes3);
            Sprite playerimage = Sprite.Create(tex3, new Rect(0.0f, 0.0f, tex3.width, tex3.height), new Vector2(0.5f, 0.5f), 100.0f);
            playerImage.texture = playerimage.texture;
            playerImage3.texture = playerimage.texture;
            GameManager.profileImge = playerimage;
        }
        else
        {
            playerImage.gameObject.SetActive(false);
        }
        GameManager.playerName = jsonvale["result_push"][0]["fullname"].ToString();
        nameP.text = GameManager.playerName;
    }
    public void OnMyProfile()
    {
        userDashboard.SetActive(true);
    }

    #region Ludo Olympic Script

    [Header("New Ludo Olympic Variables")]
    public TMP_InputField referralCodeInputField;
    public TextMeshProUGUI newCoinsText;
    public TextMeshProUGUI newNameinProfilePageText;
    public TMP_InputField newbankName;
    public TMP_InputField newAccountNumber;
    public TMP_InputField newConfirmAccountNumber;
    public TMP_InputField newIFSCCode;
    public TextMeshProUGUI newBankDetailsError;
    public TMP_InputField bankeDetailIP;
    public TextMeshProUGUI bankDetailButtonText;
    public TMP_InputField kycInputField;
    public GameObject musicOnBtn;
    public GameObject musicOffBtn;
    public GameObject effectOnBtn;
    public GameObject effectOffBtn;

    [Header("New Kyc Variables")]
    public TMP_Dropdown kycDocumentType;
    public TMP_InputField kycDocumentNumber;
    public TMP_InputField kycFirstName;
    public TMP_InputField kycLastName;
    public TMP_InputField kycDateOfBirth;
    public TMP_Dropdown kycState;
    public TMP_InputField kycDocumentFront;
    public TMP_InputField kycDocumentBack;
    public GameObject KYCAdhaarButton;
    public GameObject KYCPanButton;
    public string KYCStatus;
    public string KYCadhaarStatus;
    public string KYCpanStatus;
    public string KYCpanNumber;
    public string KYCadharNumber;
    public string KYCDob;
    public string KYCState;
    public AudioSource[] soundEffects;


    [SerializeField]
    List<string> statesName = new List<string>() { "Andaman and Nicobar Islands",
        "Arunachal Pradesh", "Bihar", "Chandigarh", "Chhattisgarh", "Daman and Diu",
    "Delhi","Goa", "Gujarat", "Haryana","Himachal Pradesh"
    ,"Jammu and Kashmir", "Jharkhand", "Karnataka","Kerala", "Ladakh", "Lakshadweep",
    "Madhya Pradesh", "Maharashtra", "Manipur", "Meghalaya", "Mizoram",
    "Puducherry", "Punjab", "Rajasthan",
    "Tripura", "Uttar Pradesh", "Uttarakhand", "West Bengal"};

    public void SetStates()
    {
        kycState.options.Clear();
        kycState.AddOptions(statesName);
    }

    public void OpenReferAFriend()
    {
        FindObjectOfType<InitMenuScript>().newReferAFriendPanel.SetActive(true);
    }

    public void SetSoundButton(bool state)
    {
        musicOnBtn.SetActive(state);
        musicOffBtn.SetActive(!state);
    }

    public void SetEffectButton(bool state)
    {
        effectOnBtn.SetActive(state);
        effectOffBtn.SetActive(!state);
    }

    public void ToggleEffect()
    {
        if (PlayerPrefs.GetInt("IsSoundEffect") == 1)
        {
            PlayerPrefs.SetInt("IsSoundEffect", 0);
            SetEffectToMute(true);
            FindObjectOfType<InitMenuScript>().SetEffectToMute(true);
        }
        else
        {
            PlayerPrefs.SetInt("IsSoundEffect", 1);
            SetEffectToMute(false);
            FindObjectOfType<InitMenuScript>().SetEffectToMute(false);
        }
    }

    public void SetEffectToMute(bool state)
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            soundEffects[i].mute = state;
        }
    }

    public void ToggleSound()
    {
        FindObjectOfType<InitMenuScript>().ToggleSound();
    }

    public void OpenMyWallet()
    {
        newMyWallet.SetActive(true);
    }

    public void OpenRedeemPanel()
    {
        FindObjectOfType<InitMenuScript>().redeemPanel.SetActive(true);
    }


    public void OpenProfilePage()
    {
        if (KYCadhaarStatus == "Verified" || KYCpanStatus == "Verified")
        {
            kycInputField.text = KYCStatus;
        }
        else kycInputField.text = "Not Verified";
        newCoinsText.text = GameManager.Instance.coinsCount.ToString();
        newNameinProfilePageText.text = GameManager.playerName;
        FindObjectOfType<RefferalCodeScript>().OnInviteFriendEnabled();
        newProfilePanel.SetActive(true);
    }

    public void RefferalClaim()
    {
        FindObjectOfType<RefferalCodeScript>().OnClaimPrize();
    }

    public void OpenNewBankPanel()
    {
        newbankName.text = GameManager.bankName;
        newIFSCCode.text = GameManager.bankIfscCode;
        newAccountNumber.text = GameManager.accountNumber;
        newConfirmAccountNumber.text = GameManager.accountNumber;
        newBankDetails.SetActive(true);
    }

    public void UpdateBankDetails()
    {
        string url123 = FindObjectOfType<ReedemCoinsScript>().bankDetailUrl;
        if (string.IsNullOrEmpty(newbankName.text))
        {
            newBankDetailsError.text = "Bank Name Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        if (string.IsNullOrEmpty(newAccountNumber.text))
        {
            newBankDetailsError.text = "Bank Acount Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        if (string.IsNullOrEmpty(newIFSCCode.text))
        {
            newBankDetailsError.text = "IFSC Can't be Blank";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        if (newAccountNumber.text != newConfirmAccountNumber.text)
        {
            newBankDetailsError.text = "Account Numbers doesn't match";
            StopCoroutine(ErrorClose());
            StartCoroutine(ErrorClose());
            return;
        }
        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Uid);
        form.AddField("bank_name", newbankName.text);
        form.AddField("ifsc_code", newIFSCCode.text);
        form.AddField("account", newAccountNumber.text);
        WWW w = new WWW(url123, form);
        ReferenceManager.refMngr.loadingPanel.SetActive(true);
        StartCoroutine(bankDetailCheck(w));
    }

    IEnumerator bankDetailCheck(WWW w)
    {
        yield return w;


        print("responce=" + w.text);
        if (w.error == null)
        {
            string msg = w.text;
            msg = msg.Replace("{", "");
            msg = msg.Replace("}", "");
            msg = msg.Replace("[", "");
            msg = msg.Replace("]", "");
            msg = msg.Replace(@"""", string.Empty);
            results = GetDataValue(msg, "message:");
            status = GetDataValue(msg, "status:");
            if (results == "Updated Successfully" || status == "True")
            {
                ReferenceManager.refMngr.ShowError("Successfully Updated", "Message");
                bankeDetailIP.text = "Verified";
                GameManager.bankName = newbankName.text;
                GameManager.bankIfscCode = newIFSCCode.text;
                GameManager.accountNumber = newAccountNumber.text;
                bankDetailButtonText.text = "Edit";
                PlayerPrefs.SetString("Bank", "1");
            }
            else
            {
                newBankDetailsError.text = results;
                StartCoroutine(DavMaster.EnableDisableWithDelay(newBankDetailsError.gameObject, 2, false));
            }
            ReferenceManager.refMngr.loadingPanel.SetActive(false);
        }
        else
        {
            StartCoroutine(DavMaster.EnableDisableWithDelay(newBankDetailsError.gameObject, 2, false));
            ReferenceManager.refMngr.loadingPanel.SetActive(false);
        }
    }
    public void OpenUpdateProfile()
    {
        newUpdateProfilePage.SetActive(true);
    }

    public void OnDocumentTypeChange()
    {
        //If Adhaar Card Selected
        if (kycDocumentType.value == 0)
        {
            kycDocumentBack.transform.parent.gameObject.SetActive(true);
            KYCAdhaarButton.SetActive(true);
            KYCPanButton.SetActive(false);
            kycDocumentNumber.placeholder.gameObject.GetComponent<TextMeshProUGUI>().text = "Adhaar Number";
            if (KYCadhaarStatus == "Verified")
            {
                kycDocumentNumber.text = KYCadharNumber;
            }
            //kycDocumentFront.text = KYCStatus;
            //kycDocumentBack.text = KYCStatus;
            kycDocumentFront.text = "Adhaar Front";
            kycDocumentBack.text = "Adhaar Back";
        }
        //If Pan Card Selected
        else
        {
            kycDocumentBack.transform.parent.gameObject.SetActive(false);
            KYCAdhaarButton.SetActive(false);
            KYCPanButton.SetActive(true);
            kycDocumentNumber.placeholder.gameObject.GetComponent<TextMeshProUGUI>().text = "Pan Number";
            if (KYCpanStatus == "Verified")
            {
                kycDocumentNumber.text = KYCpanNumber;
            }
            kycDocumentFront.text = "Pan Card";
        }
    }

    public void OpenKYCPanel()
    {
        if (KYCadhaarStatus == "Verified" || KYCpanStatus == "Verified")
        {
            kycDateOfBirth.text = KYCDob;
            for (int i = 0; i < kycState.options.Count; i++)
            {
                if (kycState.options[i].text == KYCState)
                {
                    kycState.value = i;
                }
                else kycState.value = 0;
            }
        }
        string[] abc = GameManager.playerName.Split(' ');
        kycFirstName.text = abc[0];
        if (abc.Length > 1)
        {
            kycLastName.text = abc[1];
        }
        kycDocumentType.value = 0;
        if (KYCadhaarStatus == "Verified")
        {
            kycDocumentNumber.text = KYCadharNumber;
        }
        kycDocumentFront.text = "Aadhar Front";
        kycDocumentBack.text = "Aadhar Back";
        kycDocumentNumber.placeholder.gameObject.GetComponent<TextMeshProUGUI>().text = "Adhaar Number";
        newCompleteYourKYCPanel.SetActive(true);
    }

    #endregion
}
[System.Serializable]
public class Tables
{
    public List<string> tables;
}
