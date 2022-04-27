﻿using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentAssertions;
using Xunit;
using HTEC.POC.API.Models.Requests;

namespace HTEC.POC.API.UnitTests.Models;

public class CreateCategoryRequestTests
{
    [Fact]
    public void Name_Should_BeDecoratedWith_RequiredAttribute()
    {
        // Arrange
        // Act
        // Assert
        typeof(CreateCategoryRequest)
            .Properties()
            .First(x => x.Name == "Name")
            .Should()
            .BeDecoratedWith<RequiredAttribute>();
    }

    [Fact]
    public void Name_Should_ReturnString()
    {
        // Arrange
        // Act
        // Assert
        typeof(CreateCategoryRequest)
            .Properties()
            .First(x => x.Name == "Name")
            .Should()
            .Return<string>();
    }

    [Fact]
    public void Description_Should_ReturnString()
    {
        // Arrange
        // Act
        // Assert
        typeof(CreateCategoryRequest)
            .Properties()
            .First(x => x.Name == "Description")
            .Should()
            .Return<string>();
    }
}
