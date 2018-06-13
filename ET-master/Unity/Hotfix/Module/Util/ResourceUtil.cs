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
        public static GameObject LoadGameObjectFromAb(string objName) {
            GameObject g = null;
            ResourcesComponent resourcesComponent =
                    ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle($"{objName}.unity3d");
            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset($"{objName}.unity3d", $"{objName}");
            g = UnityEngine.Object.Instantiate(bundleGameObject);
            return g;
        }
        /*
         * 卸载游戏对象
         * param:
         * objName:游戏对象名
         */
        public static void UnLoadGameObjectFromAb(string objName) {
            ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{objName}.unity3d");
        }
    }
}
