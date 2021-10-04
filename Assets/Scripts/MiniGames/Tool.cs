using ElRaccoone.Tweens;
using ElRaccoone.Tweens.Core;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tool : MonoBehaviour
{
    public string ToolID = "Tool ID Must Be Unique";

    Vector3 originalPosition;
    Tween<Vector3> returnTween;
    public ToolEvent OnDroppedOnWorkspace = new ToolEvent();

    [SerializeField, ReadOnly]
    bool dragging = false;
    [SerializeField, ReadOnly]
    bool onWorkspace = false;


    public MiniGame interaction;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        MouseInteraction.Instance.OnClick.AddListener(OnClick);
        MouseInteraction.Instance.OnRelease.AddListener(OnRelease);
    }

    private void OnRelease()
    {
        if (dragging)
        {
            returnTween = transform.TweenPosition(originalPosition, 0.5f).SetEaseBounceOut();
            dragging = false;

            // check if inside workspace and report
            if (onWorkspace)
            {
                OnDroppedOnWorkspace.Invoke(this);
            }
        }
    }

    public void Return()
    {
        returnTween = transform.TweenPosition(originalPosition, 0.5f).SetEaseBounceOut();
        dragging = false;
    }

    private void OnClick(GameObject clicked)
    {
        if (clicked == this.gameObject)
        {
            returnTween?.Cancel();
            transform.position = MouseInteraction.Instance.mouseWorldPosition;
            dragging = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            transform.position = MouseInteraction.Instance.mouseWorldPosition;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Workspace"))
        {
            onWorkspace = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Workspace"))
        {
            onWorkspace = false;
        }
    }

}

[Serializable]
public class ToolEvent : UnityEvent<Tool> { }
