using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

// 字体相关？
using System.Text.RegularExpressions;

//序列化相关
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

// 流相关
using System.IO;
namespace ETModel
{
    [ObjectSystem]
    public class MySqlComponentSystem : AwakeSystem<MySqlComponent> {

        public override void Awake(MySqlComponent self)
        {
            self.Awake();
        }
    }


    public class MySqlComponent: Component
    {
        private MySqlConnection sqlConn_;
        public void Awake() {
            /*
             * 连接数据库
             */
            string strConn = "Database=usersdata;Data Source=127.0.0.1;";
            strConn += "User Id=root;Password=066047;port=3306;";
            sqlConn_ = new MySqlConnection(strConn);
            try {
                sqlConn_.Open();
                Console.WriteLine("[数据库]连接成功");
            } catch (Exception e) {
                Console.WriteLine("[数据库]连接失败"+e.Message);
                return;
            }
        }
        //判定安全字符串
        public bool IsSafeStr(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        //是否存在该用户
        private bool CanRegister(string id)
        {
            //防sql注入
            if (!IsSafeStr(id))
                return false;
            //查询id是否存在
            string cmdStr = string.Format("select * from userdata1 where id='{0}';", id);
            MySqlCommand cmd = new MySqlCommand(cmdStr, sqlConn_);
            try
            {
                MySqlDataReader dataReader = (MySqlDataReader)cmd.ExecuteReader();
                bool hasRows = dataReader.HasRows;
                dataReader.Close();
                return !hasRows;
            }
            catch (Exception e)
            {
                Console.WriteLine("[DataMgr]CanRegister fail " + e.Message);
                return false;
            }
        }

        //注册
        public int Register(string id, string key)
        {
            //防sql注入
            if (!IsSafeStr(id))
            {
                Console.WriteLine("[DataMgr]Register 账号使用非法字符");
                return ErrorCode.ERR_AccountInvaild;
            }
            if (!IsSafeStr(key)) {
                Console.WriteLine("[DataMgr]Register 密码使用非法字符");
                return ErrorCode.ERR_PasswordInvaild;
            }
            //能否注册
            if (!CanRegister(id))
            {
                Console.WriteLine("[DataMgr]Register !CanRegister");
                return ErrorCode.ERR_AccountRepeat;
            }
            //写入数据库User表
            string cmdStr = string.Format("insert into userdata1 values({0},'{2}','{1}')", id,id,key);// ,key ='{1}' ,name='bb' key
            MySqlCommand cmd = new MySqlCommand(cmdStr, sqlConn_);
            try
            {
                cmd.ExecuteNonQuery();
                return ErrorCode.ERR_Success;
            }
            catch (Exception e)
            {
                Console.WriteLine("[DataMgr]Register " + e.Message);
                return ErrorCode.ERR_DataBaseWriteDown;
            }
        }

        //检测用户名密码
        public int CheckPassWord(string id, string pw)
        {
            //防sql注入
            if (!IsSafeStr(id))
                return ErrorCode.ERR_AccountInvaild;
            if (!IsSafeStr(pw))
                return ErrorCode.ERR_PasswordInvaild;

            //查询账号是否存在
            string cmdStr = string.Format("select * from userdata1 where id={0};",id);
            MySqlCommand cmd = new MySqlCommand(cmdStr,sqlConn_);
            try {
                MySqlDataReader dataReader = (MySqlDataReader)cmd.ExecuteReader();
                bool hasRows = dataReader.HasRows;
                dataReader.Close();
                if (hasRows) {
                    cmdStr = string.Format("select * from userdata1 where id={0} and pw='{1}';",id,pw);
                    cmd = new MySqlCommand(cmdStr, sqlConn_);
                    dataReader = (MySqlDataReader)cmd.ExecuteReader();
                    hasRows = dataReader.HasRows;
                    dataReader.Close();
                    if (hasRows)
                    {
                        Console.WriteLine("登录成功");
                        return ErrorCode.ERR_Success;
                    }
                    else {
                        Console.WriteLine("密码错误");
                        return ErrorCode.ERR_PasswordIncorrect;
                    }

                } else {
                    Console.WriteLine("账户不存在");
                    return ErrorCode.ERR_AccountNotExist;
                }
            } catch (Exception e) {
                Console.WriteLine("[DataMgr]CheckPassWord " + e.Message);
                return ErrorCode.ERR_DataBaseRead;
            }
        }
    }
}
