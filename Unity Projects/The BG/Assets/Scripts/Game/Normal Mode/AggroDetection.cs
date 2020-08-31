using System;
using UnityEngine;

public class AggroDetection : MonoBehaviour
{
    public event Action<Transform> OnAggro = delegate { };
        
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            OnAggro(other.transform);          
        }
    }
}
