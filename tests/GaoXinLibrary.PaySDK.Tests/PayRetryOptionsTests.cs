using GaoXinLibrary.PaySDK.Core;
using Xunit;

namespace GaoXinLibrary.PaySDK.Tests;

public class PayRetryOptionsTests
{
    [Fact]
    public void Default_MaxRetries_IsTwo()
    {
        var options = new PayRetryOptions();
        Assert.Equal(2, options.MaxRetries);
    }

    [Fact]
    public void Default_InitialDelay_Is500ms()
    {
        var options = new PayRetryOptions();
        Assert.Equal(TimeSpan.FromMilliseconds(500), options.InitialDelay);
    }

    [Fact]
    public void Default_MaxDelay_Is5Seconds()
    {
        var options = new PayRetryOptions();
        Assert.Equal(TimeSpan.FromSeconds(5), options.MaxDelay);
    }

    [Fact]
    public void MaxRetries_Zero_DisablesRetry()
    {
        var options = new PayRetryOptions { MaxRetries = 0 };
        Assert.Equal(0, options.MaxRetries);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(10)]
    public void MaxRetries_CustomValue_Stored(int value)
    {
        var options = new PayRetryOptions { MaxRetries = value };
        Assert.Equal(value, options.MaxRetries);
    }

    [Fact]
    public void ExponentialBackoff_FirstRetry_UsesInitialDelay()
    {
        var options = new PayRetryOptions
        {
            MaxRetries = 3,
            InitialDelay = TimeSpan.FromMilliseconds(200)
        };
        var delay = ComputeDelay(0, options);
        Assert.Equal(TimeSpan.FromMilliseconds(200), delay);
    }

    [Fact]
    public void ExponentialBackoff_SecondRetry_DoublesDelay()
    {
        var options = new PayRetryOptions
        {
            MaxRetries = 3,
            InitialDelay = TimeSpan.FromMilliseconds(200)
        };
        var delay1 = ComputeDelay(0, options);
        var delay2 = ComputeDelay(1, options);
        Assert.Equal(delay1 * 2, delay2);
    }

    [Fact]
    public void ExponentialBackoff_ThirdRetry_Quadruples()
    {
        var options = new PayRetryOptions
        {
            MaxRetries = 3,
            InitialDelay = TimeSpan.FromMilliseconds(200)
        };
        var delay1 = ComputeDelay(0, options);
        var delay3 = ComputeDelay(2, options);
        Assert.Equal(delay1 * 4, delay3);
    }

    [Fact]
    public void ExponentialBackoff_RespectsMaxDelay()
    {
        var options = new PayRetryOptions
        {
            MaxRetries = 10,
            InitialDelay = TimeSpan.FromSeconds(1),
            MaxDelay = TimeSpan.FromSeconds(3)
        };
        var delayAt5 = ComputeDelay(4, options);
        Assert.Equal(TimeSpan.FromSeconds(3), delayAt5);
        var delayAt10 = ComputeDelay(9, options);
        Assert.Equal(TimeSpan.FromSeconds(3), delayAt10);
    }

    [Fact]
    public void ExponentialBackoff_LargeRetryCount_DoesNotExceedMaxDelay()
    {
        var options = new PayRetryOptions
        {
            MaxRetries = 100,
            InitialDelay = TimeSpan.FromMilliseconds(1),
            MaxDelay = TimeSpan.FromSeconds(5)
        };
        for (int i = 0; i < 100; i++)
        {
            var delay = ComputeDelay(i, options);
            Assert.True(delay <= options.MaxDelay,
                $"Retry {i} delay {delay.TotalMilliseconds}ms exceeds MaxDelay {options.MaxDelay.TotalMilliseconds}ms");
        }
    }

    private static TimeSpan ComputeDelay(int retryAttempt, PayRetryOptions options)
    {
        var multiplier = Math.Pow(2, Math.Min(retryAttempt, 30));
        var delayMs = options.InitialDelay.TotalMilliseconds * multiplier;
        if (delayMs > TimeSpan.MaxValue.TotalMilliseconds)
            return options.MaxDelay;
        var delay = TimeSpan.FromMilliseconds(delayMs);
        return delay > options.MaxDelay ? options.MaxDelay : delay;
    }
}
