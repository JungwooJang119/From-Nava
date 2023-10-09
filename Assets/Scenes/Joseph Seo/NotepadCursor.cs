using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NotepadCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Texture2D cursorTexture;

    public void OnPointerEnter(PointerEventData eventData) {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
