# Stars and Constellations Data Design

## Overview

The data design for the Stars and Constellations feature utilizes C# records for immutable domain entities and in-memory repositories for data storage. This approach aligns with the existing SpaceGeeks architecture while providing a clean, maintainable data model for astronomical information.

## Domain Entities

### Star Entity

The Star entity represents an individual celestial body with its physical and positional properties:

```csharp
public record Star(
    int Id,
    string Name,
    string BayerDesignation,
    double RightAscension,
    double Declination,
    double ApparentMagnitude,
    double AbsoluteMagnitude,
    string SpectralClass,
    double DistanceInLightYears,
    string ConstellationId);
```

#### Attributes

- **Id**: Unique integer identifier for internal referencing
- **Name**: Common name of the star (e.g., "Sirius", "Betelgeuse")
- **BayerDesignation**: Bayer designation (e.g., "α CMa" for Sirius)
- **RightAscension**: Celestial coordinate in hours (0-24)
- **Declination**: Celestial coordinate in degrees (-90 to +90)
- **ApparentMagnitude**: Brightness as seen from Earth
- **AbsoluteMagnitude**: Intrinsic brightness at 10 parsecs distance
- **SpectralClass**: Stellar classification (O, B, A, F, G, K, M)
- **DistanceInLightYears**: Distance from Earth in light years
- **ConstellationId**: Reference to parent constellation

### Constellation Entity

The Constellation entity represents a group of stars forming a recognized pattern:

```csharp
public record Constellation(
    string Id,
    string Name,
    string LatinName,
    string Abbreviation,
    string Family,
    string Origin,
    string Meaning,
    List<int> StarIds);
```

#### Attributes

- **Id**: Unique string identifier (typically three-letter abbreviation)
- **Name**: Common name of the constellation (e.g., "Orion", "Ursa Major")
- **LatinName**: Official Latin name
- **Abbreviation**: Three-letter IAU abbreviation (e.g., "Ori", "UMa")
- **Family**: Constellation family grouping (e.g., "Ursa Major", "Orion")
- **Origin**: Cultural or mythological origin
- **Meaning**: Interpretation or representation
- **StarIds**: Collection of star IDs that form this constellation

## Relationships

### Star to Constellation

- Many-to-One relationship: Multiple stars belong to one constellation
- Represented by ConstellationId in Star entity
- Constellation entity maintains list of StarIds for bidirectional navigation

### Constellation Families

- Grouping mechanism for related constellations
- Logical categorization, not enforced in data model
- Used for browsing and filtering

## Data Repositories

### IStarRepository Interface

```csharp
public interface IStarRepository
{
    IEnumerable<Star> GetAllStars();
    Star GetStarById(int id);
    IEnumerable<Star> GetStarsByConstellation(string constellationId);
    IEnumerable<Star> SearchStars(string searchTerm);
    IEnumerable<Star> FilterStarsByMagnitude(double minMagnitude, double maxMagnitude);
    IEnumerable<Star> FilterStarsBySpectralClass(string spectralClass);
}
```

### IConstellationRepository Interface

```csharp
public interface IConstellationRepository
{
    IEnumerable<Constellation> GetAllConstellations();
    Constellation GetConstellationById(string id);
    IEnumerable<Constellation> GetConstellationsByFamily(string family);
    IEnumerable<Constellation> SearchConstellations(string searchTerm);
}
```

## Data Storage

### In-Memory Implementation

Both repositories are implemented using in-memory collections:

- **StarRepository**: Uses `List<Star>` for data storage
- **ConstellationRepository**: Uses `List<Constellation>` for data storage
- Data loaded at application startup from static sources
- No persistence between application restarts

### Sample Data Structure

Stars data example:
```json
[
  {
    "Id": 1,
    "Name": "Sirius",
    "BayerDesignation": "α CMa",
    "RightAscension": 6.7525,
    "Declination": -16.7161,
    "ApparentMagnitude": -1.46,
    "AbsoluteMagnitude": 1.42,
    "SpectralClass": "A1V",
    "DistanceInLightYears": 8.66,
    "ConstellationId": "CMa"
  }
]
```

Constellations data example:
```json
[
  {
    "Id": "CMa",
    "Name": "Canis Major",
    "LatinName": "Canis Major",
    "Abbreviation": "CMa",
    "Family": "Ursa Major",
    "Origin": "Ancient",
    "Meaning": "Greater dog",
    "StarIds": [1, 2, 3]
  }
]
```

## Data Validation

### Star Entity Validation Rules

- Id: Positive integer
- Name: Non-empty string, maximum 100 characters
- RightAscension: Between 0 and 24 hours
- Declination: Between -90 and +90 degrees
- ApparentMagnitude: Valid numerical value
- SpectralClass: One of standard classifications (O,B,A,F,G,K,M)
- DistanceInLightYears: Positive value
- ConstellationId: References existing constellation

### Constellation Entity Validation Rules

- Id: Non-empty string, typically 3 characters
- Name: Non-empty string, maximum 50 characters
- Abbreviation: Exactly 3 uppercase letters
- StarIds: All referenced IDs must exist in star data

## Future Extensibility

### Additional Entities (Planned)

#### DeepSkyObject Entity
For galaxies, nebulae, and star clusters:
- Type classification
- Angular dimensions
- Visual magnitude

#### Observation Entity
For user-submitted observations:
- Date and time
- Location coordinates
- Viewing conditions

### Enhanced Relationships

#### Star Relationships
- Binary and multiple star systems
- Variable star classifications
- Stellar evolution stages

#### Constellation Enhancements
- Boundary coordinates
- Associated meteor showers
- Visibility by geographic location

## Performance Considerations

### Indexing Strategy

- Stars indexed by Id for direct lookup
- Secondary indexes on ConstellationId, SpectralClass
- Constellations indexed by Id and Abbreviation
- Search functionality implemented with LINQ filtering

### Memory Usage

- Static data sets loaded once at startup
- Immutable entities prevent unnecessary copying
- Efficient collection types for storage and retrieval