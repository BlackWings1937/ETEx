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
        public const int ERR_AccountInvaild = 106;    //�˻����ͷ�
        public const int ERR_AccountRepeat = 107;     //�˻������ݿ��ظ�
        public const int ERR_DataBaseWriteDown = 108; //���ݿ�д��
        public const int ERR_PasswordInvaild = 109;   //���벻�ͷ�
        public const int ERR_DataBaseRead = 110;      //���ݿ����
        public const int ERR_AccountNotExist = 111;   //�˻���������
        public const int ERR_PasswordIncorrect = 112; //���벻��ȷ


		public const int ERR_Exception = 1000;

		public const int ERR_RpcFail = 2001;
		public const int ERR_SocketDisconnected = 2002;
		public const int ERR_ReloadFail = 2003;
		public const int ERR_ActorLocationNotFound = 2004;
	}
}