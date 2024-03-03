using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateController : MonoBehaviour
{
    [SerializeField] private int targetFrameRate = -1;
    
    void Awake()
    {
        Application.targetFrameRate = targetFrameRate;
    }
}
