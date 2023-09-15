using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Animated start;
    public Animated middle;
    public Animated end;

    public void SetActive(Animated animated)
    {
        start.enabled = animated == start;
        middle.enabled = animated == middle;
        end.enabled = animated == end;
    }

    public void SetDir(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg , Vector3.forward);
    }
}