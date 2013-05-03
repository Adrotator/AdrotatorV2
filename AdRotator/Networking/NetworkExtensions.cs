using System;
using System.Xml;

namespace AdRotator.Networking
{
    internal static class NetworkExtensions
    {
        public static void CheckNotNullThrowException(this object obj)
        {
            if (obj == null)
                throw new Exception(obj.GetType().ToString() + " cannot be null");
        }
        public static void IfNullDoThis(this object obj, Action act)
        {
            if (obj == null)
                act.Invoke();
        }
        public static void IfNotNullDoThis(this object obj, Action act)
        {
            if (obj != null)
                act.Invoke();
        }

        public static void IfNotNullInvoke(this Action act)
        {
            if (act != null)
                act.Invoke();
        }

     }
}
