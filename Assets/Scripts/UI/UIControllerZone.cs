using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class UIControllerZone : MonoBehaviour,  IPointerMoveHandler
{
    public static event Action<Vector2> OnPlayingZoneClicked;
    private Vector3[] playingZoneCorners = new Vector3[4];

    private Vector2 targetPosition;
    public void Awake()
    {
        GetComponent<RectTransform>().GetWorldCorners(playingZoneCorners);
    }
    private void GetClickPosition()
    {
        if (Input.mousePosition != null)
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        //targetPosition.y = Mathf.Clamp(targetPosition.y, playingZoneCorners[0].y, playingZoneCorners[2].y);

        if(targetPosition.y > playingZoneCorners[0].y && targetPosition.y < playingZoneCorners[2].y)
        {

            OnPlayingZoneClicked?.Invoke(targetPosition);
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        GetClickPosition();
    }
}
