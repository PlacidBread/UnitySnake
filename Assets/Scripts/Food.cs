using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Food : MonoBehaviour
{
    public BoxCollider2D boxArea;
    
    // Start is called before the first frame update
    private void Start()
    {
        RandomizePos(false);
    }
    
    private void RandomizePos(bool checkTail)
    {
        List<Transform> segments = Snake.Segments;
        Bounds bounds = boxArea.bounds;
        if (checkTail)
        {
            // call CheckTail while there is a tail collision (assume there is)
            bool check = false;
            while (!check)
            {
                float x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
                float y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));
                check = CheckTail(x, y, segments);
            }
        }
        else
        {
            float x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
            float y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));
            SetPos(x, y);
        }
    }
    
    private bool CheckTail(float x, float y, List<Transform> segments)
    {
        foreach (Transform t in segments)
        {
            Vector2 pos = t.position;
            int posX = (int) Mathf.Round(pos.x);
            int posY = (int) Mathf.Round(pos.y);
            if (posX == (int) x && posY == (int) y)
            {
                Debug.Log("in tail");
                return false;
            }
        }

        SetPos(x, y);
        return true;
    }

    private void SetPos(float x, float y)
    {
        transform.position = new Vector2(x, y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    { 
        RandomizePos(true);
    }
}
