
using System;
using System.Globalization;
using System.Reflection;

namespace AdRotator
{
    internal class ReflectionHelpers
    {
        public bool IsValueType(Type targetType)
        {
            if (targetType == null)
            {
                throw new NullReferenceException("Must supply the targetType parameter");
            }

            return !targetType.IsValueType;
        }

        //GetBase Type
        public Type GetBaseTpye(Type targetType)
        {
            if (targetType == null)
            {
                throw new NullReferenceException("Must supply the targetType parameter");
            }
            var type = targetType.BaseType;
            return type;
        }

//Test isAbstract
        public bool IsAbstractClass(Type t)
        {
            if (t == null)
            {
                throw new NullReferenceException("Must supply the t (type) parameter");
            }

            if (t.IsClass && !t.IsAbstract)
                return true;

            return false;
            }

//?? Reflective Methods
        public MethodInfo GetPropertyMethod(PropertyInfo property, string method)
        {
            if (property == null)
            {
                throw new NullReferenceException("Must supply the property parameter");
            }

            MethodInfo methodInfo;

            if(method == "get")
                methodInfo = property.GetGetMethod();
            else
                methodInfo = property.GetSetMethod();
            return methodInfo;

        }

// Get custom type from member
        public Attribute GetCustomAttribute(MemberInfo member, Type memberType)
        {
            if (member == null)
            {
                throw new NullReferenceException("Must supply the member parameter");
            }
            if (memberType == null)
            {
                throw new NullReferenceException("Must supply the memberType parameter");
            }

            Attribute attr = Attribute.GetCustomAttribute(member, memberType);
            return attr;
        }

//Check for public methods
        public bool HasPublicProperties(PropertyInfo property)
        {
            if (property == null)
            {
                throw new NullReferenceException("Must supply the property parameter");
            }

            foreach (MethodInfo info in property.GetAccessors(true))
            {
                if (info.IsPublic == false)
                    return true;
            }
            return false;
        }
		
        public bool IsAssignableFrom(Type type, object provider)
        {
			if (type == null)
                throw new ArgumentNullException("type");
            if (provider == null)
                throw new ArgumentNullException("provider");

            if (type.IsAssignableFrom(provider.GetType()))
				return true;
            return false;
        }


        public Type TryGetType(string assemblyName, string typeName)
        {
            try
            {
                var assem = Assembly.Load(assemblyName);
                Type t = assem.GetType(typeName, false);
                if (t != null) { return t; }
            }
            catch
            {
                throw new PlatformNotSupportedException("Provider not located in this solution");
            }

            return null;
        }

        public void TryInvokeMethod(Type type, object classInstance, string methodName)
        {
            try
            {
                if (type != null)
                {
                    MethodInfo methodInfo = type.GetMethod(methodName,new Type[0]);
                    if (methodInfo != null)
                    {
                        object result = null;
                        ParameterInfo[] parameters = methodInfo.GetParameters();
                        if (parameters.Length == 0)
                        {
                            //This works fine
                            result = methodInfo.Invoke(classInstance, null);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.ToString());
            }
        }

        public void TrySetProperty(object instance, string PropertyName, string PropertyValue)
        {
            try
            {
                PropertyInfo propertyInfo = instance.GetType().GetProperty(PropertyName);
                if (propertyInfo == null)
                {
                    // No such property name
                    return;
                }
                if (propertyInfo.PropertyType.BaseType.FullName == "System.Enum")
                {
                    propertyInfo.SetValue(instance, StringToEnum(propertyInfo.PropertyType, PropertyValue), null);
                }
                else
                {
                    propertyInfo.SetValue(instance, Convert.ChangeType(PropertyValue, propertyInfo.PropertyType, CultureInfo.InvariantCulture), null);
                }
            }
            catch { }
        }

        static object StringToEnum(Type t, string Value)
        {
            foreach (FieldInfo fi in t.GetFields())
                if (fi.Name == Value)
                    return fi.GetValue(null);    
            // We use null because
            // enumeration values
            // are static

            throw new Exception(string.Format("Can't convert {0} to {1}", Value, t.ToString()));
        }

        public MethodInfo GetMethodInfo(object targetObject, string methodname)
        {
            MethodInfo mi = null;
            if (targetObject == null)
                throw new ArgumentNullException("targetObject");
            if (methodname == null)
                throw new ArgumentNullException("methodname");
            try
            {
                mi = targetObject.GetType().GetMethod(methodname, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            }
            catch { }

            return mi;
        }

        public void WireUpEvent(object o, string eventname, Delegate handler)
        {
            if (o == null)
                throw new ArgumentNullException("o");
            if (eventname == null)
                throw new ArgumentNullException("eventname");
            if (handler == null)
                throw new ArgumentNullException("handler");

            try
            {
            EventInfo ei = o.GetType().GetEvent(eventname);
            Delegate del = Delegate.CreateDelegate(ei.EventHandlerType, handler.Target, handler.Method);

            ei.AddEventHandler(o, del);
            }
            catch { }

        }

        private Type[] GetDelegateParameterTypes(Type d)
        {
            if (d.BaseType != typeof(MulticastDelegate))
                throw new Exception("Not a delegate.");

            MethodInfo invoke = d.GetMethod("Invoke");
            if (invoke == null)
                throw new Exception("Not a delegate.");

            ParameterInfo[] parameters = invoke.GetParameters();
            Type[] typeParameters = new Type[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                typeParameters[i] = parameters[i].ParameterType;
            }
            return typeParameters;
        }

        private Type GetDelegateReturnType(Type d)
        {
            if (d.BaseType != typeof(MulticastDelegate))
                throw new Exception("Not a delegate.");

            MethodInfo invoke = d.GetMethod("Invoke");
            if (invoke == null)
                throw new Exception("Not a delegate.");

            return invoke.ReturnType;
        }




    }
}
