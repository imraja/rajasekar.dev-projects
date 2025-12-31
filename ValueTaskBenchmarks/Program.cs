using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

[MemoryDiagnoser] // shows allocations
public class ValueTaskBenchmarks
{
    private readonly int _value = 42;

    // -----------------------------
    // Sync-fast path (no async work)
    // -----------------------------
    // Baseline: Task.FromResult - allocates a Task object (may be pooled by runtime for small ints, but still shows allocation)
    [Benchmark(Baseline = true)]
    public Task<int> Task_FromResult()
        => Task.FromResult(_value);

    // ValueTask constructed around a result - no allocation when returning synchronously
    [Benchmark]
    public ValueTask<int> ValueTask_FromResult()
        => new ValueTask<int>(_value);

    // Non-generic: Task.CompletedTask vs default(ValueTask)
    [Benchmark]
    public Task Task_CompletedTask()
        => Task.CompletedTask;

    [Benchmark]
    public ValueTask ValueTask_CompletedValueTask()
        => default; // ValueTask completed synchronously, no allocation

    // --------------------------------
    // Async path (method does async work)
    // --------------------------------
    // Method genuinely asynchronous (yield) - both must allocate underlying Task
    [Benchmark]
    public async Task<int> Task_Async()
    {
        await Task.Yield(); // force async completion
        return _value;
    }

    [Benchmark]
    public async ValueTask<int> ValueTask_Async()
    {
        await Task.Yield(); // forces async path inside ValueTask
        return _value;
    }

    // ------------------------------------------------------
    // Conversion cost: ValueTask -> Task (AsTask) and awaiting
    // ------------------------------------------------------
    // If consumers call AsTask() to get a Task, that may allocate or cause extra overhead
    [Benchmark]
    public async Task<int> ValueTask_AsTask_Converted_ThenAwait()
    {
        var vt = new ValueTask<int>(_value);
        return await vt.AsTask();
    }

    // ------------------------------------------------------
    // Misuse scenario: awaiting a ValueTask multiple times
    // (safe pattern: convert to Task once and await that repeatedly)
    // ------------------------------------------------------
    [Benchmark]
    public async Task<int> ValueTask_Await_AsTask_Twice()
    {
        // Simulate consumer that wants to "await" the same operation twice
        var vt = new ValueTask<int>(_value);
        var t = vt.AsTask();    // convert to Task once
        var a = await t;
        var b = await t;        // awaiting same Task twice — cheap after conversion
        return a + b;
    }

    // If one tries to await the ValueTask struct twice without conversion, it is allowed only
    // in some cases and can be error-prone. We show a safe pattern (AsTask).
    //
    // ------------------------------------------------------
    // Boxing/Interface cost scenario (ValueTask<object> -> object)
    // ------------------------------------------------------
    [Benchmark]
    public ValueTask<object> ValueTask_Object_FromResult()
        => new ValueTask<object>(_value); // will box int -> object, but still no Task allocation for ValueTask itself

    [Benchmark]
    public Task<object> Task_Object_FromResult()
        => Task.FromResult<object>(_value); // Task<object> allocation / pooling behavior visible in results

    // ----------------------------
    // Control: simple synchronous op
    // ----------------------------
    [Benchmark]
    public int Sync_Direct()
        => _value; // zero allocations; useful as a sanity baseline
}

public static class Program
{
    public static void Main(string[] args)
    {
        // Run the benchmarks
        BenchmarkRunner.Run<ValueTaskBenchmarks>();
    }
}
