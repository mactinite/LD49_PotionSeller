using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BurnerGame : MiniGame
{
    public TMPro.TMP_Text statusText;
    public float safeZone = 0.5f;
    public float safeZoneSize = 0.1f;

    public float failTime = 2f;
    public float successTime = 5f;

    public float riseSpeed = 1f;
    public float fallSpeed = 2f;

    public Animator burnerAnimator;

    public Transform handleSprite;

    public SpriteRenderer optimalPositionSprite;
    public SpriteRenderer backgroundTrackSprite;


    bool started = false;

    [SerializeField, ReadOnly]
    private float currentValue;

    private float failTimer = 0;
    private float successTimer = 0;

    private void Start()
    {
        optimalPositionSprite.gameObject.SetActive(false);
        backgroundTrackSprite.gameObject.SetActive(false);
        handleSprite.gameObject.SetActive(false);
    }

    public override void StartGame()
    {
        started = true;
        optimalPositionSprite.gameObject.SetActive(true);
        backgroundTrackSprite.gameObject.SetActive(true);
        handleSprite.gameObject.SetActive(true);
        optimalPositionSprite.transform.localPosition = new Vector2(0, safeZone - (safeZoneSize / 2));
        optimalPositionSprite.size = new Vector2(optimalPositionSprite.size.x, safeZoneSize);
    }

    public void Update()
    {
        if (started)
        {
            

            if (Mouse.current.leftButton.IsActuated())
            {
                currentValue += Time.deltaTime * riseSpeed;
            } else
            {
                currentValue -= Time.deltaTime * fallSpeed;
            }
            currentValue = Mathf.Clamp01(currentValue);

            if(currentValue > safeZone + (safeZoneSize / 2))
            {
                // TOO HOT
                failTimer += Time.deltaTime;
                statusText.text = "TOO HOT!!";
                statusText.color = Color.red;

            } else if(currentValue > safeZone - (safeZoneSize / 2))
            {
                successTimer += Time.deltaTime;
                statusText.text = "Keep it steady!";
                statusText.color = Color.white;
            } else
            {
                statusText.text = "Needs more heat!";
                statusText.color = Color.white;
            }


            if(successTimer >= successTime)
            {
                statusText.text = "AWESOME!";
                statusText.color = Color.green;
                OnSuccess.Invoke();
            } else if(failTimer >= failTime)
            {
                statusText.text = "RUINED";
                statusText.color = Color.red;
                OnFail.Invoke();
            }
            handleSprite.transform.localPosition = new Vector2(0, currentValue);
            burnerAnimator.SetFloat("Heat", currentValue);
        }

    }

}
