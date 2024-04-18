using UnityEngine;

public interface IPlayableCharacter {
    public void SetActive(bool v);
    public Transform GetTransform();
    public void SetPosition(Vector3 position);
    public void SetForward(Vector3 forward);
}
