using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PhotonManager : MonoBehaviourPunCallbacks
{
     // 버전 입력
    private readonly string version = "1.0f";
    
    // 사용자 아이디 입력
    private string userID = "Junsu";

    void Awake()
    {
        // 같은 룸의 유저들에게 자동으로 씬을 로딩
        PhotonNetwork.AutomaticallySyncScene = true;
        
        // 같은 버전의 유저끼리 접속 허용
        PhotonNetwork.GameVersion = version;
        
        // 유저 아이디 할당
        PhotonNetwork.NickName = userID;
        
        //포톤 서버와 통신 횟수 설정. 초당 30회
        Debug.Log(PhotonNetwork.SendRate);
        // 서버 접속
        PhotonNetwork.ConnectUsingSettings();
    }
    
    // 포톤 서버에 접속 후 호출되는 콜백 함수
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master!");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        AtomManager.SceneType = "MAIN";
        PhotonNetwork.JoinLobby();
    }
    
    // 로비에 접속 후 호출되는 콜백 함수
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        if (AtomManager.SceneType == "MAIN")
        {
            PhotonNetwork.JoinRoom("MainRoom");
        }
        else
        {
            CreateMyRoom();
        }
    }
    
    
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom("MainRoom", new RoomOptions { MaxPlayers = 20 }, null);
    }

    // 룸 생성이 완료된 후 호출되는 콜백 함수
    public override void OnCreatedRoom()
    {
        Debug.Log("Create Room");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("LEFTED");
        PhotonNetwork.JoinLobby();
    }

    // 룸에 입장한 후 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        if (AtomManager.SceneType == "MAIN")
        {
            int i = 0;
            string[] names = new string[10];
        
            foreach (var player in PhotonNetwork.CurrentRoom.Players)
            {
                Debug.Log($"{player.Value.NickName},{player.Value.ActorNumber}");

                names[i] = player.Value.NickName;
                i++;
            }

            AtomManager.RoomNames = names;
        
            PhotonNetwork.LoadLevel("MainScene");
        }
        else
        {
            AsyncOperation _async;
            _async = SceneManager.LoadSceneAsync("PrivateScene");
            PhotonNetwork.LoadLevel("PrivateScene");
        }
    }

    public void CreateMyRoom()
    {
        // 룸의 속성 정의
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 5;     //최대 접속자 수 : 20명
        ro.IsOpen = true;       // 룸의 오픈 여부
        ro.IsVisible = true;    // 로비에서 룸 목록에 노출 시킬지 여부
        PhotonNetwork.JoinOrCreateRoom($"{userID} Private Room", ro, null);
    }
}
