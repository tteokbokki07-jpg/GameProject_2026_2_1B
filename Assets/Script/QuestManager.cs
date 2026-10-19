using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questText;
    private Quest currentQuest;

    void Start()
    {
        questText.text = "진행중인 퀘스트 없음";
    }

    public void TalkToNPC(Quest npcQuest)
    {
        if (npcQuest.state == QuestState.Completed)
        {
            Debug.Log("완료한 퀘스트입니다");
            return;
        }
        if (currentQuest == null || currentQuest.state == QuestState.Completed)
        {
            StartQuest(npcQuest);
            return;
        }
        if (currentQuest != npcQuest)
        {
            Debug.Log("다른 퀘스트 진행중");
            return;
        }
        if (currentQuest.state == QuestState.InProgress)
        {
            Debug.Log("아이템 부족");
        }
        else if (currentQuest.state == QuestState.ReadyToComplete)
        {
            CompleteQuest();
        }

    }

    private void StartQuest(Quest newQuest)
    {
        currentQuest = newQuest;
        currentQuest.currentAmount = 0;
        currentQuest.state = QuestState.InProgress;

        Debug.Log(currentQuest.questName + " 퀘스트 시작");
        UpdateQuestText();
    }

    public void AddQuestItem(string itemName, int amount)
    {
        if (currentQuest == null) return;
        if (currentQuest.state == QuestState.InProgress) return;
        if (currentQuest.targetItemName != itemName) return;

        currentQuest.currentAmount += amount;

        if (currentQuest.currentAmount >= currentQuest.targetAmount)
        {
            currentQuest.currentAmount = currentQuest.targetAmount;
            currentQuest.state = QuestState.ReadyToComplete;
        }

        UpdateQuestText();
    }

    public void CompleteQuest()
    {
        currentQuest.state = QuestState.Completed;
        Debug.Log("퀘스트 완료");
        UpdateQuestText();
    }

    private void UpdateQuestText()
    {
        if (currentQuest.state == QuestState.InProgress)
        {
            questText.text = currentQuest.questName + "\n" +
                currentQuest.questName + " " +
                currentQuest.currentAmount + " / " +
                currentQuest.targetAmount;
        }
        else if (currentQuest.state == QuestState.ReadyToComplete)
        {
            questText.text = "수집완료";
        }
        else if (currentQuest.state == QuestState.Completed)
        {
            questText.text = "퀘스트 완료";
        }
    }

}
