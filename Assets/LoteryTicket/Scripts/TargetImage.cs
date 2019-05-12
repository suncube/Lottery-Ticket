using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetImage
{
    public bool isPaintedFlag = false;
    //test 
    public Image Image;
    //
    public Rect Rect { get; private set; }
    private float AllSquare;
    private List<Point> points;
   
    private int wL = 10;
    private int hL = 10;

    public TargetImage(Rect rect)
    {
        Rect = rect;

        // initialize points
        points = new List<Point>((wL) * (hL));
        float stepX = Rect.width / wL;
        float stepY = Rect.height / hL;
        float X = Rect.x;
        float Y = Rect.y;

        for (int i = 0; i < wL; i++)
        {
            X = Rect.x;

            for (int j = 0; j < hL; j++)
            {
                var point = new Point(new Vector2(X, Y));
                points.Add(point);

                X += stepX;

                //     Debug.Log(string.Format("[{0}_{1}] {2}",i,j, point.Position));
            }
            
            Y += stepY;
        }

        //  Debug.Log("All counts"+points.Count);
    }

    public bool AddIntersectRects(Rect rect)
    {
        if (isPaintedFlag) return true;

        int paintCount = 0;
        for (var index = 0; index < points.Count; index++)
        {
            var point = points[index];

                  
            if (rect.Contains(point.Position) && !point.isPaint)
            {
                point.isPaint = true;
            }

            if (point.isPaint)
            {
                paintCount++;
            }

        }

        float percent = paintCount / points.Count;

        isPaintedFlag = percent > 0.9f;

        return isPaintedFlag;
    }

}

class Point
{
    public Vector2 Position { get; private set; }
    public bool isPaint { get; set; }

    public Point(Vector2 position)
    {
        Position = position;
    }
}