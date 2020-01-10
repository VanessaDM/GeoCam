# Design Decisions
  - Back-end API: C# - .NET Core (3.1)
  - Front-end:
    *  ~~Angular~~ (con: large overhead for given scope - 1 page with no interactive elements)
    * MVC - neutral
    * jQuery/AJAX - pro: best fit given requirements - has minimal failure surface and download size and requires minimal client/service-side resources
    * Blazor - pro: fun 😊
  - Database: ~~full-blown~~ vs SQL Lite 

# Estimate / Priorities (Max 4h)
## 1. API - ~1.5 hours
- PRQ: Project setup
- DEV:
  1. Mock API controller
  2. Data layer
  3. Data import
  4. Implement search API
- NTH:
  1. Testing
  2. Functional logging

## 2. CLI - ~0.5 hour
- PRQ: Project setup
- DEV:
  1. Minimal functionality (process params + API call + format results)
- NTH
  1. Testing
  2. Better error handling/feedback

## 3. Web - ~2 hours
- PRQ: Project setup
- DEV:
  1. Placeholder API call using mock
  2. Table display
  3. Integrate Google Maps
- NTH:
  1. Improved usability
  2. Testing
  3. Functional logging

## DevOps - Beyond the scope
- Azure deploy

PRQ: Prerequisite
DEV: Development
NTH: Nice to have (if there was enough time 😊)
