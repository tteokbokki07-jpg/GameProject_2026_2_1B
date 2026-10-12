using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName = "마을 주민";
    public string message = "환영.";

    public string GetNPCName()
    {
        return npcName;
    }
    public string GetMessage()
    {
        return message;
    }
    public string GetGuideText()
    {
        return "(E) " + npcName + "와 대화하기";
    }


}
