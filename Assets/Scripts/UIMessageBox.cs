using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NeworkChat
{
    public class UIMessageBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_text;
        [SerializeField] private Image m_backgroundImage;
        [SerializeField] Color m_backgroundColorForSelf;
        [SerializeField] Color m_backgroundColorForSender;

        public void SetText(string text)
        {
            m_text.text = text;
        }

        public void SetStyleBySelf()
        {
            m_backgroundImage.color = m_backgroundColorForSelf;
            m_text.alignment = TextAlignmentOptions.MidlineLeft;
        }

        public void SetStyleBySender()
        {
            m_backgroundImage.color = m_backgroundColorForSender;
            m_text.alignment = TextAlignmentOptions.MidlineRight;
        }
    }
}