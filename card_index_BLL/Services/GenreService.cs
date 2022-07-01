using AutoMapper;
using card_index_BLL.Exceptions;
using card_index_BLL.Interfaces;
using card_index_BLL.Models.DataShaping;
using card_index_BLL.Models.Dto;
using card_index_DAL.Entities;
using card_index_DAL.Entities.DataShaping;
using card_index_DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace card_index_BLL.Services
{
    /// <summary>
    /// Implements interface between webapi and repository
    /// </summary>
    public class GenreService : IGenreService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Creates service
        /// </summary>
        /// <param name="mapper">Object for entities mapping</param>
        /// <param name="unitOfWork">Object to work with DAL</param>
        public GenreService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets all genres from DB
        /// </summary>
        /// <returns>Genre list mapped to Dto</returns>
        /// <exception cref="CardIndexException">Thrown, if problems operating with DAL</exception>
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
        /// <summary>
        /// Overloaded version, takes parameters model with filtering data
        /// </summary>
        /// <param name="parameters">Parameters model, filters result</param>
        /// <returns>Filtered genres</returns>
        /// <exception cref="CardIndexException">Thrown if problems during DB operations</exception>
        public async Task<IEnumerable<GenreDto>> GetAllAsync(PagingParametersModel parameters)
        {
            var filter = new PagingParameters { PageNumber = parameters.PageNumber, PageSize = parameters.PageSize };
            try
            {
                var takenFromDb = await _unitOfWork.GenreRepository.GetAllWithDetailsAsync(filter);
                var mapped = _mapper.Map<IEnumerable<Genre>, IEnumerable<GenreDto>>(takenFromDb);
                return mapped;
            }
            catch (Exception ex)
            {
                throw new CardIndexException("Cannot get text cards", ex);
            }
        }
        /// <summary>
        /// Gets genre with given name
        /// </summary>
        /// <param name="name">Genre name to search</param>
        /// <returns>Genre matching criteria</returns>
        /// <exception cref="CardIndexException">Thrown, if problems operating with DAL</exception>
        public async Task<GenreDto> GetByNameAsync(string name)
        {
            try
            {
                var takenFromDb = await _unitOfWork.GenreRepository.GetByNameWithDetailsAsync(name);
                var mapped = _mapper.Map<Genre, GenreDto>(takenFromDb);
                return mapped;
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot get genre with name: {name}", ex);
            }
        }

        /// <summary>
        /// Adds new genre to DB
        /// </summary>
        /// <param name="model">New genre to add</param>
        /// <returns>Id of added genre</returns>
        /// <exception cref="CardIndexException">Thrown, if problems operating with DAL</exception>
        public async Task<int> AddAsync(GenreDto model)
        {
            try
            {
                var mapped = _mapper.Map<GenreDto, Genre>(model);
                mapped.Id = 0;
                await _unitOfWork.GenreRepository.AddAsync(mapped);
                await _unitOfWork.SaveChangesAsync();
                return mapped.Id;
            }
            catch (Exception ex)
            {
                throw new CardIndexException("Cannot add genre to db", ex);
            }
        }

        /// <summary>
        /// Updates genre in DB
        /// </summary>
        /// <param name="model">Model, containing data for update</param>
        /// <returns>Async operation</returns>
        /// <exception cref="CardIndexException">Thrown, if problems operating with DAL</exception>
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

        /// <summary>
        /// Deletes genre from DB
        /// </summary>
        /// <param name="modelId">Id of genre to delete</param>
        /// <returns>Async operation</returns>
        /// <exception cref="CardIndexException">Thrown, if problems operating with DAL</exception>
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
        /// <summary>
        /// Gets total number of genres stored
        /// </summary>
        /// <returns>Genre number</returns>
        /// <exception cref="CardIndexException">Thrown if problems during DB operations</exception>
        public async Task<int> GetTotalNumber()
        {
            try
            {
                return await _unitOfWork.GenreRepository.GetTotalNumberAsync();
            }
            catch (Exception ex)
            {
                throw new CardIndexException($"Cannot get number of genres", ex);
            }
        }
    }
}
