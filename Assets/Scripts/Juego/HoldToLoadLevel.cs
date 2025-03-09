using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HoldToLoadLevel : MonoBehaviour
{
    public float holdDuration = 1f; //How long you have to hold
    public Image fillCircle;
    private float holdTimer = 0;
    private bool isHolding = false;
    
    public static event Action OnHoldComplete;

    // Update is called once per frame
    void Update()
    {
        if (isHolding)
        {
            holdTimer += Time.deltaTime;
            fillCircle.fillAmount = holdTimer / holdDuration;
            if (holdTimer >= holdDuration)
            {
                OnHoldComplete.Invoke();
                ResetHold();
            }
        }
    }

    /// <summary>
    /// Mientras este pulsada la letra E en este caso la variable isHolding sera true
    /// </summary>
    /// <param name="context"></param>
    public void OnHold(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isHolding = true;
        }
        else if(context.canceled)
        {
            //Reset holding
            ResetHold();
        }
    }

    private void ResetHold() {
        isHolding = false;
        holdTimer = 0;
        fillCircle.fillAmount = 0;
    }
}
