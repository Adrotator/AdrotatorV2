
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace AdRotator
{
    internal class ReflectionHelpers
    {
        public bool IsValueType(Type type)
        {
#if UNIVERSAL
            var targetType = type.GetTypeInfo();
#else
            var targetType = type;
#endif
            if (targetType == null)
            {
                throw new NullReferenceException("Must supply the targetType parameter");
            }

            return !targetType.IsValueType;
        }

        //GetBase Type
        public Type GetBaseTpye(Type type)
        {
#if UNIVERSAL
            var targetType = type.GetTypeInfo();
#else
            var targetType = type;
#endif
            if (targetType == null)
            {
                throw new NullReferenceException("Must supply the targetType parameter");
            }
            return targetType.BaseType;;
        }

//Test isAbstract
        public bool IsAbstractClass(Type type)
        {
#if UNIVERSAL
            var targetType = type.GetTypeInfo();
#else
            var targetType = type;
#endif
            if (type == null)
            {
                throw new NullReferenceException("Must supply the t (type) parameter");
            }

            if (targetType.IsClass && !targetType.IsAbstract)
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
#if UNIVERSAL
                methodInfo = property.GetMethod;
#else
                methodInfo = property.GetGetMethod();
#endif
            else
#if UNIVERSAL
                methodInfo = property.SetMethod;
#else
                methodInfo = property.GetSetMethod();
#endif
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
#if UNIVERSAL
            Attribute attr = member.GetCustomAttribute(memberType);
#else
             Attribute attr = Attribute.GetCustomAttribute(member, memberType);
#endif
            return attr;
        }

//Check for public methods
        //public bool HasPublicProperties(PropertyInfo property)
        //{
        //    if (property == null)
        //    {
        //        throw new NullReferenceException("Must supply the property parameter");
        //    }

        //    foreach (MethodInfo info in property..GetAccessors(true))
        //    {
        //        if (info.IsPublic == false)
        //            return true;
        //    }
        //    return false;
        //}
		
        //public bool IsAssignableFrom(Type type, object provider)
        //{
        //    if (type == null)
        //        throw new ArgumentNullException("type");
        //    if (provider == null)
        //        throw new ArgumentNullException("provider");

        //    if (type.IsAssignableFrom(provider.GetType()))
        //        return true;
        //    return false;
        //}


        public Type TryGetType(string assemblyName, string typeName)
        {
#if UNIVERSAL
            var assembly = new AssemblyName(assemblyName);
#else
            var assembly = assemblyName;
#endif
            try
            {
                var assem = Assembly.Load(assembly);
#if UNIVERSAL
                Type t = assem.GetType(typeName);
#else
                Type t = assem.GetType(typeName, false);
#endif
            if (t != null) { return t; }
            }
            catch
            {
                throw new PlatformNotSupportedException(String.Format("Provider dll not located in this solution ({0})", assemblyName));
            }

            return null;
        }

        public void TryInvokeMethod(Type type, object classInstance, string methodName)
        {
#if UNIVERSAL
            var targetType = type.GetTypeInfo();
#else
            var targetType = type;
#endif
            try
            {
                if (type != null)
                {
#if UNIVERSAL
                    MethodInfo methodInfo = type.GetRuntimeMethod(methodName, new Type[0]);
#else
                    MethodInfo methodInfo = type.GetMethod(methodName,new Type[0]);
#endif
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

        public void TryInvokeMethod(Type type, object classInstance, string methodName, params object[] parameters)
        {
            try
            {
                if (type != null)
                {
#if UNIVERSAL
                    MethodInfo methodInfo = type.GetRuntimeMethod(methodName, new Type[0]);
#else
                    MethodInfo methodInfo = type.GetMethod(methodName,new Type[0]);
#endif
                    if (methodInfo != null)
                    {
                        object result = null;
                            //This works fine
                        result = methodInfo.Invoke(classInstance, parameters);
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
#if UNIVERSAL
                PropertyInfo propertyInfo = instance.GetType().GetRuntimeProperty(PropertyName);
                if (propertyInfo.GetType().GetTypeInfo().IsEnum)
#else
                PropertyInfo propertyInfo = instance.GetType().GetProperty(PropertyName);
                if (propertyInfo.PropertyType.IsEnum)
#endif
                {
                    propertyInfo.SetValue(instance, StringToEnum(propertyInfo.PropertyType, PropertyValue), null);
                }
                else if (propertyInfo.PropertyType.IsNullableEnum())
                {
 #if UNIVERSAL
                    Type nullableType = propertyInfo.PropertyType.GenericTypeArguments[0];
#else
                    Type nullableType = propertyInfo.PropertyType.GetGenericArguments()[0];
#endif
                    propertyInfo.SetValue(instance, StringToEnum(nullableType, PropertyValue), null);
                }
                else
                {
                    propertyInfo.SetValue(instance, Convert.ChangeType(PropertyValue, propertyInfo.PropertyType, CultureInfo.InvariantCulture), null);
                }
            }
            catch { }
        }

        static object StringToEnum(Type type, string Value)
        {
#if UNIVERSAL
            var targetType = type.GetTypeInfo();
            foreach (FieldInfo fi in targetType.DeclaredFields)
#else
            foreach (FieldInfo fi in type.GetFields())
#endif
                if (fi.Name == Value)
                    return fi.GetValue(null);    
            // We use null because
            // enumeration values
            // are static

            throw new Exception(string.Format("Can't convert {0} to {1}", Value, type.ToString()));
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
#if UNIVERSAL
                mi = targetObject.GetType().GetRuntimeMethod(methodname, new Type[0]);
#else
                mi = targetObject.GetType().GetMethod(methodname, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
#endif
            }
            catch { }

            return mi;
        }

        //public void WireUpEvent(object o, string eventname, Delegate handler)
        //{
        //    if (o == null)
        //        throw new ArgumentNullException("o");
        //    if (eventname == null)
        //        throw new ArgumentNullException("eventname");
        //    if (handler == null)
        //        throw new ArgumentNullException("handler");

        //    try
        //    {
        //    EventInfo ei = o.GetType().GetEvent(eventname);
        //    Delegate del = Delegate.CreateDelegate(ei.EventHandlerType, handler.Target, handler.Method);

        //    ei.AddEventHandler(o, del);
        //    }
        //    catch { }

        //}

        //private Type[] GetDelegateParameterTypes(Type d)
        //{
        //    if (d.BaseType != typeof(MulticastDelegate))
        //        throw new Exception("Not a delegate.");

        //    MethodInfo invoke = d.GetMethod("Invoke");
        //    if (invoke == null)
        //        throw new Exception("Not a delegate.");

        //    ParameterInfo[] parameters = invoke.GetParameters();
        //    Type[] typeParameters = new Type[parameters.Length];
        //    for (int i = 0; i < parameters.Length; i++)
        //    {
        //        typeParameters[i] = parameters[i].ParameterType;
        //    }
        //    return typeParameters;
        //}

        //private Type GetDelegateReturnType(Type d)
        //{
        //    if (d.BaseType != typeof(MulticastDelegate))
        //        throw new Exception("Not a delegate.");

        //    MethodInfo invoke = d.GetMethod("Invoke");
        //    if (invoke == null)
        //        throw new Exception("Not a delegate.");

        //    return invoke.ReturnType;
        //}


    }

    public static class TypeExtensions
    {
        public static bool IsNullableEnum(this Type t)
        {
            Type u = Nullable.GetUnderlyingType(t);
#if UNIVERSAL
            return (u != null) && u.GetTypeInfo().IsEnum;
#else
            return (u != null) && u.IsEnum;
#endif
        }

    }
}
