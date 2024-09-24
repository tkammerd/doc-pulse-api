using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Trouble.Errors;
using Doc.Pulse.Api.Extensions;
using Doc.Pulse.Contracts.Communications.V1.Receipts.Commands;
using Doc.Pulse.Core.Entities;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Doc.Pulse.Infrastructure.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ots.AppDmDoc.Abstractions.AutoMapper;

namespace Doc.Pulse.Api.Features.Receipts.Commands;

public class ReceiptCreateHandler
{
    public class Request : IRequest<Response>
    {
        public required ReceiptCreateCmd Command { get; set; }
    }
    public class Response : MediatorResult<CommandResponse> { }

    public class DbContextValidator : AbstractValidator<ReceiptCreateCmd>
    {
        private readonly AppDbContext _dbContext;
        public DbContextValidator(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            var keyFieldDescription = "FacilityAndFiscalYearAndReceiptNumber".SplitCamelCase();

            RuleFor(o => o.Facility).NotNull().Length(3, 255);
            RuleFor(p => p.FiscalYear).NotNull().GreaterThan(2020);
            RuleFor(p => p.ReceiptNumber).NotNull().InclusiveBetween(100, 99999);
            RuleFor(p => p.RfpId).GreaterThan(0).When(n => n != null);

            RuleFor(p => p)
                .Must(KeyFieldIsUnique)
                .WithErrorCode("UniqueFieldValidator")
                .WithMessage($"'{keyFieldDescription}' combined must be unique.");
        }

        private bool KeyFieldIsUnique(ReceiptCreateCmd cmd)
        {
            return !_dbContext.Receipts.Any(
                r => r.Facility == cmd.Facility &&
                r.FiscalYear == cmd.FiscalYear && 
                r.ReceiptNumber == cmd.ReceiptNumber);
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
                var entity = new Receipt()
                {
                    Facility = cmd.Facility,
                    FiscalYear = cmd.FiscalYear,
                    ReceiptNumber = cmd.ReceiptNumber,
                    RfpId = cmd.RfpId,
                    ReceiptDate = cmd.ReceiptDate,
                    ReceivingReportAmount = cmd.ReceivingReportAmount,
                    AmountInIsis = cmd.AmountInIsis,
                    ReceiverNumber = cmd.ReceiverNumber,
                    CheckNumber = cmd.CheckNumber,
                    CheckDate = cmd.CheckDate
                };

                await _dbContext.Receipts.AddAsync(entity, cancellationToken);
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
