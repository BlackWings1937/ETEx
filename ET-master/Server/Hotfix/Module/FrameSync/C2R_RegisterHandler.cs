using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    class C2R_RegisterHandler : AMRpcHandler<C2R_Register, R2C_Register>
    {
        protected override void Run(
            Session session,
            C2R_Register message,
            Action<R2C_Register> reply)
        {
            R2C_Register response = new R2C_Register();
            Console.WriteLine("userLogin:" + message.Account + "key:" + message.Password);
            try {
                int result = Game.Scene.GetComponent<MySqlComponent>().Register(message.Account, message.Password);
                response.Error = result;
                reply(response);
            }
            catch (Exception e) {
                ReplyError(response,e,reply);
            }

        }
    }
}
