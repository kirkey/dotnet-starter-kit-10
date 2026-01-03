#!/bin/bash

# Microfinance Entity Migration Script
# This script migrates an entity from the old structure to the new vertical slice architecture

ENTITY_NAME=$1
MODULE_PATH="/Users/kirkeypsalms/Projects/dotnet-starter-kit-10/src/Modules/Microfinance/Modules.Microfinance"
CONTRACTS_PATH="/Users/kirkeypsalms/Projects/dotnet-starter-kit-10/src/Modules/Microfinance/Modules.Microfinance.Contracts"
SOURCE_PATH="/Users/kirkeypsalms/Projects/ExternalProjects/src/api/modules/MicroFinance"

if [ -z "$ENTITY_NAME" ]; then
    echo "Usage: $0 <EntityName>"
    echo "Example: $0 Branch"
    exit 1
fi

ENTITY_PLURAL="${ENTITY_NAME}s"
ENTITY_LOWER=$(echo "$ENTITY_NAME" | tr '[:upper:]' '[:lower:]')

echo "Migrating $ENTITY_NAME..."

# Create directories
mkdir -p "$MODULE_PATH/Domain"
mkdir -p "$MODULE_PATH/Data/Configurations"
mkdir -p "$MODULE_PATH/Features/v1/$ENTITY_PLURAL/Create$ENTITY_NAME"
mkdir -p "$MODULE_PATH/Features/v1/$ENTITY_PLURAL/Get$ENTITY_NAME"
mkdir -p "$MODULE_PATH/Features/v1/$ENTITY_PLURAL/Get${ENTITY_PLURAL}"
mkdir -p "$MODULE_PATH/Features/v1/$ENTITY_PLURAL/Update$ENTITY_NAME"
mkdir -p "$MODULE_PATH/Features/v1/$ENTITY_PLURAL/Delete$ENTITY_NAME"
mkdir -p "$CONTRACTS_PATH/v1/$ENTITY_PLURAL"

echo "✓ Directories created"

# Copy and adapt domain entity
if [ -f "$SOURCE_PATH/MicroFinance.Domain/Entities/$ENTITY_NAME.cs" ]; then
    cp "$SOURCE_PATH/MicroFinance.Domain/Entities/$ENTITY_NAME.cs" "$MODULE_PATH/Domain/$ENTITY_NAME.cs"
    echo "✓ Domain entity copied"
fi

echo "Migration structure ready for $ENTITY_NAME"
echo "Next: Manually adapt the files or use templates"
