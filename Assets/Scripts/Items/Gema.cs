using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gema : MonoBehaviour, IItem
{
    public static event Action<int> OnGemCollect;
    public int valor = 5;
    public void Collect()
    {
        OnGemCollect.Invoke(valor);
        SoundEffectManager.Play("Gema");
        Destroy(gameObject);
    }
}
