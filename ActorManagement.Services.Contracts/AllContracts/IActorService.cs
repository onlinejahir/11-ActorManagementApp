using ActorManagement.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorManagement.Services.Contracts.AllContracts
{
    public interface IActorService
    {
        Task AddActorAsync(Actor actor);
        Task<Actor?> GetActorByEmailAsync(string email);
        Task<IEnumerable<Actor>> GetAllActorsAsync();
        void RemoveActor(Actor existingActor);
        void UpdateActor(Actor existingActor);
    }
}
