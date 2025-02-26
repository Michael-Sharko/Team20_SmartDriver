using System;
using System.Collections;
using UnityEngine;

public static class MonoBehaviourExtension {

    public static Coroutine LateAndInvoke(this MonoBehaviour behaviour, float time, Action @event) {
        return behaviour.StartCoroutine(LateAndInvoke(time, @event));
    }
    private static IEnumerator LateAndInvoke(float time, Action @event) {
        yield return new WaitForSeconds(time);
        @event?.Invoke();
    }

}