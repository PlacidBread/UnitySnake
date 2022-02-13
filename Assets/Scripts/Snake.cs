using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    private Vector2 _direction;
    private Transform _tr;
    private readonly List<Transform> _segments = new List<Transform>();
    private const int StartSize = 4;

    public Transform segmentPrefab;
    public Text score;
    private int _count = 0;

    private void Start()
    {
        _tr = gameObject.GetComponent<Transform>();
        ResetState();
        score.text = "Score: " + _count;
    }

    // Update is called once per frame
    private void Update()
    {
        ProcessInput();
    }

    private void FixedUpdate()
    {
        Wig();
        Move();
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.W) && !(_direction == Vector2.down))
        {
            _direction = Vector2.up;
        } 
        else if (Input.GetKey(KeyCode.S) && !(_direction == Vector2.up))
        {
            _direction = Vector2.down;
        } 
        else if (Input.GetKey(KeyCode.A) && !(_direction == Vector2.right))
        {
            _direction = Vector2.left;
        } 
        else if (Input.GetKey(KeyCode.D) && !(_direction == Vector2.left))
        {
            _direction = Vector2.right;
        }
    }

    private void Move()
    {
        Vector3 pos = _tr.position;
        float moveX = Mathf.Round(pos.x + _direction.x);
        float moveY = Mathf.Round(pos.y + _direction.y);
        _tr.position = new Vector2(moveX, moveY);
    }

    private void Wig()
    {
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }
    }

    private void Grow()
    {
        // Instantiate the prefab
        Transform segment = Instantiate(segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;
        
        _segments.Add(segment);
    }

    private void ResetState()
    {
        _tr.position = Vector2.zero;
        _direction = Vector2.zero;
        _count = 0;
        score.text = "Score: " + _count;
        
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }
        
        _segments.Clear();
        _segments.Add(transform);

        for (int i = 0; i < StartSize; i++)
        {
            Grow();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("food"))
        {
            Grow();
            _count++;
            score.text = "Score: " + _count;
        }
        else
        {
            ResetState();
        }
    }
}
