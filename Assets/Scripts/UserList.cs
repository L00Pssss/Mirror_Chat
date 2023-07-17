using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace NeworkChat
{
    public class UserList : NetworkBehaviour
    {
        public static UserList Instance;
        private void Awake()
        {
            Instance = this;
        }

        [SerializeField] private List<UserData> AllUsersData = new List<UserData>();

        public static UnityAction<List<UserData>> UpdateUserList;

        public override void OnStartClient()
        {
            base.OnStartClient();
            // Не нужно очищать список при старте клиента.
            // Вместо этого, он будет синхронизирован с сервером.
        }

        [Server]
        public void SvAddCurrentUser(UserData data)
        {
            AllUsersData.Add(data);
            RpcUpdateUserList(AllUsersData);
        }

        [Server]
        public void SvRemoveCurrentUser(UserData data)
        {
            AllUsersData.Remove(data);
            RpcUpdateUserList(AllUsersData);
        }

        [ClientRpc]
        private void RpcUpdateUserList(List<UserData> userList)
        {
            // Обновляем список только на клиенте, к которому применяется это RPC.
            UpdateUserList?.Invoke(userList);
        }
    }
}
