using Warehouse.Application.Interfaces;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces;

namespace Warehouse.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IRepository<Notification> _repo;

    public NotificationService(IRepository<Notification> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<NotificationDto>> GetUnreadAsync()
    {
        var notes = await _repo.FindAsync(n => !n.IsRead);
        return notes.Select(n => new NotificationDto
        {
            Id = n.Id,
            Message = n.Message,
            CreatedAt = n.CreatedAt
        });
    }

    public async Task MarkAsReadAsync(Guid id)
    {
        var n = await _repo.GetByIdAsync(id);
        if (n != null)
        {
            n.IsRead = true;
            await _repo.UpdateAsync(n);
        }
    }
}
