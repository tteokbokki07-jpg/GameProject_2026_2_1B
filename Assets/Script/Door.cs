using UnityEngine;

public class Door : MonoBehaviour
{
    public float openAngle = 90f;
    public float openSpeed = 3f;
    private bool isOpen;
    public Quaternion closedRotation;
    public Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    void Update()
    {
        Quaternion targetRotation;
        if (isOpen)
        {
            targetRotation = openRotation;
        }
        else
        {
            targetRotation = closedRotation;
        }
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, openSpeed * Time.deltaTime);
    }

    public void Interact()
    {
        isOpen = !isOpen; //bool값 뒤집기
    }
    public string GetGuideText() //가이드 텍스트 호출
    {
        if(isOpen)
        {
            return "(E)문 닫기";
        }
        return "(E)문 열기";
    }    
}
