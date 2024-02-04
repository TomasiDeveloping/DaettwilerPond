// Environment configuration for development
export const environment = {
  // Flag indicating development environment
  production: false,

  // Application version with development indicator
  version: "1.15.3-DEV",

  // Base URL for API endpoints during development
  apiBaseUrl: 'https://localhost:7005/api/v1',

  // URI for resetting passwords in the local environment
  resetPasswordUri: 'http://localhost:4200/resetPassword'
};

