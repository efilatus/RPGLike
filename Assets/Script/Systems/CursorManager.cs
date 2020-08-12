using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    List<TheCursor> _cursors = new List<TheCursor>();

    void Start()
    {
        SetCursorTexture(_cursors[0].cursorTexture);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000))
        {
            for(int i = 0; i < _cursors.Count; i++)
            {
                if(hit.collider.tag == _cursors[i].tag)
                {
                    SetCursorTexture(_cursors[i].cursorTexture);
                    return;
                }
            }
        }
        SetCursorTexture(_cursors[0].cursorTexture);
    }

    void SetCursorTexture(Texture2D tex)
    {
        Cursor.SetCursor(tex, Vector2.zero, CursorMode.Auto);
    }
}
