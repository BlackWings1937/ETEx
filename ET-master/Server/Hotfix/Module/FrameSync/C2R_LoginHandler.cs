using System;
using System.Net;
using ETModel;
using MongoDB.Bson;

namespace ETHotfix
{
	[MessageHandler(AppType.Realm)]
	public class C2R_LoginHandler : AMRpcHandler<C2R_Login, R2C_Login>
	{
		protected override async void Run(Session session, C2R_Login message, Action<R2C_Login> reply)
		{
			R2C_Login response = new R2C_Login();
			try
			{
                Console.WriteLine("userLogin:"+message.Account+"key:"+message.Password);
                /*
                 * 账户密码验证
                 */
                int result = Game.Scene.GetComponent<MySqlComponent>().CheckPassWord(message.Account, message.Password);
                if (result == ErrorCode.ERR_Success) {
                    // 随机分配一个Gate
                    StartConfig config = Game.Scene.GetComponent<RealmGateAddressComponent>().GetAddress();
                    //Log.Debug($"gate address: {MongoHelper.ToJson(config)}");
                    IPEndPoint innerAddress = config.GetComponent<InnerConfig>().IPEndPoint;
                    Console.WriteLine("userLogin:" + innerAddress);
                    Session gateSession = Game.Scene.GetComponent<NetInnerComponent>().Get(innerAddress);

                    // 向gate请求一个key,客户端可以拿着这个key连接gate
                    G2R_GetLoginKey g2RGetLoginKey = (G2R_GetLoginKey)await gateSession.Call(new R2G_GetLoginKey() { Account = message.Account });

                    string outerAddress = config.GetComponent<OuterConfig>().IPEndPoint2.ToString();

                    response.Address = outerAddress;
                    response.Key = g2RGetLoginKey.Key;
                }
                response.Error = result;
                reply(response);
			}
			catch (Exception e)
			{
				ReplyError(response, e, reply);
			}
		}
	}
}