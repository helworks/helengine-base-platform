# Platform Asset Builder Contract Design

## Goal

Define the shared contract that every playable-platform builder must implement for cooking game assets and scenes into platform-readable runtime data.

This foundation is only for asset-related build work. It does not yet cover:

- building the engine runtime
- appending player code
- Docker-based final packaging

Those concerns should be able to layer on top of this contract later without forcing a redesign.

## Scope

The contract in this repository defines:

- builder identity and compatibility metadata
- the fully resolved input manifest shape
- progress and diagnostics reporting
- the final machine-readable build report
- the execution interface all platform builders follow

The contract does not define:

- platform-specific cooked output formats
- asset discovery or dependency resolution
- project scanning
- engine compilation or player linking

## Design Principles

### Builders consume resolved manifests

Builders must never discover content on their own.

The editor/export side is responsible for determining which maps, scenes, and assets the playable build uses. By the time a builder runs, it receives a fully resolved manifest that already contains the exact scene and asset payload references it must process.

### Scenes are first-class

Scenes are a major part of the runtime data model and must not be flattened into generic assets. The manifest must expose scenes as their own top-level input category, with each scene carrying the exact cooked-content inputs it requires.

### Compatibility is explicit

A builder must be able to describe which platform it targets and which engine-version range it supports. Orchestration tools must be able to query compatibility before invoking the build.

### Diagnostics are first-class

Build failures need structured diagnostics, not only thrown exceptions or plain log lines. Diagnostics must be carried both during execution and in the final build report.

### Filesystem outputs are enough for now

The base contract should let builders write to an output root and working root on disk. This keeps the foundation simple and gives platform builders freedom to produce the exact layout their player expects.

## Top-Level Architecture

The repository should provide one shared contract library that platform-specific builders implement.

Each concrete builder package represents one builder implementation for one target platform and can declare:

- builder id
- builder version
- target platform id
- supported engine version range
- supported manifest format version range

The orchestration flow is:

1. The caller prepares a fully resolved build manifest.
2. The caller checks builder compatibility.
3. The caller invokes the builder with the manifest and output paths.
4. The builder emits progress and diagnostics during execution.
5. The builder returns a final build report.

## Core Contract Types

### PlatformBuilderDescriptor

Describes a concrete builder implementation.

It should include:

- `BuilderId`
- `BuilderVersion`
- `TargetPlatformId`
- `SupportedEngineVersions`
- `SupportedManifestVersions`
- optional capability flags for future expansion

### EngineCompatibilityRange

Represents the engine-version range the builder supports.

This should support range-based compatibility rather than exact single-version matching whenever possible.

### PlatformBuildManifest

Represents the fully resolved build input.

It should include:

- manifest format version
- project identity
- project version
- required engine version
- target platform id
- scenes
- loose assets
- optional metadata needed during cooking

This manifest is the canonical input boundary for asset cooking.

### PlatformBuildScene

Represents one resolved scene input.

It should include:

- scene id
- logical scene name
- source path or source identity
- fully expanded asset payload references required by the scene
- scene-specific build metadata already resolved by the caller

The builder must not resolve additional scene dependencies beyond what this object carries.

### PlatformBuildAsset

Represents one resolved loose asset input.

It should include:

- asset id
- logical asset name
- source path or source identity
- payload reference
- already resolved asset metadata required for cooking

### PlatformBuildRequest

Wraps the invocation-specific data for a build run.

It should include:

- build manifest
- output root path
- temporary working root path
- cancellation token

### PlatformBuildProgressUpdate

Represents live progress sent during execution.

It should include:

- stage name
- current item identity
- completed item count
- total item count when known
- human-readable message

### PlatformBuildDiagnostic

Represents a structured diagnostic emitted during a build.

It should include:

- severity
- code
- message
- related scene id when applicable
- related asset id when applicable
- source path or logical source identity when applicable

### PlatformBuildReport

Represents the durable result of the build.

It should include:

- overall success/failure state
- collected diagnostics
- per-scene outcome summary
- per-loose-asset outcome summary
- optional output summary

The report should be rich enough to explain failures even when the process is no longer running.

## Execution Interface

The main execution entry point should be one builder interface, such as `IPlatformAssetBuilder`.

The interface should expose:

- a descriptor property
- a build method that accepts a request plus reporting hooks

The build method should:

- consume the fully resolved manifest
- write cooked outputs under the provided output root
- use the working root for temporary build data
- stream progress updates during execution
- stream diagnostics during execution
- return a final build report

The builder should be free to throw for invalid invocation state, but normal content failures should still surface through diagnostics and the final report.

## Reporting Model

### Progress

Progress should be streamed during the build instead of only summarized at the end. This allows future launcher/editor/build tools to show responsive execution state.

### Diagnostics

Diagnostics should be structured and machine-readable.

At minimum, diagnostics need:

- severity
- stable code
- message
- item context

This allows orchestration layers to show meaningful errors and warnings without parsing log text.

### Build report detail level

The initial report does not need a full artifact inventory, but it must at least preserve:

- whether each scene succeeded or failed
- whether each loose asset succeeded or failed
- which diagnostics were emitted

That keeps the foundation small while still making failures debuggable.

## Responsibilities Boundary

### The caller is responsible for

- discovering which game content belongs in the build
- expanding scene and asset dependencies
- selecting the target platform
- selecting a compatible builder
- preparing output and working roots

### The builder is responsible for

- validating compatibility with the provided request
- reading resolved payloads from the manifest
- converting content into platform-readable runtime data
- writing cooked outputs
- reporting progress and diagnostics

### The builder is not responsible for

- discovering maps or scenes
- traversing dependency graphs
- deciding which assets are reachable
- compiling the engine runtime
- linking player code
- producing final distributable packages

## Future Growth

The naming in this contract should stay broad enough that future orchestration can layer additional stages around it.

Future work can add:

- engine build contracts
- player packaging contracts
- Docker-driven build execution
- final distribution bundling

Those systems should treat asset cooking as one stage in a larger build pipeline, not replace this contract.

## Initial Repository Shape

The repository should start with:

- one solution
- one production contract library
- one test project

There should not be a real platform implementation in production yet. Tests can use fake builders to validate the contract behavior.

## Testing Strategy

The first implementation should cover:

- descriptor and compatibility model behavior
- manifest construction invariants
- build request validation
- progress and diagnostic reporting contract behavior
- final build report behavior

The tests should prove the contract is clear and usable before any Windows, PS2, or Wii builder is introduced.
