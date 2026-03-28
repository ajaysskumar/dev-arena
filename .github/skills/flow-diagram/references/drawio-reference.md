# draw.io XML Reference

Complete reference for generating draw.io/diagrams.net compatible XML files.

## File Structure

```xml
<mxfile host="app.diagrams.net">
  <diagram id="unique-id" name="Diagram Name">
    <mxGraphModel dx="1000" dy="700" grid="1" gridSize="10" guides="1"
                  tooltips="1" connect="1" arrows="1" fold="1" page="1"
                  pageScale="1" pageWidth="1100" pageHeight="850"
                  math="0" shadow="0">
      <root>
        <mxCell id="0"/>
        <mxCell id="1" parent="0"/>
        <!-- All shapes and edges go here -->
      </root>
    </mxGraphModel>
  </diagram>
</mxfile>
```

### Multi-page diagrams
Add multiple `<diagram>` elements inside `<mxfile>`, each with a unique `id` and `name`.

---

## Node (Vertex) Template

```xml
<mxCell id="uniqueId" value="Display Text" style="STYLE_STRING"
        parent="1" vertex="1">
  <mxGeometry x="X" y="Y" width="W" height="H" as="geometry"/>
</mxCell>
```

### Attributes
| Attribute | Required | Description |
|-----------|----------|-------------|
| `id` | Yes | Unique identifier within the diagram |
| `value` | Yes | Display text (supports HTML: `&lt;br&gt;` for newlines) |
| `style` | Yes | Semicolon-separated style properties |
| `parent` | Yes | Parent cell ID (`"1"` for top-level, container ID for grouped) |
| `vertex` | Yes | Must be `"1"` for nodes |

---

## Edge (Connection) Template

```xml
<mxCell id="edgeId" value="Label" style="STYLE_STRING"
        parent="1" source="sourceId" target="targetId" edge="1">
  <mxGeometry relative="1" as="geometry">
    <Array as="points">
      <mxPoint x="X" y="Y"/>  <!-- optional waypoints -->
    </Array>
  </mxGeometry>
</mxCell>
```

### Edge attributes
| Attribute | Required | Description |
|-----------|----------|-------------|
| `source` | Yes | ID of the source node |
| `target` | Yes | ID of the target node |
| `edge` | Yes | Must be `"1"` |
| `value` | No | Edge label text |

---

## Shape Library

### Basic Flowchart Shapes

| Shape | Style |
|-------|-------|
| Rectangle (process) | `rounded=0;whiteSpace=wrap;html=1;` |
| Rounded rectangle | `rounded=1;whiteSpace=wrap;html=1;` |
| Diamond (decision) | `rhombus;whiteSpace=wrap;html=1;` |
| Circle | `ellipse;whiteSpace=wrap;html=1;aspect=fixed;` |
| Cylinder (database) | `shape=cylinder3;whiteSpace=wrap;html=1;boundedLbl=1;backgroundOutline=1;size=15;` |
| Parallelogram (I/O) | `shape=parallelogram;perimeter=parallelogramPerimeter;whiteSpace=wrap;html=1;fixedSize=1;` |
| Hexagon | `shape=hexagon;perimeter=hexagonPerimeter2;whiteSpace=wrap;html=1;fixedSize=1;size=15;` |
| Stadium (pill) | `shape=mxgraph.flowchart.terminator;whiteSpace=wrap;html=1;` |
| Document | `shape=document;whiteSpace=wrap;html=1;boundedLbl=1;backgroundOutline=1;size=0.27;` |
| Cloud | `ellipse;shape=cloud;whiteSpace=wrap;html=1;` |
| Arrow (process) | `shape=mxgraph.arrows2.arrow;whiteSpace=wrap;html=1;` |

### Container / Group

```xml
<mxCell id="group1" value="Group Title" style="swimlane;startSize=30;fillColor=#dae8fc;strokeColor=#6c8ebf;"
        parent="1" vertex="1">
  <mxGeometry x="50" y="50" width="300" height="200" as="geometry"/>
</mxCell>
<!-- Child nodes set parent="group1" -->
<mxCell id="child1" value="Step" style="rounded=1;whiteSpace=wrap;html=1;"
        parent="group1" vertex="1">
  <mxGeometry x="20" y="40" width="120" height="40" as="geometry"/>
</mxCell>
```

### Swimlanes

```xml
<mxCell id="pool" value="Pool" style="shape=table;startSize=30;container=1;collapsible=0;childLayout=tableLayout;fixedRows=1;rowLines=0;fontStyle=1;strokeColor=#666666;fillColor=#f5f5f5;"
        parent="1" vertex="1">
  <mxGeometry x="50" y="50" width="600" height="400" as="geometry"/>
</mxCell>
```

---

## Edge Styles

| Style | draw.io Style String |
|-------|---------------------|
| Solid arrow | `endArrow=classic;html=1;` |
| Dashed arrow | `endArrow=classic;html=1;dashed=1;` |
| Thick arrow | `endArrow=classic;html=1;strokeWidth=3;` |
| No arrow (line) | `endArrow=none;html=1;` |
| Bidirectional | `endArrow=classic;startArrow=classic;html=1;` |
| Orthogonal routing | `edgeStyle=orthogonalEdgeStyle;rounded=0;` |
| Curved routing | `edgeStyle=none;curved=1;` |
| Entity relation | `edgeStyle=entityRelationEdgeStyle;` |

---

## Color Palette

### Professional Software Engineering Colors

