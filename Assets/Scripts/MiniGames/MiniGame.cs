using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MiniGame: MonoBehaviour
{
    public UnityEvent OnSuccess = new UnityEvent();
    public UnityEvent OnFail = new UnityEvent();

    public virtual void StartGame()
    {

    }

}
