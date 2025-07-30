using AutoMapper;
using BankLoanSystem.Application.CQRS.Commands.LoanType;
using BankLoanSystem.Application.CQRS.Queries;
using BankLoanSystem.Application.CQRS.Queries.LoanType;
using BankLoanSystem.Core.Models.DTOs.LoanDtos;
using BankLoanSystem.Core.Models.DTOs.LoanTypeDtos;
using BankLoanSystem.Core.Models.ResponseModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BankLoanSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanTypesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public LoanTypesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllLoanTypesQuery());
            var response = ApiResponse<List<LoanTypeDto>>.SuccessResponse(result);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetLoanTypeByIdQuery(id));
            if (result == null)
            {
                var notFoundResponse = ApiResponse<object>.ErrorResponse("Loan Type Not Found", 404);
                return NotFound(notFoundResponse);
            }

            var response = ApiResponse<LoanTypeDto>.SuccessResponse(result);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateLoanTypeCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                if (result == null)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Loan type already exists.", 400));
                }

                var response = ApiResponse<LoanTypeDto>.SuccessResponse(result, "Loan Type created successfully", 201);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while creating loan type.", 500, new List<string> { ex.Message }));
            }
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLoanTypeDto updateLoanTypeDto)
        {

            UpdateLoanTypeCommand command = _mapper.Map<UpdateLoanTypeCommand>(updateLoanTypeDto);

            command.Id = id;

            var result = await _mediator.Send(command);

            if (result == null)
            {
                var notFoundResponse = ApiResponse<object>.ErrorResponse("Loan Type Not Found", 404);
                return NotFound(notFoundResponse);
            }

            var response = ApiResponse<LoanTypeDto>.SuccessResponse(result, "Loan Type updated successfully");
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _mediator.Send(new DeleteLoanTypeCommand(id));

            if (!success)
            {
                var notFoundResponse = ApiResponse<object>.ErrorResponse("Loan Type Not Found", 404);
                return NotFound(notFoundResponse);
            }

            var response = ApiResponse<object>.SuccessResponse(null, "Loan Type deleted successfully");
            return Ok(response);
        }
    }
}