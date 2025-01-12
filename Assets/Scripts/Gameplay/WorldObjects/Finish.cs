using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private GameObject finishMenu;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            finishMenu.SetActive(true);
        }
    }
}
