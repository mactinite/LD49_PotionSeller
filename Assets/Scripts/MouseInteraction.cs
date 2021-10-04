using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mactinite.ToolboxCommons;
using System;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class MouseInteraction : SingletonMonobehavior<MouseInteraction>
{
    Camera cameraComponent;

    public UnityEvent<GameObject> OnClick = new UnityEvent<GameObject>();
    public UnityEvent OnRelease = new UnityEvent();
    public Vector2 mouseWorldPosition;
    public static GameObject hovering;

    bool mouseDown = false;
    Mouse mouse;
    // Start is called before the first frame update
    void Start()
    {
        cameraComponent = GetComponent<Camera>();
        mouse = Mouse.current;
    }

    // Update is called once per frame
    void Update()
    {
        mouseWorldPosition = cameraComponent.ScreenToWorldPoint(mouse.position.ReadValue());
        Vector3 origin = cameraComponent.ScreenToWorldPoint(mouse.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(origin, transform.forward, 20f);

        if (hit)
        {
            hovering = hit.transform.gameObject;
        } else
        {
            hovering = null;
        }

        if (mouse.leftButton.IsActuated())
        { // if left button pressed...
            if (hit)
            {
                // the object identified by hit.transform was clicked
                // do whatever you want
                OnClick.Invoke(hit.transform.gameObject);
                
            }

            mouseDown = true;
        } else if(mouseDown == true)
        {
            mouseDown = false;
            OnRelease.Invoke();
        }
    }
}
