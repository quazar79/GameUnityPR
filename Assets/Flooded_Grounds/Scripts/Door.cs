using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isOpen = false; // ������� �� �����
    private Quaternion closedRotation;
    private Quaternion openRotation;
    public float openAngle = 90f; // ����, �� ������� ����� ����������� �����
    public float speed = 2f; // �������� ��������

    
    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
    }

    public void ToggleDoor()
    {
        StopAllCoroutines(); // ���������� ���������� ��������/��������
        StartCoroutine(RotateDoor(isOpen ? closedRotation : openRotation));
        isOpen = !isOpen;
    }

    private System.Collections.IEnumerator RotateDoor(Quaternion targetRotation)
    {
        float time = 0f;
        Quaternion startRotation = transform.rotation;

        while (time < 1f)
        {
            time += Time.deltaTime * speed;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, time);
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}
