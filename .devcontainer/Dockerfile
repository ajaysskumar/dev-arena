# syntax=docker/dockerfile:1
# ---------------------------------------------------------------------------
# Dev-container image — .NET stack
# Base: Microsoft's official devcontainer image (non-root user 'vscode',
#       git, curl, common utilities already included).
#
# This is a DEVELOPMENT image, not a production runtime.
# Keep it as an ASPNET SDK image so developers can build, test, and debug.
# ---------------------------------------------------------------------------

ARG VARIANT=10.0
FROM mcr.microsoft.com/devcontainers/dotnet:${VARIANT}

# ---------------------------------------------------------------------------
# System-level packages (installed as root).
# Add anything here that apt can provide and that isn't covered by Features.
# ---------------------------------------------------------------------------
USER root

RUN apt-get update && apt-get install -y --no-install-recommends \
    jq \
    && rm -rf /var/lib/apt/lists/*

# ---------------------------------------------------------------------------
# .NET global tools (installed into a shared, PATH-visible location).
#
# Installing under /usr/local makes tools available to all users including
# the non-root 'vscode' user without needing to modify PATH.
# ---------------------------------------------------------------------------
ENV DOTNET_ROOT=/usr/share/dotnet
ENV PATH="${PATH}:/usr/local/share/dotnet-tools"

RUN mkdir -p /usr/local/share/dotnet-tools \
    # Entity Framework Core CLI — migrations, database scaffolding
    && dotnet tool install --tool-path /usr/local/share/dotnet-tools dotnet-ef \
    # CSharpier — opinionated C# formatter
    && dotnet tool install --tool-path /usr/local/share/dotnet-tools csharpier \
    # Ensure the tool directory is accessible to non-root users
    && chmod -R a+rx /usr/local/share/dotnet-tools

# ---------------------------------------------------------------------------
# Drop back to the non-root 'vscode' user for all subsequent operations.
# ---------------------------------------------------------------------------
USER vscode
