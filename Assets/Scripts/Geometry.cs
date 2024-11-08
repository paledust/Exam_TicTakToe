using UnityEngine;

public static class Geometry
{
    public const float UNIT_LENGTH = 5;
    public static Vector3 PointFromGrid(Vector2Int gridPoint){
        float x = gridPoint.x*1.0f-1f;
        float z = gridPoint.y*1.0f-1f;
        return new Vector3(x,0,z)*UNIT_LENGTH;
    }
    public static Vector2Int GridPoint(int col, int row){
        return new Vector2Int(col, row);
    }
    public static bool ValidPoint(Vector2Int gridPoint){
        return gridPoint.x >=0 && gridPoint.x <= 2 && gridPoint.y >= 0 && gridPoint.y <= 2;
    }
    public static Vector2Int GridFromPoint(Vector3 point){
        int col = Mathf.FloorToInt(1.5f + point.x/UNIT_LENGTH);
        int row = Mathf.FloorToInt(1.5f + point.z/UNIT_LENGTH);

        return new Vector2Int(col, row);
    }
    public static int IndexFromGrid(Vector2Int gridPoint){
        return IndexFromGrid(gridPoint.x, gridPoint.y);
    }
    public static int IndexFromGrid(int col, int row){
        return col*3+row;
    }
    public static Vector2Int GridFromIndex(int index){
        return new Vector2Int(index/3, index%3);
    }
}
