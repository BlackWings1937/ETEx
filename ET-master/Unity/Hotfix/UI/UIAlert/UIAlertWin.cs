using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIAlertWinSystem : AwakeSystem<UIAlertWin> {
        public override void Awake(UIAlertWin self)
        {
            self.Awake();
        }
    }

    public class UIAlertWin:UIAlertBase
    {
        //---------------私有成员-----------------
        private GameObject btnCheck_;
        private GameObject textBtnCheck_;
        private AlertCallBack checkCb_;

        //---------------生命周期方法-------------
        public override void Awake() {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            btnCheck_ = rc.Get<GameObject>("BtnOk");
            textBtnCheck_ = rc.Get<GameObject>("TextBtnOk");

            btnCheck_.GetComponent<Button>().onClick.AddListener(onBtnCheckClick);
            textBtnCheck_.GetComponent<Text>().text = "好的";
            base.Awake();
        }
        //---------------私有方法-----------------
        private void onBtnCheckClick() {
            if (checkCb_ != null) {
                checkCb_.Invoke();
            }
            closeWin();
        }

        //----------------对外接口--------------------
        /*
         * 设置check 回调
         * param：
         * cb:check 回调
         */
        public void SetCheckCb(AlertCallBack cb) {
            checkCb_ = cb;
        }

        /*
         * 设置确定按钮的文本
         * param：
         * text:确定按钮文本
         */
        public void SetCheckBtnText(string text ="好的") { textBtnCheck_.GetComponent<Text>().text = text; }
    }
}
