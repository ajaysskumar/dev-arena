# Image Resize Tool

## Overview

The `resize-image.sh` script automates image thumbnail generation using ImageMagick. By default, it resizes images to 50% of their original size, but you can specify a custom percentage with the `-r` parameter.

**Location:** `scripts/tools/resize-image.sh`

## Purpose

Instead of manually running ImageMagick commands each time you need to create a thumbnail, this script:
- Automates the resize operation (default 50%, customize with `-r`)
- Generates properly named thumbnails (`<basename>-thumbnail.<ext>`)
- Handles batch processing (multiple images in one command)
- Provides clear output with file sizes and dimensions
- Validates inputs and provides helpful error messages

## Prerequisites

ImageMagick must be installed on your system:

```bash
# macOS
brew install imagemagick

# Ubuntu/Debian
apt-get install imagemagick

# CentOS/RHEL
yum install ImageMagick
```

## Usage

### Basic Syntax

```bash
./scripts/tools/resize-image.sh [-r <percentage>] <image_path> [<image_path2> ...]
```

### Options

- `-r, --resize <percentage>` — Resize percentage (1-100). Default: 50%
- `-h, --help` — Show help message

### Single Image (Default 50%)

```bash
./scripts/tools/resize-image.sh <image_path>
```

Example:
```bash
./scripts/tools/resize-image.sh src/DevCodex/wwwroot/images/blog/ai/bedrock/banner.png
```

### Single Image (Custom Percentage)

```bash
./scripts/tools/resize-image.sh -r <percentage> <image_path>
```

Example:
```bash
./scripts/tools/resize-image.sh -r 40 src/DevCodex/wwwroot/images/blog/ai/bedrock/banner.png
```

### Multiple Images with Custom Percentage

```bash
./scripts/tools/resize-image.sh -r <percentage> <image1> <image2> <image3> ...
```

Example:
```bash
./scripts/tools/resize-image.sh -r 40 src/DevCodex/wwwroot/images/blog/react/module-federation/banner.png src/DevCodex/wwwroot/images/blog/frontend/events-demo/banner.png
```

### Batch Processing with Glob Patterns

```bash
# Default 50%
./scripts/tools/resize-image.sh src/DevCodex/wwwroot/images/blog/**/banner.png

# Custom percentage
./scripts/tools/resize-image.sh -r 40 src/DevCodex/wwwroot/images/blog/**/banner.png
```

Example (all banner images in subdirectories at 40%):
```bash
cd /Users/ajaykumar/Projects/Personal/DevCodex
./scripts/tools/resize-image.sh -r 40 src/DevCodex/wwwroot/images/blog/*/banner.png
```

## Output

For each image processed, the script outputs:
- Status indicator (✓ for success, ✗ for error, ⚠ for warnings)
- Thumbnail file path
- Original dimensions and file size
- Thumbnail dimensions and file size
- Summary with success/error count

Example output:
```
✓ Created thumbnail: src/DevCodex/wwwroot/images/blog/ai/bedrock/banner-thumbnail.png
  Original: 1280x702 (1.3M)
  Thumbnail: 640x351 (325K)

==========================================
Summary: 1/1 successful
All thumbnails created successfully!
```

## Features

✅ **Automatic naming** — Creates `<name>-thumbnail.<ext>` following project conventions

✅ **Custom resize percentage** — Use `-r` to specify any size from 1-100%. Default 50%

✅ **Colored output** — Easy to scan results (green for success, red for errors, yellow for warnings)

✅ **Batch processing** — Handle multiple images in one command

✅ **Safety checks** — Verifies file existence and detects existing thumbnails

✅ **Information rich** — Shows original vs. thumbnail dimensions and file sizes

✅ **Error handling** — Validates ImageMagick installation, input parameters, and percentage range

## Common Workflows

### Resize all Bedrock blog banners to 50% (default):
```bash
cd /Users/ajaykumar/Projects/Personal/DevCodex
./scripts/tools/resize-image.sh src/DevCodex/wwwroot/images/blog/ai/bedrock*/banner.png
```

### Resize a single article's banner to 40%:
```bash
./scripts/tools/resize-image.sh -r 40 src/DevCodex/wwwroot/images/blog/ai/bedrock/inference-profile-cost-tracking/banner.png
```

### Create thumbnails at 75% for a batch of images:
```bash
./scripts/tools/resize-image.sh -r 75 src/DevCodex/wwwroot/images/blog/*/banner.png
```

### Show help:
```bash
./scripts/tools/resize-image.sh --help
```

## Troubleshooting

**ImageMagick not found:**
```
Error: ImageMagick is not installed.
Install it with: brew install imagemagick (macOS) or apt-get install imagemagick (Linux)
```
→ Install ImageMagick using the command above.

**Permission denied:**
```
bash: ./scripts/tools/resize-image.sh: Permission denied
```
→ Make the script executable: `chmod +x scripts/tools/resize-image.sh`

**File not found:**
```
✗ File not found: /path/to/image.png
```
→ Verify the file path and try again.

## Notes

- Original images are never modified; only thumbnails are created.
- Existing thumbnails are overwritten silently (with a warning).
- The script works with PNG, JPG, GIF, WebP, and other common image formats supported by ImageMagick.
- Resize percentage is configurable via `-r` parameter (1-100). Default: 50%
- Percentage must be a valid integer between 1 and 100; decimal values are not supported.

## Integration with Blog Workflow

After creating a new blog article:

1. Add your article's banner image to `src/DevCodex/wwwroot/images/blog/<category>/<article>/banner.png`
2. Run the resize script:
   ```bash
   ./scripts/tools/resize-image.sh src/DevCodex/wwwroot/images/blog/<category>/<article>/banner.png
   ```
3. The thumbnail will automatically be available and picked up by the SiteMap when the article is published (see `SiteMap.cs` GetThumbnailName method).
