using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Trouble.Errors;
using Doc.Pulse.Api.Extensions;
using Doc.Pulse.Contracts.Communications.V1.Vendors.Commands;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Doc.Pulse.Infrastructure.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ots.AppDmDoc.Abstractions.AutoMapper;
using System.Text.Json;

namespace Doc.Pulse.Api.Features.Vendors.Commands;

public class VendorUpdateHandler
{
    public class Request : IRequest<Response>
    {
        public required VendorUpdateCmd Command { get; set; }
    }
    public class Response : MediatorResult<CommandResponse> { }

    public class DbContextValidator : AbstractValidator<VendorUpdateCmd>
    {
        private readonly AppDbContext _dbContext;
        public DbContextValidator(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            var tableDescription = "Vendors";

            RuleFor(p => p.Id).NotNull().NotEqual(0)
                .WithMessage($"Id not valid: Please indicate a valid Identifier.");
            RuleFor(p => p.VendorName).NotNull().Length(3, 255);
            RuleFor(p => p)
                .Must(command => {
                    var updatingEntity = _dbContext.Vendors.FirstOrDefault(o => o.Id == command.Id);
                    return (updatingEntity?.RowVersion ?? []).SequenceEqual(command.RowVersion ?? []);
                })
                .WithErrorCode("RowVersionCheck")
                .WithMessage($"'{tableDescription}' record was changed by another user. Please refresh your browser.");
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
                var entity = await _dbContext.Vendors
                    .AsTracking(QueryTrackingBehavior.TrackAll)
                    .SingleOrDefaultAsync(o => o.Id == cmd.Id, cancellationToken);
                if (entity == null)
                    return response.WithError<Response>(new EntityNotFound(cmd.Id.ToString()));

                entity.VendorName = cmd?.VendorName ?? "";
                entity.Inactive = cmd?.Inactive ?? false;

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
