using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonRoomManager : MonoBehaviourPunCallbacks
{
    public static void EnterRoom(int num)
    {
        AtomManager.SceneType = "PRIVATE";
        AsyncOperation _async;
        _async = SceneManager.LoadSceneAsync("PrivateScene");
        PhotonNetwork.LoadLevel("PrivateScene");
        AtomManager.PrivateNumber = num;
    }
}