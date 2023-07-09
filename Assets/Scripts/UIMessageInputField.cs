using UnityEngine;
using TMPro;
using Mirror;
using UnityEngine.Serialization;

namespace NeworkChat
{
    public class UIMessageInputField : MonoBehaviour
    {
        public static UIMessageInputField Instance;
        
        [SerializeField] private TMP_InputField m_MessageInputField;

        [SerializeField] private TMP_InputField m_NickNameField;

        public bool IsEmpty => m_MessageInputField.text == "";


        private void Awake()
        {
            Instance = this;
        }

        public string GetString()
        {
            return m_MessageInputField.text;
        }

        public void ClearString()
        {
            m_MessageInputField.text = "";
        }

        public string GetNickName()
        {
            return m_NickNameField.text;
        }

        public void SendMessageToChat()
        {
            User.Local.SendMessageToChat();
        }

        public void JoinToChat()
        {
            User.Local.JoinToChat();
        }

    }
}