using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class UiAlertsGrayComponentAwakeSystem : AwakeSystem<UIAlertsGrayComponent>
    {
        public override void Awake(UIAlertsGrayComponent self)
        {
            self.Awake();
        }
    }

    //声明代理
    public delegate void AlertCallBack();
    public delegate void CloseWinCalBack(UI win);

    public class UIAlertsGrayComponent : UIAlertsComponent {
        //----------------生命周期方法---------------------
        public override void Awake()
        {
            base.Awake();
            greyBg_ = ResourceUtil.LoadGameObjectFromAb("AlertBg");
            greyBg_.transform.SetParent(alertsRoot_.transform);
        }
        //----------------重写方法------------------------
        protected override void closeBg()
        {
            greyBg_.SetActive(false);
        }
        protected override void openBg()
        {
            greyBg_.SetActive(true);
        }

        public override void Dispose() {
            ResourceUtil.UnLoadGameObjectFromAb("AlertBg");
            base.Dispose();
        }

        //----------------私有成员-------------------------
        private GameObject greyBg_;
    }

    public class UIAlertsComponent:Component
    {
        //---------------生命周期方法--------------------------
        public virtual void Awake() {
            alertsRoot_ = GameObject.Find("Global/UI/AlertsCanvas/");
            wins_ = new List<UI>();
            // 加载 持有预制 然后卸载 ab note
        }
        //---------------私有成员----------------------------
        /*
         * 子窗口根节点
         */
        protected GameObject alertsRoot_;

        /*
         * 子窗口s
         */
        private List<UI> wins_;

        //---------------私有方法-----------------------------
        private void closeWin(UI win) {
            if (wins_.Contains(win)) {
                wins_.Remove(win);
                /*
                 * 优化点 对象池
                 */
                win.Dispose();
            }
            checkCloseBg();
        }
        private void checkOpenBg() {
            if (wins_.Count>0) {
                openBg();
            }
        }
        private void checkCloseBg() {
            if (wins_.Count<=0) {
                closeBg();
            }
        }
        protected virtual void openBg() { }
        protected virtual void closeBg() { }

        //---------------对外接口------------------------------
        /* 添加一个警告窗口 并 置顶
         * param:
         * strWord:窗口文本
         * strBtn:按钮文本
         * cb:按钮事件
         */
        public void CreateAlertWin(
            string strWord,
            string strBtn, 
            AlertCallBack cb
            ) {
            //GameObject g = ResourceUtil.LoadGameObjectFromAb("AlertWin");
        }

        /*
         * 添加一个确认窗口并置顶
         * param:
         * strWord:窗口文本
         * strBtnOk:确定按钮文本
         * strBtnCancle:取消按钮文本
         * cbOk:确定按钮回调
         * cbCancle:取消按钮回调
         */
        public void CreateDoubleCheckWin(
            string strWord,
            string strBtnOk,
            string strCancle,
            AlertCallBack cbOk,
            AlertCallBack cbCancle
            ) {

        }

        /*
         * 清理方法 清理所有子窗口
         */
        public override void Dispose() {
            for (int i = wins_.Count-1;i>=0;--i) {
                UI ui = wins_[i];
                wins_.RemoveAt(i);
                ui.Dispose();
            }
        }
    }
}
