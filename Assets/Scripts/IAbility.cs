using UnityEngine;

public interface IAbility
{
    void Activate(Transform user);
    float Cooldown { get; }
}