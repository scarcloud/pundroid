﻿using UnityEngine;

/// <summary>
/// Abstract class for all Spawnable ingame objects.
/// </summary>
public abstract class Spawnable : MonoBehaviour
{
    public float minTorque = -500f;
    public float maxTorque = 50f;
    public float minForce = 100f;
    public float maxForce = 400f;

    public float leftConstraint = 0.0f;
    public float rightConstraint = 0.0f;
    public float topConstraint = 0.0f;
    public float bottomConstraint = 0.0f;
    public float buffer = 0.1f;

    public float speed;

    float distanceZ;

    Camera cam;
    Rigidbody2D rb;

    protected Player player;
    protected SoundController soundController;

    /// <summary>
    /// Basic Start method for all Spawnables, can be extended with
    /// IndividualStartConfiguration()
    /// </summary>
    public virtual void Start()
    {
        cam = Camera.main;
        distanceZ = Mathf.Abs(cam.transform.position.z + transform.position.z);
        rb = GetComponent<Rigidbody2D>();

        soundController = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();
        if (soundController == null) { Debug.Log("SoundController nicht gefunden!"); }
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (player == null) { Debug.Log("Player nicht gefunden!"); }

        IndividualStartConfiguration();

        leftConstraint = cam.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, distanceZ)).x;
        rightConstraint = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, distanceZ)).x;
        bottomConstraint = cam.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, distanceZ)).y;
        topConstraint = cam.ScreenToWorldPoint(new Vector3(0.0f, Screen.height, distanceZ)).y;

        float magnitude = Random.Range(minForce, maxForce);
        float torque = Random.Range(minTorque, maxForce);
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);

        rb.AddForce(magnitude * new Vector2(x, y));
        rb.AddTorque(torque);
    }

    /// <summary>
    /// Additional Start options for individual child classes
    /// </summary>
    public abstract void IndividualStartConfiguration();

    /// <summary>
    /// Currently no overrides
    /// </summary>
    public virtual void FixedUpdate()
    {
        if (transform.position.x < leftConstraint - buffer) { transform.position = new Vector3(rightConstraint + buffer, transform.position.y, transform.position.z); }
        if (transform.position.x > rightConstraint + buffer) { transform.position = new Vector3(leftConstraint - buffer, transform.position.y, transform.position.z); }
        if (transform.position.y < bottomConstraint - buffer) { transform.position = new Vector3(transform.position.x, topConstraint + buffer, transform.position.z); }
        if (transform.position.y > topConstraint + buffer) { transform.position = new Vector3(transform.position.x, bottomConstraint - buffer, transform.position.z); }
    }

    /// <summary>
    /// Must be implemented individually
    /// </summary>
    /// <param name="other"></param>
    public abstract void OnTriggerEnter2D(Collider2D other);
}
