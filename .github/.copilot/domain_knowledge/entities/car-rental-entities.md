# Car Rental Entity Relationship Model

This document describes the entity relationship model for a car rental company's online booking system.

## Entities and Relationships

### 1. Make
- **Description**: A vehicle manufacturer (e.g., Toyota, Ford, BMW)
- **Attributes**:
  - Make ID
  - Name
  - Country of origin
  - Founded year
  - Logo
- **Relationships**:
  - One Make has many Models

### 2. Model
- **Description**: A specific vehicle model produced by a manufacturer
- **Attributes**:
  - Model ID
  - Name
  - Year introduced
  - Class/segment (compact, SUV, luxury, etc.)
  - Default seating capacity
  - Base fuel efficiency
  - Is active (still in production)
- **Relationships**:
  - One Model belongs to one Make
  - One Model has many Vehicles

### 3. Vehicle
- **Description**: An individual vehicle in the rental fleet
- **Attributes**:
  - Vehicle ID
  - VIN (Vehicle Identification Number)
  - License plate
  - Year of manufacture
  - Color
  - Odometer reading (mileage)
  - Status (available, rented, maintenance, retired)
  - Daily rental rate
  - Purchase date
  - Current location
- **Relationships**:
  - One Vehicle belongs to one Model
  - One Vehicle has many Tags (through Vehicle-Tag relation)

### 4. Tag
- **Description**: Labels used to categorize vehicles by features or characteristics
- **Attributes**:
  - Tag ID
  - Name
  - Description
  - Type (feature, condition, restriction, promotion)
- **Relationships**:
  - One Tag can be applied to many Vehicles (through Vehicle-Tag relation)

### 5. Vehicle-Tag (Junction Entity)
- **Description**: Connects vehicles with their associated tags
- **Attributes**:
  - Vehicle ID
  - Tag ID
  - Date added
  - Added by (staff member)
- **Relationships**:
  - Links Vehicles and Tags in a many-to-many relationship

## Entity Relationship Summary

- A **Make** has multiple **Models**
- A **Model** belongs to one **Make**
- A **Model** has multiple **Vehicles**
- A **Vehicle** belongs to one **Model**
- A **Vehicle** can have multiple **Tags**
- A **Tag** can be applied to multiple **Vehicles**

## Diagram (Text Representation)

```
Make (1) --- (N) Model (1) --- (N) Vehicle
                                  (N)
                                   |
                                   |
                                  (M)
                                  Tag
```

This structure provides the foundation for tracking vehicle inventory while maintaining proper relationships between manufacturers, models, and individual vehicles, with a flexible tagging system to categorize vehicles by their features and characteristics.