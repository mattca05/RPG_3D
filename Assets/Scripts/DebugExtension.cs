using UnityEngine;

public static class DebugExtension
{
    public static void DebugWireSphere(Vector3 position, Color color, float radius)
    {
        float angleStep = 10f;
        for (int i = 0; i < 360; i += (int)angleStep)
        {
            float angle = i * Mathf.Deg2Rad;
            float nextAngle = (i + angleStep) * Mathf.Deg2Rad;
            Vector3 point1 = position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Vector3 point2 = position + new Vector3(Mathf.Cos(nextAngle), 0, Mathf.Sin(nextAngle)) * radius;
            Debug.DrawLine(point1, point2, color);
        }
    }
}