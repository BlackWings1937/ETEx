namespace ETModel
{
	public static class ErrorCode
	{
		public const int ERR_Success = 0;
		public const int ERR_NotFoundActor = 2;
		public const int ERR_ActorNoMailBoxComponent = 3;
		public const int ERR_ActorTimeOut = 4;
		public const int ERR_PacketParserError = 5;

		public const int ERR_AccountOrPasswordError = 102;
		public const int ERR_SessionActorError = 103;
		public const int ERR_NotFoundUnit = 104;
		public const int ERR_ConnectGateKeyError = 105;
        public const int ERR_AccountInvaild = 106;    //账户不和法
        public const int ERR_AccountRepeat = 107;     //账户名数据库重复
        public const int ERR_DataBaseWriteDown = 108; //数据库写崩
        public const int ERR_PasswordInvaild = 109;   //密码不和法
        public const int ERR_DataBaseRead = 110;      //数据库读崩
        public const int ERR_AccountNotExist = 111;   //账户名不存在
        public const int ERR_PasswordIncorrect = 112; //密码不正确


		public const int ERR_Exception = 1000;

		public const int ERR_RpcFail = 2001;
		public const int ERR_SocketDisconnected = 2002;
		public const int ERR_ReloadFail = 2003;
		public const int ERR_ActorLocationNotFound = 2004;
	}
}