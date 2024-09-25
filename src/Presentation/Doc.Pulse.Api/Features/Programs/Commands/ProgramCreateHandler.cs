using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Trouble.Errors;
using Doc.Pulse.Api.Extensions;
using Doc.Pulse.Contracts.Communications.V1.Programs.Commands;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Doc.Pulse.Infrastructure.Extensions;
using FluentValidation;
using MediatR;
using Ots.AppDmDoc.Abstractions.AutoMapper;

namespace Doc.Pulse.Api.Features.Programs.Commands;

public class ProgramCreateHandler
{
    public class Request : IRequest<Response>
    {
        public required ProgramCreateCmd Command { get; set; }
    }
    public class Response : MediatorResult<CommandResponse> { }

    public class DbContextValidator : AbstractValidator<ProgramCreateCmd>
    {
        private readonly AppDbContext _dbContext;
        public DbContextValidator(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            var keyFieldDescription = "ProgramCode".SplitCamelCase();

            RuleFor(p => p.ProgramCode).NotNull().Length(3, 255);
            RuleFor(p => p.ProgramName).Length(3, 255).When(n => n != null);
            RuleFor(p => p.ProgramDescription).Length(3, 255).When(n => n != null);

            RuleFor(p => p)
                .Must(KeyFieldIsUnique)
                .WithErrorCode("UniqueFieldValidator")
                .WithMessage($"'{keyFieldDescription}' must be unique.");
        }

        private bool KeyFieldIsUnique(ProgramCreateCmd cmd)
        {
            return !_dbContext.Programs.Any(o => o.ProgramCode == cmd.ProgramCode);
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
                var entity = new Core.Entities.Program()
                {
                    ProgramCode = cmd.ProgramCode,
                    ProgramName = cmd.ProgramName,
                    ProgramDescription = cmd.ProgramDescription,
                    Inactive = cmd.Inactive
                };

                await _dbContext.Programs.AddAsync(entity, cancellationToken);
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
