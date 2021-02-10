// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1069:Enums values should not be duplicated", Justification = "<Pending>", Scope = "type", Target = "~T:Sidekick.Platform.Windows.DllImport.SystemMetric")]
[assembly: SuppressMessage("Minor Code Smell", "S1939:Inheritance list should not be redundant", Justification = "<Pending>", Scope = "type", Target = "~T:Sidekick.Platform.Windows.DllImport.SystemMetric")]
[assembly: SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>", Scope = "member", Target = "~M:Sidekick.Platform.Windows.Processes.ProcessProvider.IsUserRunAsAdmin~System.Boolean")]
[assembly: SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>", Scope = "member", Target = "~M:Sidekick.Platform.Windows.Processes.ProcessProvider.IsPathOfExileRunAsAdmin~System.Threading.Tasks.Task{System.Boolean}")]
[assembly: SuppressMessage("Performance", "CA1806:Do not ignore method results", Justification = "<Pending>", Scope = "member", Target = "~M:Sidekick.Platform.Windows.Processes.ProcessProvider.GetActiveWindowProcess~System.Diagnostics.Process")]
[assembly: SuppressMessage("Performance", "CA1806:Do not ignore method results", Justification = "<Pending>", Scope = "member", Target = "~M:Sidekick.Platform.Windows.Processes.ProcessProvider.IsSidekickInFocus~System.Boolean")]
