# Multi-Target Cook Profile Contract Design

## Goal

Extend the shared platform build contract so one build request can target multiple runtime variants at once while still sharing cooked content wherever the output format is identical.

This change is needed because platform and backend boundaries do not map cleanly to asset boundaries.

Examples:

- Windows DirectX and Windows Vulkan may share most or all cooked game assets.
- Windows Vulkan and Linux Vulkan may share large parts of their cooked output.
- Audio may be identical across many targets.
- Texture output may diverge only when compression requirements differ.

The contract needs a way to express that intentionally instead of forcing every build to act like one platform means one completely separate cooked output set.

## Problem With The Current Contract

The current base contract assumes one request targets one platform id through one manifest.

That model is too narrow because:

- it treats runtime backend choice as if it must always imply separate asset outputs
- it cannot represent building `windows-directx` and `windows-vulkan` together in one request
- it gives no shared identity for asset outputs that multiple targets can reuse
- it pushes future builders toward ad hoc capability comparison instead of explicit sharing rules

If a Windows builder were implemented on top of the current model, it would immediately need Windows-specific wrapper types or duplicated request orchestration. That would be the wrong foundation.

## Design Principles

### One manifest, many target variants

The caller should be able to resolve content once and request multiple runtime targets in one build invocation.

The manifest should continue to describe the game content being built. The request should describe which runtime targets need that content.

### Sharing must be explicit

Sharing should not be inferred only by comparing many capability fields at runtime.

Instead, target variants should point to explicit cook profiles. Targets that reference the same cook profile share the same cooked asset outputs for that profile.

### Capabilities still matter

Cook profiles should not be opaque names only.

Each profile should also carry structured capability data so humans and orchestration tools can understand why profiles share or diverge. The identity used for sharing is the declared profile id, but the capabilities explain what that profile means.

### Builders group work by cook profile

Builders should not blindly cook once per target variant.

They should group shared content work by cook profile, then layer target-specific runtime work later if needed.

### The base contract stays asset-build focused

This change still only addresses asset and scene cooking.

It does not yet cover:

- engine runtime compilation
- player linking
- Docker packaging
- final installer generation

Those later stages can build on top of this model.

## Top-Level Model Change

The build request should evolve from:

- one manifest
- one target platform id

to:

- one manifest
- one or more target variants
- one or more cook profiles referenced by those target variants

This keeps content resolution separate from runtime targeting.

## New Core Concepts

### PlatformBuildTargetVariant

Represents one concrete runtime target requested by the caller.

It should include:

- `TargetVariantId`
- `PlatformId`
- `RuntimeBackendId`
- `CookProfileId`

Examples:

- `windows-directx`
- `windows-vulkan`
- `linux-vulkan`

This object identifies what the caller wants to ship, not what must be cooked separately.

### PlatformCookProfile

Represents one explicit shared asset-output profile.

It should include:

- `CookProfileId`
- `DisplayName`
- structured capability metadata that explains the output expectations

At minimum the capability metadata should leave room for fields like:

- graphics backend family
- texture compression family
- audio encoding family
- scene/runtime serialization family

The exact capability set can start small, but the profile must be able to grow without redesign.

### PlatformCookProfileCapabilities

Represents the structured capability data attached to a cook profile.

This should be descriptive metadata, not the primary sharing key. Two targets share because they reference the same cook profile id, not because a builder re-derives equivalence every time from field comparisons.

## Updated Existing Types

### PlatformBuildManifest

The manifest should stop carrying a single `TargetPlatformId`.

It should remain focused on resolved project content:

- manifest version
- project id
- project version
- required engine version
- scenes
- loose assets

Targeting information belongs on the build request side instead.

### PlatformBuildRequest

The request should evolve to include:

- `Manifest`
- `TargetVariants`
- `CookProfiles`
- `OutputRoot`
- `WorkingRoot`

Validation rules should include:

- at least one target variant is required
- at least one cook profile is required
- every target variant references an existing cook profile
- duplicate target variant ids are invalid
- duplicate cook profile ids are invalid

### PlatformBuilderDescriptor

The descriptor should stop implying that a builder targets exactly one narrow output identity like `windows-directx`.

It should still carry `TargetPlatformId`, but that platform id should represent the builder family, for example `windows`, not one specific runtime backend.

The descriptor should also grow capability metadata describing which runtime backends and cook-profile families the builder understands.

Recommended additions:

- supported runtime backend ids
- supported cook profile capability families

That allows orchestration tools to reject invalid requests before build.

## Execution Model

The shared interface `IPlatformAssetBuilder` can remain the main execution entry point.

What changes is how the builder interprets the request:

1. Validate manifest compatibility.
2. Validate all requested target variants.
3. Validate all referenced cook profiles.
4. Group target variants by cook profile id.
5. Process shared content work per cook profile.
6. Later, if needed, process target-specific runtime work on top of those shared groups.

For the current foundation phase, builders do not need to write cooked files yet. But they should be able to validate and report the shared grouping model correctly.

## Failure Behavior

The failure model should stay strict.

If a requested target variant, cook profile, scene, or asset is unsupported, the build should fail.

The user should remove unsupported content or change the request before building successfully. Builders should not silently drop content and continue.

Builders should emit a structured diagnostic for the failure and return a failed report.

## Reporting Expectations

Progress and diagnostics remain first-class.

The report model should be expanded enough to explain shared planning decisions:

- which target variants were requested
- which cook profiles were involved
- which target variants mapped to each cook profile
- where validation failed if it did

The foundation does not need full output-file inventories yet, but it does need clear reporting around shared-vs-target-specific grouping.

## Windows Motivation

This change is required before a proper Windows builder foundation can be implemented cleanly.

The Windows builder should be able to accept one request that includes multiple runtime variants like:

- `windows-directx`
- `windows-vulkan`

and later potentially combinations like:

- `windows-vulkan`
- `linux-vulkan`

when those targets intentionally share a cook profile.

Without this shared contract update, the Windows implementation would either:

- duplicate orchestration concepts locally, or
- hardcode assumptions that do not scale beyond one backend

Neither is acceptable for the foundation.

## Initial Implementation Scope

The first implementation pass in this repository should:

- add the new target-variant and cook-profile contract types
- update `PlatformBuildRequest`
- update `PlatformBuildManifest`
- extend `PlatformBuilderDescriptor`
- adjust tests to validate the new request model

It should not yet:

- define every final cook capability field
- implement concrete asset cooking behavior
- define output directory conventions for shared profile groups

Those belong to the first concrete builder repositories that consume this contract.

## Success Criteria

This contract update is successful when:

- one build request can represent multiple runtime targets
- shared cooked-output groups are explicit through cook profiles
- the manifest remains focused on resolved content rather than target selection
- future builders can group work by cook profile without inventing local wrapper abstractions
- Windows, Linux, and Mac builders can all use the same request model when assets are shareable across targets
