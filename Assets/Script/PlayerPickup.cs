using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerPickup : MonoBehaviour
{
    [Header("직접 연결")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;
    [Header("아이템 탐색")]
    [SerializeField] private float detectionRadius = 2f;
    [Header("줍기 모션")]
    [SerializeField] private float pickupDuration = 1.2f;
    [SerializeField] private float pickupMoment = 0.55f;
    private PickupItem targetItem;
    private bool isPickingUp;

    void Start()
    {
        
    }

    void Update()
    {
        if (isPickingUp) return;

        targetItem = FindClosestItem(); //가장 가까운 아이템 찾기
        if (targetItem == null) return;

        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.eKey.wasPressedThisFrame) //아이템이 있다면 E키로 줍기
        {
            StartCoroutine(PickupAnimation());
        }
    }

    private PickupItem FindClosestItem()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, ~0, QueryTriggerInteraction.Collide);
        PickupItem closestItem = null;
        float closestDistance = Mathf.Infinity;

        foreach(Collider itemCollider in colliders)
        {
            PickupItem item = itemCollider.GetComponentInParent<PickupItem>();
            
            if (item == null ) continue;

            float distance = Vector3.Distance(transform.position, item.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestItem = item;
            }
        }

        return closestItem;
    }

    private IEnumerator PickupAnimation()
    {
        isPickingUp = true;
        //현재 찾은 아이템 저장
        PickupItem itemToCollect = targetItem;
        //아이템 방향으로 캐릭터를 돌림
        Vector3 itemDirection = itemToCollect.transform.position - transform.position;
        itemDirection.y = 0f;

        if(itemDirection.sqrMagnitude > 0.001)
        {
            transform.rotation = Quaternion.LookRotation(itemDirection);
        }

        playerController.ChangeState(PlayerState.PickUp); //줍기 상태로 변경
        animator.CrossFade("PickUp", 0.1f); //전환선없이 pickUp01 상태로 이동
        yield return new WaitForSeconds(pickupMoment); //손이 아이템에 닿는 시점까지 기다린다

        if (itemToCollect) //아이템을 실제로 획득
        {
            itemToCollect.Collect();
        }
        yield return new WaitForSeconds(pickupDuration - pickupMoment); //애니메이션 시간 대기

        animator.CrossFade("Blend Tree", 0.1f); //이동 블랜드 트리로 돌아가기
        playerController.ChangeState(PlayerState.Normal); //이동 허용

        targetItem = null;
        isPickingUp = false;

    }

}
