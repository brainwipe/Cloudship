using UnityEngine;

public interface IWindMaker
{
    Vector3 GetCycloneForce(Vector3 cycloneForce);
    
    float WindMagnitudeAt(Vector3 location);
    
    Quaternion WindDirectionAt(Vector3 location);
}