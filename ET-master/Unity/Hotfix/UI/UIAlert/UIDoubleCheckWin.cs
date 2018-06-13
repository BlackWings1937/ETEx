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

        //---------------生命周期方法--------------
        public override void Awake() {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            btnCancle_ = rc.Get<GameObject>("BtnCancle");
            btnOk_ = rc.Get<GameObject>("BtnOk");
            btnOk_.GetComponent<Button>().onClick.AddListener(onBtnOkClick);
            btnCancle_.GetComponent<Button>().onClick.AddListener(onBtnCancleClick);
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
        public void SetOkCb(AlertCallBack cb) { okCb_ = cb; }
        public void SetCancleCb(AlertCallBack cb) { cancleCb_ = cb; }
    }
}
