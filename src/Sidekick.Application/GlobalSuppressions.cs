// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "<Pending>", Scope = "member", Target = "~M:Sidekick.Application.Initialization.InitializeHandler.Handle(Sidekick.Domain.Initialization.Commands.InitializeCommand,System.Threading.CancellationToken)~System.Threading.Tasks.Task{MediatR.Unit}")]
[assembly: SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "<Pending>", Scope = "member", Target = "~F:Sidekick.Application.Wikis.OpenWikiPageHandler.PoeDb_BaseUri")]
[assembly: SuppressMessage("Major Code Smell", "S1168:Empty arrays and collections should be returned instead of null", Justification = "<Pending>", Scope = "member", Target = "~M:Sidekick.Application.Game.Items.Parser.ParseItemHandler.ParseSockets(Sidekick.Domain.Game.Items.ParsingItem)~System.Collections.Generic.List{Sidekick.Domain.Game.Items.Models.Socket}")]
