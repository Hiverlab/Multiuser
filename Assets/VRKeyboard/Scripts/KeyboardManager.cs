/***
 * Author: Yunhan Li
 * Any issue please contact yunhn.lee@gmail.com
 ***/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;

namespace VRKeyboard.Utils
{
    public class KeyboardManager : MonoBehaviour
    {

		public static KeyboardManager instance;

		#region Public Variables
		[Header("Spawn Properties")]
		public Vector3 spawnOffsetToInputField = new Vector3(0,-40,-10);

        [Header("User defined")]
        [Tooltip("If the character is uppercase at the initialization")]
        public bool isUppercase = false;
        public int maxInputLength;

        [Header("UI Elements")]
        public Text inputText;
		public string inputString;
		public TMP_InputField inputField;
		public Button linkedButtonfromEnter;

        [Header("Essentials")]
        public Transform keys;
        #endregion

        private string originalPlaceholder;

        #region Private Variables
        private string Input
        {
            //get { return inputText.text; }
            //set { inputText.text = value; }

			get { return inputField.text; }
			set { inputField.text = value; }
		}
        private Key[] keyList;
        private bool capslockFlag;
        #endregion

        #region Monobehaviour Callbacks
        void Awake() {
            if (!instance) {
                instance = this;
            } else {
                Destroy(instance);
            }

            keyList = keys.GetComponentsInChildren<Key>();
            
            gameObject.SetActive(false);
        }

        void Start()
        {
            foreach (var key in keyList)
            {
                key.OnKeyClicked += GenerateInput;
            }
            capslockFlag = isUppercase;
            CapsLock();
        }

		private void Update()
		{
			// TODO modify based on VR platform================================================================
			//switch (VRInputController.Instance.vrInputMethod)
			//{
			//	case (VRInputController.VRInputMethod.Gaze):
			//		if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad) || UnityEngine.Input.GetMouseButtonDown(0))
			//		{
						

			//			// condition whether to show/unshow keyboard
			//			if (EventSystem.current.currentSelectedGameObject)
			//			{
			//				if (!EventSystem.current.currentSelectedGameObject.CompareTag("Keyboard"))
			//				{
			//					linkedButtonfromEnter = null;
			//					inputField = null;
			//					this.gameObject.SetActive(false);
			//				}												
			//			}
			//			else
			//			{
			//				linkedButtonfromEnter = null;
			//				inputField = null;
			//				this.gameObject.SetActive(false);
			//			}

			//		}
			//		break;

			//	case (VRInputController.VRInputMethod.LTrackedRemote):
			//		if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || UnityEngine.Input.GetMouseButtonDown(0))
			//		{


			//			// condition whether to show/unshow keyboard
			//			if (EventSystem.current.currentSelectedGameObject)
			//			{
			//				if (!EventSystem.current.currentSelectedGameObject.CompareTag("Keyboard"))
			//				{
			//					linkedButtonfromEnter = null;
			//					inputField = null;
			//					this.gameObject.SetActive(false);
			//				}
			//			}
			//			else
			//			{
			//				linkedButtonfromEnter = null;
			//				inputField = null;
			//				this.gameObject.SetActive(false);
			//			}

			//		}
			//		break;

			//	case (VRInputController.VRInputMethod.RTrackedRemote):
			//		if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || UnityEngine.Input.GetMouseButtonDown(0))
			//		{


			//			// condition whether to show/unshow keyboard
			//			if (EventSystem.current.currentSelectedGameObject)
			//			{
			//				if (!EventSystem.current.currentSelectedGameObject.CompareTag("Keyboard"))
			//				{
			//					linkedButtonfromEnter = null;
			//					inputField = null;
			//					this.gameObject.SetActive(false);
			//				}
			//			}
			//			else
			//			{
			//				linkedButtonfromEnter = null;
			//				inputField = null;
			//				this.gameObject.SetActive(false);
			//			}

			//		}
			//		break;
			//}
			// ====================================================================================================
		}
		#endregion

		#region Public Methods
		public void LinkKeyboardToInputField(TMP_InputField _inputField)
		{
            Debug.Log("Linking to: " + _inputField.name);

            // If previous input field is not null, then reset it
            if (inputField != null) {
                ResetPlaceholder();
                inputField.text = "";
                inputText.text = "";
            }

            gameObject.SetActive(true);
			inputField = _inputField;

            originalPlaceholder = inputField.placeholder.GetComponent<TextMeshProUGUI>().text;
            inputField.placeholder.GetComponent<TextMeshProUGUI>().text = "|";
		}

        private void ResetPlaceholder() {
            inputField.placeholder.GetComponent<TextMeshProUGUI>().text = originalPlaceholder;
        }

        public void HideKeyboard() {
            if (inputField != null) {
                inputField.text = "";
                ResetPlaceholder();
            }
            inputText.text = "";
            inputField = null;
            gameObject.SetActive(false);
        }

		public void LinkEnterToButton(Button button)
		{
			linkedButtonfromEnter = button;
		}

		public void Enter()
		{
			if (linkedButtonfromEnter != null)
			{
				linkedButtonfromEnter.onClick.Invoke();
				linkedButtonfromEnter = null;
			}

			this.gameObject.SetActive(false);
		}

        public void Backspace()
        {
            if (Input.Length > 0)
            {
                Input = Input.Remove(Input.Length - 1);
            }
            else
            {
                return;
            }
        }

        public void Clear()
        {
            Input = "";
        }

        public void CapsLock()
        {
            foreach (var key in keyList)
            {
                if (key is Alphabet)
                {
                    key.CapsLock(capslockFlag);
                }
            }
            capslockFlag = !capslockFlag;
        }

        public void Shift()
        {
            foreach (var key in keyList)
            {
                if (key is Shift)
                {
                    key.ShiftKey();
                }
            }
        }

        public void GenerateInput(string s)
        {
            if (Input.Length > maxInputLength) { return; }
            Input += s;

            if (Input.Length == 0) {
                ResetPlaceholder();
            }
        }


        #endregion
    }
}