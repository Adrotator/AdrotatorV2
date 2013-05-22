
using System;
using System.Globalization;
using System.Reflection;

namespace AdRotator
{
    public class ReflectionHelpers
    {
        public bool IsValueType(Type targetType)
        {
            if (targetType == null)
            {
                throw new NullReferenceException("Must supply the targetType parameter");
            }
#if WINRT
            return !targetType.GetTypeInfo().IsValueType;
#else
            return !targetType.IsValueType;
#endif
           
        }

        //GetBase Type
        public Type GetBaseTpye(Type targetType)
        {
            if (targetType == null)
            {
                throw new NullReferenceException("Must supply the targetType parameter");
            }
#if WINRT
            var type = targetType.GetTypeInfo().BaseType;
#else
            var type = targetType.BaseType;
#endif
            return type;
        }

//Test isAbstract
        public bool IsAbstractClass(Type t)
        {
            if (t == null)
            {
                throw new NullReferenceException("Must supply the t (type) parameter");
            }
#if WINRT
            var ti = t.GetTypeInfo();
            if (ti.IsClass && !ti.IsAbstract)
                return true;
#else
            if (t.IsClass && !t.IsAbstract)
                return true;
#endif
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
#if WINRT
            if(method == "get")
                methodInfo = property.GetMethod;
            else
                methodInfo = property.SetMethod;
#else
            if(method == "get")
                methodInfo = property.GetGetMethod();
            else
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
#if WINRT
            Attribute attr = member.GetCustomAttribute(memberType);
#else
            Attribute attr = Attribute.GetCustomAttribute(member, memberType);
#endif
            return attr;
        }

//Check for public methods
        public bool HasPublicProperties(PropertyInfo property)
        {
            if (property == null)
            {
                throw new NullReferenceException("Must supply the property parameter");
            }
#if WINRT
            if ( property.GetMethod != null && !property.GetMethod.IsPublic )
                return true;
            if ( property.SetMethod != null && !property.SetMethod.IsPublic )
                return true;
#else
            foreach (MethodInfo info in property.GetAccessors(true))
            {
                if (info.IsPublic == false)
                    return true;
            }
#endif
            return false;
        }
		
        public bool IsAssignableFrom(Type type, object provider)
        {
			if (type == null)
                throw new ArgumentNullException("type");
            if (provider == null)
                throw new ArgumentNullException("provider");
#if WINRT
            if (type.GetTypeInfo().IsAssignableFrom(provider.GetType().GetTypeInfo()))
				return true;
#else
            if (type.IsAssignableFrom(provider.GetType()))
				return true;
#endif
            return false;
        }


        public Type TryGetType(string assemblyName, string typeName)
        {
            try
            {
#if WINRT
                var assem = Assembly.Load(new AssemblyName("MSAdvertisingXaml, Version=6.1"));
                Type t = assem.GetType(typeName);
                if (t != null) { return t; }
#else
                var assem = Assembly.Load(assemblyName);
                Type t = assem.GetType(typeName, false);
                if (t != null) { return t; }
#endif
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
#if WINRT
                    MethodInfo methodInfo = type.GetRuntimeMethod(methodName,null);
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

        public void TrySetProperty(object instance, string PropertyName, string PropertyValue)
        {
            try
            {
#if WINRT
                PropertyInfo propertyInfo = instance.GetType().GetRuntimeProperty(PropertyName);
#else
                PropertyInfo propertyInfo = instance.GetType().GetProperty(PropertyName);
#endif
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

    }
}
