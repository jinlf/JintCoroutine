using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jint;
using Jint.Native;
using Jint.Native.Function;
using Jint.Runtime.Interop;
using System;
using System.Linq;

namespace JintCoroutine
{
    public class JavaScriptExecutor
    {
        private MonoBehaviour env;
        private Engine engine;
        private List<JsEventHandler> eventHandlers = new List<JsEventHandler>();
        private Queue<JsEvent> events = new Queue<JsEvent>();
        private Coroutine coroutine;

        public JavaScriptExecutor(MonoBehaviour environment)
        {
            engine = new Engine();
            env = environment;

            CreateContext();
        }

        private void CreateContext()
        {
            engine.Global.FastAddProperty("onstart", new ClrFunctionInstance(engine, HandleOnStart), true, false, true);
            engine.Global.FastAddProperty("ontouch", new ClrFunctionInstance(engine, HandleOnTouch), true, false, true);
            engine.Global.FastAddProperty("wait", new ClrFunctionInstance(engine, HandleWait), true, false, true);
            engine.Global.FastAddProperty("print", new ClrFunctionInstance(engine, HandlePrint), true, false, true);
        }

        private JsValue HandleOnStart(JsValue arg1, JsValue[] arg2)
        {
            var id = arg2[0].AsString();
            var func = arg2[1].TryCast<FunctionInstance>();

            var jsTarget = GetJsTargetById(id);
            eventHandlers.Add(new JsEventHandler
            {
                EventType = "EventStart",
                TargetId = id,
                ThisObject = arg1,
                Arguments = new JsValue[] { jsTarget },
                Callback = func
            });
            return arg1;
        }

        private JsValue GetJsTargetById(string id)
        {
            var actors = engine.Global.Get("actors");
            var actor = actors.AsObject().Get(id);
            return actor;
        }

        private JsValue HandleOnTouch(JsValue arg1, JsValue[] arg2)
        {
            var id = arg2[0].AsString();
            var func = arg2[1].TryCast<FunctionInstance>();

            var jsTarget = GetJsTargetById(id);
            eventHandlers.Add(new JsEventHandler
            {
                EventType = "EventTouch",
                TargetId = id,
                ThisObject = arg1,
                Arguments = new JsValue[] { jsTarget },
                Callback = func
            });
            return arg1;
        }
        private JsValue HandleWait(JsValue arg1, JsValue[] arg2)
        {
            var secs = arg2[0].AsNumber();

            var value = new JsValue("Instruction");
            //value.Instruction = new WaitForSeconds((float)secs);
            return value;
        }
        private JsValue HandlePrint(JsValue arg1, JsValue[] arg2)
        {
            Debug.Log(arg2[0].AsString());
            return arg1;
        }

        public void Execute(string code)
        {
            if (coroutine != null)
            {
                env.StopCoroutine(coroutine);
            }
            coroutine = env.StartCoroutine(ExecuteLoop(code));
        }

        public void Stop()
        {
            if (coroutine != null)
            {
                env.StopCoroutine(coroutine);
                coroutine = null;
            }
        }


        internal void TriggerEvent(string eventType, string targetId, JsValue eventData)
        {
            events.Enqueue(new JsEvent
            {
                EventType = eventType,
                TargetId = targetId,
                EventData = eventData
            });
        }

        private IEnumerator ExecuteLoop(string code)
        {
            yield return null;
            //yield return env.StartCoroutine(engine.ExecuteCo(code, null));
            //while (true)
            //{
            //    if (events.Count > 0)
            //    {
            //        var eventInfo = events.Dequeue();
            //        var handlers = eventHandlers.Where((JsEventHandler arg) => arg.EventType == eventInfo.EventType && arg.TargetId == eventInfo.TargetId);
            //        foreach (var handler in handlers)
            //        {
            //            yield return env.StartCoroutine(handler.Callback?.CallCo(handler.ThisObject, handler.Arguments, null));
            //        }
            //    }
            //}
        }
    }
}