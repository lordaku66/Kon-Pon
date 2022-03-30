using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D basic;
    public Texture2D talk;
    public Texture2D inspect;
    public CursorMode cursorMode = CursorMode.Auto;
    public static Texture2D defaultCursor
    {
        get
        {
            return FindObjectOfType<CursorManager>().basic;
        }
    }
    public static Texture2D talkCursor
    {
        get
        {
            return FindObjectOfType<CursorManager>().talk;
        }
    }
    public static Texture2D inspectCursor
    {
        get
        {
            return FindObjectOfType<CursorManager>().inspect;
        }
    }
    public static void SetCursor(Texture2D cursor)
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }
}
