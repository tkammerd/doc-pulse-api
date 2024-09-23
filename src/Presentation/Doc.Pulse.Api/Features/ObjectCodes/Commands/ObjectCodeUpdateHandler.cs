using FluentValidation;
using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Trouble.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Doc.Pulse.Contracts.Communications.V1.ObjectCodes.Commands;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Ots.AppDmDoc.Abstractions.AutoMapper;
using Doc.Pulse.Infrastructure.Extensions;

namespace Doc.Pulse.Api.Features.ObjectCodes.Commands;

public class ObjectCodeUpdateHandler
{
    public class Request : IRequest<Response>
    {
        public required ObjectCodeUpdateCmd Command { get; set; }
    }
    public class Response : MediatorResult<CommandResponse> { }

    public class Validator : AbstractValidator<ObjectCodeUpdateCmd>
    {
        public Validator()
        {
            RuleFor(o => o.Id).NotNull().NotEqual(0)
                .WithMessage($"Id not valid: Please indicate a valid Identifier.");
            RuleFor(o => o.CodeName).NotNull().Length(7, 255);
        }
    }

    public class Handler(AppDbContext dbContext, IMapperAdapter mapper) : HandlerBase<AppDbContext, Request, Response>(dbContext, mapper)
    {
        public async override Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            Response response = new();

            try
            {
                var entity = await _dbContext.ObjectCodes
                    .AsTracking(QueryTrackingBehavior.TrackAll)
                    .SingleOrDefaultAsync(o => o.Id == request.Command.Id, cancellationToken);
                if (entity == null)
                    return response.WithError<Response>(new EntityNotFound(request.Command.Id.ToString()));

                entity.CodeNumber = request?.Command?.CodeNumber ?? 0;
                entity.CodeName = request?.Command?.CodeName ?? "";
                entity.CodeCategoryId = request?.Command?.CodeCategoryId ?? 0;
                entity.Inactive = request?.Command?.Inactive ?? false;

                await _dbContext.SaveChangesAsync(cancellationToken);

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
