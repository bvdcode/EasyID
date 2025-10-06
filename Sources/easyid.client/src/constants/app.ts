/**
 * Application-wide constants
 */

/** Base permission namespace for the application */
export const PERMISSION_NAMESPACE = "easyid";

/** Permission categories */
export const PERMISSIONS = {
  APPS: `${PERMISSION_NAMESPACE}.apps`,
  GROUPS: `${PERMISSION_NAMESPACE}.groups`,
  ROLES: `${PERMISSION_NAMESPACE}.roles`,
  PERMISSIONS: `${PERMISSION_NAMESPACE}.permissions`,
  USERS: `${PERMISSION_NAMESPACE}.users`,
} as const;
