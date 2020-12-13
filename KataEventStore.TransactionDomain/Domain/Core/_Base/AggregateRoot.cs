using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using KataEventStore.Events;

namespace KataEventStore.TransactionDomain.Domain.Core._Base
{
    public abstract class AggregateRoot
    {
        protected void ApplyEvent(IDomainEvent domainEvent)
        {
            this.Invoke("Apply", domainEvent);
        }
    }

    #region Dynamic invocation

    internal static class DynamicInvoker
    {
        private const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        private static volatile Dictionary<int, CompiledMethodInfo> _cachedMembers = new Dictionary<int, CompiledMethodInfo>();
        private static readonly object _lockObj = new object();

        internal static object Invoke<T>(this T obj, string methodname, params object[] args)
        {
            var type = obj.GetType();
            var hash = Hash(type, methodname, args);
            var exists = _cachedMembers.TryGetValue(hash, out var method);
            if (exists) return method?.Invoke(obj, args);
            lock (_lockObj) {
                //Recheck if exist inside lock in case another thread has added it.
                exists = _cachedMembers.TryGetValue(hash, out method);
                if (exists) return method?.Invoke(obj, args);

                var argtypes = GetArgTypes(args);
                var m = GetMember(type, methodname, argtypes);
                method = m == null ? null : new CompiledMethodInfo(m, type);

                var dict = new Dictionary<int, CompiledMethodInfo>(_cachedMembers) { { hash, method } };

                _cachedMembers = dict;
                return method?.Invoke(obj, args);
            }
        }

        private static int Hash(Type type, string methodname, object[] args)
        {
            var hash = 23;
            hash = hash * 31 + type.GetHashCode();
            hash = hash * 31 + methodname.GetHashCode();
            for (var index = 0; index < args.Length; index++) {
                var argtype = args[index].GetType();
                hash = hash * 31 + argtype.GetHashCode();
            }
            return hash;
        }

        private static Type[] GetArgTypes(object[] args)
        {
            var argtypes = new Type[args.Length];
            for (var i = 0; i < args.Length; i++) {
                var argtype = args[i].GetType();
                argtypes[i] = argtype;
            }
            return argtypes;
        }

        private static MethodInfo GetMember(Type type, string name, Type[] argtypes)
        {
            while (true) {
                var methods = type.GetMethods(bindingFlags).Where(m => m.Name == name).ToArray();
                var member = methods.FirstOrDefault(m => m.GetParameters().Select(p => p.ParameterType).SequenceEqual(argtypes)) ??
                             methods.FirstOrDefault(m => m.GetParameters().Select(p => p.ParameterType).ToArray().Matches(argtypes));

                if (member != null) {
                    return member;
                }
                var t = type.GetTypeInfo().BaseType;
                if (t == null) {
                    return null;
                }
                type = t;
            }
        }

        private static bool Matches(this Type[] arr, Type[] args)
        {
            if (arr.Length != args.Length) return false;
            for (var i = 0; i < args.Length; i++) {
                if (!arr[i].IsAssignableFrom(args[i]))
                    return false;
            }
            return true;
        }
    }

    internal class CompiledMethodInfo
    {
        private readonly Func<object, object[], object> _func;

        public CompiledMethodInfo(MethodInfo methodInfo, Type type)
        {
            var instanceExpression = Expression.Parameter(typeof(object), "instance");
            var argumentsExpression = Expression.Parameter(typeof(object[]), "arguments");
            var parameterInfos = methodInfo.GetParameters();

            var argumentExpressions = new Expression[parameterInfos.Length];
            for (var i = 0; i < parameterInfos.Length; ++i) {
                var parameterInfo = parameterInfos[i];
                argumentExpressions[i] = Expression.Convert(Expression.ArrayIndex(argumentsExpression, Expression.Constant(i)), parameterInfo.ParameterType);
            }
            var callExpression = Expression.Call(!methodInfo.IsStatic ? Expression.Convert(instanceExpression, type) : null, methodInfo, argumentExpressions);
            if (callExpression.Type == typeof(void)) {
                var action = Expression.Lambda<Action<object, object[]>>(callExpression, instanceExpression, argumentsExpression).Compile();
                _func = (instance, arguments) => {
                    action(instance, arguments);
                    return null;
                };
            }
            else {
                _func = Expression.Lambda<Func<object, object[], object>>(Expression.Convert(callExpression, typeof(object)), instanceExpression, argumentsExpression).Compile();
            }
        }

        public object Invoke(object instance, params object[] arguments) => _func(instance, arguments);
    }

    #endregion
}