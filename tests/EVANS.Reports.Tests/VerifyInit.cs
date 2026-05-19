using System.Runtime.CompilerServices;
using VerifyTests;

namespace EVANS.Reports.Tests;

public static class VerifyInit
{
    [ModuleInitializer]
    public static void Initialize()
    {
        // No scrubbing needed: we snapshot extracted text (not raw PDF bytes),
        // so there are no per-run timestamps or metadata fields to strip.
    }
}
