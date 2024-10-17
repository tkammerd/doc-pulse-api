﻿using FluentValidation;
using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Trouble.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Doc.Pulse.Contracts.Communications.V1.CodeCategories.Commands;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Ots.AppDmDoc.Abstractions.AutoMapper;
using Doc.Pulse.Infrastructure.Extensions;
using Doc.Pulse.Api.Extensions;

namespace Doc.Pulse.Api.Features.CodeCategories.Commands;

public class CodeCategoryUpdateHandler
{
    public class Request : IRequest<Response>
    {
        public required CodeCategoryUpdateCmd Command { get; set; }
    }
    public class Response : MediatorResult<CommandResponse> { }

    public class DbContextValidator : AbstractValidator<CodeCategoryUpdateCmd>
    {
        private readonly AppDbContext _dbContext;
        public DbContextValidator(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            var keyFieldDescription = "CategoryNumber".SplitCamelCase();
            var tableDescription = "Code Categories";

            RuleFor(p => p.Id).NotNull().NotEqual(0)
                .WithMessage($"Id not valid: Please indicate a valid Identifier.");
            RuleFor(p => p.CategoryNumber).NotNull().InclusiveBetween(51, 59);
            RuleFor(p => p.CategoryShortName).Length(3, 255).When(n => n != null);
            RuleFor(p => p.CategoryName).Length(3, 255).When(n => n != null);
            RuleFor(p => p)
                .Must(command => {
                    var updatingEntity = _dbContext.CodeCategories.FirstOrDefault(o => o.Id == command.Id);
                    return (updatingEntity?.RowVersion ?? []).SequenceEqual(command.RowVersion ?? []);
                })
                .WithErrorCode("RowVersionCheck")
                .WithMessage($"'{tableDescription}' record was changed by another user. Please refresh your browser.");

            RuleFor(p => p)
                .Must(KeyFieldIsUnique)
                .WithErrorCode("UniqueFieldValidator")
                .WithMessage($"'{keyFieldDescription}' must be unique.");
        }

        private bool KeyFieldIsUnique(CodeCategoryUpdateCmd cmd)
        {
            return !_dbContext.CodeCategories.Any(o => o.Id != cmd.Id && o.CategoryNumber == cmd.CategoryNumber);
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
                var entity = await _dbContext.CodeCategories
                    .AsTracking(QueryTrackingBehavior.TrackAll)
                    .SingleOrDefaultAsync(o => o.Id == cmd.Id, cancellationToken);
                if (entity == null)
                    return response.WithError<Response>(new EntityNotFound(cmd.Id.ToString()));

                entity.CategoryNumber = cmd?.CategoryNumber ?? 0;
                entity.CategoryShortName = cmd?.CategoryShortName ?? "";
                entity.CategoryName = cmd?.CategoryName;
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
