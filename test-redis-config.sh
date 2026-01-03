#!/bin/bash

echo "========================================="
echo "Configuration Check"
echo "========================================="
echo ""
echo "appsettings.json (Base):"
grep -A2 "CachingOptions" /Users/kirkeypsalms/Projects/dotnet-starter-kit-10/src/Apps/Apps.Api/appsettings.json
echo ""
echo "appsettings.Development.json:"
grep -A2 "CachingOptions" /Users/kirkeypsalms/Projects/dotnet-starter-kit-10/src/Apps/Apps.Api/appsettings.Development.json
echo ""
echo "========================================="
echo "Building API..."
echo "========================================="
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit-10/src/Apps/Apps.Api
dotnet build --no-incremental -c Debug > /tmp/build.log 2>&1

if [ $? -eq 0 ]; then
    echo "✅ Build succeeded"
else
    echo "❌ Build failed"
    cat /tmp/build.log
    exit 1
fi

echo ""
echo "========================================="
echo "Starting API (watch for [Caching] logs)"
echo "========================================="
echo ""
echo "Expected logs:"
echo "  [Caching] Redis Connection String: ''"
echo "  [Caching] Using DistributedMemoryCache (in-memory) - No Redis configured"
echo "  [MultitenancyModule] Redis Config: '', HasRedis: False"
echo "  [MultitenancyModule] Skipping DistributedCacheStore - using EFCoreStore only"
echo ""
echo "If you see these logs, Redis is properly disabled!"
echo ""
echo "========================================="
echo "Press Ctrl+C to stop"
echo "========================================="
dotnet run

