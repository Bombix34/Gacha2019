using UnityEngine;
using System.Collections;

public class EditorCurve
{
    public Vector3 pinStart;
    public Vector3 pinEnd;

    public Vector3 pinStartDirection;


    public Vector3 pinEndDirection;

    public float Step;

    public bool Complete;

    public EditorCurve(Vector3 start, Vector3 dir)
    {
        pinStart = start;
        pinStartDirection = dir;

    }
    
    public void SetEnd(Vector3 end, Vector3 dir)
    {
        pinEnd = end;
        pinEndDirection = dir;
        Complete = true;
    }

    public Vector3[] positionsSteped()
    {

        Vector3 mover = pinStart;

        int Size = Mathf.RoundToInt(Vector3.Distance(pinStart, pinEnd) / Step);

        Vector3[] positionsAlong = new Vector3[Size];

        

        for (int i = 0; i < Size; i++)
        {
            positionsAlong[i] = mover;

            mover = Vector3.MoveTowards(mover, pinEnd, Step);



        }

        return positionsAlong;

    }


}
