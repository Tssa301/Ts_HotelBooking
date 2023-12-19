using Application;
using Application.Guest.DTO;
using Application.Guest.Requests;
using Domain.Entities;
using Domain.Enums;
using Domain.Ports;
using Domain.ValueObjects;
using Moq;

namespace ApplicationTests;

public class Tests
{
    GuestManager guestManager;
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task HappyPath()
    {
        var guestDto = new GuestDTO
        {
            Name = "Jhon",
            Surname = "Green",
            Email = "Jhon@gmail.com",
            IdNumber = "1234",
            IdTypeCode = 1
        };

        const int expectedId = 222;

        var request = new CreateGuestRequest()
        {
            Data = guestDto,
        };
        
        var fakeRepo = new Mock<IGuestRepository>();

        fakeRepo.Setup(x => x.Create(
            It.IsAny<Guest>())).Returns(Task.FromResult(expectedId));
        
        guestManager = new GuestManager(fakeRepo.Object);

        var res = await guestManager.CreateGuest(request);
        Assert.IsNotNull(res);
        Assert.True(res.Success);
        Assert.That(res.Data.Id, Is.EqualTo(expectedId));
        Assert.That(guestDto.Name, Is.EqualTo(res.Data.Name));
    }
    
    [TestCase("")]
    [TestCase(null)]
    [TestCase("a")]
    [TestCase("ab")]
    [TestCase("abc")]
    
    public async Task Should_Return_InvalidPersonDocumentIdException_WhenDocsAreInvalid(string docNumber)
    {
        var guestDto = new GuestDTO
        {
            Name = "Jhon",
            Surname = "Green",
            Email = "Jhon@gmail.com",
            IdNumber = docNumber,
            IdTypeCode = 1
        };
        
        var request = new CreateGuestRequest()
        {
            Data = guestDto,
        };
        
        var fakeRepo = new Mock<IGuestRepository>();

        fakeRepo.Setup(x => x.Create(
            It.IsAny<Guest>())).Returns(Task.FromResult(222));
        
        guestManager = new GuestManager(fakeRepo.Object);

        var res = await guestManager.CreateGuest(request);
        Assert.IsNotNull(res);
        Assert.False(res.Success);
        Assert.That(res.ErrorCode, Is.EqualTo(ErrorCodes.INVALID_PERSON_ID));
        Assert.That(res.Message, Is.EqualTo("The ID entered is not valid"));
    }
    
    [TestCase("", "Green", "asdf@gmail.com")]
    [TestCase(null, "Green", "asdf@gmail.com")]
    [TestCase("Jhon", "", "asdf@gmail.com")]
    [TestCase("Jhon", null, "asdf@gmail.com")]
    [TestCase("Jhon", "Green", "")]
    [TestCase("Jhon", "Green", null)]
    
    public async Task Should_Return_MissingRequiredInformation_WhenDocsAreInvalid(string name, string surname, string email)
    {
        var guestDto = new GuestDTO
        {
            Name = name,
            Surname = surname,
            Email = email,
            IdNumber = "abcd",
            IdTypeCode = 1
        };
        
        var request = new CreateGuestRequest()
        {
            Data = guestDto,
        };
        
        var fakeRepo = new Mock<IGuestRepository>();

        fakeRepo.Setup(x => x.Create(
            It.IsAny<Guest>())).Returns(Task.FromResult(222));
        
        guestManager = new GuestManager(fakeRepo.Object);

        var res = await guestManager.CreateGuest(request);
        Assert.IsNotNull(res);
        Assert.False(res.Success);
        Assert.That(res.ErrorCode, Is.EqualTo(ErrorCodes.MISSING_REQUIRED_INFORMATION));
        Assert.That(res.Message, Is.EqualTo("Missing required information passed"));
    }
    
    [TestCase("Jhon", "Green", "b@b.com")]
    public async Task Should_Return_InvalidateEmail_WhenEmailIsInvalid(string name, string surname, string email)
    {
        var guestDto = new GuestDTO
        {
            Name = name,
            Surname = surname,
            Email = email,
            IdNumber = "abc1",
            IdTypeCode = 1
        };
        
        var request = new CreateGuestRequest()
        {
            Data = guestDto,
        };
        
        var fakeRepo = new Mock<IGuestRepository>();

        fakeRepo.Setup(x => x.Create(
            It.IsAny<Guest>())).Returns(Task.FromResult(222));
        
        guestManager = new GuestManager(fakeRepo.Object);

        var res = await guestManager.CreateGuest(request);
        Assert.IsNotNull(res);
        Assert.False(res.Success);
        Assert.That(res.ErrorCode, Is.EqualTo(ErrorCodes.INVALID_EMAIL));
        Assert.That(res.Message, Is.EqualTo("The Email entered is not valid"));
    }
    
    [Test]
    public async Task Should_Return_GuestNotFound_When_GuestDoesntExist()
    {
        var fakeRepo = new Mock<IGuestRepository>();
        
        fakeRepo.Setup(x => x.Get(333)).Returns(Task.FromResult((Guest?)null));
        
        guestManager = new GuestManager(fakeRepo.Object);
        
        var res = await guestManager.GetGuest(333);
        
        Assert.IsNotNull(res);
        Assert.False(res.Success);
        Assert.That(res.ErrorCode, Is.EqualTo(ErrorCodes.GUEST_NOT_FOUND));
        Assert.That(res.Message, Is.EqualTo("No Guest record was found with the given Id"));
    }

    [Test]
    public async Task Should_Return_Guest_Success()
    {
        var fakeRepo = new Mock<IGuestRepository>();

        var fakeGuest = new Guest()
        {
            Id = 333,
            Name = "Jhon",
            DocumentId = new PersonId()
            {
                DocumentType = DocumentType.DriveLicence,
                IdNumber = "0123"
            }
        };

        fakeRepo.Setup(x => x.Get(333)).Returns(Task.FromResult((Guest?)fakeGuest));
        
        guestManager = new GuestManager(fakeRepo.Object);
        
        var res = await guestManager.GetGuest(333);
        
        Assert.IsNotNull(res);
        Assert.True(res.Success);
        Assert.That(res.Data.Id, Is.EqualTo(fakeGuest.Id));
        Assert.That(res.Data.Name, Is.EqualTo(fakeGuest.Name));
    }
}