using System;
using UnityEngine;
using System.Collections;

public class DelayHelper : Singleton<DelayHelper>
{
    public static void DelayedAction(Action action, float delaySec)
    {
        if (Instance != null)
            Instance.InstanceDelayedAction(action, delaySec);
    }

    public void InstanceDelayedAction(Action action, float delaySec)
    {
        StartHelperCoroutine(DelayedActionCoroutine(action, delaySec));
    }

    IEnumerator DelayedActionCoroutine(Action action, float sec)
    {
        yield return new WaitForSeconds(sec);

        if (action != null)
            action();
    }

    public static Coroutine StartHelperCoroutine(IEnumerator coroutine)
    {
        return Instance.InstanceStartCoroutine(coroutine);
    }
    public static void StopHelperCoroutine(Coroutine coroutine)
    {
        Instance.InstanceStopCoroutine(coroutine);
    }

    public Coroutine InstanceStartCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }
    public void InstanceStopCoroutine(Coroutine coroutine)
    {
        StopCoroutine(coroutine);
    }
}