```
Success / Approved:    fillColor=#d5e8d4;strokeColor=#82b366;fontColor=#2d6a2e
Error / Failed:        fillColor=#f8cecc;strokeColor=#b85450;fontColor=#9c2b2b
Warning / Pending:     fillColor=#fff2cc;strokeColor=#d6b656;fontColor=#8c7a2b
Info / Active:         fillColor=#dae8fc;strokeColor=#6c8ebf;fontColor=#2b5493
Neutral / Default:     fillColor=#f5f5f5;strokeColor=#666666;fontColor=#333333
Primary / Highlight:   fillColor=#e1d5e7;strokeColor=#9673a6;fontColor=#5c3d6e
Dark / Header:         fillColor=#647687;strokeColor=#314354;fontColor=#ffffff
```

### Vivid Colors (for emphasis)
```
Red:     fillColor=#e74c3c;strokeColor=#c0392b;fontColor=#ffffff
Green:   fillColor=#2ecc71;strokeColor=#27ae60;fontColor=#ffffff
Blue:    fillColor=#3498db;strokeColor=#2980b9;fontColor=#ffffff
Purple:  fillColor=#9b59b6;strokeColor=#8e44ad;fontColor=#ffffff
Orange:  fillColor=#f39c12;strokeColor=#e67e22;fontColor=#ffffff
Gray:    fillColor=#95a5a6;strokeColor=#7f8c8d;fontColor=#ffffff
```

---

## Common Style Properties

| Property | Values | Description |
|----------|--------|-------------|
| `fillColor` | `#hex` or `none` | Background fill |
| `strokeColor` | `#hex` | Border color |
| `fontColor` | `#hex` | Text color |
| `fontSize` | number | Font size in points |
| `fontStyle` | `0`=normal, `1`=bold, `2`=italic, `3`=bold+italic | Font weight |
| `rounded` | `0` or `1` | Rounded corners |
| `shadow` | `0` or `1` | Drop shadow |
| `opacity` | `0`–`100` | Transparency |
| `strokeWidth` | number | Border thickness |
| `dashed` | `0` or `1` | Dashed border |
| `whiteSpace` | `wrap` | Enable text wrapping |
| `html` | `1` | Enable HTML in labels |
| `align` | `left`, `center`, `right` | Horizontal text alignment |
| `verticalAlign` | `top`, `middle`, `bottom` | Vertical text alignment |

---

## Sizing Guidelines

| Element | Recommended Size |
|---------|-----------------|
| Standard process box | 120 × 60 |
| Decision diamond | 80 × 80 or 100 × 80 |
| Start/end circle | 60 × 60 |
| Database cylinder | 80 × 80 |
| Swimlane header | startSize=30 |
| Container group padding | 20px inside |
| Gaps between nodes (horizontal) | 80–120px |
| Gaps between rows (vertical) | 60–100px |

---

## Complete Example: CI/CD Pipeline

```xml
<mxfile host="app.diagrams.net">
  <diagram id="cicd-pipeline" name="CI/CD Pipeline">
    <mxGraphModel dx="1000" dy="700" grid="1" gridSize="10" guides="1" tooltips="1" connect="1" arrows="1" fold="1" page="1" pageScale="1" pageWidth="1100" pageHeight="850" math="0" shadow="0">
      <root>
        <mxCell id="0"/>
        <mxCell id="1" parent="0"/>

        <mxCell id="commit" value="Git Commit" style="shape=mxgraph.flowchart.terminator;whiteSpace=wrap;html=1;fillColor=#dae8fc;strokeColor=#6c8ebf;fontSize=12;" parent="1" vertex="1">
          <mxGeometry x="40" y="160" width="120" height="40" as="geometry"/>
        </mxCell>

        <mxCell id="build" value="Build" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#f5f5f5;strokeColor=#666666;fontSize=12;" parent="1" vertex="1">
          <mxGeometry x="220" y="160" width="100" height="40" as="geometry"/>
        </mxCell>

        <mxCell id="test" value="Run Tests" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#f5f5f5;strokeColor=#666666;fontSize=12;" parent="1" vertex="1">
          <mxGeometry x="380" y="160" width="100" height="40" as="geometry"/>
        </mxCell>

        <mxCell id="testPass" value="Pass?" style="rhombus;whiteSpace=wrap;html=1;fillColor=#fff2cc;strokeColor=#d6b656;fontSize=12;" parent="1" vertex="1">
          <mxGeometry x="530" y="140" width="80" height="80" as="geometry"/>
        </mxCell>

        <mxCell id="deploy" value="Deploy to Prod" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#d5e8d4;strokeColor=#82b366;fontSize=12;fontStyle=1;" parent="1" vertex="1">
          <mxGeometry x="680" y="160" width="120" height="40" as="geometry"/>
        </mxCell>

        <mxCell id="fail" value="Notify &amp;&#10;Fix" style="rounded=1;whiteSpace=wrap;html=1;fillColor=#f8cecc;strokeColor=#b85450;fontSize=12;" parent="1" vertex="1">
          <mxGeometry x="530" y="280" width="80" height="50" as="geometry"/>
        </mxCell>

        <!-- Edges -->
        <mxCell id="e1" style="endArrow=classic;html=1;" parent="1" source="commit" target="build" edge="1">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        <mxCell id="e2" style="endArrow=classic;html=1;" parent="1" source="build" target="test" edge="1">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        <mxCell id="e3" style="endArrow=classic;html=1;" parent="1" source="test" target="testPass" edge="1">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        <mxCell id="e4" value="Yes" style="endArrow=classic;html=1;fontStyle=1;fillColor=#d5e8d4;strokeColor=#82b366;" parent="1" source="testPass" target="deploy" edge="1">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
        <mxCell id="e5" value="No" style="endArrow=classic;html=1;dashed=1;fillColor=#f8cecc;strokeColor=#b85450;" parent="1" source="testPass" target="fail" edge="1">
          <mxGeometry relative="1" as="geometry"/>
        </mxCell>
      </root>
    </mxGraphModel>
  </diagram>
</mxfile>
```
