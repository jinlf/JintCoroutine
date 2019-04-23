using System;
using Jint;
using Jint.Native;
using Jint.Native.Function;

namespace JintCoroutine
{
    public class JsEventHandler
    {
        public string EventType;
        public string TargetId;
        public JsValue ThisObject;
        public JsValue[] Arguments;
        public FunctionInstance Callback;
    }
}
