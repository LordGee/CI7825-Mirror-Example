using Mirror;
using UnityEngine;

public class PlayerControl : NetworkBehaviour
{
    public override void OnStartLocalPlayer() {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = Vector3.zero;
    }

    void Update() {
        if (!isLocalPlayer) { return; }
        float moveH = Input.GetAxis("Horizontal") * Time.deltaTime * 110.0f;
        float moveV = Input.GetAxis("Vertical") * Time.deltaTime * 4f;
        transform.Rotate(0, moveH, 0);
        transform.Translate(0, 0, moveV);
    }
}

