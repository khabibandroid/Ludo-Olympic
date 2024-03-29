﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase.Auth;
using Firebase.Extensions;
using System;
 
//Santosh sir: 9872816950

public class NewGameManager : MonoBehaviour
{
    [SerializeField] GameObject splashScreen;
    public Image loadingBarFill;
    [SerializeField] float time;
    public GameObject newLoginScreen;
    public GameObject mobileVerificationScreen;
    public GameObject EnterYourPinScreen;

    [SerializeField] TMP_InputField phoneNumberField;
    [SerializeField]GoogleSignInDemo googleInstance;
    private uint phoneAuthTimeoutMs;
    [SerializeField] TMP_InputField otpField;
    [SerializeField] APIManager apimngr;
    ForceResendingToken tokenn;
    [SerializeField] Button resendButton;

    string verificationId;

    private void Start()
    {
        //Debug.LogError(DateTime.Now);
        phoneAuthTimeoutMs = 1000000000;
    }

    private void FixedUpdate()
    {
        if (splashScreen.activeInHierarchy)
        {
            loadingBarFill.fillAmount += 0.0167f / time;
            if (loadingBarFill.fillAmount >= 0.95f  && loadingBarFill.fillAmount < 0.98f)
            {
                //Debug.LogError(loadingBarFill.fillAmount + ": fill amount");
                //newLoginScreen.SetActive(true);
                splashScreen.SetActive(false);
                //Debug.LogError(DateTime.Now);
            }
        }
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void SendOtpToPhoneNumber()
    {
        resendButton.interactable = true;
        UIFlowHandler.uihandler.loadingPanel.SetActive(true);
        PhoneAuthProvider provider = PhoneAuthProvider.GetInstance(googleInstance.auth);
        
        provider.VerifyPhoneNumber("+91" + phoneNumberField.text, phoneAuthTimeoutMs, null,
          verificationCompleted: (credential) =>
          {
              // Auto-sms-retrieval or instant validation has succeeded (Android only).
              // There is no need to input the verification code.
              // `credential` can be used instead of calling GetCredential().
              UIFlowHandler.uihandler.loadingPanel.SetActive(false);
              Debug.LogError("Completed: " + credential.ToString());
              SignIn(credential);
              
          },
          verificationFailed: (error) =>
          {
              // The verification code was not sent.
              // `error` contains a human readable explanation of the problem.
              UIFlowHandler.uihandler.loadingPanel.SetActive(false);
              UIFlowHandler.uihandler.ShowError(error, "Error");
              Debug.LogError("Error: " + error);
          },
          codeSent: (id, token) =>
          {
              // Verification code was successfully sent via SMS.
              // `id` contains the verification id that will need to passed in with
              // the code from the user when calling GetCredential().
              // `token` can be used if the user requests the code be sent again, to
              // tie the two requests together.
              UIFlowHandler.uihandler.loadingPanel.SetActive(false);
              verificationId = id;
              Debug.LogError("id: " + id);
              tokenn = token;
              UIFlowHandler.uihandler.ShowError("OTP Sent Successfully", "Success");
              Debug.LogError("token: " + token.ToString());

          },
          codeAutoRetrievalTimeOut: (id) =>
          {
              // Called when the auto-sms-retrieval has timed out, based on the given
              // timeout parameter.
              // `id` contains the verification id of the request that timed out.
              UIFlowHandler.uihandler.loadingPanel.SetActive(false);
              verificationId = id;
              Debug.LogError("timeout id: " + id);
          });
    }


    public void ResendOtpToPhoneNumber()
    {
        resendButton.interactable = false;
        UIFlowHandler.uihandler.loadingPanel.SetActive(true);
        PhoneAuthProvider provider = PhoneAuthProvider.GetInstance(googleInstance.auth);

        provider.VerifyPhoneNumber("+91" + phoneNumberField.text, phoneAuthTimeoutMs, tokenn,
          verificationCompleted: (credential) =>
          {
              // Auto-sms-retrieval or instant validation has succeeded (Android only).
              // There is no need to input the verification code.
              // `credential` can be used instead of calling GetCredential().
              UIFlowHandler.uihandler.loadingPanel.SetActive(false);
              Debug.LogError("Completed: " + credential.ToString());
              SignIn(credential);
          },
          verificationFailed: (error) =>
          {
              // The verification code was not sent.
              // `error` contains a human readable explanation of the problem.
              UIFlowHandler.uihandler.loadingPanel.SetActive(false);
              UIFlowHandler.uihandler.ShowError(error, "Error");
              Debug.LogError("Error: " + error);
          },
          codeSent: (id, token) =>
          {
              // Verification code was successfully sent via SMS.
              // `id` contains the verification id that will need to passed in with
              // the code from the user when calling GetCredential().
              // `token` can be used if the user requests the code be sent again, to
              // tie the two requests together.
              UIFlowHandler.uihandler.loadingPanel.SetActive(false);
              verificationId = id;
              UIFlowHandler.uihandler.ShowError("OTP Sent Successfully", "Success");
          },
          codeAutoRetrievalTimeOut: (id) =>
          {
              // Called when the auto-sms-retrieval has timed out, based on the given
              // timeout parameter.
              // `id` contains the verification id of the request that timed out.
              UIFlowHandler.uihandler.loadingPanel.SetActive(false);
              verificationId = id;
              Debug.LogError("timeout id: " + id);
          });
    }

    public void SigninWithOTP()
    {
        string otp;
        //for (int i = 0; i < otpFields.Length; i++)
        //{
        //    otp += otpFields[i].text;
        //}
        otp = otpField.text;
        PhoneAuthProvider provider = PhoneAuthProvider.GetInstance(googleInstance.auth);
        Credential credential = provider.GetCredential(verificationId, otp);
        SignIn(credential);
    }

    public void SignIn(Credential credntialll)
    {
        googleInstance.auth.SignInWithCredentialAsync(credntialll).ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " +
                               task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result;
            apimngr.phoneEmail = newUser.PhoneNumber;
            apimngr.phoneEmail = apimngr.phoneEmail.Replace("+", "");
            Debug.LogWarning("User signed in successfully");
            // This should display the phone number.
            Debug.LogWarning("Phone number: " + newUser.PhoneNumber);
            // The phone number providerID is 'phone'.
            Debug.LogWarning("Phone provider ID: " + newUser.ProviderId);

            apimngr.PhoneMediaSignInStart();
        });
    }

    //public void MoveToNextField(int curFieldIndex)
    //{
    //    if (curFieldIndex + 1 < otpFields.Length)
    //        otpFields[curFieldIndex + 1].ActivateInputField();
    //    else Debug.LogError("Do nothing");
    //}

    public void EditorTestingSK()
    {
        apimngr.isTestingLogin = false;
        //apimngr.phoneEmail = "911234527890";
        apimngr.phoneEmail = "910000000000";
        apimngr.phoneEmail.Replace("+", "");
        apimngr.PhoneMediaSignInStart();
    }

    public void EditorTestingDS()
    {
        apimngr.isTestingLogin = false;
        apimngr.phoneEmail = "919876152916";
        apimngr.phoneEmail.Replace("+", "");
        apimngr.PhoneMediaSignInStart();
    }

    public void EditorSahilTesterLogin()
    {
        apimngr.isTestingLogin = true;
        apimngr.phoneEmail = "pooja";
        //apimngr.phoneEmail = "919805101052";
        apimngr.phoneEmail.Replace("+", "");
        apimngr.PhoneMediaSignInStart();
    }
}
