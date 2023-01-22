using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddAcidCount : MonoBehaviour
{
   public TextMeshProUGUI WeightChange;
   public int number = 0;

   public void AcidClicked()
   {
        if(number < 14)
        {
             number += 2;
            WeightChange.text = number.ToString();
        }
   }

    public void TimeClicked()
   {
        if(number < 12)
        {
             number += 4;
         WeightChange.text = number.ToString();
        }

   }

    public void BlackHoleClicked()
    {
        
        if(number < 9)
        {
             number += 7;
         WeightChange.text = number.ToString();
    
        }
    
    }
   

}
