using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace NeworkChat
{
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
        private UIMessageInputField messageInputField;

        public static UnityAction<int, string> ReciveMessageToChat;

        public UserData Data => data;

        private void Start()
        {
            messageInputField = UIMessageInputField.Instance;

            data = new UserData((int)netId, "Nickname");
        }

        private void Update()
        {
            if (isOwned == false) return;
            if (Input.GetKeyUp(KeyCode.Return))
            {
                SendMessageToChat();
            }
        }

        public void SendMessageToChat()
        {
            if (isOwned == false) return;

            if (messageInputField.IsEmpty == true) return;

            CmdSendMessageToChat(data.Id, messageInputField.GetString()); 

            messageInputField.ClearString();
        }

        [Command]
        private void CmdSendMessageToChat(int userId, string message)
        {
            Debug.Log($"user sent message to server. Message {message}");

            SvPostMessage(userId, message);
        }

        [Server]
        private void SvPostMessage(int userId, string message)
        {
            Debug.Log($"Server received message by user. Message {message}");

            RpcReciveMessage(userId, message);
        }

        [ClientRpc]
        private void RpcReciveMessage(int userId, string message)
        {
            Debug.Log($"User received message. Message: {message}");

            ReciveMessageToChat?.Invoke(userId, message);
        }
    }
}