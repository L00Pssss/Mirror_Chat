using UnityEngine;
using TMPro;
using Mirror;
using System;
using System.Collections.Generic;

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

        private void OnReciveMessageToChat(int userId, string message, string nickname)
        {
            AppendMessage(userId, message, nickname);
        }

        private void AppendMessage(int userId, string message, string nickname)
        {
            UIMessageBox messageBox = Instantiate(m_messageBox);

            messageBox.SetText(nickname + ": " + message);

            messageBox.transform.SetParent(m_messagePanel);
            messageBox.transform.localScale = Vector3.one;

            if (userId == User.Local.Data.Id)
            {
                messageBox.SetStyleBySelf();
            }
            else
            {
                messageBox.SetStyleBySender();
            }
        }

        private void OnUpdateUserList(List<UserData> userList)
        {
            foreach (Transform childTransform in m_userListPanel)
            {
                Destroy(childTransform.gameObject);
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
