using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RemoveInventory : MonoBehaviour
{
    public TextMeshProUGUI AcidNumber;
    public TextMeshProUGUI TimeNumber;
    public TextMeshProUGUI BlackHoleNumber;

    public int AcidNum1 = 0;
    public int TimeNum = 0;
    public int BlackHoleNum = 0;


    public void AcidTrapPlace()
    {
        
            AcidNum1--;
            AcidNumber.text = AcidNum1.ToString();
        
    }

    public void TimeTrapPlace()
    {
        TimeNum--;
        TimeNumber.text = TimeNum.ToString();

    }

    public void BLackHoleTrapPlace()
    {
        BlackHoleNum--;
        BlackHoleNumber.text = BlackHoleNum.ToString();
    }
}
