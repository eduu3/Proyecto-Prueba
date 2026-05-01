using UnityEngine;

public static class CheckBounds
{
    public static Vector3 CheckMarges(Vector3 pos) 
    {
        if (pos.x >= 8.5f) pos = new Vector3(8.4f, pos.y);
        if (pos.x <= -8.5f) pos = new Vector3(-8.4f, pos.y);
        if (pos.y >= 4.3) pos = new Vector3(pos.x, 4.2f);
        if (pos.y <= -4.3) pos = new Vector3(pos.x, -4.2f);

        return pos;
    }
}
