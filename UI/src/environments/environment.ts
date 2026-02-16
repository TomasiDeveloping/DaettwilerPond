// Environment configuration for development
export const environment = {
  // Flag indicating development environment
  production: false,

  // Application version with development indicator
  version: "1.2.0-DEV",

  serverUrl: 'https://localhost:7005/',

  // Base URL for API endpoints during development
  apiBaseUrl: 'https://localhost:7005/api/v1',

  // URI for resetting passwords in the local environment
  resetPasswordUri: 'http://localhost:4200/resetPassword',

  licenseValidationUrl: 'http://localhost:4200/validate/'
};

