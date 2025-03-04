using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AggressionTrigger : MonoBehaviour
{
    public bool onAtack {get; private set;} = false;

    [SerializeField]
    private float aggressionRadius = 50f;

    private SphereCollider aggressionTrigger => gameObject.GetComponent<SphereCollider>();

    private void Start()
    {
        aggressionTrigger.radius = aggressionRadius;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
            onAtack = true;        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            onAtack = false;
    }
}
