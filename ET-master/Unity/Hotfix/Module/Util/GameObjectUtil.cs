using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public class GameObjectUtil
    {
        public static void SetParentToChild(GameObject parent,GameObject child,bool stay = true) {
            child.transform.SetParent(parent.transform, stay);
        }
        public static void SetZ(GameObject obj,float z) {
            TransformUtil.SetLocalZ(obj.transform,z);
        }
        public static void SetX(GameObject obj,float x) {
            TransformUtil.SetLocalX(obj.transform,x);
        }
        public static void SetY(GameObject obj,float y) {
            TransformUtil.SetLocalY(obj.transform,y);
        }
        public static void SetXY(GameObject obj,float x,float y) {
            TransformUtil.SetLocalXY(obj.transform,x,y);
        }
    }
}
