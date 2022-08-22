using Application.Features.Brands.Dtos;
using Application.Features.Brands.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Brands.Commands.CreateBrand
{
    public class CreateBrandEntityCommand : IRequest<CreatedBrandEntityDto>
    {
        public string Name { get; set; }

        public class CreateBrandCommendHandler : IRequestHandler<CreateBrandEntityCommand, CreatedBrandEntityDto>
        {
            private readonly IBrandRepository _brandRepository;
            private readonly IMapper _mapper;
            private readonly BrandBusinessRules _brandBusinessRules;

            public CreateBrandCommendHandler(IBrandRepository brandRepository, IMapper mapper, BrandBusinessRules brandBusinessRules)
            {
                _brandRepository = brandRepository;
                _mapper = mapper;
                _brandBusinessRules = brandBusinessRules;
            }

            public async Task<CreatedBrandEntityDto> Handle(CreateBrandEntityCommand request, CancellationToken cancellationToken)
            {
                await _brandBusinessRules.BrandNameCanNotBeDuplicatedWhenInserted(request.Name);

                Brand mappedBrand = _mapper.Map<Brand>(request);
                Brand createdBrand = await _brandRepository.AddAsync(mappedBrand);
                CreatedBrandEntityDto createdBrandEntityDto = _mapper.Map<CreatedBrandEntityDto>(createdBrand);

                return createdBrandEntityDto;
            }
        }

    }
}
