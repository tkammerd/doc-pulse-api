using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Trouble.Errors;
using Doc.Pulse.Api.Extensions;
using Doc.Pulse.Contracts.Communications.V1.Programs.Commands;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Doc.Pulse.Infrastructure.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ots.AppDmDoc.Abstractions.AutoMapper;

namespace Doc.Pulse.Api.Features.Programs.Commands;

public class ProgramUpdateHandler
{
    public class Request : IRequest<Response>
    {
        public required ProgramUpdateCmd Command { get; set; }
    }
    public class Response : MediatorResult<CommandResponse> { }

    public class DbContextValidator : AbstractValidator<ProgramUpdateCmd>
    {
        private readonly AppDbContext _dbContext;
        public DbContextValidator(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            var keyFieldDescription = "ProgramCode".SplitCamelCase();
            var tableDescription = "Programs";

            RuleFor(p => p.Id).NotNull().NotEqual(0)
                .WithMessage($"Id not valid: Please indicate a valid Identifier.");
            RuleFor(p => p.ProgramCode).NotNull().Length(3, 255);
            RuleFor(p => p.ProgramName).Length(3, 255).When(n => n != null);
            RuleFor(p => p.ProgramDescription).Length(3, 255).When(n => n != null);
            RuleFor(p => p)
                .Must(command => {
                    var updatingEntity = _dbContext.Programs.FirstOrDefault(o => o.Id == command.Id);
                    return (updatingEntity?.RowVersion ?? []).SequenceEqual(command.RowVersion ?? []);
                })
                .WithErrorCode("RowVersionCheck")
                .WithMessage($"'{tableDescription}' record was changed by another user. Please refresh your browser.");

            RuleFor(p => p)
                .Must(KeyFieldIsUnique)
                .WithErrorCode("UniqueFieldValidator")
                .WithMessage($"'{keyFieldDescription}' must be unique.");
        }

        private bool KeyFieldIsUnique(ProgramUpdateCmd cmd)
        {
            return !_dbContext.Programs.Any(o => o.Id != cmd.Id && o.ProgramCode == cmd.ProgramCode);
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
                var entity = await _dbContext.Programs
                    .AsTracking(QueryTrackingBehavior.TrackAll)
                    .SingleOrDefaultAsync(o => o.Id == cmd.Id, cancellationToken);
                if (entity == null)
                    return response.WithError<Response>(new EntityNotFound(cmd.Id.ToString()));

                entity.ProgramCode = cmd?.ProgramCode ?? "";
                entity.ProgramName = cmd?.ProgramName;
                entity.ProgramDescription = cmd?.ProgramDescription;
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
