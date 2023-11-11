using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cContinueButton : MonoBehaviour
{
   public void OnClick()
   {
      cGameManager.Instance.ContinueButton();
   }
}
