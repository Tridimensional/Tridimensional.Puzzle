using UnityEngine;

public class OpeningAnimation : MonoBehaviour
{
    public float CircleDistance;
    public float CorneringSpeed;
    public float Distance;
    public float FlightHeight;
    public float PieceWidth;
    public float RotationSpeed;
    public float StraightSpeed;
    public float VisionWidth;
    public Vector3 Position;

    public bool Finished { get { return Distance <= 0; } }

    void Update()
    {
        if (Finished)
        {
            return;
        }

        if (Distance > CircleDistance)
        {
            var straightStep = StraightSpeed * Time.deltaTime;
            var distance = Distance - CircleDistance;
            Distance = distance >= straightStep ? (Distance - straightStep) : (CircleDistance - (Time.deltaTime - (distance / StraightSpeed)) * CorneringSpeed);
        }
        else
        {
            Distance -= CorneringSpeed * Time.deltaTime;
        }

        if (Distance < 0) { Distance = 0; }

        if (2 * (Distance + Position.x - PieceWidth) < VisionWidth)
        {
            var criticalAngle = 180;

            if (Distance >= CircleDistance)
            {
                gameObject.transform.position = Position + new Vector3(Distance - CircleDistance, 0, -FlightHeight);
            }
            else
            {
                var radius = FlightHeight * 0.5f;
                var angle = Distance / CircleDistance * Mathf.PI;
                gameObject.transform.position = Position + new Vector3(-radius * Mathf.Sin(angle), 0, -radius * (1 - Mathf.Cos(angle)));
            }

            if (Distance >= CircleDistance)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, criticalAngle - (Distance - CircleDistance) * RotationSpeed, 0);
            }
            else
            {
                gameObject.transform.rotation = Quaternion.Euler(0, criticalAngle + (360 - criticalAngle) * (CircleDistance - Distance) / CircleDistance, 0);
            }
        }
    }
}
