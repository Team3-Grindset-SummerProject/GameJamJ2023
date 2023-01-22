using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCount : MonoBehaviour
{
    public TextMeshProUGUI AcidNumber;
    public TextMeshProUGUI TimeNumber;
    public TextMeshProUGUI BlackHoleNumber;

    public int AcidNum = 0;
    public int TimeNum = 0;
    public int BlackHoleNum = 0;

    public void AcidClicked()
   {
        if(AcidNum < 14)
        {
             AcidNum++;
            AcidNumber.text = AcidNum.ToString();
        }
   }
}
