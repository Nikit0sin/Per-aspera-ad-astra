using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam; // �������� [SerializeField], ����� ���� ������������ � ����������

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
}