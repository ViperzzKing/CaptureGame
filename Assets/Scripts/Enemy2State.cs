using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class Enemy2State : MonoBehaviour, Trapped
{

    #region States
    [Header("Refrences")]
    public GameObject player;
    public Rigidbody rb;
    public Renderer render;

    private Color patrol = Color.green;

    private Vector3 originalScale;

    public enum State
    {
        Patrol,
        Chasing,
        Attack,
    }
    public State state;



    private void Start()
    {
        GameObject.Find("Player");
        NextState();
        originalScale = transform.localScale;
    }

    void NextState()
    {
        switch (state)
        {
            case State.Patrol:
                StartCoroutine(PatrolState());
                break;
            case State.Chasing:
                StartCoroutine(ChasingState());
                break;
            case State.Attack:
                StartCoroutine(AttackState());
                break;
            default:
                break;
        }
    }

    bool IsFacingPlayer()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        directionToPlayer.Normalize();

        float dotResult = Vector3.Dot(directionToPlayer, transform.forward);
        return dotResult >= 0.95f;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == player)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();

            Vector3 hitDir = player.transform.position - other.contacts[0].point;



            rb.AddForce(hitDir.normalized * 100f * rb.linearVelocity.magnitude);
        }
    }

    IEnumerator PatrolState()
    {
        

        Debug.Log("Entering Patrol State");
        while (state == State.Patrol)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(!IsFacingPlayer())
            transform.rotation *= Quaternion.Euler(0f, 50f * Time.deltaTime, 0f);

            render.material.color = patrol;

            if(distance <= 5)
            {
                state = State.Attack;
            }

            if (IsFacingPlayer() && distance <= 15)
            {
                state = State.Chasing;
            }

            yield return null; // Waits for a frame
        }
        Debug.Log("Exiting Patrol State");
        NextState();
    }

    IEnumerator ChasingState()
    {
        Debug.Log("Entering Chasing State");
        while (state == State.Chasing)
        {
            render.material.color = Color.yellow;

            float wave = Mathf.Sin(Time.time * 20f) * 0.1f + 1f;
            float wave2 = Mathf.Cos(Time.time * 20f) * 0.1f + 1f;
            transform.localScale = new Vector3(wave, wave2, wave);

            Vector3 direction = player.transform.position - transform.position;
            rb.AddForce(direction.normalized * (-1000f * Time.deltaTime));

            if (direction.magnitude < 5f)
            {
                state = State.Attack;
            }

            if (!IsFacingPlayer())
            {
                state = State.Patrol;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if(distanceToPlayer >= 20)
            {
                state = State.Patrol;
            }

            yield return null; // Waits for a frame
        }
        Debug.Log("Exiting Chasing State");
        NextState();
    }

    IEnumerator AttackState()
    {
        render.material.color = Color.red;

        Debug.Log("Entering attack state");

        transform.localScale = new Vector3(transform.localScale.x * 0.4f, transform.localScale.y * 0.4f, transform.localScale.z * 3);

        Vector3 direction = player.transform.position + transform.position;
        rb.AddForce(direction.normalized * 1000f);

        while (state == State.Attack)
        {
            yield return new WaitForSeconds(2f);
            state = State.Patrol;
        }

        transform.localScale = originalScale;
        Debug.Log("no attack now");
        NextState();
    }

    #endregion

    #region Capture
    public bool IsBeingCaptured { get; set; } = false;
    private float timer = 1;

    public bool CaptureAnim()
    {
        IsBeingCaptured = true;
        transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, timer);
        timer -= Time.deltaTime * 1;
        if (timer <= 0)
        {
            return false;
        }

        return true;
    }

    public int Points()
    {
        return 2;
    }
    #endregion


}
