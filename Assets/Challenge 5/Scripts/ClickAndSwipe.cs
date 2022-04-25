using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(TrailRenderer), typeof(BoxCollider))]

public class ClickAndSwipe : MonoBehaviour
{
    private GameManagerX gameManagerX;
    private Camera camera;
    private Vector3 mousePos;
    private TrailRenderer trail;
    private BoxCollider col;
    private bool swiping = false;
    public int pointValue;
    // Start is called before the first frame update
    void Awake()
    {
        camera = Camera.main;
        trail = GetComponent<TrailRenderer>();
        col = GetComponent<BoxCollider>();
        trail.enabled = false;
        col.enabled = false;

        gameManagerX = GameObject.Find("Game Manager").GetComponent<GameManagerX>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManagerX.isGameActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                swiping = true;
                UpdateComponent();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                swiping = false;
                UpdateComponent();
            }
            else if (swiping)
            {
                UpdateMousePosition();
            }
        }
    }

    void UpdateMousePosition()
    {
        mousePos = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        transform.position = mousePos;
    }

    void UpdateComponent()
    {
        trail.enabled = swiping;
        col.enabled = swiping;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<TargetX>())
        {
            collision.gameObject.GetComponent<TargetX>().DestroyTarget();
            gameManagerX.UpdateScore(pointValue);
        }
    }

}
