using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyToolbox 
{
    public static Vector2 GetRandomVector2(float _xMin, float _xMax, float _yMin, float _yMax)
    {
        Vector2 value = new Vector2();

        value.x = Random.Range(_xMin, _xMax);
        value.y = Random.Range(_yMin, _yMax);

        return value;
    }

    public static Vector3 GetRandomVector3(float _xMin, float _xMax, float _yMin, float _yMax, float _zMin, float _zMax)
    {
        Vector3 value = new Vector3();

        value.x = Random.Range(_xMin, _xMax);
        value.y = Random.Range(_yMin, _yMax);
        value.z = Random.Range(_zMin, _zMax);
        return value;
    }

    public static void MoveItemBetweenLists<T>(List<T> source, List<T> destination, T item)
    {
        destination.Add(item);
        source.Remove(item);
    }
}
