# Changelog

## [2.0.0] - 2026-11-09

### Changed

- Added optional Redis key-pattern filtering while preserving retrieval of all keys by default.
- Updated target framework to .NET 8.0
- **[Breaking Change]** Original exceptions are now rethrown instead of being wrapped in a generic Exception, allowing for more specific exception handling.

## [1.2.0] - 2026-03-03

### Added

- Add support for Microsoft Entra ID

## [1.1.0] - 2025-07-31

### Changed

- Make default error message empty by default

## [1.0.0] - 2025-05-13

### Changed

- Initial implementation
