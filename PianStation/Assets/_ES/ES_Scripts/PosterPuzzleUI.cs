using UnityEngine;

public class PosterPuzzleUI : MonoBehaviour
{
    [Header("포스터 리스트")]
    public PostController[] posts;

    [Header("퍼즐 완료 후 활성화할 버튼")]
    public GameObject openAreaButton; 

    private int fallenPosterCount = 0;

    private void Start()
    {
        foreach (var post in posts)
        {
            post.OnPosterFallen += HandlePosterFallen;
        }

        if (openAreaButton != null)
            openAreaButton.SetActive(false); 
    }

    private void HandlePosterFallen(PostController post)
    {
        fallenPosterCount++;
        Debug.Log($"[{post.name}] 포스터가 떨어졌습니다. ({fallenPosterCount}/{posts.Length})");

        if (fallenPosterCount >= posts.Length)
        {
            Debug.Log("모든 포스터가 떨어짐 → 버튼 활성화");
            if (openAreaButton != null)
                openAreaButton.SetActive(true); 
        }
    }
}
