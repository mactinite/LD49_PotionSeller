using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElRaccoone.Tweens;
using ElRaccoone.Tweens.Core;
using UnityEngine.Events;

public class GrindReagents : MiniGame
{

    public GameObject pestleSprite;
    public float optimalSpeed = 200f;
    public float maxDeviation = 5f;

    public float circleRadius = 0.32f;

    public DrawCircle circleElement;
    public Transform handle;
    public float maxSpeed = 6000f;

    public TMPro.TMP_Text feedbackText;


    public float goalValue = 1f;
    public float explosionValue = 1f;
    
    bool dragging = false;
    Tween<float> pestlePosXTween;
    Tween<float> pestleWobbleTween;

    public float currentSpeed;

    private float angleVelocity;
    private float angle = 0;
    private float speedVelocity;

    private float completionValue = 0;
    private float explodeValue = 0;


    // Start is called before the first frame update
    void Start()
    {
        circleElement.radius = circleRadius;
        handle.transform.localPosition = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad) * circleRadius, Mathf.Cos(angle * Mathf.Deg2Rad) * circleRadius);

        MouseInteraction.Instance.OnClick.AddListener(DragStart);
        MouseInteraction.Instance.OnRelease.AddListener(DragEnd);

        pestlePosXTween = pestleSprite.transform.TweenLocalPositionX(-0.13f, 1f).SetPingPong().SetInfinite().SetTime(0).SetPaused(true);
        pestleWobbleTween = pestleSprite.transform.TweenLocalRotationZ(35, 1).SetPingPong().SetInfinite().SetTime(0).SetPaused(true);
        circleElement.gameObject.SetActive(false);
        handle.gameObject.SetActive(false);
        feedbackText.gameObject.SetActive(false);
    }

    public override void StartGame()
    {
        base.StartGame();
        circleElement.gameObject.SetActive(true);
        handle.gameObject.SetActive(true);
        feedbackText.gameObject.SetActive(true);

    }

    public void DragStart(GameObject clicked)
    {
        if (clicked == handle.gameObject)
        {
            dragging = true;
            pestlePosXTween.SetPaused(false);
            pestleWobbleTween.SetPaused(false);
        }
    }


    public void DragEnd()
    {
        if (dragging)
        {
            dragging = false;
            pestlePosXTween.SetPaused(true);
            pestleWobbleTween.SetPaused(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            Vector2 mouseDirection = MouseInteraction.Instance.mouseWorldPosition - (Vector2)transform.position;
            mouseDirection.Normalize();

            angle = Mathf.SmoothDampAngle(angle, -Vector2.SignedAngle(Vector3.up, mouseDirection), ref angleVelocity, 0.01f, maxSpeed);

            Vector2 circlePosition = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad) * circleRadius, Mathf.Cos(angle * Mathf.Deg2Rad) * circleRadius);


            handle.transform.localPosition = circlePosition;
            float value = Mathf.Abs((180 - (angle % 360)) / 180);
            pestlePosXTween.SetTime(value);
            pestleWobbleTween.SetTime(value);

            currentSpeed = Mathf.SmoothDamp(currentSpeed, angleVelocity, ref speedVelocity, 0.3f);

            if (currentSpeed > optimalSpeed + maxDeviation)
            {
                feedbackText.text = "Slower!";
                explodeValue += Time.deltaTime;
            }
            else if (currentSpeed < optimalSpeed - maxDeviation)
            {
                feedbackText.text = "Faster!";
            }
            else
            {
                feedbackText.text = "Keep it up!";
                completionValue += Time.deltaTime;
            }

            if (completionValue >= goalValue)
            {
                feedbackText.text = "Nice!";
                feedbackText.color = Color.green;
                DragEnd();
                circleElement.gameObject.SetActive(false);
                handle.gameObject.SetActive(false);
                OnSuccess.Invoke();

            } else if(explodeValue >= explosionValue)
            {
                feedbackText.text = "Ruined!";
                feedbackText.color = Color.red;
                DragEnd();
                circleElement.gameObject.SetActive(false);
                handle.gameObject.SetActive(false);
                OnFail.Invoke();
            }
        }
    }
}
