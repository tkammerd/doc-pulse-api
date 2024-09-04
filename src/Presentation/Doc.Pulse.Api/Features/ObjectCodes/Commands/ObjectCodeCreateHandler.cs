using FluentValidation;
using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Trouble.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Doc.Pulse.Contracts.Communications.V1.ObjectCodes.Commands;
using Doc.Pulse.Core.Abstractions;
using Doc.Pulse.Core.Entities;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Ots.AppDmDoc.Abstractions.AutoMapper;
using Doc.Pulse.Infrastructure.Extensions;

namespace Doc.Pulse.Api.Features.ObjectCodes.Commands;

public class ObjectCodeCreateHandler
{
    public class Request : IRequest<Response>
    {
        public required ObjectCodeCreateCmd Command { get; set; }
    }
    public class Response : MediatorResult<CommandResponse> { }

    public class Validator : AbstractValidator<ObjectCodeCreateCmd>
    {
        public Validator()
        {
            RuleFor(o => o.CodeName).NotNull().Length(5, 255);
        }
    }

    public class Handler(AppDbContext dbContext, IMapperAdapter mapper, TimeProvider dateTimeProvider) : HandlerBase<AppDbContext, Request, Response>(dbContext, mapper)
    {
        private readonly TimeProvider _clock = dateTimeProvider;

        public async override Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var cmd = request.Command;
            Response response = new();

            try
            {
                var entity = new ObjectCode()
                {
                    CodeNumber = cmd.CodeNumber,
                    CodeName = cmd.CodeName,
                    CodeCategoryId = cmd.CodeCategoryId,
                    Inactive = cmd.Inactive
                };

                await _dbContext.ObjectCodes.AddAsync(entity, cancellationToken);
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
