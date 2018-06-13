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
            greyBg_ = UnityEngine.Object.Instantiate(ResourceUtil.LoadPrefabFromAb(PREFAB_NAME_ALERTBG));
            GameObjectUtil.SetParentToChild(alertsRoot_,greyBg_,false);
            GameObjectUtil.SetZ(greyBg_,ZORDER_BG);
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
            ResourceUtil.UnLoadGameObjectFromAb(PREFAB_NAME_ALERTBG);
            base.Dispose();
        }

        //----------------私有成员-------------------------
        private GameObject greyBg_;
        /*
         * 预制体名
         */
        private const string PREFAB_NAME_ALERTBG = "AlertBg";
    }

    public class UIAlertsComponent:Component
    {

        //---------------生命周期方法--------------------------
        public virtual void Awake() {
            alertsRoot_ = GameObject.Find("Global/UI/AlertsCanvas/");
            wins_ = new List<UI>();
            // 加载 持有预制 然后卸载 ab note
            prefabAlert_ = ResourceUtil.LoadPrefabFromAb(PREFAB_NAME_ALERT);
            prefabDoubleCheck_ = ResourceUtil.LoadPrefabFromAb(PREFAB_NAME_DOUBLECHECK);
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

        /*
         * 界面预制体
         */
        private GameObject prefabAlert_;
        private GameObject prefabDoubleCheck_;

        /*
         * 预制体名
         */
        private const string PREFAB_NAME_ALERT = "AlertWin";
        private const string PREFAB_NAME_DOUBLECHECK = "DoubleCheckWin";

        /*
         * 警告层z值层级
         */
        protected const float ZORDER_BG = -1;
        protected const float ZORDER_ALERT = -2;

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
         * cb:按钮事件
         * strBtn:按钮文本
         */
        public void CreateAlertWin(
            string strWord,
            AlertCallBack cb,
            string strBtn = "好的"
            ) {
            GameObject g = UnityEngine.Object.Instantiate(prefabAlert_);
            g.layer = LayerMask.NameToLayer(LayerNames.UI);
            UI ui = ComponentFactory.Create<UI,GameObject>(g);

            UIAlertWin win = ui.AddComponent<UIAlertWin>();
            GameObjectUtil.SetParentToChild(alertsRoot_,ui.GameObject,false);
            GameObjectUtil.SetZ(ui.GameObject, ZORDER_ALERT);

            win.SetTextContent(strWord);
            win.SetCheckCb(cb);
            win.SetCheckBtnText(strBtn);
            win.SetCloseCallBack(closeWin);
            wins_.Add(ui);
            checkOpenBg();
        }

        /*
         * 添加一个确认窗口并置顶
         * param:
         * strWord:窗口文本
         * cbOk:确定按钮回调
         * cbCancle:取消按钮回调
         * strBtnOk:确定按钮文本
         * strBtnCancle:取消按钮文本
         */
        public void CreateDoubleCheckWin(
            string strWord,
            AlertCallBack cbOk,
            AlertCallBack cbCancle,
            string strBtnOk = "确定",
            string strCancle = "取消"
            ) {
            GameObject g = UnityEngine.Object.Instantiate(prefabAlert_);
            g.layer = LayerMask.NameToLayer(LayerNames.UI);
            UI ui = ComponentFactory.Create<UI,GameObject>(g);

            UIDoubleCheckWin win = ui.AddComponent<UIDoubleCheckWin>();
            GameObjectUtil.SetParentToChild(alertsRoot_,ui.GameObject,false);
            GameObjectUtil.SetZ(ui.GameObject, ZORDER_ALERT);

            win.SetTextContent(strWord);
            win.SetOkCb(cbOk);
            win.SetCancleCb(cbCancle);
            win.SetBtnOkText(strBtnOk);
            win.SetBtnCancleText(strCancle);
            win.SetCloseCallBack(closeWin);
            wins_.Add(ui);
            checkOpenBg();
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
            //卸载警告窗资源
            ResourceUtil.UnLoadGameObjectFromAb(PREFAB_NAME_ALERT);
            ResourceUtil.UnLoadGameObjectFromAb(PREFAB_NAME_DOUBLECHECK);
        }
    }
}
