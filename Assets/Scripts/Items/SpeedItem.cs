using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedItem : MonoBehaviour, IItem
{
    public static event Action<float> OnSpeedCollected;
    public float speedMultiplier = 1.5f;
    /// <summary>
    /// Cuando recoge el item se destruye y temporalmente aumenta su velocidad y deja un trail
    /// </summary>
    public void Collect()
    {
        OnSpeedCollected.Invoke(speedMultiplier);
        Destroy(gameObject);
    }
}
