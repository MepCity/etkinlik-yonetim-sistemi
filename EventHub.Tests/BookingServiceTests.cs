using BLL;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Services.Mail;
using Xunit;

namespace EventHub.Tests;

public class BookingServiceTests
{
    [Fact]
    public async Task Book_ShouldThrow_WhenQuantityExceedsAvailableTickets()
    {
        var eventRepository = new InMemoryRepository<Event>(
            new Event
            {
                Id = 1,
                Name = "Workshop",
                Description = "Test event",
                Location = "Room A",
                Capacity = 2,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(1).AddHours(2),
                Bookings = []
            });
        var bookingRepository = new InMemoryRepository<Booking>();
        var service = CreateBookingService(bookingRepository, eventRepository);

        var exception = await Assert.ThrowsAsync<Exception>(() =>
            service.Book(1, "user-1", "alice", "alice@example.com", 3));

        Assert.Contains("Only 2 ticket(s) are available", exception.Message);
    }

    [Fact]
    public async Task Book_ShouldThrow_WhenUserAlreadyJoinedEvent()
    {
        var eventRepository = new InMemoryRepository<Event>(
            new Event
            {
                Id = 1,
                Name = "Workshop",
                Description = "Test event",
                Location = "Room A",
                Capacity = 5,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(1).AddHours(2),
                Bookings = []
            });
        var bookingRepository = new InMemoryRepository<Booking>(
            new Booking
            {
                Id = 1,
                EventId = 1,
                UserId = "user-1",
                CustomerName = "alice",
                Quantity = 1,
                IsPaid = true,
                BookingDate = DateTime.UtcNow
            });
        var service = CreateBookingService(bookingRepository, eventRepository);

        var exception = await Assert.ThrowsAsync<Exception>(() =>
            service.Book(1, "user-1", "alice", "alice@example.com", 1));

        Assert.Equal("You have already joined this event.", exception.Message);
    }

    [Fact]
    public async Task Book_ShouldPersistBooking_WhenRequestIsValid()
    {
        var eventRepository = new InMemoryRepository<Event>(
            new Event
            {
                Id = 1,
                Name = "Workshop",
                Description = "Test event",
                Location = "Room A",
                Capacity = 5,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(1).AddHours(2),
                Bookings = []
            });
        var bookingRepository = new InMemoryRepository<Booking>();
        var service = CreateBookingService(bookingRepository, eventRepository);

        await service.Book(1, "user-1", "alice", "alice@example.com", 2);

        var bookings = await bookingRepository.GetAllAsync();
        var booking = Assert.Single(bookings);
        Assert.Equal("user-1", booking.UserId);
        Assert.Equal("alice", booking.CustomerName);
        Assert.Equal(2, booking.Quantity);
    }

    [Fact]
    public async Task GetBookingsByUser_ShouldReturnOnlyMatchingUserBookings()
    {
        var bookingRepository = new InMemoryRepository<Booking>(
            new Booking { Id = 1, EventId = 1, UserId = "user-1", CustomerName = "alice", Quantity = 1, IsPaid = true, BookingDate = DateTime.UtcNow },
            new Booking { Id = 2, EventId = 2, UserId = "user-2", CustomerName = "bob", Quantity = 1, IsPaid = true, BookingDate = DateTime.UtcNow });
        var eventRepository = new InMemoryRepository<Event>();
        var service = CreateBookingService(bookingRepository, eventRepository);

        var bookings = await service.GetBookingsByUser("user-1");

        var booking = Assert.Single(bookings);
        Assert.Equal("user-1", booking.UserId);
    }

    private static BookingService CreateBookingService(
        IGenericRepository<Booking> bookingRepository,
        IGenericRepository<Event> eventRepository)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["MailSettings:MailUser"] = "",
                ["MailSettings:MailHost"] = "",
                ["MailSettings:MailPort"] = "587",
                ["MailSettings:MailPassword"] = ""
            })
            .Build();

        return new BookingService(bookingRepository, eventRepository, new BookingMailService(configuration));
    }

    private sealed class InMemoryRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly List<T> _items;

        public InMemoryRepository(params T[] items)
        {
            _items = items.ToList();
        }

        public Task<IEnumerable<T>> GetAllAsync() => Task.FromResult<IEnumerable<T>>(_items.ToList());

        public Task<T> GetByIdAsync(int id)
        {
            var item = _items.FirstOrDefault(entity => GetId(entity) == id);
            if (item is null)
            {
                throw new Exception($"{typeof(T).Name} with ID {id} not found.");
            }

            return Task.FromResult(item);
        }

        public Task AddAsync(T entity)
        {
            _items.Add(entity);
            return Task.CompletedTask;
        }

        public void Update(T entity)
        {
            var existingIndex = _items.FindIndex(current => GetId(current) == GetId(entity));
            if (existingIndex >= 0)
            {
                _items[existingIndex] = entity;
            }
        }

        public void Delete(int id)
        {
            _items.RemoveAll(entity => GetId(entity) == id);
        }

        private static int GetId(T entity)
        {
            var idProperty = typeof(T).GetProperty("Id")
                ?? throw new InvalidOperationException($"{typeof(T).Name} does not define an Id property.");
            return (int)(idProperty.GetValue(entity)
                ?? throw new InvalidOperationException($"{typeof(T).Name}.Id is null."));
        }
    }
}
