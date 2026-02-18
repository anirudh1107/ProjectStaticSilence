using UnityEngine;

public interface IDeflectable
{
    // Returns true if deflection was successful
    bool OnDeflect(Vector2 direction, float speed, GameObject deflector);
}