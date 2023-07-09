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
	    
	    [SerializeField] private List<UserData> AllUsersData = new List<UserData>(); // изменить на приват. 
        public static UnityAction<List<UserData>> UpdateUserList;
        public override void OnStartClient()
        {
            base.OnStartClient();

            AllUsersData.Clear();
        }

        [Server]
        public void SvAddCurrentUser(int userId, string userNickname)
        {
		    UserData userData = new UserData(userId, userNickname);
            AllUsersData.Add(userData);

            if (isServerOnly == true)
            {
                RpcClearUserDataList();
            }

            foreach (UserData userdata in AllUsersData)
            {
                RpcAddCurrentUser(userdata.Id, userdata.Nickname);
            }
        }

        [Server]
        public void SvRemoveCurrentUser(int userId)
        {
            List<UserData> usersToRemove = new List<UserData>();

            foreach (UserData userData in AllUsersData)
            {
                if (userData.Id == userId)
                {
                    usersToRemove.Add(userData);
                    break;
                }
            }

            foreach (UserData userToRemove in usersToRemove)
            {
                AllUsersData.Remove(userToRemove);
            }

            RpcRemoveCurrentUser(userId);
        }

        [ClientRpc]
        private void RpcClearUserDataList()
        {
		    AllUsersData.Clear();
        }

        [ClientRpc]
        private void RpcAddCurrentUser(int userId, string userNickname)
        {
            if (isClient == true && isServer == true)
            {
                UpdateUserList?.Invoke(AllUsersData);
                return;
            }

            UserData userData = new UserData(userId, userNickname);
            AllUsersData.Add(userData);

            UpdateUserList?.Invoke(AllUsersData);
        }

        [ClientRpc]
        private void RpcRemoveCurrentUser(int userId)
        {
            List<UserData> usersToRemove = new List<UserData>();

            foreach (UserData userData in AllUsersData)
            {
                if (userData.Id == userId)
                {
                    usersToRemove.Add(userData);
                    break;
                }
            }

            foreach (UserData userToRemove in usersToRemove)
            {
                AllUsersData.Remove(userToRemove);
            }

            UpdateUserList?.Invoke(AllUsersData);
        }
    }
}