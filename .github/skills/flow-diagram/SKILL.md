---
name: flow-diagram
description: "**WORKFLOW SKILL** — Generate industry-standard software engineering flow diagrams. USE FOR: \"create a flowchart\", \"draw a sequence diagram\", \"architecture diagram\", \"state machine diagram\", \"draw.io diagram\", \"mermaid diagram\", \"CI/CD pipeline diagram\", \"system design diagram\", \"ER diagram\", \"class diagram\", \"data flow diagram\". Produces diagrams in Mermaid (validated + previewed) or draw.io XML format. DO NOT USE FOR: general documentation writing, code generation, non-diagram visuals."
argument-hint: "Describe the diagram you need — e.g. 'CI/CD pipeline for a Node.js app' or 'sequence diagram for OAuth2 flow'"
---

# Flow Diagram Skill

You are an expert software engineering diagram creator. You produce clear, professional, industry-standard diagrams in two formats: **Mermaid** (for quick, version-controlled diagrams) and **draw.io XML** (for rich, editable diagrams).

---

## Step 0 — Clarify Requirements

Before creating any diagram, establish these details (infer from context when obvious):

| Question | Why it matters |
|----------|---------------|
| **What process or system?** | Determines nodes, edges, and scope |
| **Which diagram type?** | Flowchart, sequence, state, class, ER, architecture, C4, etc. |
| **Which format?** | Mermaid (default — quick, previewable) or draw.io XML (rich styling) |
| **Audience?** | Developers → technical detail. Stakeholders → high-level overview |
| **Where will it live?** | Repository docs, blog article, wiki, presentation |

If the user doesn't specify a format, **default to draw.io XML** — it produces publication-quality diagrams with rich styling and full drag-and-drop editability. Use Mermaid when the user explicitly requests it, or when quick iteration and version-control friendliness are priorities.

---

## Step 1 — Choose the Right Diagram Type

Match the user's intent to the best diagram type:

| Intent | Diagram Type | Mermaid Keyword |
|--------|-------------|-----------------|
| Process / workflow / pipeline | Flowchart | `flowchart TD` or `flowchart LR` |
| API call chains / request-response | Sequence Diagram | `sequenceDiagram` |
| Object relationships / domain model | Class Diagram | `classDiagram` |
| Database schema | ER Diagram | `erDiagram` |
| Lifecycle / state transitions | State Diagram | `stateDiagram-v2` |
| Cloud / infrastructure / CI/CD | Architecture Diagram | `architecture-beta` |
| System context / containers | C4 Diagram | `C4Context` or `C4Container` |
| Decision tree / conditional logic | Flowchart with diamonds | `flowchart TD` |
| Timeline / milestones | Timeline | `timeline` |
| Sprint / task board | Kanban | `kanban` |
| Git branching strategy | Git Graph | `gitGraph` |
| User flow / UX journey | User Journey | `journey` |
| Module / component blocks | Block Diagram | `block-beta` |

---

## Step 2 — Create the Diagram

### Path A: Mermaid Diagram (Default)

Follow this **mandatory sequence** for every Mermaid diagram:

#### 2A.1 — Load syntax docs
Call `get-syntax-docs-mermaid` with the relevant diagram type file to get current syntax. **Always do this first.** Available types:
- `flowchart.md`, `sequenceDiagram.md`, `classDiagram.md`, `entityRelationshipDiagram.md`
- `stateDiagram.md`, `architecture.md`, `c4.md`, `block.md`, `gitgraph.md`
- `gantt.md`, `kanban.md`, `mindmap.md`, `timeline.md`, `userJourney.md`
- `pie.md`, `quadrantChart.md`, `requirementDiagram.md`, `sankey.md`, `packet.md`, `xyChart.md`

#### 2A.2 — Write the diagram code
Follow these design principles:

**Layout & Direction:**
- Use `TD` (top-down) for hierarchical flows, pipelines, decision trees
- Use `LR` (left-right) for sequential processes, timelines, data flows
- Use `BT` (bottom-up) sparingly — only for dependency trees showing "builds on"

**Node Naming:**
- Use descriptive PascalCase IDs: `BuildStep`, `AuthService`, `UserDB`
- Always add display labels: `BuildStep["Build & Compile"]`
- Never use single-letter IDs like `A`, `B`, `C`

**Node Shapes (Flowchart):**
- Rectangles `["text"]` — process steps, actions
- Rounded `("text")` — start/end terminals
- Diamonds `{"text"}` — decision points
- Cylinders `[("text")]` — databases, storage
- Parallelograms `[/"text"/]` — input/output
- Hexagons `{{"text"}}` — preparation / setup steps
- Stadiums `(["text"])` — events, triggers

**Edges:**
- Solid arrows `-->` for normal flow
- Dotted arrows `-.->` for optional/async paths
- Thick arrows `==>` for critical/primary paths
- Add labels on edges: `-->|"Yes"|` or `-->|"On Failure"|`

**Subgraphs:**
- Group related nodes: `subgraph Title [" ... "]`
- Use for: environments (Dev/Staging/Prod), layers (Frontend/Backend/DB), phases (Build/Test/Deploy)
- Style subgraphs with `style` directives for visual distinction

