using UnityEngine;
using TMPro;
using Mirror;
using System;

namespace NeworkChat
{
    public class UIMessageViewer : MonoBehaviour
    {
        [SerializeField] private UIMessageBox m_messageBox;

        [SerializeField] private Transform m_messagePanel;

        private void Start()
        {
            User.ReciveMessageToChat += OnReciveMessageToChat;
        }

        private void OnDestroy()
        {
            User.ReciveMessageToChat -= OnReciveMessageToChat;

        }

        private void OnReciveMessageToChat(int userId, string message)
        {
            AppendMessage(userId, message);
        }

        private void AppendMessage(int userId, string message)
        {
            UIMessageBox messageBox = Instantiate(m_messageBox);

            messageBox.SetText(userId + ": " + message);

            messageBox.transform.SetParent(m_messagePanel);

            if (userId == User.Local.Data.Id)
            {
                messageBox.SetStyleBySelf();
            }
            else
            {
                messageBox.SetStyleBySender();
            }
        }
    }
}
