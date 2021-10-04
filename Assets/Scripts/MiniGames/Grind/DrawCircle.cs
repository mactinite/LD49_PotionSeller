using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircle : MonoBehaviour
{

    public float radius = 5f;
    public LineRenderer lineRenderer;

    public int segments = 16;


    // Start is called before the first frame update
    void Start()
    {

        Draw();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Draw")]
    public void Draw()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();

        lineRenderer.useWorldSpace = false;
        StartCoroutine(CreatePoints());

    }

    IEnumerator CreatePoints()
    {

        float x;
        float y;

        float change = 2 * (float)Mathf.PI / segments;
        float angle = 0;


        for (int i = 0; i < (segments); i++)
        {
            x = Mathf.Sin(angle) * (radius / 2);
            y = Mathf.Cos(angle) * (radius / 2);
            lineRenderer.positionCount = i + 1;
            lineRenderer.SetPosition((int)i, new Vector3(x, y, transform.position.z));

            angle += change;
        }

        yield return null;
    }
}
