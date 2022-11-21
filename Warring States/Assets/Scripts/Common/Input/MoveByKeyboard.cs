using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByKeyboard : MonoBehaviour
{
    public float move_speed = 0.1f;
    Vector3 originalPosition;
    Vector3 originalEulerAngles;

    [SerializeField]Dictionary<KeyCode, Vector3> keyToDirections = new Dictionary<KeyCode, Vector3>();
    [SerializeField] Dictionary<KeyCode, Vector3> keyToRotations = new Dictionary<KeyCode, Vector3>();
    [SerializeField] KeyCode resetKey = KeyCode.X;

    // Start is called before the first frame update
    void Start()
    {
        originalEulerAngles = transform.eulerAngles;
        originalPosition = transform.position;

        keyToDirections.Add(KeyCode.W, Vector3.forward);
        keyToDirections.Add(KeyCode.S, Vector3.back);
        keyToDirections.Add(KeyCode.A, Vector3.left);
        keyToDirections.Add(KeyCode.D, Vector3.right);
        keyToDirections.Add(KeyCode.Q, Vector3.up);
        keyToDirections.Add(KeyCode.E, Vector3.down);

        keyToRotations.Add(KeyCode.Z, Vector3.right);
        keyToRotations.Add(KeyCode.C, Vector3.left);
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

        foreach (KeyValuePair<KeyCode, Vector3> pair in keyToRotations)
        {
            if (Input.GetKey(pair.Key))
            {
                transform.Rotate(pair.Value * move_speed);
            }
        }
        if (Input.GetKeyUp(resetKey))
        {
            transform.position = originalPosition;
            transform.eulerAngles = originalEulerAngles;
        }        
    }
}
