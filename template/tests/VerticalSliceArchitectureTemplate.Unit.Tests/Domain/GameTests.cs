﻿using FluentAssertions;
using VerticalSliceArchitectureTemplate.Domain;

namespace VerticalSliceArchitectureTemplate.Unit.Tests.Domain;

public class GameTests
{
    [Fact]
    public void Game_Constructor_CreatesGame()
    {
        // Act
        var game = new Game(GameId.From(Guid.NewGuid()), "Some Game");
        
        // Assert
        game.Should().NotBeNull();
    }
    
    [Fact]
    public void Game_Constructor_BindsName()
    {
        // Arrange
        const string name = "Test Game Name";
        
        // Act
        var game = new Game(GameId.From(Guid.NewGuid()), name);
        
        // Assert
        game.Name.Should().Be(name);
    }
    
    [Fact]
    public void Game_Constructor_GeneratesBoard()
    {
        // Act
        var game = new Game(GameId.From(Guid.NewGuid()), "Some Game");
        
        // Assert
        game.Board.Should().NotBeNull();
        game.Board.Value.Should().NotBeNull();
        game.Board.Value.Should().HaveCount(3);
        game.Board.Value[0].Should().HaveCount(3);
        game.Board.Value[1].Should().HaveCount(3);
        game.Board.Value[2].Should().HaveCount(3);
    }
    
    [Fact]
    public void Game_MakeMove_BindsTile()
    {
        // Arrange
        var game = new Game(GameId.From(Guid.NewGuid()), "Some Game");
        var tile = Tile.X;
        
        // Act
        game.MakeMove(0, 0, tile);
        
        // Assert
        game.Board.Value[0][0].Should().Be(tile);
    }
    
    [Fact]
    public void Game_MakeMove_SwitchesState()
    {
        // Arrange
        var game = new Game(GameId.From(Guid.NewGuid()), "Some Game");
        var tile = Tile.X;
        
        // Act
        game.MakeMove(0, 0, tile);
        
        // Assert
        game.State.Should().Be(GameState.OTurn);
    }
    
    [Fact]
    public void Game_MakeMove_ThrowsException_WhenTurnOutOfOrder()
    {
        // Arrange
        var game = new Game(GameId.From(Guid.NewGuid()), "Some Game");
        game.MakeMove(0, 0, Tile.X);
        
        // Act
        var act = () => game.MakeMove(0, 1, Tile.X);
        
        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Game is already over");
    }
    
    
    
    [Fact]
    public void Game_MakeMove_ThrowsException_WhenGameIsOver()
    {
        // Arrange
        var game = new Game(GameId.From(Guid.NewGuid()), "Some Game");
        game.MakeMove(0, 0, Tile.X);
        game.MakeMove(0, 1, Tile.O);
        game.MakeMove(1, 0, Tile.X);
        game.MakeMove(1, 1, Tile.O);
        game.MakeMove(2, 0, Tile.X);
        
        // Act
        Action act = () => game.MakeMove(2, 1, Tile.O);
        
        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Game is already over");
    }
}