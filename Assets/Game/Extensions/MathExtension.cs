using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathExtension
{
    /// <summary>
    /// Returns a random point inside a circle of Radius <paramref name="radius"/>, around <paramref name="center"/>.
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static Vector2 RandomPointInsideCircle(Vector2 center, float radius)
    {
        return RandomPointInsideCircle(center, radius, 0f);
    }

    /// <summary>
    /// Returns an random point inside a circle of Max Radius <paramref name="radius"/> at least <paramref name="innerRadius"/> distance away from its <paramref name="center"/>
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <param name="innerRadius"></param>
    /// <returns></returns>
    public static Vector2 RandomPointInsideCircle(Vector2 center, float radius, float innerRadius)
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float distanceToCenter = Random.Range(innerRadius, radius);
        return center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distanceToCenter;
    }
}
