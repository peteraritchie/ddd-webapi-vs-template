# title

## WebAPI

DTO are anemic data containers. Validation, serialization, etc. requirements use asp.net specific attributes. 

### Infrastructure

### Application
Is this just application services?  Be clear about the distinction of application services.

## Core (Domain)

Core shouldn't know about front-ends, so translation to/from domain happens in front-end.

Domain classes have all read-only properties, mutation is done through methods (ala Command Query Separation.)

## Infrastructure

## Tests

Test names detail the expectation, no need for assert text.

Tests verify controllers via test server.
