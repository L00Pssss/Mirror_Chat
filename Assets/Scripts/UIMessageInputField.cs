using UnityEngine;
using TMPro;
using Mirror;

namespace NeworkChat
{
    public class UIMessageInputField : MonoBehaviour
    {
        public static UIMessageInputField Instance;

        [SerializeField] private TMP_InputField m_inputField;

        public bool IsEmpty => m_inputField.text == "";


        private void Awake()
        {
            Instance = this;
        }

        public string GetString()
        {
            return m_inputField.text;
        }

        public void ClearString()
        {
            m_inputField.text = "";
        }

        public void SendMessageToChat()
        {
            User.Local.SendMessageToChat();
        }



    }
}