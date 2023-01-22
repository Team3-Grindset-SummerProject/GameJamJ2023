using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddAcidCount : MonoBehaviour
{
   public TextMeshProUGUI WeightChange;
   public int number = 0;

   public void ButtonClicked()
   {
    number += 2;
    WeightChange.text = number.ToString();
   }
}
