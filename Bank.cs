using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 150;

    [SerializeField] int currentBalance;

    [SerializeField] TextMeshProUGUI displayBalance;

    public int CurrentBalance { get { return currentBalance;} }

    void Awake()
    {
        currentBalance = startingBalance;
        updateDisplay();
    }

    public void Deposit(int depositAmount)
    {
        currentBalance += Mathf.Abs(depositAmount);
        updateDisplay();
    }

    public void Withdraw(int withdrawAmount)
    {

        currentBalance -= Mathf.Abs(withdrawAmount);
        updateDisplay();
        if(currentBalance < 0 )
        {
            //lose game
            ReloadScene();
        }

    }
    void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
    void updateDisplay()
    {
        displayBalance.text = $"Gold: {currentBalance}";
    }
}
