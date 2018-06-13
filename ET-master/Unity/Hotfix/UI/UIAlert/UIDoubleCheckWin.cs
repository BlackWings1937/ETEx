using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIDoubleCheckWinAwakeSystem : AwakeSystem<UIDoubleCheckWin>
    {
        public override void Awake(UIDoubleCheckWin self)
        {
            self.Awake();
        }
    }

    public class UIDoubleCheckWin:UIAlertBase
    {
        //---------------私有成员------------------
        private GameObject btnOk_;
        private GameObject btnCancle_;
        private AlertCallBack okCb_;
        private AlertCallBack cancleCb_;
        private GameObject textBtnOk_;
        private GameObject textBtnCancle_;

        //---------------生命周期方法--------------
        public override void Awake() {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            btnCancle_ = rc.Get<GameObject>("BtnCancle");
            btnOk_ = rc.Get<GameObject>("BtnOk");
            textBtnOk_ = rc.Get<GameObject>("TextBtnOk");
            textBtnCancle_ = rc.Get<GameObject>("TextBtnCancle");

            btnOk_.GetComponent<Button>().onClick.AddListener(onBtnOkClick);
            btnCancle_.GetComponent<Button>().onClick.AddListener(onBtnCancleClick);
            textBtnOk_.GetComponent<Text>().text = "确定";
            textBtnCancle_.GetComponent<Text>().text = "取消";
            base.Awake();
        }
        //----------------私有方法-----------------
        private void onBtnOkClick() {
            if (okCb_ != null) { okCb_.Invoke(); }
            closeWin();
        }
        private void onBtnCancleClick() {
            if (cancleCb_ != null) { cancleCb_.Invoke(); }
            closeWin();
        }
        //----------------对外接口-----------------

        /*
         * 设置点击确定的回调
         * param:
         * cb:点击确定的回调
         */
        public void SetOkCb(AlertCallBack cb) { okCb_ = cb; }

        /*
         * 设置点击取消的回调
         * param:
         * cb:点击取消的回调
         */
        public void SetCancleCb(AlertCallBack cb) { cancleCb_ = cb; }

        /*
         * 设置确定按钮标题
         * param:
         * text:确定按钮标题
         */
        public void SetBtnOkText(string text = "确定") {
            textBtnOk_.GetComponent<Text>().text = text;
        }

        /*
         * 设置取消按钮标题
         * param:
         * text:取消按钮标题
         */
        public void SetBtnCancleText(string text = "取消") {
            textBtnCancle_.GetComponent<Text>().text = text;
        }
    }
}
