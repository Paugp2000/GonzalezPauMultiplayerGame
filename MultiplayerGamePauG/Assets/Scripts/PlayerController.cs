using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    public float speed = 5f;
    private void Update()
    {
        if (!IsOwner) return;

        float h = Keyboard.current.aKey.IsPressed() ? -1 : 0 + (Keyboard.current.dKey.IsPressed() ? 1 : 0);
        float v = Keyboard.current.sKey.IsPressed() ? -1 : 0 + (Keyboard.current.wKey.IsPressed() ? 1 : 0);
        
        Vector3 move = new Vector3 (h, 0 ,v);

        transform.Translate(move * speed * Time.deltaTime);
    }
}
