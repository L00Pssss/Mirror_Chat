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
        public void SvAddCurrentUser(UserData data)
        {
            AllUsersData.Add(data);

            if (isServerOnly == true)
            {
                RpcClearUserDataList();
            }

            foreach (UserData userdata in AllUsersData)
            {
                RpcAddCurrentUser(data);
            }
        }

        [Server]
        public void SvRemoveCurrentUser(UserData data)
        {
            List<UserData> usersToRemove = new List<UserData>();

            foreach (UserData userData in AllUsersData)
            {
                if (userData.Id == data.Id)
                {
                    usersToRemove.Add(userData);
                    break;
                }
            }

            foreach (UserData userToRemove in usersToRemove)
            {
                AllUsersData.Remove(userToRemove);
            }

            RpcRemoveCurrentUser(data);
        }

        [ClientRpc]
        private void RpcClearUserDataList()
        {
		    AllUsersData.Clear();
        }

        [ClientRpc]
        private void RpcAddCurrentUser(UserData data)
        {
            if (isClient == true && isServer == true)
            {
                UpdateUserList?.Invoke(AllUsersData);
                return;
            }
            AllUsersData.Add(data);

            UpdateUserList?.Invoke(AllUsersData);
        }

        [ClientRpc]
        private void RpcRemoveCurrentUser(UserData data)
        {
            List<UserData> usersToRemove = new List<UserData>();

            foreach (UserData userData in AllUsersData)
            {
                if (userData.Id == data.Id)
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