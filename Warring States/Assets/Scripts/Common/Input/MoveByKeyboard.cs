using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByKeyboard : MonoBehaviour
{
    public float move_speed = 0.1f;

    [SerializeField]Dictionary<KeyCode, Vector3> keyToDirections = new Dictionary<KeyCode, Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        keyToDirections.Add(KeyCode.W, Vector3.forward);
        keyToDirections.Add(KeyCode.S, Vector3.back);
        keyToDirections.Add(KeyCode.A, Vector3.left);
        keyToDirections.Add(KeyCode.D, Vector3.right);
        keyToDirections.Add(KeyCode.Q, Vector3.up);
        keyToDirections.Add(KeyCode.E, Vector3.down);


    }

    // Update is called once per frame
    void Update()
    {
        foreach(KeyValuePair<KeyCode, Vector3> pair in keyToDirections)
        {
            if (Input.GetKey(pair.Key))
            {
                transform.position = transform.position + (pair.Value * move_speed);
            }
        }
        
    }
}
