using WebDating.Entities.PostEntities;
using WebDating.Entities.ProfileEntities;

namespace WebDating.Interfaces
{
    public interface IDatingRequestRepository
    {
        void Insert(DatingRequest entity);
        int InsertAndGetId(DatingRequest entity);
        void Remove(int id);
        void Remove(DatingRequest entity);
        DatingRequest Get(int id);
        Task<List<DatingRequest>> GetBySender(int senderId);
        Task<DatingRequest> GetIfInConfirmedRelationship(int userId);

        Task<DatingRequest> GetIfExistWaiting(int userId1, int userId2);

        DatingRequest GetIfDeniedBefore(int userId);


        void RemoveIfMeDenied(int userId, int crushId);

        Task<DatingRequest> GetWaitingRequest(int senderId, int crushId);

        void RemoveAllWaitingRequest(int userId, int excludeId);
    }
}
