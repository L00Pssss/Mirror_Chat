using UnityEngine;
using TMPro;
using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions.Must;

namespace NeworkChat
{
    public class UIMessageViewer : MonoBehaviour
    {
        [SerializeField] private UIMessageBox m_messageBox;

        [SerializeField] private Transform m_messagePanel;
        
        [SerializeField] private UIMessageBox m_userBox;
        
        [SerializeField] private Transform m_userListPanel;

        private void Start()
        {
            User.ReciveMessageToChat += OnReciveMessageToChat;
            UserList.UpdateUserList += OnUpdateUserList;
        }

        private void OnDestroy()
        {
            User.ReciveMessageToChat -= OnReciveMessageToChat;
            UserList.UpdateUserList -= OnUpdateUserList;
        }

        private void OnReciveMessageToChat(UserData data, string message)
        {
            AppendUserToUserList(data, message);
        }

        private void OnUpdateUserList(List<UserData> userList)
        {
            UpdateUserList(userList);
        }

        private void AppendUserToUserList(UserData data, string message)
        {
            UIMessageBox messageBox = Instantiate(m_messageBox);

            messageBox.SetText(data.Nickname + ": " + message);

            messageBox.transform.SetParent(m_messagePanel);
            messageBox.transform.localScale = Vector3.one;

            if (data.Id == User.Local.Data.Id)
            {
                messageBox.SetStyleBySelf();
            }
            else
            {
                messageBox.SetStyleBySender();
            }
        }

        private void UpdateUserList(List<UserData> userList)
        {
            foreach (Transform GetChild in m_userListPanel)
            {
                Destroy(GetChild.gameObject);
            }
            
            foreach (var userListPanel in userList)
            {
                UIMessageBox userBox = Instantiate(m_userBox);

                userBox.SetText(userListPanel.Nickname);

                userBox.transform.SetParent(m_userListPanel);
                userBox.transform.localScale = Vector3.one;
            }
        }
    }
}
