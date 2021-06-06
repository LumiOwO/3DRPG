using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class MouseManager : MonoBehaviour
{
    public static MouseManager instance;

    public Texture2D cursor_point;
    public Texture2D cursor_doorway;
    public Texture2D cursor_attack;
    public Texture2D cursor_target;
    public Texture2D cursor_arrow;


    public event Action<Vector3> OnMouseClicked;
    public event Action<GameObject> OnEnemyClicked;

    private RaycastHit _hitInfo;

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        }
        instance = this;
    }

    void Update() {
        SetCursorTexture();
        MouseControl();
    }

    void SetCursorTexture() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out _hitInfo)) {
            // update cursor texture
            switch(_hitInfo.collider.gameObject.tag) {
                case "Ground":
                    Cursor.SetCursor(cursor_target, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(cursor_attack, new Vector2(16, 16), CursorMode.Auto);
                    break;


            }
        }
    }

    void MouseControl() {
        if (Input.GetMouseButtonDown(0) && _hitInfo.collider != null) {
            // left button
            if (_hitInfo.collider.gameObject.CompareTag("Ground")) {
                OnMouseClicked?.Invoke(_hitInfo.point);
            }
            if (_hitInfo.collider.gameObject.CompareTag("Enemy")) {
                OnEnemyClicked?.Invoke(_hitInfo.collider.gameObject);
            }
        }
    }
}
