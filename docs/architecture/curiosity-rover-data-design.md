# NASA Curiosity Rover Mission Feature - Data Design

## 1. Purpose

This document describes the data design for the NASA Curiosity Rover mission feature, including the structure of the mission data and how it integrates with the existing data model.

## 2. Data Model

### 2.1 NasaMission Record

The existing `NasaMission` record is used unchanged for the Curiosity Rover data:

```csharp
public sealed record NasaMission(
    string Name,
    string Description,
    DateTime LaunchDate,
    DateTime? EndDate,
    string Status,           // Active, Completed, Failed
    string ImagePath         // relative URL to static asset
);
```

### 2.2 Curiosity Rover Data Instance

The Curiosity Rover mission will be represented as an instance of the `NasaMission` record with the following values:

| Property | Value |
|----------|-------|
| Name | "Mars Rover Curiosity" |
| Description | "Mars rover investigating the planet's climate and geology" |
| LaunchDate | July 26, 2011 |
| EndDate | null (mission is ongoing) |
| Status | "Active" |
| ImagePath | "/images/curiosity.webp" |

## 3. Data Storage

### 3.1 In-Memory Repository

The Curiosity Rover mission data is stored in the `InMemoryNasaMissionRepository` as part of the static `_missions` array:

```csharp
new NasaMission(
    "Mars Rover Curiosity",
    "Mars rover investigating the planet's climate and geology",
    new DateTime(2011, 7, 26),
    null,
    "Active",
    "/images/curiosity.webp"
)
```

### 3.2 Data Ordering

The repository returns missions ordered by `LaunchDate`. The Curiosity Rover will appear chronologically between the Voyager 1 mission (launched 1977) and the Mars Rover Perseverance mission (launched 2020).

## 4. Data Integration

### 4.1 Seamless Integration

The Curiosity Rover data integrates seamlessly with existing mission data because:
- It uses the same `NasaMission` record structure
- It follows the same initialization pattern as other missions
- It will be ordered chronologically with other missions
- It uses the same presentation components

### 4.2 No Schema Changes

No changes to the data schema are required because:
- The existing `NasaMission` record already accommodates all necessary fields
- The `EndDate` field is nullable for ongoing missions
- The `Status` field can indicate active missions
- The `ImagePath` field references static assets in the same way

## 5. Data Validation

### 5.1 Required Fields

All fields in the `NasaMission` record are required and will be populated:
- `Name`: Descriptive mission name
- `Description`: Educational description of the mission
- `LaunchDate`: Historical launch date
- `EndDate`: null for ongoing mission
- `Status`: "Active" to indicate ongoing mission
- `ImagePath`: Path to mission image asset

### 5.2 Data Quality

Data quality is ensured through:
- Accurate historical dates
- Educational descriptions appropriate for the target audience
- Consistent status indicators
- Valid image paths referencing static assets

## 6. Future Extensibility

### 6.1 Adding More Missions

Additional missions can be added by:
- Creating new `NasaMission` instances in the static array
- Following the same initialization pattern
- Ensuring proper chronological ordering through launch dates

### 6.2 Data Model Evolution

If future missions require additional fields:
- The record structure would need to be updated
- All existing mission instances would need to be updated
- This represents a breaking change requiring careful planning

## 7. Static Asset Requirements

### 7.1 Image Asset

An image asset is required at `/wwwroot/images/curiosity.webp`:
- Format: WebP for optimal web performance
- Size: Consistent with other mission images
- Content: Educational imagery of the Curiosity Rover
- Alt text: Provided through the mission name in the UI