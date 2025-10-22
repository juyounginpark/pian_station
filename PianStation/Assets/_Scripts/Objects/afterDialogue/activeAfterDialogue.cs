using UnityEngine;

public class ChangeTagAfterDialogue : MonoBehaviour, IPostDialogueAction
{
    [Header("대상 오브젝트 설정")]
    [Tooltip("대화가 끝난 후 'interact' 태그를 부여할 오브젝트 목록입니다.")]
    [SerializeField] private GameObject[] objectsToChangeTag;

    private bool hasActionBeenPerformed = false;

    public void OnDialogueEnd()
    {
        if (!hasActionBeenPerformed)
        {
            hasActionBeenPerformed = true;

            if (objectsToChangeTag != null && objectsToChangeTag.Length > 0)
            {
                foreach (GameObject obj in objectsToChangeTag)
                {
                    if (obj != null)
                    {
                        obj.tag = "interact";
                        Debug.Log($"'{obj.name}' 오브젝트의 태그를 'interact'로 변경했습니다.");
                    }
                }
            }
        }
    }
}