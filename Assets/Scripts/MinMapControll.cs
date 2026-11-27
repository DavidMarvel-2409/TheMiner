
using UnityEngine;

public class MinMapControll : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private float limit = 5f;
    [SerializeField] private float smoothTime = 0.3f;

    private Vector2 pos_player = Vector2.zero;
    private float velocityX = 0f;

    void LateUpdate()
    {
        if (Player == null) return;

        pos_player = Player.transform.position;

        float dif = pos_player.x - transform.position.x;

        if (Mathf.Abs(dif) >= limit)
        {
            float targetX = pos_player.x -
                Mathf.Sign(dif) * limit;

            targetX = Mathf.Clamp(targetX, -25f, 25f);

            float smoothX = Mathf.SmoothDamp(
                transform.position.x,
                targetX,
                ref velocityX,
                smoothTime
            );

            transform.position = new Vector3(
                smoothX,
                transform.position.y,
                transform.position.z
            );
        }
    }
}
