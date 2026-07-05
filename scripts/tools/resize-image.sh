#!/bin/bash

###############################################################################
# resize-image.sh
# 
# Generates thumbnail images using ImageMagick.
# Follows the project's naming convention: <basename>-thumbnail.<ext>
#
# Usage:
#   ./resize-image.sh [-r <percentage>] <image_file> [<image_file2> ...]
#   ./resize-image.sh -r 40 /path/to/banner.png       # 40% resize
#   ./resize-image.sh -r 50 *.png                      # 50% resize (default)
#   ./resize-image.sh /path/to/banner.png              # Uses default 50%
#
# Options:
#   -r <percentage>    Resize percentage (1-100). Default: 50%
#
# Example:
#   ./scripts/tools/resize-image.sh -r 40 src/DevCodex/wwwroot/images/blog/ai/bedrock/banner.png
#   ./scripts/tools/resize-image.sh -r 50 *.png
#
# Output:
#   Creates banner-thumbnail.png in the same directory as the original.
#   Original file is untouched.
#
###############################################################################

set -e  # Exit on error

# Color codes for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Check if ImageMagick is installed
if ! command -v magick &> /dev/null && ! command -v convert &> /dev/null; then
    echo -e "${RED}Error: ImageMagick is not installed.${NC}"
    echo "Install it with: brew install imagemagick (macOS) or apt-get install imagemagick (Linux)"
    exit 1
fi

# Default resize percentage
resize_percent=50

# Parse options
while [[ $# -gt 0 ]]; do
    case "$1" in
        -r|--resize)
            if [ -z "$2" ]; then
                echo -e "${RED}Error: -r option requires a percentage argument.${NC}"
                exit 1
            fi
            resize_percent="$2"
            # Validate percentage is a number between 1-100
            if ! [[ "$resize_percent" =~ ^[0-9]+$ ]] || [ "$resize_percent" -lt 1 ] || [ "$resize_percent" -gt 100 ]; then
                echo -e "${RED}Error: Resize percentage must be a number between 1 and 100.${NC}"
                exit 1
            fi
            shift 2
            ;;
        -h|--help)
            echo -e "${YELLOW}Usage: $0 [-r <percentage>] <image_file> [<image_file2> ...]${NC}"
            echo ""
            echo "Options:"
            echo "  -r, --resize <percentage>   Resize percentage (1-100). Default: 50%"
            echo ""
            echo "Examples:"
            echo "  $0 banner.png                              # Uses default 50%"
            echo "  $0 -r 40 banner.png                        # Resize to 40%"
            echo "  $0 -r 50 *.png                             # Batch process with 50%"
            echo "  $0 -r 75 /path/to/image1.png /path/to/image2.png"
            echo ""
            exit 0
            ;;
        -*)
            echo -e "${RED}Error: Unknown option '$1'${NC}"
            echo "Use '$0 --help' for usage information."
            exit 1
            ;;
        *)
            # First non-option argument, stop parsing options
            break
            ;;
    esac
done

# Check if at least one image file was provided
if [ $# -eq 0 ]; then
    echo -e "${YELLOW}Usage: $0 [-r <percentage>] <image_file> [<image_file2> ...]${NC}"
    echo ""
    echo "Examples:"
    echo "  $0 banner.png"
    echo "  $0 -r 40 banner.png"
    echo "  $0 -r 50 *.png"
    echo ""
    echo "Use '$0 --help' for full usage information."
    exit 1
fi

# Counter for summary
total_files=0
success_count=0
error_count=0

# Process each image file
for image_file in "$@"; do
    total_files=$((total_files + 1))
    
    # Check if file exists
    if [ ! -f "$image_file" ]; then
        echo -e "${RED}✗ File not found: $image_file${NC}"
        error_count=$((error_count + 1))
        continue
    fi
    
    # Extract directory, basename, and extension
    dir=$(dirname "$image_file")
    filename=$(basename "$image_file")
    name="${filename%.*}"
    ext="${filename##*.}"
    
    # Generate thumbnail filename
    thumbnail="${dir}/${name}-thumbnail.${ext}"
    
    # Check if thumbnail already exists
    if [ -f "$thumbnail" ]; then
        echo -e "${YELLOW}⚠ Thumbnail already exists: $thumbnail (overwriting)${NC}"
    fi
    
    # Get original dimensions for reference
    if command -v magick &> /dev/null; then
        original_dims=$(magick identify -format "%wx%h" "$image_file" 2>/dev/null || echo "unknown")
    else
        original_dims=$(convert identify -format "%wx%h" "$image_file" 2>/dev/null || echo "unknown")
    fi
    
    # Resize to specified percentage using ImageMagick
    if command -v magick &> /dev/null; then
        magick "$image_file" -resize ${resize_percent}% "$thumbnail" 2>/dev/null
    else
        convert "$image_file" -resize ${resize_percent}% "$thumbnail" 2>/dev/null
    fi
    
    # Verify the thumbnail was created
    if [ -f "$thumbnail" ]; then
        # Get thumbnail dimensions
        if command -v magick &> /dev/null; then
            thumb_dims=$(magick identify -format "%wx%h" "$thumbnail" 2>/dev/null || echo "unknown")
        else
            thumb_dims=$(convert identify -format "%wx%h" "$thumbnail" 2>/dev/null || echo "unknown")
        fi
        
        # Get file sizes
        original_size=$(ls -lh "$image_file" | awk '{print $5}')
        thumbnail_size=$(ls -lh "$thumbnail" | awk '{print $5}')
        
        echo -e "${GREEN}✓ Created thumbnail: $thumbnail${NC}"
        echo "  Original: ${original_dims} (${original_size})"
        echo "  Thumbnail: ${thumb_dims} (${thumbnail_size})"
        success_count=$((success_count + 1))
    else
        echo -e "${RED}✗ Failed to create thumbnail: $thumbnail${NC}"
        error_count=$((error_count + 1))
    fi
done

# Summary
echo ""
echo "=========================================="
echo "Summary: ${success_count}/${total_files} successful"
if [ $error_count -gt 0 ]; then
    echo -e "${RED}Errors: ${error_count}${NC}"
    exit 1
else
    echo -e "${GREEN}All thumbnails created successfully!${NC}"
fi
