using System;
using Jellyfin.Plugin.ListenBrainz.Api.Exceptions;
using Jellyfin.Plugin.ListenBrainz.Api.Resources;
using Xunit;

namespace Jellyfin.Plugin.ListenBrainz.Api.Tests;

public class LimitsTests
{
    [Theory]
    [InlineData(0, 0, true)]
    [InlineData(30, 40, false)]
    [InlineData(30, 90, true)]
    [InlineData(4 * TimeSpan.TicksPerMinute, TimeSpan.TicksPerHour, false)]
    public void ListenBrainzLimits_EvaluateSubmitConditions(long position, long runtime, bool throws)
    {
        if (throws)
        {
            Assert.Throws<ListenBrainzException>(() => Limits.AssertSubmitConditions(position, runtime));
        }
        else
        {
            Limits.AssertSubmitConditions(position, runtime);
        }
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(30, 40)]
    [InlineData(30, 90)]
    [InlineData(4 * TimeSpan.TicksPerMinute, TimeSpan.TicksPerHour)]
    public void ListenBrainzLimits_EvaluateSubmitConditions_WithBypass(long position, long runtime)
    {
        // When bypass is enabled, all conditions should pass
        Limits.AssertSubmitConditions(position, runtime, bypassMinimumPlayDuration: true);
    }
}
