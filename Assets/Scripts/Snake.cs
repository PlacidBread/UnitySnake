using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    private static Vector2 _direction;
    private Transform _tr;
    [NonSerialized] public static readonly List<Transform> Segments = new List<Transform>();
    private const int StartSize = 4;
    private int _hScore;

    public Transform segmentPrefab;
    public Text score;
    public Text highScore;
    private int _count;

    private void Start()
    {
        _tr = gameObject.GetComponent<Transform>();
        ResetState();
        score.text = "Score: " + _count;
        _hScore = PlayerPrefs.GetInt("highScore");
        highScore.text = "High score: " + _hScore;
    }

    private void FixedUpdate()
    {
        ProcessInput();
        Wig();
        Move();
    }

    private void ProcessInput()
    {
        if (_direction.x != 0f)
        {
            if (Input.GetKey(KeyCode.W))
            {
                _direction = Vector2.up;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _direction = Vector2.down;
            }
        }

        else if (_direction.y != 0f)
        {
            if (Input.GetKey(KeyCode.A))
            {
                _direction = Vector2.left;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                _direction = Vector2.right;
            }
        }

        // use space as start key
        else if (Input.GetKey(KeyCode.Space))
        {
            _direction = Vector2.right;
        }
    }

    private void Move()
    {
        Vector3 pos = _tr.position;
        float moveX = Mathf.Round(pos.x) + _direction.x;
        float moveY = Mathf.Round(pos.y) + _direction.y;
        _tr.position = new Vector2(moveX, moveY);
    }

    private void Wig()
    {
        for (int i = Segments.Count - 1; i > 0; i--)
        {
            Segments[i].position = Segments[i - 1].position;
        }
    }

    private void Grow()
    {
        // Instantiate the prefab
        Transform segment = Instantiate(segmentPrefab);
        segment.position = Segments[Segments.Count - 1].position;

        Segments.Add(segment);
    }

    private void ResetState()
    {
        if (_count > _hScore)
        {
            HighScore();
        }

        _tr.position = Vector2.zero;
        _direction = Vector2.zero;
        _count = 0;
        score.text = "Score: " + _count;

        for (int i = 1; i < Segments.Count; i++)
        {
            Destroy(Segments[i].gameObject);
        }

        Segments.Clear();
        Segments.Add(transform);

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

    private void HighScore()
    {
        PlayerPrefs.SetInt("highScore", _count);
        highScore.text = "High score: " + _count;
    }
}
