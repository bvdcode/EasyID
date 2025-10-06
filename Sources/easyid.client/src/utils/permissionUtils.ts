/**
 * Utility functions for permission checking
 */

/**
 * Check if user has at least one permission matching the given prefix pattern
 * @param userPermissions - Array of user's permissions
 * @param permissionPrefix - Prefix to match (e.g., "easyid.apps")
 * @returns true if at least one permission starts with the prefix
 */
export function hasPermissionPrefix(
  userPermissions: string[] | undefined,
  permissionPrefix: string,
): boolean {
  if (!userPermissions || userPermissions.length === 0) return false;
  const normalized = userPermissions.map((p) => p.toLowerCase());
  const prefix = permissionPrefix.toLowerCase();
  return normalized.some((perm) => perm.startsWith(prefix));
}

/**
 * Check if user has any of the specified permission prefixes
 * @param userPermissions - Array of user's permissions
 * @param permissionPrefixes - Array of prefixes to check
 * @returns true if user has at least one matching permission
 */
export function hasAnyPermissionPrefix(
  userPermissions: string[] | undefined,
  permissionPrefixes: string[],
): boolean {
  return permissionPrefixes.some((prefix) =>
    hasPermissionPrefix(userPermissions, prefix),
  );
}
