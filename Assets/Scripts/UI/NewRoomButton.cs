using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO> THIS IS AN ANSWER CLASS
public class NewRoomButton : MonoBehaviour {
    [SerializeField] private GameObject room;

    void Start() {
        GetComponent<Button>().onClick.AddListener(Clicked);
    }

    private void Clicked() {
        string s = Serializer.Main.Serialize();
        GUIUtility.systemCopyBuffer = s;
        Debug.Log("Copied previous room to clipboard!");

        Serializer.Main.ClearRoom();
        Debug.Log("Cleared Room!");
        room.transform.rotation = Quaternion.identity;
    }
}
