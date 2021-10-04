using ElRaccoone.Tweens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dropper : MiniGame
{
    public TMPro.TMP_Text statusText;


    public float targetDropRadius = 0.32f;
    public float maxDeviation = 0.08f;
    public float growSpeed = 0.01f;

    public DrawCircle goalCircle;
    public GameObject dropletSprite;

    private float currentDropRadius = 0;
    private bool started;
    private bool dropped = false;
    private bool dropletFormed;
    // Start is called before the first frame update
    void Start()
    {
        statusText.text = "Ready?";
        goalCircle.gameObject.SetActive(false);
        dropletSprite.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (started && !dropped)
        {
            if (Mouse.current.leftButton.IsPressed() && !dropped)
            {
                currentDropRadius += Time.deltaTime * growSpeed;
                dropletFormed = true;
                statusText.text = "Release when ready.";
            }


            if (!dropped && dropletFormed && !Mouse.current.leftButton.IsPressed())
            {
                dropped = true;
            }

            if (!dropped)
            {
                dropletSprite.transform.localScale = new Vector2(currentDropRadius, currentDropRadius);
            }
            else
            {
                if (currentDropRadius > targetDropRadius + maxDeviation)
                {
                    goalCircle.lineRenderer.endColor = Color.red;
                    goalCircle.lineRenderer.startColor = Color.red;

                    statusText.text = "Ruined..";
                    statusText.color = Color.red;
                    OnFail.Invoke();
                }
                else if (currentDropRadius < targetDropRadius - maxDeviation)
                {
                    goalCircle.lineRenderer.startColor = Color.red;
                    goalCircle.lineRenderer.endColor = Color.red;

                    statusText.text = "Ruined..";
                    statusText.color = Color.red;
                    OnFail.Invoke();
                }
                else
                {
                    statusText.text = "Great!";
                    statusText.color = Color.green;
                    goalCircle.lineRenderer.startColor = Color.green;
                    OnSuccess.Invoke();
                }
                dropletSprite.transform.TweenPositionY(-3, 2f);
            }
        }
    }


    public override void StartGame()
    {
        started = true;
        statusText.text = "Click and hold";
        goalCircle.gameObject.SetActive(true);
        dropletSprite.gameObject.SetActive(true);
        goalCircle.radius = targetDropRadius;
        goalCircle.Draw();
    }
}
