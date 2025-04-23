using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam; // ƒобавили [SerializeField], чтобы поле отображалось в инспекторе

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
}