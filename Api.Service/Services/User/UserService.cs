using Api.Domain.Dtos.User;
using Api.Domain.Entities;
using Api.Domain.Interfaces;
using Api.Domain.Interfaces.Services.User;
using Api.Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Service.Services.User
{
    public class UserService : IUserService
    {
        private readonly IRepository<UserEntity> _repository;
        private readonly IMapper _mapper;

        public UserService(IRepository<UserEntity> repository,
                           IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<UserDto> GetAsync(Guid id)
        {
            var retorno = await _repository.SelectAsync(id);
            return _mapper.Map<UserDto>(retorno);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var retorno = await _repository.SelectAsync();
            return _mapper.Map<IEnumerable<UserDto>>(retorno);
        }

        public async Task<UserDtoCreateResult> PostAsync(UserDtoCreate userDto)
        {
            var model = _mapper.Map<UserModel>(userDto);
            var entity = _mapper.Map<UserEntity>(model);
            var result = await _repository.InsertAsync(entity);
            return _mapper.Map<UserDtoCreateResult>(result);
        }

        public async Task<UserDtoUpdateResult> PutAsync(UserDtoUpdate userDto)
        {
            var model = _mapper.Map<UserModel>(userDto);
            var entity = _mapper.Map<UserEntity>(model);
            var result = await _repository.UpdateAsync(entity);
            return _mapper.Map<UserDtoUpdateResult>(result);
        }
    }
}
