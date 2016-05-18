using UnityEngine;
using System.Collections.Generic;


public static class MathB
{
    /// <summary>
    /// Returns a List of GameObjects all evenly spaced in one of 3 shapes.( Wall, HalfCircle, Circle)
    /// </summary>
    /// <param name="pf">Prefab</param>
    /// <param name="radius">This is also used for wall length</param>
    /// <param name="amount">Pillar amount</param>
    /// <param name="shape"></param>
    /// <returns></returns>
    public static List<GameObject> ShapesOfGameObjects(GameObject pf, float radius, int amount, Shapes shape)
    {
        List<GameObject> objects = new List<GameObject>(amount);

        if (shape == Shapes.Wall)
        {
            for (int i = 0; i < amount; i++)
            {
                float wallPos = -radius + i*radius/amount*2;//maybe remove raduis/2
                var pos = new Vector3(wallPos, 0, 1);
                //Instantiate
                GameObject obj = Object.Instantiate(pf, pos, Quaternion.identity) as GameObject;
                objects.Add(obj);
            }
        }
        else
        {
            //Circle or Half Circle Shape
            float n = shape == Shapes.Circle ? 2 : 1; //n=2 (full circle) n=1 (half circle)

            for (int i = 0; i < amount; i++)
            {
                float angle = i * Mathf.PI * n / amount;
                Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle))*radius;

                Quaternion rot= Quaternion.LookRotation(position-Vector3.zero);

                GameObject obj = Object.Instantiate(pf, position, rot) as GameObject;

                objects.Add(obj);
            }
        }
        
        return objects;
    }

}