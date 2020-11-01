using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace KoKo.Property {

    internal class NativeEventListener {

        private const string ActionFieldName        = "EventSender";
        private const string EventHandlerMethodName = "EventReceiver";

        private static readonly IDictionary<Type[], Type> EventProxyStructCache = new Dictionary<Type[], Type>();

        private static ModuleBuilder? _moduleBuilder;
        private static long           _classNameCounter = 0;

        public event EventHandler? OnEvent;

        public NativeEventListener(object nativeObject, string nativeEventName) {
            EventInfo nativeEvent = nativeObject.GetType().GetTypeInfo().GetEvent(nativeEventName);

            Type        handlerType  = nativeEvent.EventHandlerType;
            MethodInfo? invokeMethod = handlerType.GetMethod("Invoke");
            if (invokeMethod == null) {
                throw new ArgumentException($"Event {nativeObject.GetType().Name}.{nativeEventName} does not have an Invoke() method");
            }

            ParameterInfo[] parameterInfo  = invokeMethod.GetParameters();
            Type[]          parameterTypes = parameterInfo.Select(param => param.ParameterType).ToArray();

            if (!EventProxyStructCache.TryGetValue(parameterTypes, out Type eventProxyStructType)) {
                if (_moduleBuilder == null) {
                    AssemblyName    assemblyName    = new AssemblyName("DynamicTypes");
                    AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
                    _moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
                }

                long classNumber = Interlocked.Increment(ref _classNameCounter);
                TypeBuilder typeBuilder = _moduleBuilder.DefineType("EventProxyStruct" + classNumber, TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.SequentialLayout |
                    TypeAttributes.AnsiClass | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit, typeof(ValueType));
                FieldBuilder  actionFieldBuilder  = typeBuilder.DefineField(ActionFieldName, typeof(Action), FieldAttributes.Public);
                MethodBuilder eventHandlerBuilder = typeBuilder.DefineMethod(EventHandlerMethodName, MethodAttributes.Public, invokeMethod.ReturnType, parameterTypes);

                ILGenerator il = eventHandlerBuilder.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, actionFieldBuilder);
                il.Emit(OpCodes.Callvirt, typeof(Action).GetMethod(nameof(Action.Invoke))!);
                il.Emit(OpCodes.Ret);

                eventProxyStructType = typeBuilder.CreateType();

                EventProxyStructCache[parameterTypes] = eventProxyStructType;
            }

            object eventProxyStruct = Activator.CreateInstance(eventProxyStructType);
            eventProxyStructType.GetField(ActionFieldName).SetValue(eventProxyStruct, (Action) (() => { OnEvent?.Invoke(this, EventArgs.Empty); }));
            MethodInfo eventReceiverMethod = eventProxyStructType.GetMethod(EventHandlerMethodName)!;

            Delegate eventHandlerDelegate = Delegate.CreateDelegate(handlerType, eventProxyStruct, eventReceiverMethod);
            nativeEvent.AddEventHandler(nativeObject, eventHandlerDelegate);
        }

    }

}