using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddAcidCount : MonoBehaviour
{
    public TextMeshProUGUI WeightChange;

    public TextMeshProUGUI AcidNumber;
    public TextMeshProUGUI TimeNumber;
    public TextMeshProUGUI BlackHoleNumber;

    [SerializeField] private PlaceTrap playerTrap;
    [SerializeField] private GameObject acidTrap, timeTrap, blackTrap;
    


    public int number = 0;

    public int AcidNum = 0;
    public int TimeNum = 0;
    public int BlackHoleNum = 0;

    public void Start()
    {
        playerTrap = GameObject.FindWithTag("Player").GetComponent<PlaceTrap>();
        Pause();
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        Time.timeScale = 1;
    }

    public void AcidClicked()
   {
        if(number < 14)
        {
             number += 2;
            WeightChange.text = number.ToString();
            
            AcidNum++;
            AcidNumber.text = AcidNum.ToString();
        }
   }

    public void TimeClicked()
   {
        if(number < 12)
        {
             number += 4;
            WeightChange.text = number.ToString();

            TimeNum++;
            TimeNumber.text = TimeNum.ToString();
        }

   }

    public void BlackHoleClicked()
    {
        
        if(number < 9)
        {
             number += 7;
             WeightChange.text = number.ToString();

            BlackHoleNum++;
            BlackHoleNumber.text = BlackHoleNum.ToString();
        }
    
    }

    public void AcidRemove()
    {
        if(number >= 2)
        {
            number -= 2;    
            WeightChange.text = number.ToString();

            if(AcidNum > 0)
            {
                AcidNum--;
                AcidNumber.text = AcidNum.ToString();
            }
        }
        
    }

    public void TimeRemove()
    {
        if(number >= 4){
            number -= 4;
            WeightChange.text = number.ToString();

            if(TimeNum > 0)
            {
                TimeNum--;
                TimeNumber.text = TimeNum.ToString();
            }
        }
    }

    public void BlackHoleRemove()
    {
        if(number >=7 ){
            number -= 7;
            WeightChange.text = number.ToString();

            if(BlackHoleNum > 0)
            {
                BlackHoleNum--;
                BlackHoleNumber.text = BlackHoleNum.ToString();
            }
        }
    }

    public void AcidTrapPlace()
    {
        if(AcidNum > 0)
        {
            playerTrap.currentTrap = acidTrap;
            
            if (!playerTrap.OnPlaceTrap())
                return;
            
            AcidNum--;
            AcidNumber.text = AcidNum.ToString();
        }
        
    }

    public void TimeTrapPlace()
    {
        if(TimeNum >0)
        {
            playerTrap.currentTrap = timeTrap;
            
            if (!playerTrap.OnPlaceTrap())
                return;
            
            TimeNum--;
            TimeNumber.text = TimeNum.ToString();
        }
    }

    public void BlackHoleTrapPlace()
    {
        if(BlackHoleNum >0)
        {
            playerTrap.currentTrap = blackTrap;

            if (!playerTrap.OnPlaceTrap())
                return;
            
            
            BlackHoleNum--;
            BlackHoleNumber.text = BlackHoleNum.ToString();
        }
    }
}
