# DockPanelSuite Migration Assessment

## Current State

NovelSpider Net10 still uses DockPanelSuite as the active document workspace. `MdiForm` opens the main pages as `DockContent` instances, including task configuration, rule management, rule testing, manual collection, automatic collection, repair/replace tools, update notes, and operational status pages.

V10.5.x introduced `DockWorkspaceService` so new opening paths can depend on an internal workspace facade instead of calling DockPanelSuite directly everywhere. Existing forms still inherit from `DockContent`, so the runtime dependency remains intentional.

## Option 1: Keep DockPanelSuite

Risk: low. Benefit: stable behavior and lowest regression cost.

This keeps current document docking, page reuse, close behavior, menu integration, and theme behavior unchanged. It is the recommended short-term route for V10.x because most modernization work is in background execution, network cancellation, database reads, and collection stability rather than a full UI shell rewrite.

## Option 2: Keep DockPanelSuite Behind A Stronger Facade

Risk: medium-low. Benefit: better architecture and easier future migration.

Continue expanding `DockWorkspaceService` so `MdiForm` and menu handlers ask for pages through named workspace operations. New pages should use the facade first. This reduces direct DockPanelSuite API spread and makes a later UI shell replacement measurable instead of invasive.

Recommended next steps:
- Add named page keys for single-instance tool windows.
- Centralize document reuse, activation, and disposal rules.
- Keep WinForms designer files untouched unless a specific page is being modernized.

## Option 3: Replace With Krypton Toolkit

Risk: high. Benefit: mostly visual and shell-level consistency.

Krypton Toolkit is a UI control/theme toolkit, not a direct DockPanelSuite replacement for the current document docking model. A migration would require redesigning workspace navigation, page lifetime, themes, menus, and all `DockContent` inheritance. The likely regression surface includes automatic collection, manual collection, rule testing, configuration pages, and long-running tool windows.

This should be treated as a separate major UI version, not part of Net10 runtime modernization.

## Recommendation

Keep DockPanelSuite for V10.x. Continue the facade route through `DockWorkspaceService`, modernize high-frequency forms incrementally, and postpone Krypton until a dedicated UI redesign milestone with screenshots, interaction specs, and a full regression matrix.
