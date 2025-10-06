# Initialization and Validation Architecture

## Overview
This document describes the refactored initialization system and validation architecture for user data.

## File Structure

### Validation
- **UpdateUserRequestValidator.cs** - Validates user profile updates with strict username rules:
  - Username: starts with lowercase letter, only lowercase letters/digits/hyphens/underscores, 3-64 chars
  - FirstName: required, min 1 non-whitespace char, max 100 chars
  - LastName/MiddleName: optional, max 100 chars, cannot be only whitespace

### Filters
- **EmptyStringToNullFilter.cs** - Automatically converts empty strings to null in DTOs for consistency

### Initialization Seeders (Requests/Initialization/)
- **UserSeeder.cs** - Creates the initial admin user
- **GroupSeeder.cs** - Creates system groups (admins, users) and assigns user to groups
- **PermissionSeeder.cs** - Creates all system permissions organized by category:
  - Global: `*`, `easyid.*`
  - Users: view, create, edit, delete, change_avatar, change_password
  - Apps: view, create, update, delete
  - Flags: view, create, update, delete
  - Groups: view, create, update, delete, manage_members
  - Permissions: view, create, update, delete, assign
- **RoleSeeder.cs** - Creates roles, links them to groups, and grants permissions:
  - Admin role: all permissions
  - User role: view users, change own avatar, change own password

### Main Handler
- **InitializeInstanceQuery.cs** - Orchestrates the initialization process using all seeders

## Permission Structure

### Constants.SystemPermissions
```
??? Users (builtin.users.*)
??? Apps (builtin.apps.*)
??? Flags (builtin.flags.*)
??? Groups (builtin.groups.*)
??? Permissions (builtin.permissions.*)
```

## Usage

### Validation
FluentValidation automatically validates DTOs on controller actions. Empty strings are normalized to null before validation.

### Initialization
Send a POST request with username and password to initialize the instance. The system:
1. Creates admin user
2. Creates system groups and adds user to them
3. Creates all permissions
4. Creates roles
5. Links roles to groups
6. Grants permissions to roles

## Architecture Benefits
- **Separation of Concerns**: Each seeder handles one domain
- **Testability**: Individual seeders can be tested independently
- **Maintainability**: Adding new permissions/roles is straightforward
- **Clean Code**: No more 200+ line handler methods
- **Type Safety**: All constants are strongly typed
