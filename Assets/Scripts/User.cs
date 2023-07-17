using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace NeworkChat
{       

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

        public static UnityAction<UserData, string> ReciveMessageToChat;


        public override void OnStopServer()
        {
            base.OnStopServer();
            UserList.Instance.SvRemoveCurrentUser(data);
        }

        public UserData Data => data;

        private void Start()
        {
            data = new UserData((int)netIdentity.netId, "NickName " + ((int)netIdentity.netId).ToString());

            userInput = UIMessageInputField.Instance;
        }

        private void Update()
        {
            if (!isOwned) return;
            if (Input.GetKeyUp(KeyCode.Return))
            {
                SendMessageToChat();
            }
        }
        #region Join
        public void JoinToChat()
        {
            data.Nickname = userInput.GetNickName();

            CmdAddUser(data);
        }

        [Command]
        private void CmdAddUser(UserData data)
        {
            UserList.Instance.SvAddCurrentUser(data);
        }

        [Command]
        private void CmdRemoveUser(UserData data)
        {
            UserList.Instance.SvRemoveCurrentUser(data);
        }
        #endregion

        #region Chat
        public void SendMessageToChat()
        {
            if (!isOwned) return;

            if (userInput.IsEmpty == true) return;

            CmdSendMessageToChat(data, userInput.GetString()); 

            userInput.ClearString();
        }

        [Command]
        private void CmdSendMessageToChat(UserData data, string message)
        {
            Debug.Log($"user sent message to server. Message {message}");

            SvPostMessage(data, message);
        }

        [Server]
        private void SvPostMessage(UserData data, string message)
        {
            Debug.Log($"Server received message by user. Message {message}");

            RpcReciveMessage(data, message);
        }

        [ClientRpc]
        private void RpcReciveMessage(UserData data, string message)
        {
            Debug.Log($"User received message. Message: {message}");

            ReciveMessageToChat?.Invoke(data, message);
        }

        #endregion
    }
}