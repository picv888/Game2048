using System;
using System.Collections.Generic;
namespace ConsoleUIKit {
    /// <summary>
    /// 模仿IOS的UIKit的UIViewController
    /// </summary>
    public class ConsoleSence {
        List<ConsoleSpirit> spirits = new List<ConsoleSpirit>(10);
        public List<ConsoleSpirit> Spirits {
            get {
                return spirits;
            }
        }

        public ConsoleSence() {

        }

        public void addSpirit(ConsoleSpirit spirit) {
            spirits.Add(spirit);
        }

        public virtual void SenceDidLoad() {

        }

        public virtual void SenceWillDisappear() {

        }

        public void DrawSence() {
            foreach(var spirit in this.spirits) {
                spirit.Draw();
            }
        }


        //接收用户操作
        public virtual void DidAcceptInput(ConsoleKeyInfo info) {

        }

        public virtual void RefreshSence() {
            foreach(var item in spirits) {
                item.DrawIfNeed();
            }
        }
    }
}
