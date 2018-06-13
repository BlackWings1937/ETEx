using System;
using System.Net;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
namespace ETHotfix
{
	[ObjectSystem]
	public class UiLoginComponentSystem : AwakeSystem<UILoginComponent>
	{
		public override void Awake(UILoginComponent self)
		{
			self.Awake();
		}
	}
	
	public class UILoginComponent: Component
	{
		private GameObject account;
        private GameObject password;
		private GameObject loginBtn;
        private GameObject registerBtn;
       // private Button btn;

        //----------------------生命周期方法----------------------------
        public void Awake()
		{
			ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			loginBtn = rc.Get<GameObject>("LoginBtn");
			loginBtn.GetComponent<Button>().onClick.Add(OnLogin);
            registerBtn = rc.Get<GameObject>("RegisterBtn");
            registerBtn.GetComponent<Button>().onClick.Add(OnRegister);

            this.account = rc.Get<GameObject>("Account");
            this.password = rc.Get<GameObject>("Password");
		}
        //-----------------------私有方法---------------------------------
        private void onAlertCbLoginFail() { }
        //-----------------------UI事件-----------------------------------
        public void OnRegister() {
            Game.Scene.GetComponent<UIComponent>().Create(UIType.UIRegister);
            Game.Scene.GetComponent<UIComponent>().Remove(UIType.UILogin);
        }

        public async void OnLogin()
		{
			try
			{
                //
                IPEndPoint connetEndPoint = NetworkHelper.ToIPEndPoint(GlobalConfigComponent.Instance.GlobalProto.Address);


				// 创建一个ETModel层的Session
				ETModel.Session session = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(connetEndPoint);
				Session realmSession = ComponentFactory.Create<Session, ETModel.Session>(session);

                /*
                 * 请求登录
                 */
                R2C_Login r2CLogin = (R2C_Login) await realmSession.Call(new C2R_Login()
                    {Account = this.account.GetComponent<InputField>().text,
                    Password = this.password.GetComponent<InputField>().text}
                );
                realmSession.Dispose();

                /*
                 * 处理登录结果 
                 */
                if (r2CLogin.Error == ErrorCode.ERR_Success) {
                    connetEndPoint = NetworkHelper.ToIPEndPoint(r2CLogin.Address);
                    // 创建一个ETModel层的Session,并且保存到ETModel.SessionComponent中"111.230.133.20:10002"
                    ETModel.Session gateSession = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(connetEndPoint);
                    ETModel.Game.Scene.AddComponent<ETModel.SessionComponent>().Session = gateSession;

                    // 创建一个ETHotfix层的Session, 并且保存到ETHotfix.SessionComponent中
                    Game.Scene.AddComponent<SessionComponent>().Session = ComponentFactory.Create<Session, ETModel.Session>(gateSession);

                    G2C_LoginGate g2CLoginGate = (G2C_LoginGate)await SessionComponent.Instance.Session.Call(new C2G_LoginGate() { Key = r2CLogin.Key });

                    Log.Info("登陆gate成功!");

                    // 创建Player
                    Player player = ETModel.ComponentFactory.CreateWithId<Player>(g2CLoginGate.PlayerId);
                    PlayerComponent playerComponent = ETModel.Game.Scene.GetComponent<PlayerComponent>();
                    playerComponent.MyPlayer = player;

                    Game.Scene.GetComponent<UIComponent>().Create(UIType.UILobby);
                    Game.Scene.GetComponent<UIComponent>().Remove(UIType.UILogin);
                } else if (r2CLogin.Error == ErrorCode.ERR_AccountInvaild) {
                    Game.Scene.GetComponent<UIAlertsGrayComponent>().CreateAlertWin("账号中有不法字符~", onAlertCbLoginFail, "好的~");
                }
                else if (r2CLogin.Error == ErrorCode.ERR_PasswordInvaild) {
                    Game.Scene.GetComponent<UIAlertsGrayComponent>().CreateAlertWin("密码中有不法字符~", onAlertCbLoginFail, "好的~");
                }
                else if (r2CLogin.Error == ErrorCode.ERR_PasswordIncorrect) {
                    Game.Scene.GetComponent<UIAlertsGrayComponent>().CreateAlertWin("密码不正确~", onAlertCbLoginFail, "好的~");
                }
                else if (r2CLogin.Error == ErrorCode.ERR_AccountNotExist) {
                    UIAlertsGrayComponent l = Game.Scene.GetComponent<UIAlertsGrayComponent>();
                    Game.Scene.GetComponent<UIAlertsGrayComponent>().CreateAlertWin("账号不存在~", onAlertCbLoginFail, "好的~");
                }
                else if (r2CLogin.Error == ErrorCode.ERR_DataBaseRead) {
                    Game.Scene.GetComponent<UIAlertsGrayComponent>().CreateAlertWin("网络异常~", onAlertCbLoginFail, "好的~");
                }




            }
			catch (Exception e)
			{
				Log.Error(e);
			}
		}
	}
}
