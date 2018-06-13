using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public class ResourceUtil
    {
        /*
         * 从ab 包中加载游戏对象
         * param:
         * objName:游戏对象名
         */
         /*
        public static GameObject LoadGameObjectFromAb(string objName) {
            GameObject g = null;
s           //g = UnityEngine.Object.Instantiate(LoadPrefabFromAb(objName));
            return g;
        }*/

        /*
         * 卸载游戏对象
         * param:
         * objName:游戏对象名
         */
        public static void UnLoadGameObjectFromAb(string objName) {
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{objName}.unity3d");
        }

        /*
         * 从ab包中获取预制体
         * param:
         * objName:预制体名称
         */
        public static GameObject LoadPrefabFromAb(string objName) {
            GameObject g = null;
            ResourcesComponent resourcesComponent =
                    ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle($"{objName}.unity3d");
            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{objName}.unity3d", $"{objName}");
            g = bundleGameObject;
            return g;
        }
    }
}
