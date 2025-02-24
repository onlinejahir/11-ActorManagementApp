using ActorManagement.Models.EntityModels;
using ActorManagement.Repositories.Contracts.AllContracts;
using ActorManagement.Services.Contracts.AllContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActorManagement.Services.AllServices
{
    public class ActorService : IActorService
    {
        private readonly IUnitRepository _unitRepository;
        public ActorService(IUnitRepository unitRepository)
        {
            this._unitRepository = unitRepository;
        }

        public async Task AddActorAsync(Actor actor)
        {
            await _unitRepository.ActorRepository.AddAsync(actor);
        }

        public async Task<Actor?> GetActorByEmailAsync(string email)
        {
            return await _unitRepository.ActorRepository.GetActorByEmailAsync(email);
        }

        public async Task<IEnumerable<Actor>> GetAllActorsAsync()
        {
            return _unitRepository.ActorRepository.GetAll();
        }

        public void RemoveActor(Actor existingActor)
        {
            _unitRepository.ActorRepository.Remove(existingActor);
        }

        public void UpdateActor(Actor existingActor)
        {
            _unitRepository.ActorRepository.Update(existingActor);
        }
    }
}
