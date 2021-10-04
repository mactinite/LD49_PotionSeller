using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelingMiniGame : MiniGame
{
    public TMPro.TMP_Text statusText;
    public DrawCircle boundingCircle;
    public GameObject handleObject;

    public float radius = 0.64f;
    public float moveSpeed = 0.5f;

    public float channelingTime = 2f;
    public float failTime = 3f;

    [SerializeField]
    private bool started;

    private float channelingTimer = 0f;
    private float failTimer = 0f;
    private Vector2 newPosition;
    private Vector2 lastPosition;
    private SpriteRenderer handleSprite;
    private bool prepped = false;

    // Start is called before the first frame update
    void Start()
    {
        boundingCircle.radius = radius * 2;
        boundingCircle.Draw();
        handleSprite = handleObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (started && prepped)
        {
            if (MouseInteraction.hovering == handleObject)
            {
                //Good
                channelingTimer += Time.deltaTime;
                handleSprite.color = Color.green;
                statusText.text = "Good!";
            }
            else
            {
                //Bad
                failTimer += Time.deltaTime;
                handleSprite.color = Color.red;
                statusText.text = "Keep your cursor on target!";
            }


            if (channelingTimer >= channelingTime)
            {
                OnSuccess.Invoke();
                started = false;
                handleSprite.gameObject.SetActive(false);
                statusText.text = "Success!";
            }
            else if (failTimer >= failTime)
            {
                OnFail.Invoke();
                started = false;
                handleSprite.gameObject.SetActive(false);
                statusText.text = "Failed!";
            }
        }
    }

    public override void StartGame()
    {
        
        started = true;
        StartCoroutine(MoveHandle());
    }

    IEnumerator MoveHandle()
    {
        lastPosition = handleObject.transform.localPosition;
        newPosition = RandomPositionInBounds();
        float t = 0f;

        statusText.text = "Keep your cursor on target.";
        while (MouseInteraction.hovering != handleObject)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        prepped = true;
        while (started)
        {
            t += Time.deltaTime * moveSpeed;
            if(t >= 1)
            {
                lastPosition = newPosition;
                newPosition = RandomPositionInBounds();
                t = 0;
            } else
            {
                handleObject.transform.localPosition = Vector2.Lerp(lastPosition, newPosition, t);
            }
            yield return null;
        }
    }

    public Vector2 RandomPositionInBounds()
    {
        return Random.insideUnitCircle * radius;
    }
}
