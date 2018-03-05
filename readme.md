
### Test case 1

```sh
> cd src/Host
> dotnet run
```

```sh
> cd src/Client
> dotnet run
```

exception:
```
Unhandled Exception: System.AggregateException: One or more errors occurred. (Named type "Games.Games.Games.Adventure.PlayerStore.State" is invalid: Type string "Games.Games.Games.Adventure.PlayerStore.State" cannot be resolved.) ---> System.TypeAccessException: Named type "Games.Games.Games.Adventure.PlayerStore.State" is invalid: Type string "Games.Games.Games.Adventure.PlayerStore.State" cannot be resolved.
   at Orleans.Serialization.BinaryTokenStreamReaderExtensinons.ReadSpecifiedTypeHeader(IBinaryTokenStreamReader this, SerializationManager serializationManager) in D:\build\agent\_work\15\s\src\Orleans.Core\Serialization\BinaryTokenStreamReader.cs:line 425
   at Orleans.Serialization.BinaryTokenStreamReaderExtensinons.ReadFullTypeHeader(IBinaryTokenStreamReader this, SerializationManager serializationManager, Type expected) in D:\build\agent\_work\15\s\src\Orleans.Core\Serialization\BinaryTokenStreamReader.cs:line 464
   at Orleans.Serialization.BinaryTokenStreamReaderExtensinons.ReadSpecifiedTypeHeader(IBinaryTokenStreamReader this, SerializationManager serializationManager) in D:\build\agent\_work\15\s\src\Orleans.Core\Serialization\BinaryTokenStreamReader.cs:line 338
   at Orleans.Serialization.SerializationManager.DeserializeInner(Type expected, IDeserializationContext context) in D:\build\agent\_work\15\s\src\Orleans.Core\Serialization\SerializationManager.cs:line 1315
   at Orleans.Serialization.BuiltInTypes.DeserializeOrleansResponse(Type expected, IDeserializationContext context) in D:\build\agent\_work\15\s\src\Orleans.Core\Serialization\BuiltInTypes.cs:line 2220
   at Orleans.Serialization.SerializationManager.DeserializeInner(Type expected, IDeserializationContext context) in D:\build\agent\_work\15\s\src\Orleans.Core\Serialization\SerializationManager.cs:line 1364
   at Orleans.Serialization.SerializationManager.Deserialize(Type t, IBinaryTokenStreamReader stream) in D:\build\agent\_work\15\s\src\Orleans.Core\Serialization\SerializationManager.cs:line 1243
   at Orleans.Runtime.Message.GetDeserializedBody(SerializationManager serializationManager) in D:\build\agent\_work\15\s\src\Orleans.Core\Messaging\Message.cs:line 405
   at Orleans.Runtime.GrainReferenceRuntime.ResponseCallback(Message message, TaskCompletionSource`1 context) in D:\build\agent\_work\15\s\src\Orleans.Core\Runtime\GrainReferenceRuntime.cs:line 179
--- End of stack trace from previous location where exception was thrown ---
   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   at Orleans.OrleansTaskExtentions.<<ToTypedTask>g__ConvertAsync4_0>d`1.MoveNext() in D:\build\agent\_work\15\s\src\Orleans.Core\Async\TaskExtensions.cs:line 100
--- End of stack trace from previous location where exception was thrown ---
   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   at System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1.ConfiguredTaskAwaiter.GetResult()
```
