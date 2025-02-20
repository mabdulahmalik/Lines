using Purp = SOL.Service.PatientEncounter.Purpose.Domain.Purpose;

namespace SOL.Service.PatientEncounter.Tests.Domain;

public class PurposeTests
{
    [Fact(DisplayName = "Create should return new purpose")]
    public void Create_ShouldReturnNewPurpose()
    {
        var name = "Test Purpose";
        var purpose = Purp.Create(name);
        
        Assert.NotNull(purpose);
        Assert.Equal(name, purpose.Name);
        Assert.False(purpose.Archived);
    }
    
    [Fact(DisplayName = "Create should throw exception when name is empty")]
    public void Create_WithEmptyName_ShouldThrowException()
    {
        var emptyName = string.Empty;

        Assert.Throws<ArgumentException>(() => Purp.Create(emptyName));
    }   

    [Fact(DisplayName = "Rename should update the name")]
    public void Rename_ShouldUpdateName()
    {
        var name = "Initial Name";
        var purpose = Purp.Create(name);

        var newName = "Updated Name";
        purpose.Rename(newName);

        Assert.Equal(newName, purpose.Name);
    }
    
    [Fact(DisplayName = "Rename should throw exception when name is empty")]
    public void Rename_WithEmptyName_ShouldThrowException()
    {
        var name = "Initial Name";
        var purpose = Purp.Create(name);

        var emptyName = string.Empty;

        Assert.Throws<ArgumentException>(() => purpose.Rename(emptyName));
    }
}