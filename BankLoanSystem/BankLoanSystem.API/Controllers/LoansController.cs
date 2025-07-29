using BankLoanSystem.Application.CQRS.Commands.Loan;
using BankLoanSystem.Application.CQRS.Queries.Loan;
using BankLoanSystem.Core.Models.DTOs.LoanDtos;
using BankLoanSystem.Core.Models.ResponseModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BankLoanSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {

        private readonly IMediator _mediator;

        public LoansController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllLoansQuery());
            var response = ApiResponse<List<LoanDTO>>.SuccessResponse(result);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetLoanByIdQuery(id));

            if (result == null)
            {
                var notFoundResponse = ApiResponse<object>.ErrorResponse("Loan not found", 404);
                return NotFound(notFoundResponse);
            }

            var response = ApiResponse<LoanDTO>.SuccessResponse(result);
            return Ok(response);
        }

        [HttpGet("user")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> GetByUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse("User authentication failed", 401));
            }

            var result = await _mediator.Send(new GetLoansByUserQuery(userId));

            if (result == null || !result.Any())
            {
                var notFoundResponse = ApiResponse<object>.ErrorResponse("No loans found for this user", 404);
                return NotFound(notFoundResponse);
            }

            var response = ApiResponse<List<LoanDTO>>.SuccessResponse(result);
            return Ok(response);
        }

        [HttpGet("type/{loanTypeId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByType(int loanTypeId)
        {
            var result = await _mediator.Send(new GetLoansByTypeQuery(loanTypeId));

            if (result == null || !result.Any())
            {
                var notFoundResponse = ApiResponse<object>.ErrorResponse("No loans found for this type", 404);
                return NotFound(notFoundResponse);
            }

            var response = ApiResponse<List<LoanDTO>>.SuccessResponse(result);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Create([FromForm] CreateLoanCommand command)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse("User authentication failed", 401));
            }

            command.AppUserId = userId;

            var result = await _mediator.Send(command);

            var response = ApiResponse<LoanDTO>.SuccessResponse(result, "Loan created successfully", 201);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateLoanCommand command)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse("User authentication failed", 401));
            }

            command.Id = id;
            command.CurrentUserId = currentUserId;

            var result = await _mediator.Send(command);

            if (result == null)
            {
                var notFoundResponse = ApiResponse<object>.ErrorResponse("Loan not found", 404);
                return NotFound(notFoundResponse);
            }

            var response = ApiResponse<LoanDTO>.SuccessResponse(result, "Loan updated successfully");
            return Ok(response);
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateLoanStatusCommand command)
        {
            command.LoanId = id;

            var result = await _mediator.Send(command);

            if (result == null)
            {
                var errorResponse = ApiResponse<object>.ErrorResponse(
                    "Loan not found",
                    400);
                return BadRequest(errorResponse);
            }

            var response = ApiResponse<LoanDTO>.SuccessResponse(
                result,
                $"Loan status updated to {command.LoanStatus}");
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse("User authentication failed", 401));
            }

            var success = await _mediator.Send(new DeleteLoanCommand(id, currentUserId));

            if (!success)
            {
                var notFoundResponse = ApiResponse<object>.ErrorResponse("Loan not found", 404);
                return NotFound(notFoundResponse);
            }

            var response = ApiResponse<object>.SuccessResponse(null, "Loan deleted successfully");
            return Ok(response);
        }
    }
}