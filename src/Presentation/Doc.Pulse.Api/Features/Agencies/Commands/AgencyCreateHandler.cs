using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Trouble.Errors;
using Doc.Pulse.Api.Extensions;
using Doc.Pulse.Contracts.Communications.V1.Agencies.Commands;
using Doc.Pulse.Core.Entities;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Doc.Pulse.Infrastructure.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ots.AppDmDoc.Abstractions.AutoMapper;

namespace Doc.Pulse.Api.Features.Agencies.Commands;

public class AgencyCreateHandler
{
    public class Request : IRequest<Response>
    {
        public required AgencyCreateCmd Command { get; set; }
    }
    public class Response : MediatorResult<CommandResponse> { }

    public class DbContextValidator : AbstractValidator<AgencyCreateCmd>
    {
        private readonly AppDbContext _dbContext;
        public DbContextValidator(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            var keyFieldDescription = "AgencyName".SplitCamelCase();

            RuleFor(o => o.AgencyName).NotNull().Length(3, 255);

            RuleFor(p => p)
                .Must(KeyFieldIsUnique)
                .WithErrorCode("UniqueFieldValidator")
                .WithMessage($"'{keyFieldDescription}' must be unique.");
        }

        private bool KeyFieldIsUnique(AgencyCreateCmd cmd)
        {
            return !_dbContext.Agencies.Any(o => o.AgencyName == cmd.AgencyName);
        }

    }

    public class Handler(AppDbContext dbContext, IMapperAdapter mapper) : HandlerBase<AppDbContext, Request, Response>(dbContext, mapper)
    {
        public async override Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var cmd = request.Command;
            Response response = new();

            var validator = new DbContextValidator(_dbContext);
            var validationResult = await validator.ValidateAsync(cmd, cancellationToken);

            if (validationResult.Errors.Count > 0)
            {
                foreach (var item in validationResult.Errors.Select(e => e.ErrorMessage))
                {
                    response.WithError(new FluentResults.Error(item));
                }
                return response;
            }

            try
            {
                var entity = new Agency()
                {
                    AgencyName = cmd.AgencyName,
                    Inactive = cmd.Inactive
                };

                await _dbContext.Agencies.AddAsync(entity, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                if (entity.Id == 0)
                    return response.WithError<Response>(new DatabaseSaveFailed());

                response.WithValue<Response>(new CommandResponse()
                {
                    Id = entity.Id
                });
            }
            catch (Exception exception)
            {
                response.WithException(exception);
            }

            return response;
        }
    }
}
