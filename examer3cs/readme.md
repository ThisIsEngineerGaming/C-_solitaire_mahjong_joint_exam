# Mahjong Game (C# WinForms)

![Mahjong Banner](https://png.pngtree.com/thumb_back/fh260/back_our/20190625/ourmid/pngtree-mahjong-board-game-banner-background-image_259917.jpg)

## Project Description

Mahjong Game is a Windows Forms (WinForms) application written in C#.  
The game is a digital version of the classic tile-matching puzzle.  
The player's objective is to find and remove matching pairs of Mahjong tiles that are not blocked by neighboring tiles.

The project uses an object-oriented architecture with several interfaces:

- `IRenderable`
- `ISelectable`
- `IMovable`
- `ISaveable`

Core components of the application include scenes (MenuScene, GameScene, PauseScene), managers (GameManager, ScoreManager, InputHandler, UndoManager, SoundManager), a timer, a save/load system, and tile-related classes (MahjongTile, TileStack, Board).

---

## System Requirements and Compatibility

- Operating System: Windows 10 / Windows 11  
- Runtime: .NET 6 or later (Windows Forms)  
- RAM: 3 GB minimum  
- External Libraries:  
  - Newtonsoft.Json  
  - System.Drawing.Common (if needed)  
- Recommended IDE: Visual Studio 2022 with WinForms Designer support

---

## Installation and Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/mahjong-winforms.git


