using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    public class ObjectEventArgs<T> : EventArgs
    {
        public T Object;

        public ObjectEventArgs(T Object)
        {
            this.Object = Object;
        }
    }
}