**Styling:**
- Use `classDef` for reusable styles: `classDef success fill:#2ecc71,stroke:#27ae60,color:#fff`
- Apply with `:::` syntax: `DeployStep:::success`
- Standard color palette:
  - Success/green: `fill:#2ecc71,stroke:#27ae60,color:#fff`
  - Error/red: `fill:#e74c3c,stroke:#c0392b,color:#fff`
  - Warning/amber: `fill:#f39c12,stroke:#e67e22,color:#fff`
  - Info/blue: `fill:#3498db,stroke:#2980b9,color:#fff`
  - Neutral/gray: `fill:#95a5a6,stroke:#7f8c8d,color:#fff`
  - Primary/purple: `fill:#9b59b6,stroke:#8e44ad,color:#fff`

#### 2A.3 — Validate
Call `mermaid-diagram-validator` with the complete diagram code. Fix any errors before proceeding. **Never skip this step.**

#### 2A.4 — Preview
Call `mermaid-diagram-preview` with the validated code (for new diagrams) or `documentUri` (for existing files). Confirm the diagram renders correctly.

#### 2A.5 — Save
Save the diagram as a `.mmd` or `.md` file in the appropriate location:
- Repository docs: `docs/diagrams/`
- Blog article: alongside the article files
- Reference material: `reference/diagrams/`
- Or wherever the user specifies

### Path B: draw.io XML Diagram

Use when the user explicitly requests draw.io format, or needs:
- Rich visual styling (gradients, shadows, custom shapes)
- Drag-and-drop editing after generation
- Presentation-quality output
- Complex layouts that need manual tuning

#### 2B.1 — Write the XML
Follow the draw.io XML structure. Refer to [draw.io XML reference](./references/drawio-reference.md) for the full template and shape library.

**Core structure:**
```xml
<mxfile host="app.diagrams.net">
  <diagram id="unique-id" name="Diagram Name">
    <mxGraphModel dx="1000" dy="700" grid="1" gridSize="10" guides="1" tooltips="1" connect="1" arrows="1" fold="1" page="1" pageScale="1" pageWidth="1100" pageHeight="850" math="0" shadow="0">
      <root>
        <mxCell id="0"/>
        <mxCell id="1" parent="0"/>
        <!-- nodes and edges here -->
      </root>
    </mxGraphModel>
  </diagram>
</mxfile>
```

**Key principles:**
- Use unique, descriptive IDs for every cell
- Set `parent="1"` for all top-level shapes
- Use `vertex="1"` for nodes, `edge="1"` for connections
- Set `source` and `target` attributes on edges to connect nodes
- Position nodes on a grid (multiples of 10) for clean alignment
- Use consistent sizing: standard nodes 120×60, decision diamonds 80×80, start/end circles 60×60

**Layout spacing:**
- Horizontal gap between nodes: 80–120px
- Vertical gap between rows: 60–100px
- Subgroup padding: 20px inside containers

#### 2B.2 — Save
Save as `.drawio` file in the appropriate location.

---

## Step 3 — Industry Diagram Patterns

Use these proven patterns for common software engineering diagrams. Each pattern includes the recommended structure and key elements to include.

### CI/CD Pipeline
```
Source → Build → Test → Security Scan → Stage Deploy → Approval Gate → Prod Deploy → Monitor
```
- Use `LR` direction, subgraphs for phases
- Diamond for approval gate
- Dotted lines for rollback paths
- Color: green for success path, red for failure/rollback

### Microservices Architecture
- Subgraphs for each service boundary
- API Gateway at the entry point
- Message queues between async services
- Databases as cylinders attached to their owning service
- Use `TD` direction

### Authentication Flow (OAuth2/OIDC)
- Sequence diagram with participants: User, Client App, Auth Server, Resource Server
- Show token exchange, redirect, and error paths
- Mark token expiry and refresh flows

### State Machine
- Clear initial and final states
- Guard conditions on transitions: `[condition]`
- Composite states for nested behavior
- Use `stateDiagram-v2`

### Database Schema (ER)
- Entity names in PascalCase
- Show cardinality: `||--o{`, `}o--||`
- Include key attributes (PK, FK)
- Group related entities visually

### System Design / C4
- Use appropriate C4 level:
  - Context: system + external actors
  - Container: apps, databases, message brokers inside the system
  - Component: internal modules within a container
- Label every relationship with verb phrases

---

## Step 4 — Quality Checklist

Before delivering, verify:

- [ ] **Readable**: Every node has a clear label (no cryptic abbreviations)
- [ ] **Directional**: Flow direction is consistent and intuitive
- [ ] **Complete**: All paths shown (including error/alternative paths when relevant)
- [ ] **Styled**: Colors used meaningfully (not decoratively), consistent palette
- [ ] **Validated**: Mermaid diagrams passed `mermaid-diagram-validator`
- [ ] **Previewed**: Mermaid diagrams rendered via `mermaid-diagram-preview`
- [ ] **Labeled edges**: Decision branches and important transitions are labeled
- [ ] **No orphans**: Every node has at least one connection
- [ ] **Proper scope**: Diagram shows one concern clearly, not everything at once
- [ ] **File saved**: Diagram saved to the correct location with descriptive filename

---

## Format Selection Guide

| Criterion | Choose Mermaid | Choose draw.io |
|-----------|---------------|----------------|
| Speed of creation | Faster | Slower |
| Version control friendly | Yes (text-based) | Partial (XML diffs are noisy) |
| In-editor preview | Yes (via MCP tools) | No (open in diagrams.net) |
| Visual richness | Good | Excellent |
| Manual layout control | Limited | Full drag-and-drop |
| CI/CD integration | Easy (render in docs) | Requires export step |
| Collaboration | Git merge-friendly | Conflict-prone |
| Presentation quality | Good | Publication-grade |

**When both are requested:** Create the Mermaid version first (quick iteration), then translate to draw.io XML once the structure is finalized.
