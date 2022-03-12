using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class GameMain : MonoBehaviour
{
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate(this.prefab.name, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}