using System;
using System.Net;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
namespace ETHotfix
{
	[ObjectSystem]
	public class UiRegisterComponentSystem : AwakeSystem<UIRegisterComponent>
	{
		public override void Awake(UIRegisterComponent self)
		{
			self.Awake();
		}
	}
	
	public class UIRegisterComponent: Component
	{
        private GameObject btnRegister_;
        private GameObject btnBack_;
        private GameObject inputFieldCount_;
        private GameObject inputFieldPassword_;
        private GameObject inputFieldPasswordRepeat_;
		public void Awake()
		{
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            btnRegister_ = rc.Get<GameObject>("BtnRegister");
            btnBack_ = rc.Get<GameObject>("BtnCancle");
            inputFieldCount_ = rc.Get<GameObject>("InputFieldCount");
            inputFieldPassword_ = rc.Get<GameObject>("InputFieldPassword");
            inputFieldPasswordRepeat_ = rc.Get<GameObject>("InputFieldPasswordRepeat");

            btnRegister_.GetComponent<Button>().onClick.AddListener(OnRegister);
            btnBack_.GetComponent<Button>().onClick.AddListener(OnBack);
		}

        private async void register(string count,string pw) {
            IPEndPoint connetEndPoint = NetworkHelper.ToIPEndPoint(GlobalConfigComponent.Instance.GlobalProto.Address);
            ETModel.Session session = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(connetEndPoint);
            Session realmSession = ComponentFactory.Create<Session, ETModel.Session>(session);
            R2C_Register r2cRegister = (R2C_Register)await realmSession.Call(new C2R_Register() { Account = count,Password = pw });
            if (r2cRegister.Error == ErrorCode.ERR_Success) {
                Debug.Log("注册成功");
            } else if (r2cRegister.Error == ErrorCode.ERR_AccountInvaild) {
                Debug.Log("账号不和法");
            } else if (r2cRegister.Error == ErrorCode.ERR_PasswordInvaild) {
                Debug.Log("密码不和法");
            } else if (r2cRegister.Error == ErrorCode.ERR_DataBaseWriteDown) {
                Debug.Log("网络异常");
            }
            realmSession.Dispose();
        }

        public void OnRegister() {
            if (inputFieldCount_.GetComponent<InputField>().text == "") {
                Debug.Log("账户名不能为空");
                return;
            }
            if (inputFieldPassword_.GetComponent<InputField>().text == "")
            {
                Debug.Log("密码不能为空");
                return;
            }
            if (inputFieldPasswordRepeat_.GetComponent<InputField>().text == "") {
                Debug.Log("请再填写一次密码");
                return;
            }
            if (inputFieldPasswordRepeat_.GetComponent<InputField>().text != inputFieldPassword_.GetComponent<InputField>().text)
            {
                Debug.Log("两次输入的密码不同");
                return;
            }
            register(inputFieldCount_.GetComponent<InputField>().text,inputFieldPassword_.GetComponent<InputField>().text);
        }

        public void OnBack() {
            Game.Scene.GetComponent<UIComponent>().Create(UIType.UILogin);
            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UIRegister);
        }

	}
}
