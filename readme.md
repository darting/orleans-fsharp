
### Test case 2

```sh
> cd src/Host
> dotnet run
```
```sh
> cd src/Client
> dotnet run
```

exception on silo host:
```
fail: Orleans.Runtime.GrainTypeManager[100092]
      Cannot instantiate generic class Grains.Say.GameGrain`2[[System.Int32, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e],[Games.Game1+GameAction, Games, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]]
System.ArgumentException: An item with the same key has already been added. Key: Grains.Say.GameGrain`2
   at System.ThrowHelper.ThrowAddingDuplicateWithKeyArgumentException(Object key)
   at System.Collections.Generic.Dictionary`2.TryInsert(TKey key, TValue value, InsertionBehavior behavior)
   at Orleans.Runtime.GrainTypeManager.get_Item(String className) in D:\build\agent\_work\15\s\src\Orleans.Runtime\GrainTypeManager\GrainTypeManager.cs:line 119
fail: Orleans.Runtime.Dispatcher[101540]
      Error creating activation for Grains.Say.GameGrain`2. Message NewPlacement Request S127.0.0.1:11111:257932102*cli/faaefb9f@f09f15be->S127.0.0.1:11111:257932102*grn/B2CADC52/00000000+game1::ms::darting@43d6b5b0 #4:
System.Collections.Generic.KeyNotFoundException: Cannot instantiate generic class Grains.Say.GameGrain`2[[System.Int32, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e],[Games.Game1+GameAction, Games, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]] ---> System.ArgumentException: An item with the same key has already been added. Key: Grains.Say.GameGrain`2
   at System.ThrowHelper.ThrowAddingDuplicateWithKeyArgumentException(Object key)
   at System.Collections.Generic.Dictionary`2.TryInsert(TKey key, TValue value, InsertionBehavior behavior)
   at Orleans.Runtime.GrainTypeManager.get_Item(String className) in D:\build\agent\_work\15\s\src\Orleans.Runtime\GrainTypeManager\GrainTypeManager.cs:line 119
   --- End of inner exception stack trace ---
   at Orleans.Runtime.GrainTypeManager.get_Item(String className) in D:\build\agent\_work\15\s\src\Orleans.Runtime\GrainTypeManager\GrainTypeManager.cs:line 125
   at Orleans.Runtime.Catalog.CreateGrainInstance(String grainTypeName, ActivationData data, String genericArguments) in D:\build\agent\_work\15\s\src\Orleans.Runtime\Catalog\Catalog.cs:line 712
   at Orleans.Runtime.Catalog.SetupActivationInstance(ActivationData result, String grainType, String genericArguments) in D:\build\agent\_work\15\s\src\Orleans.Runtime\Catalog\Catalog.cs:line 526
   at Orleans.Runtime.Catalog.GetOrCreateActivation(ActivationAddress address, Boolean newPlacement, String grainType, String genericArguments, Dictionary`2 requestContextData, Task& activatedPromise) in D:\build\agent\_work\15\s\src\Orleans.Runtime\Catalog\Catalog.cs:line 515
   at Orleans.Runtime.Dispatcher.ReceiveMessage(Message message) in D:\build\agent\_work\15\s\src\Orleans.Runtime\Core\Dispatcher.cs:line 77
info: Orleans.Runtime.Messaging.Gateway[101302]
      Recorded closed socket from endpoint 127.0.0.1:56140, client ID *cli/faaefb9f.
```

exception on client:
```
Unhandled Exception: System.AggregateException: One or more errors occurred. (Error creating activation for Grains.Say.GameGrain`2. Message NewPlacement Request S127.0.0.1:11111:257932102*cli/faaefb9f@f09f15be->S127.0.0.1:11111:257932102*grn/B2CADC52/00000000+game1::ms::darting@43d6b5b0 #4: ) ---> Orleans.Runtime.OrleansException: Error creating activation for Grains.Say.GameGrain`2. Message NewPlacement Request S127.0.0.1:11111:257932102*cli/faaefb9f@f09f15be->S127.0.0.1:11111:257932102*grn/B2CADC52/00000000+game1::ms::darting@43d6b5b0 #4:  ---> System.Collections.Generic.KeyNotFoundException: Cannot instantiate generic class Grains.Say.GameGrain`2[[System.Int32, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e],[Games.Game1+GameAction, Games, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]] ---> System.ArgumentException: An item with the same key has already been added. Key: Grains.Say.GameGrain`2
   at System.ThrowHelper.ThrowAddingDuplicateWithKeyArgumentException(Object key)
   at System.Collections.Generic.Dictionary`2.TryInsert(TKey key, TValue value, InsertionBehavior behavior)
   at Orleans.Runtime.GrainTypeManager.get_Item(String className) in D:\build\agent\_work\15\s\src\Orleans.Runtime\GrainTypeManager\GrainTypeManager.cs:line 119
   --- End of inner exception stack trace ---
   at Orleans.Runtime.GrainTypeManager.get_Item(String className) in D:\build\agent\_work\15\s\src\Orleans.Runtime\GrainTypeManager\GrainTypeManager.cs:line 125
   at Orleans.Runtime.Catalog.CreateGrainInstance(String grainTypeName, ActivationData data, String genericArguments) in D:\build\agent\_work\15\s\src\Orleans.Runtime\Catalog\Catalog.cs:line 712
   at Orleans.Runtime.Catalog.SetupActivationInstance(ActivationData result, String grainType, String genericArguments) in D:\build\agent\_work\15\s\src\Orleans.Runtime\Catalog\Catalog.cs:line 526
   at Orleans.Runtime.Catalog.GetOrCreateActivation(ActivationAddress address, Boolean newPlacement, String grainType, String genericArguments, Dictionary`2 requestContextData, Task& activatedPromise) in D:\build\agent\_work\15\s\src\Orleans.Runtime\Catalog\Catalog.cs:line 515
   at Orleans.Runtime.Dispatcher.ReceiveMessage(Message message) in D:\build\agent\_work\15\s\src\Orleans.Runtime\Core\Dispatcher.cs:line 77
   --- End of inner exception stack trace ---
   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   at Orleans.OrleansTaskExtentions.<<ToTypedTask>g__ConvertAsync4_0>d`1.MoveNext() in D:\build\agent\_work\15\s\src\Orleans.Core\Async\TaskExtensions.cs:line 100
--- End of stack trace from previous location where exception was thrown ---
   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   at System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1.ConfiguredTaskAwaiter.GetResult()
   at Program.worker1@25-8.Invoke(Unit unitVar0) in C:\Users\Valor\github\valor\orleans-fsharp\src\Client\Program.fs:line 25
   at Giraffe.TaskBuilder.StepStateMachine`1.nextAwaitable()
--- End of stack trace from previous location where exception was thrown ---
   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
   at System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1.ConfiguredTaskAwaiter.GetResult()
   at Program.t@116-11.Invoke(Unit unitVar0) in C:\Users\Valor\github\valor\orleans-fsharp\src\Client\Program.fs:line 116
   at Giraffe.TaskBuilder.tryFinally[a](FSharpFunc`2 step, FSharpFunc`2 fin)
   at Giraffe.TaskBuilder.StepStateMachine`1.nextAwaitable()
   --- End of inner exception stack trace ---
   at System.Threading.Tasks.Task.ThrowIfExceptional(Boolean includeTaskCanceledExceptions)
   at System.Threading.Tasks.Task.Wait(Int32 millisecondsTimeout, CancellationToken cancellationToken)
   at System.Threading.Tasks.Task.Wait()
```