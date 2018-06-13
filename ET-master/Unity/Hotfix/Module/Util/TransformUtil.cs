using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public class TransformUtil
    {
        public static void SetLocalZ(Transform t,float z) {
            Vector3 pos = t.localPosition;
            pos.z = z;
            t.localPosition = pos;
        }
        public static void SetLocalX(Transform t, float x) {
            Vector3 pos = t.localPosition;
            pos.x = x;
            t.localPosition = pos;
        }
        public static void SetLocalY(Transform t, float y) {
            Vector3 pos = t.localPosition;
            pos.y = y;
            t.localPosition = pos;
        }
        public static void SetLocalXY(Transform t, float x,float y) {
            Vector3 pos = t.localPosition;
            pos.x = x;
            pos.y = y;
            t.localPosition = pos;
        }
    }
}
