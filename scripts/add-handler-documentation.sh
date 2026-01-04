#!/bin/bash

# Phase 2: Add XML Documentation to Handlers
# This script adds standardized XML documentation to handler classes and their commands/queries

MODULE_PATH="/Users/kirkeypsalms/Projects/dotnet-starter-kit-10/src/Modules/Accounting/Module.Accounting/Features/v1"

# Function to extract operation name from path
get_operation_name() {
    local path=$1
    local filename=$(basename "$path")
    # Remove "Handler.cs" suffix
    echo "${filename%Handler.cs}"
}

# Function to determine if it's a Command or Query
get_command_type() {
    local path=$1
    if grep -q ": IQuery" "$path"; then
        echo "Query"
    else
        echo "Command"
    fi
}

# Function to add documentation
add_documentation() {
    local file=$1
    local operation=$(get_operation_name "$file")
    local entity=$(echo "$file" | sed -n 's/.*Features\/v1\/\([^/]*\).*/\1/p')
    local command_type=$(get_command_type "$file")
    
    # Check if already documented
    if grep -q "/// <summary>" "$file"; then
        return
    fi
    
    # Create backup
    cp "$file" "${file}.bak"
    
    # Add documentation using sed
    sed -i '' "1i\\
/// <summary>\\
/// Handler for the $operation $command_type.\\
/// Processes the $operation request for the $entity aggregate.\\
/// </summary>\\
" "$file"
    
    echo "Documented: $operation"
}

# Process all handlers
count=0
for handler_file in $(find "$MODULE_PATH" -name "*Handler.cs" -type f); do
    add_documentation "$handler_file"
    ((count++))
    
    if (( count % 50 == 0 )); then
        echo "Processed $count handlers..."
    fi
done

echo "Documentation complete: $count handlers updated"
