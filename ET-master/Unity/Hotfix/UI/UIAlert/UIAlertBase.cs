using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
namespace ETHotfix
{

    public class UIAlertBase:Component
    {
        //------------------私有成员---------------------
        //private UIAlertsComponent alertsComponent_ = null;
        protected GameObject textContent_ = null;
        private CloseWinCalBack closeWinCb_ = null;

        //------------------生命周期方法-----------------
        public virtual void Awake() {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            textContent_ = rc.Get<GameObject>("TextContent");
        }

        //------------------属性-------------------------
        /*
        public UIAlertsComponent CacheAlertsComponent {
            get {
                if (alertsComponent_ == null) {
                    alertsComponent_ = Game.Scene.GetComponent<UIAlertsComponent>();
                }
                return alertsComponent_;
            }
        }*/

        //-----------------私有方法-----------------------
        protected void closeWin() {
            if (closeWinCb_ != null) { closeWinCb_.Invoke(this.GetParent<UI>()); }
        }
        //-----------------对外接口-----------------------
        /*
         * 设置窗口文本内容
         * param:
         * context:窗口文本内容
         */
        public void SetTextContent(string content) { textContent_.GetComponent<Text>().text = content; }

        /*
         * 设置关闭窗口回调
         * param:
         * cb:关闭窗口的回调
         */
        public void SetCloseCallBack(CloseWinCalBack cb) { closeWinCb_ = cb;}
    }
}
