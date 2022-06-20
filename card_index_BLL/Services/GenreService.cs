using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.Dto;
using card_index_DAL.Entities;
using card_index_DAL.Interfaces;

namespace card_index_BLL.Services
{
    internal class GenreService :IGenreService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GenreService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GenreDto>> GetAllAsync()
        {
            try
            {
                var takenFromDb = await _unitOfWork.GenreRepository.GetAllAsync();
                var mapped = _mapper.Map<IEnumerable<Genre>, IEnumerable<GenreDto>>(takenFromDb);
                return mapped;
            }
            catch (Exception ex)
            {
                throw new CardIndexException("Cannot get genres", ex);
            }
        }

        public async Task<GenreDto> GetByNameAsync(string name)
        {
            try
            {
                var takenFromDb =
                    (await _unitOfWork.GenreRepository.GetAllAsync()).FirstOrDefault(g => g.Title == name);
                var mapped = _mapper.Map<Genre, GenreDto>(takenFromDb);
                return mapped;
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot get genre with name: {name}", ex);
            }
        }

        public async Task<int> AddAsync(GenreDto model)
        {
            try
            {
                var mapped = _mapper.Map<GenreDto, Genre>(model);
                await _unitOfWork.GenreRepository.AddAsync(mapped);
                await _unitOfWork.SaveChangesAsync();
                return mapped.Id;
            }
            catch (Exception ex)
            {
                throw new CardIndexException("Cannot add genre to db", ex);
            }
        }

        public async Task UpdateAsync(GenreDto model)
        {
            try
            {
                var mapped = _mapper.Map<GenreDto, Genre>(model);
                _unitOfWork.GenreRepository.Update(mapped);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot update genre with id: {model.Id}", ex);
            }
        }

        public async Task DeleteAsync(int modelId)
        {
            try
            {
                await _unitOfWork.GenreRepository.DeleteByIdAsync(modelId);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot delete genre with id: {modelId}", ex);
            }
        }
    }
}
