# Mahjong Solitaire – C# Windows Forms Project

## 🎮 Project Overview
This project is a Mahjong Solitaire game developed in C# using Windows Forms as part of a joint programming exam.  
The goal of the game is to match identical free tiles and remove them from the board until no moves remain.

The project demonstrates object-oriented programming, advanced C# features, SOLID principles, design patterns, and persistent data storage.

![Mahjong Banner](https://png.pngtree.com/thumb_back/fh260/back_our/20190625/ourmid/pngtree-mahjong-board-game-banner-background-image_259917.jpg)

---

## Used Programming Elements

### Core C# Language Features
- Classes and Objects
- Methods and Functions
- Constructors
- The `this` keyword
- Access Modifiers (`public`, `private`, `protected`)
- Properties (`get` / `set`)
- Indexers (`this[int index]`)
- Operator Overloading (`+` operator)
- Enums
- Exception Handling (`try-catch`)
- Events and Delegates
- Static Members
- Generic Types (`List<T>`, `IEnumerable<T>`)

---

### Object-Oriented Programming (OOP)
- Encapsulation – Private fields with public properties
- Inheritance – Tile subtypes inherit from base `Tile` class
- Polymorphism – Virtual and overridden methods
- Interfaces – `ISaveable` interface implemented

---

### Collections & Data Processing
- `List<T>`
- `IEnumerable<T>`
- Custom Collections (`TileCollection`)
- LINQ:
  - `Where`
  - `ToList`
  - `Repeat`

---

### File System & Serialization
- File Creation & Reading
- JSON Serialization
- JSON Deserialization
- Persistent Save & Load System

---

### Windows Forms & UI
- Forms
- Buttons
- Panels
- Mouse Events
- Custom Images for Tiles
- Application Icon
- Window Titles
- Background Images

---

### Design Patterns Used
- Strategy Pattern – Tile comparers via `IComparer<Tile>`

---

### SOLID Principles
- S – Single Responsibility Principle
- O – Open/Closed Principle
- L – Liskov Substitution Principle
- I – Interface Segregation Principle
- D – Dependency Inversion Principle

---

##  Project Requirements (All done)

| Requirement |
|-------------|--------|
| Program runs without crashes |
| Uses own classes and methods |
| Encapsulation, Inheritance, Polymorphism |
| Interface implementations |
| Properties |
| Indexers |
| Operator Overloading |
| Delegates and Events |
| Generics and Collections |
| File System usage |
| LINQ usage |
| Serialization|
| Design Patterns (1–2) |
| SOLID Principles |
| User-friendly interface |
| Thoughtful data design |
| Documentation comments |
| Generated HTML documentation (Doxygen) |
| Application title |
| Application icon |

---

## Data Persistence
- Tile collections are saved and loaded using JSON serialization
- File operations are handled through the file system
- Game state can be restored from saved files

---

## User Interface
- Main Menu Form
- Game Board Form
- Interactive tile clicking
- Back button navigation
- Custom background and tile images
- Application icon and window title included

---

## Documentation
- XML documentation comments added to public classes and methods
- Full HTML documentation generated using Doxygen
- Documentation available in the `/html` directory

---

## How to Run the Application
1. Open the solution in Visual Studio
2. Make sure `tileAssets` folder and `mahjong.ico` are present
3. Click Start (F5) to run the program

