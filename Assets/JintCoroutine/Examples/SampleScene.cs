using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using JintCoroutine;

public class SampleScene : MonoBehaviour
{
    private JavaScriptExecutor jsExecutor;
    // Start is called before the first frame update
    void Start()
    {
        var code = @"
var actors = {}
actors['0'] = {}
actors['0']['walk'] = function () {
  print('0 walk')
}
actors['0']['eat'] = function () {
  print('0 eat')
}
onstart('0', function (actor) {
  while (true) {
    print('0')
    wait(2)
    actor.walk()
  }
})
ontouch('0', function (actor) {
  print('0 touched')
  actor.eat()
})
";
        jsExecutor = new JavaScriptExecutor(this);

        transform.Find("Load").GetComponent<Button>().onClick.AddListener(() => {
            jsExecutor.Execute(code);
        });
        transform.Find("Start").GetComponent<Button>().onClick.AddListener(() => {
            jsExecutor.TriggerEvent("EventStart", "0", null);
        });
        transform.Find("Touch").GetComponent<Button>().onClick.AddListener(() => {
            jsExecutor.TriggerEvent("EventTouch", "0", null);
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
