using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance;

    public Texture2D cursor_point;
    public Texture2D cursor_doorway;
    public Texture2D cursor_attack;
    public Texture2D cursor_target;
    public Texture2D cursor_arrow;


    public event Action<Vector3> OnMouseClicked;

    private RaycastHit hitInfo;

    void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
        }
        Instance = this;
    }

    void Update() {
        SetCursorTexture();
        MouseControl();
    }

    void SetCursorTexture() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo)) {
            // update cursor texture
            switch(hitInfo.collider.gameObject.tag) {
                case "Ground":
                    Cursor.SetCursor(cursor_target, new Vector2(16, 16), CursorMode.Auto);
                    break;


            }
        }
    }

    void MouseControl() {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null) {
            // left button
            if (hitInfo.collider.gameObject.CompareTag("Ground")) {
                OnMouseClicked?.Invoke(hitInfo.point);
            }
        }
    }
}
