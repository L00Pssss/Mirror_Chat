using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace NeworkChat
{       
    [System.Serializable]
    public class UserData
    {
        public int Id;
        public string Nickname;

        public UserData(int id, string nickname)
        {
            Id = id;
            Nickname = nickname;
        }
    }

    public class User : NetworkBehaviour
    {
        public static User Local
        {
            get
            {
                var x = NetworkClient.localPlayer;

                if(x != null)
                    return x.GetComponent<User>();

                return null;
            }
        }
        

        private UserData data;
        private UIMessageInputField userInput;

        public static UnityAction<int, string, string> ReciveMessageToChat;


        public override void OnStopServer()
        {
            base.OnStopServer();
            UserList.Instance.SvRemoveCurrentUser(data.Id);
        }

        public UserData Data => data;

        private void Start()
        {
            userInput = UIMessageInputField.Instance;

            data = new UserData((int)netId, userInput.GetNickName());
        }

        private void Update()
        {
            if (isOwned == false) return;
            if (Input.GetKeyUp(KeyCode.Return))
            {
                SendMessageToChat();
            }
        }
        #region Join
        public void JoinToChat()
        {
            data.Nickname = userInput.GetNickName();

            CmdAddUser(data.Id, data.Nickname);
        }

        [Command]
        private void CmdAddUser(int id, string nickname)
        {
            UserList.Instance.SvAddCurrentUser(id, nickname);
        }

        [Command]
        private void CmdRemoveUser(int id)
        {
            UserList.Instance.SvRemoveCurrentUser(id);
        }
        #endregion

        #region Chat
        public void SendMessageToChat()
        {
            if (isOwned == false) return;

            if (userInput.IsEmpty == true) return;

            CmdSendMessageToChat(data.Id, userInput.GetString(), data.Nickname); 

            userInput.ClearString();
        }

        [Command]
        private void CmdSendMessageToChat(int userId, string message, string nickname)
        {
            Debug.Log($"user sent message to server. Message {message}");

            SvPostMessage(userId, message, nickname);
        }

        [Server]
        private void SvPostMessage(int userId, string message, string nickname)
        {
            Debug.Log($"Server received message by user. Message {message}");

            RpcReciveMessage(userId, message, nickname);
        }

        [ClientRpc]
        private void RpcReciveMessage(int userId, string message, string nickname)
        {
            Debug.Log($"User received message. Message: {message}");

            ReciveMessageToChat?.Invoke(userId, message, nickname);
        }

        #endregion
    }
}