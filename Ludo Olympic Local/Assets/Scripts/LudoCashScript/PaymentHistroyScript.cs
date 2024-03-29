﻿using System;
using LitJson;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PaymentHistroyScript : MonoBehaviour
{
    [Header("String Attribute")]

    string paymentHistoryScript;

    [Header("WithDraw String Attribute")]

    public string transactionId;
    public string walletType;
    public string amount;
    public string disc;
    public string date;
    public string nameP;

    [Header("WithDraw GameObject Attribute")]

    public GameObject transactionPrefab;
    public GameObject scrollPanel;
    public Transform contentPanel;
    public GameObject transactionPanel;

    [Header("Credit GameObject Attribute")]

    public GameObject credittransactionPrefab;
    public GameObject creditscrollPanel;
    public Transform creditcontentPanel;

    [Header("Debit GameObject Attribute")]

    public GameObject debittransactionPrefab;
    public GameObject debitscrollPanel;
    public Transform debitcontentPanel;

    [Header("GameObject Attribute")]

    public GameObject credittransactionPanel;
    public GameObject debittransactionPanel;
    public GameObject withdrwalTransactionPanel;

    public GameObject loadingPanel;
    public GameObject infoPanel;
    WWW w;
    string status;


    private string results;
    public string Results
    {
        get
        {
            return results;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        paymentHistoryScript= GameManager.apiBase1 + "transaction_history";
    }

    public void OnPaymentHistory()
    {
        transactionPanel.SetActive(true);
        OnWithdrwalBtn("3");
    }
    public void OnWithdrwalBtn(string id)
    {
        for (int i = 0; i < contentPanel.childCount; i++)
        {
            Destroy(contentPanel.GetChild(i).gameObject);
        }
        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Uid);
        form.AddField("walletType", id);
        WWW w = new WWW(paymentHistoryScript, form);
        withdrwalTransactionPanel.SetActive(true);
        credittransactionPanel.SetActive(false);
        debittransactionPanel.SetActive(false);
        StartCoroutine(PaymentWithDrawData(w));
        loadingPanel.SetActive(true);
    }
    public void OnCreditBtn(string id)
    {
        for (int i = 0; i < creditcontentPanel.childCount; i++)
        {
            Destroy(creditcontentPanel.GetChild(i).gameObject);
        }
        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Uid);
        form.AddField("walletType", id);
        WWW w = new WWW(paymentHistoryScript, form);
        withdrwalTransactionPanel.SetActive(false);
        credittransactionPanel.SetActive(true);
        debittransactionPanel.SetActive(false);
        StartCoroutine(PaymentCreditData(w));
        loadingPanel.SetActive(true);
    }
    public void OnDebitBtn(string id)
    {
        for (int i = 0; i < debitcontentPanel.childCount; i++)
        {
            Destroy(debitcontentPanel.GetChild(i).gameObject);
        }
        WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.Uid);
        form.AddField("walletType", id);
        Debug.LogError(GameManager.Uid);
        Debug.LogError(id);
        WWW w = new WWW(paymentHistoryScript, form);
        withdrwalTransactionPanel.SetActive(false);
        credittransactionPanel.SetActive(false);
        debittransactionPanel.SetActive(true);
        StartCoroutine(PaymentDebitData(w));
        loadingPanel.SetActive(true);
    }

    IEnumerator PaymentWithDrawData(WWW w)
    {
        yield return w;
        Debug.Log(w.error);

        if (w.error == null)
        {
            JsonData jsonvale = JsonMapper.ToObject(w.text);
            print(jsonvale["result_push"].Count + "count data");
            if (jsonvale["result_push"].Count !=2)
            {
                contentPanel.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
                int length = jsonvale["result_push"].Count;
                contentPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(contentPanel.GetComponent<RectTransform>().sizeDelta.x, 140 * length);
                for (int i = 0; i < jsonvale["result_push"].Count; i++)
                {
                    PaymentScript paymentScript = Instantiate(transactionPrefab, contentPanel).GetComponent<PaymentScript>();

                    paymentScript.newImage.sprite = redArrow;
                    transactionId = jsonvale["result_push"][i]["txn_id"].ToString();
                    paymentScript.newPaymentId.text = "Payment Id -#" +  transactionId;
                    //walletType = jsonvale["result_push"][i]["walletType"].ToString();
                    //paymentScript.transactionby.text = walletType;
                    amount = jsonvale["result_push"][i]["amount"].ToString();
                    paymentScript.newAmountText.text = "Amount - " + amount;
                    //disc = jsonvale["result_push"][i]["description"].ToString();
                    //paymentScript.status.text = disc;
                    date = jsonvale["result_push"][i]["created_at"].ToString();
                    paymentScript.newDate.text = date;
                    loadingPanel.SetActive(false);
                }

            }
            else
            {
                loadingPanel.SetActive(false);
                //infoPanel.SetActive(true);
            }
        }

           
       
    }
    IEnumerator PaymentCreditData(WWW w)
    {
        yield return w;
        Debug.Log(w.text);

        if (w.error == null)
        {
            JsonData jsonvale = JsonMapper.ToObject(w.text);
            creditcontentPanel.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
            int length = jsonvale["result_push"].Count;
            creditcontentPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(creditcontentPanel.GetComponent<RectTransform>().sizeDelta.x, 140 * length);
            for (int i = 0; i < jsonvale["result_push"].Count; i++)
            {
                PaymentScript paymentScript = Instantiate(credittransactionPrefab, creditcontentPanel).GetComponent<PaymentScript>();
                paymentScript.newImage.sprite = greenArrow;
                transactionId = jsonvale["result_push"][i]["txn_id"].ToString();
                paymentScript.newPaymentId.text = "Payment Id -#" + transactionId;
                //walletType = jsonvale["result_push"][i]["walletType"].ToString();
                //paymentScript.transactionby.text = walletType;
                amount = jsonvale["result_push"][i]["amount"].ToString();
                paymentScript.newAmountText.text = "Amount - " + amount;
                //disc = jsonvale["result_push"][i]["description"].ToString();
                //paymentScript.status.text = disc;
                date = jsonvale["result_push"][i]["created_at"].ToString();
                paymentScript.newDate.text = date;
                loadingPanel.SetActive(false);
            }
        }
        
        
    }
    IEnumerator PaymentDebitData(WWW w)
    {
        yield return w;
        Debug.Log(w.text);

        if (w.error == null)
        {
            JsonData jsonvale = JsonMapper.ToObject(w.text);
            if (jsonvale["result_push"].Count != 2)
            {
                debitcontentPanel.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
                int length = jsonvale["result_push"].Count;
                debitcontentPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(debitcontentPanel.GetComponent<RectTransform>().sizeDelta.x, 140 * length);
                for (int i = 0; i < jsonvale["result_push"].Count; i++)
                {
                    PaymentScript paymentScript = Instantiate(debittransactionPrefab, debitcontentPanel).GetComponent<PaymentScript>();

                    paymentScript.newImage.sprite = redArrow;
                    transactionId = jsonvale["result_push"][i]["txn_id"].ToString();
                    paymentScript.newPaymentId.text = "Payment Id -#" + transactionId;
                    //walletType = jsonvale["result_push"][i]["walletType"].ToString();
                    //paymentScript.transactionby.text = walletType;
                    amount = jsonvale["result_push"][i]["amount"].ToString();
                    paymentScript.newAmountText.text = "Amount - " + amount;
                    //disc = jsonvale["result_push"][i]["description"].ToString();
                    //paymentScript.status.text = disc;
                    date = jsonvale["result_push"][i]["created_at"].ToString();
                    paymentScript.newDate.text = date;
                    loadingPanel.SetActive(false);
                    //transactionId = jsonvale["result_push"][i]["txn_id"].ToString();
                    //paymentScript.transactionId.text = transactionId;
                    //walletType = jsonvale["result_push"][i]["walletType"].ToString();
                    //paymentScript.transactionby.text = walletType;
                    //amount = jsonvale["result_push"][i]["amount"].ToString();
                    //paymentScript.transactionAmount.text = amount;
                    //disc = jsonvale["result_push"][i]["description"].ToString();
                    //paymentScript.status.text = disc;
                    //nameP = jsonvale["result_push"][i]["transfer_to"].ToString();
                    //paymentScript.name.text = nameP;
                    //date = jsonvale["result_push"][i]["created_at"].ToString();
                    //Debug.Log(date);
                    //paymentScript.date.text = date;
                    //loadingPanel.SetActive(false);
                }
            }
            else
            {
                loadingPanel.SetActive(false);
                //infoPanel.SetActive(true);
            }
        }
     }
    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains(","))
            value = value.Remove(value.IndexOf(","));

        return value;
    }

    public TextMeshProUGUI newCreditText, newDebitText, newWithdrawText;
    public Sprite redArrow, greenArrow;

    public void ChangeButtonColor(TextMeshProUGUI clickedButton)
    {
        newCreditText.color = Color.red;
        newDebitText.color = Color.red;
        newWithdrawText.color = Color.red;
        clickedButton.color = Color.green;
    }

}
